using Philip.Grid;
using Philip.Utilities.Maths;
using UnityEngine;
using Philip.Utilities;
using Philip.Building;
using System.Collections.Generic;
using UnityEngine.Analytics;

namespace Philip.WorldGeneration
{
    public class WorldGenerationHandler : MonoBehaviourSingleton<WorldGenerationHandler>
    {
        public delegate void WorldGenerationFished();
        public static WorldData s_worldData;
        public WorldGenerationFished onWorldGenerationFinished;
        [field: SerializeField, Header("World")] public int Seed { private set; get; }
        [field: SerializeField] public WorldGenerationSettings WorldGenerationSettings { private set; get; }
        [field: SerializeField] public NoiseSettings LandSettings { private set; get; }
        [field: SerializeField] public NoiseSettings PrecipitationSettings { private set; get; }
        [field: SerializeField] public NoiseSettings TemperatureSettings { private set; get; }
        [field: SerializeField] public NoiseSettings BaronSettings { private set; get; }
        [field: SerializeField] public NoiseSettings TropicalitySettings { private set; get; }
        [field: SerializeField] public NoiseSettings ObjectSettings { private set; get; }
        [field: SerializeField, Header("Chunk Setup")] public GameObject ChunkPrefab { private set; get; }

        public void Start()
        {
            s_worldData = GenerateWorldData(Seed);
            CreateWorldFromData();
            FinishWorldGeneration();
        }

        private void FinishWorldGeneration()
        {
            s_worldData.FinishWorldGeneration();
            onWorldGenerationFinished?.Invoke();
        }

        private void CreateWorldFromData()
        {
            GenerateChunkObjects();
            GenerateWater();
            GenerateBiomes();
            GenerateWorldObjects();
        }

        private WorldData GenerateWorldData(int seed)
        {
            // Generates the noise we use for water randomisation
            float[,] waterNoiseMap = Noise.GenerateNoiseMap(
                WorldGenerationSettings.WorldWidth, 
                WorldGenerationSettings.WorldHeight,
                seed,
                LandSettings.Offset,
                LandSettings.Octaves,
                LandSettings.Persistance,
                LandSettings.Lacunarity,
                LandSettings.NoiseScale);

            // Generates the noise we use for precipitation, this will affect biome generation
            float[,] precipitationNoiseMap = Noise.GenerateNoiseMap(
                WorldGenerationSettings.WorldWidth,
                WorldGenerationSettings.WorldHeight,
                seed,
                PrecipitationSettings.Offset,
                PrecipitationSettings.Octaves,
                PrecipitationSettings.Persistance,
                PrecipitationSettings.Lacunarity,
                PrecipitationSettings.NoiseScale);

            // Generates the noise we use for temperature, this will affect biome generation
            float[,] temperatureNoiseMap = Noise.GenerateNoiseMap(
                WorldGenerationSettings.WorldWidth,
                WorldGenerationSettings.WorldHeight,
                seed,
                TemperatureSettings.Offset,
                TemperatureSettings.Octaves,
                TemperatureSettings.Persistance,
                TemperatureSettings.Lacunarity,
                TemperatureSettings.NoiseScale);

            float[,] baronNoiseMap = Noise.GenerateNoiseMap(
                WorldGenerationSettings.WorldWidth,
                WorldGenerationSettings.WorldHeight,
                seed,
                BaronSettings.Offset,
                BaronSettings.Octaves,
                BaronSettings.Persistance,
                BaronSettings.Lacunarity,
                BaronSettings.NoiseScale);

            float[,] tropicalityNoiseMap = Noise.GenerateNoiseMap(
                WorldGenerationSettings.WorldWidth,
                WorldGenerationSettings.WorldHeight,
                seed,
                TropicalitySettings.Offset,
                TropicalitySettings.Octaves,
                TropicalitySettings.Persistance,
                TropicalitySettings.Lacunarity,
                TropicalitySettings.NoiseScale);

            float[,] objectNoiseMap = Noise.GenerateNoiseMap(
                WorldGenerationSettings.WorldHeight,
                WorldGenerationSettings.WorldWidth,
                seed,
                ObjectSettings.Offset,
                ObjectSettings.Octaves,
                ObjectSettings.Persistance,
                ObjectSettings.Lacunarity,
                ObjectSettings.NoiseScale);

            // Creates the grid that we use for single tile placement
            Grid<WorldNode> worldGrid = new Grid<WorldNode>(WorldGenerationSettings.WorldWidth, 
                WorldGenerationSettings.WorldHeight, 
                WorldGenerationSettings.TileSize, 
                (Grid<WorldNode> g, int x, int y) => new WorldNode(g, x, y), debug: false, originPosition: default);
            
            // Creates the grid that we use for chunks which holds the tiles displayed
            Grid<ChunkNode> chunkGrid = new Grid<ChunkNode>(WorldGenerationSettings.WorldWidth / WorldGenerationSettings.ChunkSize,
                WorldGenerationSettings.WorldHeight / WorldGenerationSettings.ChunkSize,
                WorldGenerationSettings.ChunkSize, 
                (Grid<ChunkNode> g, int x, int y) => new ChunkNode(g, x, y), debug: true, originPosition: default);

            Placement<IBuildable> placement = new Placement<IBuildable>(
                WorldGenerationSettings.WorldWidth,
                WorldGenerationSettings.WorldHeight,
                WorldGenerationSettings.TileSize,
                originPosition: default);

            return new WorldData(worldGrid, chunkGrid, waterNoiseMap, precipitationNoiseMap, temperatureNoiseMap, baronNoiseMap, tropicalityNoiseMap, objectNoiseMap, placement);
        }

        private void GenerateChunkObjects()
        {
            Vector3 chunkOffset = new Vector3(0.5f, 0.5f, 0f);

            // Creates each chunk
            for (int y = 0; y < WorldGenerationSettings.WorldHeight / WorldGenerationSettings.ChunkSize; y++)
            {
                for (int x = 0; x < WorldGenerationSettings.WorldWidth / WorldGenerationSettings.ChunkSize; x++)
                {
                    // Gets the world position of the chunk
                    ChunkNode chunkNode = s_worldData.ChunkGrid.GetGridObject(x, y);
                    Vector3 worldPosition = s_worldData.ChunkGrid.GetWorldPosition(x, y);

                    // Creates the chunk at the right position which we can use to display tiles
                    GameObject chunkPrefab = Instantiate(ChunkPrefab, worldPosition + chunkOffset, Quaternion.identity, transform);
                    chunkPrefab.name = $"chunk_{x}_{y}";

                    // Adds the tilemaps to the chunks
                    chunkNode.SetupChunk(chunkPrefab);
                }
            }
        }

        // Generates water and oceans
        private void GenerateWater()
        {
            for (int y = 0; y < WorldGenerationSettings.WorldHeight; y++)
            {
                for (int x = 0; x < WorldGenerationSettings.WorldWidth; x++)
                {
                    float currentHeight = s_worldData.HeightMap[x, y];

                    // Sets water tiles at right height
                    if (currentHeight <= 0.4f)
                    {
                        s_worldData.WorldGrid.GetGridObject(x, y).SetIsWater(true);
                        continue;
                    }

                    s_worldData.WorldGrid.GetGridObject(x, y).SetIsWater(false);
                }
            }
        }

        private BiomeObject GetBestBiome(int x, int y)
        {
            BiomeObject bestBiomeObject = null;
            float bestBiomeDistance = 9999999f;
            float precipitationHeight = s_worldData.PrecipitationMap[x, y];
            float temperatureHeight = s_worldData.TemperatureMap[x, y];

            for (int i = 0; i < WorldGenerationSettings.BiomeObjects.Length; i++)
            {
                BiomeObject currentBiomeObject = WorldGenerationSettings.BiomeObjects[i];

                // Euclidian distance which is absolute of ((a1 - b1) - (a2 - b2)) ^ 2
                Vector2 mapVector = new Vector2(precipitationHeight, temperatureHeight);
                Vector2 biomeVector = new Vector2(currentBiomeObject.Precipitation, currentBiomeObject.Temperature);
                float currentBiomeDistance = PVector.GetDistanceBetween(mapVector, biomeVector);

                if (bestBiomeDistance > currentBiomeDistance)
                {
                    bestBiomeDistance = currentBiomeDistance;
                    bestBiomeObject = currentBiomeObject;
                }
            }

            return bestBiomeObject;
        }

        private void GenerateBiomes()
        {
            for (int y = 0; y < WorldGenerationSettings.WorldHeight; y++)
            {
                for (int x = 0; x < WorldGenerationSettings.WorldWidth; x++)
                {
                    // Gets the precipitation and temp map and creates a default biome we can start with
                    BiomeObject bestBiome = GetBestBiome(x, y);

                    // Set biome
                    s_worldData.WorldGrid.GetGridObject(x, y).SetBiome(bestBiome == null ? WorldGenerationSettings.BiomeObjects[0].Biome : bestBiome.Biome);
                }
            }
        }

        private ResourceObject GetBestResource(BiomeObject biomeObject, int x, int y)
        {
            float baronHeight = s_worldData.BaronMap[x, y];
            float tropicalHeight = s_worldData.TropicalityMap[x, y];

            ResourceObject bestResourceObject = null;
            float bestResourceDistance = 100000f;

            for (int i = 0; i < biomeObject.ResourceObjects.Length; i++)
            {
                ResourceObject resourceObject = biomeObject.ResourceObjects[i];

                //float currentResourceDistance = PVector.GetDistanceBetween(mapVector, resourceVector);
                float currentResourceDistance = Mathf.Abs(tropicalHeight - resourceObject.Tropicality) + Mathf.Abs(baronHeight - resourceObject.Baron);

                if (bestResourceDistance > currentResourceDistance)
                {
                    bestResourceDistance = currentResourceDistance;
                    bestResourceObject = resourceObject;
                }
            }

            return bestResourceObject;
        }

        private void GenerateWorldObjects()
        {
            for (int y = 0; y < WorldGenerationSettings.WorldHeight; y++)
            {
                for (int x = 0; x < WorldGenerationSettings.WorldWidth; x++)
                {
                    WorldNode worldNode = s_worldData.WorldGrid.GetGridObject(x, y);

                    if (worldNode.HasWaterNeighbours())
                    {
                        continue;
                    }

                    if (s_worldData.ObjectMap[x, y] >= 0.2f)
                    {
                        continue;
                    }

                    Biome biome = worldNode.Biome;
                    BiomeObject biomeObject = WorldGenerationSettings.GetBiomeObject(biome);

                    if(biomeObject.ResourceObjects.Length == 0)
                    {
                        continue;
                    }

                    int chunkX = x / WorldGenerationSettings.ChunkSize;
                    int chunkY = y / WorldGenerationSettings.ChunkSize;

                    ChunkNode chunk = s_worldData.ChunkGrid.GetGridObject(chunkX, chunkY);
                    Vector3 worldPosition = s_worldData.WorldGrid.GetWorldPosition(x, y);

                    //System.Random prng = new System.Random(Seed);
                    ResourceObject resourceObject = GetBestResource(biomeObject, x, y);
                    //ResourceObject resourceObject = biomeObject.ResourceObjects[prng.Next(0, biomeObject.ResourceObjects.Length - 1)];
                    PlaceObjectAtNode(resourceObject, worldPosition, chunk, new Vector2Int(x, y));
                }
            }
        }

        private void PlaceObjectAtNode(ResourceObject resourceObject, Vector3 worldPosition, ChunkNode chunk, Vector2Int givenCoords)
        {
            if (s_worldData.Placement.CanPlaceBuildingAtNode(resourceObject.StructureObjectSettings, givenCoords))
            {
                GameObject obj = Instantiate(resourceObject.Prefab, worldPosition, Quaternion.identity, chunk.ChunkGameObject.transform);
                IBuildable buildable = obj.GetComponentInChildren<IBuildable>();
                Placement<IBuildable>.Instance.PlaceObjectInNode(buildable, givenCoords);
            }
        }
    }
}