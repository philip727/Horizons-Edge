using Philip.Building;
using Philip.Grid;
using Philip.Utilities;
using Philip.Utilities.Maths;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Philip.WorldGeneration
{
    public class WorldGenerationHandler : MonoBehaviourSingleton<WorldGenerationHandler>
    {
        private const int MAP_WIDTH = 1024;
        private const int MAP_HEIGHT = 1024;
        private const int TILE_SIZE = 1;
        private const int CHUNK_SIZE = 32;


        [SerializeField, Header("World Randomisation")] private float _noiseScale = 1f;

        [SerializeField] private int _octaves;
        [SerializeField, Range(0f, 1f)] private float _persistance;
        [SerializeField] private float _lacunarity;

        [SerializeField] private int _seed;
        [SerializeField] private Vector2 _offset;

        [field: SerializeField] public GameObject ChunkPrefab { private set; get; }


        private float[,] _generatedNoiseMap;
        private Grid<WorldNode> _worldGrid;
        private Grid<ChunkNode> _chunkGrid;


        [SerializeField] private Tile _tile;
        

        public void Start()
        {
            GenerateMap();
        }

        public void GenerateMap()
        {
            _generatedNoiseMap = Noise.GenerateNoiseMap(MAP_WIDTH, MAP_HEIGHT, _seed, _offset, _octaves, _persistance, _lacunarity, _noiseScale);
            _worldGrid = new Grid<WorldNode>(MAP_WIDTH, MAP_HEIGHT, TILE_SIZE, (Grid<WorldNode> g, int x, int y) => new WorldNode(g, x, y), debug: false, originPosition: default);
            _chunkGrid = new Grid<ChunkNode>(MAP_WIDTH / CHUNK_SIZE, MAP_HEIGHT / CHUNK_SIZE, CHUNK_SIZE, (Grid<ChunkNode> g, int x, int y) => new ChunkNode(g, x, y), debug: true, originPosition: default);

            GenerateChunkObjects();
            //GenerateWater();
        }

        public void GenerateChunkObjects()
        {
            Vector3 _offsetOfGrid = new Vector3(0.5f, 0.5f, 0f);
            for (int y = 0; y < MAP_HEIGHT / CHUNK_SIZE; y++)
            {
                for (int x = 0; x < MAP_WIDTH / CHUNK_SIZE; x++)
                {
                    ChunkNode chunkNode = _chunkGrid.GetGridObject(x, y);
                    Vector3 worldPosition = _chunkGrid.GetWorldPosition(x, y);
                    GameObject _chunkPrefab = Instantiate(ChunkPrefab, worldPosition + _offsetOfGrid, Quaternion.identity, transform);
                    _chunkPrefab.name = $"chunk_{x}_{y}";
                    chunkNode.SetWalkableTilemap(_chunkPrefab.transform.GetChild(0).GetComponent<Tilemap>());
                    chunkNode.SetColliderTilemap(_chunkPrefab.transform.GetChild(1).GetComponent<Tilemap>());


                    chunkNode.WalkableTilemap.SetTile(new Vector3Int(0, 0, 0), _tile);
                }
            }
        }

        public void GenerateWater()
        {
            for (int y = 0; y < MAP_HEIGHT; y++)
            {
                for (int x = 0; x < MAP_WIDTH; x++)
                {
                    float currentHeight = _generatedNoiseMap[x, y];
                    if (currentHeight <= 0.4f)
                    {
                        _worldGrid.GetGridObject(x, y).SetIsWater(true);
                    }
                    else
                    {
                        _worldGrid.GetGridObject(x, y).SetIsWater(false);
                    }

                }
            }
        }

        //public void DisplayWorld()
        //{
        //    WalkableMap.ClearAllTiles();
        //    ColliderMap.ClearAllTiles();
        //    for (int y = 0; y < MAP_HEIGHT; y++)
        //    {
        //        for (int x = 0; x < MAP_WIDTH; x++)
        //        {
        //            Vector3Int currentCell = new Vector3Int(x, y, 0);
        //            if(_worldGrid.GetGridObject(x, y).IsWater)
        //            {
        //                ColliderMap.SetTile(currentCell, colliderTile);
        //            }
        //            else
        //            {
        //                WalkableMap.SetTile(currentCell, groundTile);
        //            }
        //        }
        //    }
        //}


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

    public enum BiomeTypes
    {
        Water,
        Forest,
        Snow,
    }
}



