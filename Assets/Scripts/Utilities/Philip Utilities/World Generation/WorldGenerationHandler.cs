using Philip.Grid;
using Philip.Utilities.Math;
using UnityEngine;
using Philip.Utilities;
using Philip.Building;
using System.Collections.Generic;
using UnityEngine.Analytics;
using UnityEngine.Tilemaps;

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
        

        public void Start()
        {

        }


        private void Update()
        {
        }

        public bool IsCoordinateWater(Vector2Int coordinates)
        {
            float currentHeight = Noise.GenerateHeight(
                coordinates.x,
                coordinates.y,
                Seed,
                LandSettings.Offset,
                LandSettings.Octaves,
                LandSettings.Persistance,
                LandSettings.Lacunarity,
                LandSettings.NoiseScale);

            return currentHeight >= 0.1f;
        }

       
        public ChunkData RequestChunkData(int x, int y)
        {
            Vector2Int coords = new Vector2Int(x, y);

            float[,] precipMap = Noise.GenerateMap(
                WorldGenerationSettings.ChunkSize,
                WorldGenerationSettings.ChunkSize,
                Seed,
                PrecipitationSettings.Offset + coords,
                PrecipitationSettings.Octaves,
                PrecipitationSettings.Persistance,
                PrecipitationSettings.Lacunarity,
                PrecipitationSettings.NoiseScale,
                Noise.NormalizeMode.Global);

            float[,] tempMap = Noise.GenerateMap(
                WorldGenerationSettings.ChunkSize,
                WorldGenerationSettings.ChunkSize,
                Seed,
                TemperatureSettings.Offset + coords,
                TemperatureSettings.Octaves,
                TemperatureSettings.Persistance,
                TemperatureSettings.Lacunarity,
                TemperatureSettings.NoiseScale,
                Noise.NormalizeMode.Global);

            float[,] baronMap = Noise.GenerateMap(
                WorldGenerationSettings.ChunkSize,
                WorldGenerationSettings.ChunkSize,
                Seed,
                BaronSettings.Offset + coords,
                BaronSettings.Octaves,
                BaronSettings.Persistance,
                BaronSettings.Lacunarity,
                BaronSettings.NoiseScale,
                Noise.NormalizeMode.Global);

            float[,] tropicalMap = Noise.GenerateMap(
                WorldGenerationSettings.ChunkSize,
                WorldGenerationSettings.ChunkSize,
                Seed,
                TropicalitySettings.Offset + coords,
                TropicalitySettings.Octaves,
                TropicalitySettings.Persistance,
                TropicalitySettings.Lacunarity,
                TropicalitySettings.NoiseScale,
                Noise.NormalizeMode.Global);


            return new ChunkData(coords, precipMap, tempMap, baronMap, tropicalMap);
        }
    }

    public class ChunkData
    {
        public Vector2Int Coordinates { private set; get; }
        public float[,] PrecipitationMap { private set; get; }
        public float[,] TemperatureMap { private set; get; }
        public float[,] BaronMap { private set; get; }
        public float[,] TropicalityMap { private set; get; }
        public GameObject ChunkGameObject { private set; get; }
        public Tilemap WalkableTilemap { private set; get; }
        public Tilemap WaterTilemap { private set; get; }
        public Tilemap ColliderTilemap { private set; get; }
        public bool IsVisible
        {
            get
            {
                return ChunkGameObject.activeSelf;
            }
        }

        public ChunkData(Vector2Int coordinates, float[,] precipitationMap, float[,] temperatureMap, float[,] baronMap, float[,] tropicalityMap)
        {
            Coordinates = coordinates;
            PrecipitationMap = precipitationMap;
            TemperatureMap = temperatureMap;
            BaronMap = baronMap;
            TropicalityMap = tropicalityMap;
        }

        public void SetupChunk(GameObject chunkObject)
        {
            ChunkGameObject = chunkObject;
            WalkableTilemap = chunkObject.transform.GetChild(0).GetComponent<Tilemap>();
            WaterTilemap = chunkObject.transform.GetChild(1).GetComponent<Tilemap>();
            ColliderTilemap = chunkObject.transform.GetChild(2).GetComponent<Tilemap>();
            //SetVisible(false);
        }

        public void SetVisible(bool value)
        {
            ChunkGameObject.SetActive(value);
        }
    }

}