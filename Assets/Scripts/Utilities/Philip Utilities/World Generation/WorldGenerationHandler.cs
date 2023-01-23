using Philip.Grid;
using Philip.Utilities.Maths;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections.Generic;
using Philip.Utilities;

namespace Philip.WorldGeneration
{
    public class WorldGenerationHandler : MonoBehaviourSingleton<WorldGenerationHandler>
    {
        public static WorldData s_worldData;
        [field: SerializeField, Header("World")] public int Seed { private set; get; }
        [field:SerializeField] public WorldGenerationSettings WorldGenerationSettings { private set; get; }
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
            DisplayTilesInChunks();
        }

        public WorldData GenerateWorldData(int seed)
        {
            // Generates the noise we use for randomisation
            float[,] waterNoiseMap = Noise.GenerateNoiseMap(
                WorldGenerationSettings.WorldWidth, 
                WorldGenerationSettings.WorldHeight,
                seed,
                LandSettings.Offset,
                LandSettings.Octaves,
                LandSettings.Persistance,
                LandSettings.Lacunarity,
                LandSettings.NoiseScale);

            // Creates the required grids for chunking and placing tiles
            Grid<WorldNode> worldGrid = new Grid<WorldNode>(WorldGenerationSettings.WorldWidth, 
                WorldGenerationSettings.WorldHeight, 
                WorldGenerationSettings.TileSize, 
                (Grid<WorldNode> g, int x, int y) => new WorldNode(g, x, y), debug: false, originPosition: default);
            
            Grid<ChunkNode>  chunkGrid = new Grid<ChunkNode>(WorldGenerationSettings.WorldWidth / WorldGenerationSettings.ChunkSize,
                WorldGenerationSettings.WorldHeight / WorldGenerationSettings.ChunkSize,
                WorldGenerationSettings.ChunkSize, 
                (Grid<ChunkNode> g, int x, int y) => new ChunkNode(g, x, y), debug: true, originPosition: default);

            return new WorldData(worldGrid, chunkGrid, waterNoiseMap);
        }

        public void GenerateChunkObjects()
        {
            
            Vector3 chunkOffset = new Vector3(0.5f, 0.5f, 0f);

            // Creates each chunk
            for (int y = 0; y < WorldGenerationSettings.WorldHeight / WorldGenerationSettings.ChunkSize; y++)
            {
                for (int x = 0; x < WorldGenerationSettings.WorldWidth / WorldGenerationSettings.ChunkSize; x++)
                {
                    ChunkNode chunkNode = s_worldData.ChunkGrid.GetGridObject(x, y);
                    Vector3 worldPosition = s_worldData.ChunkGrid.GetWorldPosition(x, y);

                    // Creates the chunk at the right position
                    GameObject chunkPrefab = Instantiate(ChunkPrefab, worldPosition + chunkOffset, Quaternion.identity, transform);
                    chunkPrefab.name = $"chunk_{x}_{y}";

                    // Adds the tilemaps to the chunks
                    chunkNode.SetupChunk(chunkPrefab);
                }
            }
        }

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

        public void DisplayTilesInChunks()
        {
            for (int y = 0; y < WorldGenerationSettings.WorldHeight; y++)
            {
                for (int x = 0; x < WorldGenerationSettings.WorldWidth; x++)
                {
                    WorldNode worldNode = s_worldData.WorldGrid.GetGridObject(x, y);
                    Vector3 worldPosition = s_worldData.WorldGrid.GetWorldPosition(x, y);
                    ChunkNode chunkNode = s_worldData.ChunkGrid.GetGridObject(worldPosition);

                    // Makes sure the tile is in the right position of its current chunk tilemap
                    Vector3Int tilemapCoordinate = new Vector3Int(x - WorldGenerationSettings.ChunkSize * chunkNode.X, 
                                                                  y - WorldGenerationSettings.ChunkSize * chunkNode.Y);
                    if(worldNode.IsWater)
                    {
                        chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, _waterTile);
                        continue;
                    }

                    chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, _tile);
                }
            }
        }
    }
}



