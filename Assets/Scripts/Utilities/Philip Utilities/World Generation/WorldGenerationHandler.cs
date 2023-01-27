using Philip.Grid;
using Philip.Utilities.Maths;
using UnityEngine;
using UnityEngine.Tilemaps;
using Philip.Utilities;

namespace Philip.WorldGeneration
{
    public class WorldGenerationHandler : MonoBehaviourSingleton<WorldGenerationHandler>
    {
        public static WorldData s_worldData;
        [field: SerializeField, Header("World")] public int Seed { private set; get; }
        [field: SerializeField] public WorldGenerationSettings WorldGenerationSettings { private set; get; }
        [field: SerializeField] public NoiseSettings LandSettings { private set; get; }
        [field: SerializeField] public NoiseSettings PrecipitationSettings { private set; get; }
        [field: SerializeField] public NoiseSettings TemperatureSettings { private set; get; }
        [field: SerializeField, Header("Chunk Setup")] public GameObject ChunkPrefab { private set; get; }


        [SerializeField] private Tile _tile;
        [SerializeField] private Tile _waterTile;



        public void Start()
        {
            s_worldData = GenerateWorldData(Seed);
            CreateWorldFromData();
            s_worldData.FinishWorldGeneration();
        }

        public void CreateWorldFromData()
        {
            GenerateChunkObjects();
            GenerateWater();
            GenerateBiomes();
            //DisplayTilesInChunks();
        }

        public WorldData GenerateWorldData(int seed)
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

            return new WorldData(worldGrid, chunkGrid, waterNoiseMap, precipitationNoiseMap, temperatureNoiseMap);
        }

        public void GenerateChunkObjects()
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
        public void GenerateWater()
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

        public void GenerateBiomes()
        {
            for (int y = 0; y < WorldGenerationSettings.WorldHeight; y++)
            {
                for (int x = 0; x < WorldGenerationSettings.WorldWidth; x++)
                {
                    // Gets the precipitation and temp map and creates a default biome we can start with
                    BiomeObject bestBiome = null;
                    float precipitationHeight = s_worldData.PrecipitationMap[x, y];
                    float temperatureHeight = s_worldData.TemperatureMap[x, y];

                    // Loops through all the biomes and checks if its the best fit for that block
                    for (int i = 0; i < WorldGenerationSettings.BiomeObjects.Length; i++)
                    {
                        BiomeObject currentBiomeObject = WorldGenerationSettings.BiomeObjects[i];


                        float currentBiomeEuclidianDistance = Mathf.Abs(Mathf.Pow(precipitationHeight - currentBiomeObject.Precipitation, 2f) +
                            Mathf.Pow(temperatureHeight - currentBiomeObject.Temperature, 2f));

                        float bestBiomeEuclidianDistance = bestBiome == null ? 0f : Mathf.Abs(Mathf.Pow(precipitationHeight - bestBiome.Precipitation, 2f) +
                            Mathf.Pow(temperatureHeight - bestBiome.Temperature, 2f));

                        if (currentBiomeEuclidianDistance > bestBiomeEuclidianDistance)
                        {
                            bestBiome = currentBiomeObject;
                        }
                    }

                    // Set biome
                    s_worldData.WorldGrid.GetGridObject(x, y).SetBiome(bestBiome == null ? WorldGenerationSettings.BiomeObjects[0].Biome : bestBiome.Biome);
                }
            }
        }

        //public void DisplayTilesInChunks()
        //{
        //    for (int y = 0; y < WorldGenerationSettings.WorldHeight; y++)
        //    {
        //        for (int x = 0; x < WorldGenerationSettings.WorldWidth; x++)
        //        {
        //            // Gets the chunk node
        //            WorldNode worldNode = s_worldData.WorldGrid.GetGridObject(x, y);
        //            Vector3 worldPosition = s_worldData.WorldGrid.GetWorldPosition(x, y);
        //            ChunkNode chunkNode = s_worldData.ChunkGrid.GetGridObject(worldPosition);

        //            // Makes sure the tile is in the right position of its current chunk tilemap
        //            Vector3Int tilemapCoordinate = new Vector3Int(x - WorldGenerationSettings.ChunkSize * chunkNode.X,
        //                                                          y - WorldGenerationSettings.ChunkSize * chunkNode.Y);
        //            if (worldNode.IsWater)
        //            {
        //                chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, _waterTile);
        //                continue;
        //            }

        //            if (worldNode.Biome.ID == "biomes:void_shores")
        //                Debug.Log("Hi");

        //            //Debug.Log(worldNode.Biome.ID);

        //            chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules);
        //        }
        //    }
        //}
    }
}



