using Philip.Grid;
using Philip.Utilities.Maths;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

namespace Philip.WorldGeneration
{
    public class WorldGenerationHandler : MonoBehaviour
    {
        public static WorldData s_worldData;
        private const int MAP_WIDTH = 1024;
        private const int MAP_HEIGHT = 1024;
        private const int TILE_SIZE = 1;
        private const int CHUNK_SIZE = 16;

        [SerializeField, Header("World Randomisation")] private float _noiseScale = 1f;

        [SerializeField] private int _octaves;
        [SerializeField, Range(0f, 1f)] private float _persistance;
        [SerializeField] private float _lacunarity;

        [SerializeField] private int _seed;
        [SerializeField] private Vector2 _offset;

        [field: SerializeField, Header("World Setup")] public GameObject ChunkPrefab { private set; get; }


        [SerializeField] private Tile _tile;
        [SerializeField] private Tile _waterTile;


        public void Start()
        {
            s_worldData = GenerateWorldData();
            CreateWorldFromData();
            s_worldData.FinishWorldGeneration();
        }

        public void CreateWorldFromData()
        {
            GenerateChunkObjects();
            GenerateWater();
            DisplayTilesInChunks();
        }

        public WorldData GenerateWorldData()
        {
            // Generates the noise we use for randomisation
            float[,] generatedNoiseMap = Noise.GenerateNoiseMap(MAP_WIDTH, MAP_HEIGHT, _seed, _offset, _octaves, _persistance, _lacunarity, _noiseScale);

            // Creates the required grids for chunking and placing tiles
            Grid<WorldNode> worldGrid = new Grid<WorldNode>(MAP_WIDTH, MAP_HEIGHT, TILE_SIZE, (Grid<WorldNode> g, int x, int y) => new WorldNode(g, x, y), debug: false, originPosition: default);
            Grid<ChunkNode>  chunkGrid = new Grid<ChunkNode>(MAP_WIDTH / CHUNK_SIZE, MAP_HEIGHT / CHUNK_SIZE, CHUNK_SIZE, (Grid<ChunkNode> g, int x, int y) => new ChunkNode(g, x, y), debug: true, originPosition: default);

            return new WorldData(worldGrid, chunkGrid, generatedNoiseMap);
        }

        public void GenerateChunkObjects()
        {
            
            Vector3 chunkOffset = new Vector3(0.5f, 0.5f, 0f);

            // Creates each chunk
            for (int y = 0; y < MAP_HEIGHT / CHUNK_SIZE; y++)
            {
                for (int x = 0; x < MAP_WIDTH / CHUNK_SIZE; x++)
                {
                    ChunkNode chunkNode = s_worldData.ChunkGrid.GetGridObject(x, y);
                    Vector3 worldPosition = s_worldData.ChunkGrid.GetWorldPosition(x, y);

                    // Creates the chunk at the right position
                    GameObject chunkPrefab = Instantiate(ChunkPrefab, worldPosition + chunkOffset, Quaternion.identity, transform);
                    chunkPrefab.name = $"chunk_{x}_{y}";

                    // Adds the tilemaps to the chunks
                    chunkNode.SetChunkGameObject(chunkPrefab);
                    chunkNode.SetWalkableTilemap(chunkPrefab.transform.GetChild(0).GetComponent<Tilemap>());
                    chunkNode.SetColliderTilemap(chunkPrefab.transform.GetChild(1).GetComponent<Tilemap>());
                    chunkNode.SetVisible(false);
                }
            }
        }

        public void GenerateWater()
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                for (int x = 0; x < MAP_WIDTH; x++)
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
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    WorldNode worldNode = s_worldData.WorldGrid.GetGridObject(x, y);
                    Vector3 worldPosition = s_worldData.WorldGrid.GetWorldPosition(x, y);
                    ChunkNode chunkNode = s_worldData.ChunkGrid.GetGridObject(worldPosition);

                    // Makes sure the tile is in the right position of its current chunk tilemap
                    Vector3Int tilemapCoordinate = new Vector3Int(x - CHUNK_SIZE * chunkNode.X, y - CHUNK_SIZE * chunkNode.Y);
                    if(worldNode.IsWater)
                    {
                        chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, _waterTile);
                        continue;
                    }

                    chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, _tile);
                }
            }
        }

        private void OnValidate()
        {
            if (_lacunarity < 1)
            {
                _lacunarity = 1;
            }

            if(_octaves < 0)
            {
                _octaves = 0;
            }
        }
    }
}



