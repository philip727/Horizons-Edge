using Philip.Grid;
using Philip.Utilities.Maths;
using UnityEngine;
using Philip.Utilities;
using Philip.Building;

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

            return new WorldData(worldGrid, chunkGrid, waterNoiseMap, precipitationNoiseMap, temperatureNoiseMap, baronNoiseMap, tropicalityNoiseMap, placement);
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
            BiomeObject bestBiome = null;
            float precipitationHeight = s_worldData.PrecipitationMap[x, y];
            float temperatureHeight = s_worldData.TemperatureMap[x, y];

            for (int i = 0; i < WorldGenerationSettings.BiomeObjects.Length; i++)
            {
                BiomeObject currentBiomeObject = WorldGenerationSettings.BiomeObjects[i];

                float currentBiomeDistance = Mathf.Abs(Mathf.Pow((precipitationHeight - currentBiomeObject.Precipitation) +
                    (temperatureHeight - currentBiomeObject.Temperature), 2f));

                float bestBiomeDistance = bestBiome == null ? 0f : Mathf.Abs(Mathf.Pow((precipitationHeight - bestBiome.Precipitation) +
                    (temperatureHeight - bestBiome.Temperature), 2f));

                if (currentBiomeDistance > bestBiomeDistance)
                {
                    bestBiome = currentBiomeObject;
                }
            }

            return bestBiome;
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

            ResourceObject bestResourceObject = biomeObject.ResourceObjects[0];

            for (int i = 0; i < biomeObject.ResourceObjects.Length; i++)
            {
                ResourceObject resourceObject = biomeObject.ResourceObjects[i];
                float currentResourceDistance = Mathf.Abs(Mathf.Pow((baronHeight - resourceObject.Baron) + 
                    (tropicalHeight - resourceObject.Tropicality), 2f));

                float bestResourceDistance = bestResourceObject ==  null ? 0f : Mathf.Abs(Mathf.Pow((baronHeight - bestResourceObject.Baron) + 
                    (tropicalHeight - bestResourceObject.Tropicality), 2f));

                if(currentResourceDistance > bestResourceDistance)
                {
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

                    ResourceObject resourceObject = GetBestResource(biomeObject, x, y);
                    if (resourceObject.Resource != ResourceObject.ResourceType.Nothing)
                    {
                        Instantiate(resourceObject.Prefab, worldPosition, Quaternion.identity, chunk.ChunkGameObject.transform);
                    }
                }
            }
        }

    }
}