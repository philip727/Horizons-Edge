using Philip.Building;
using Philip.Grid;
using UnityEngine;
using static Philip.WorldGeneration.WorldData;

namespace Philip.WorldGeneration
{
    public class WorldData
    {

        public bool WorldGenerationCompleted { private set; get; }
        public Grid<WorldNode> WorldGrid { get; }
        public Grid<ChunkNode> ChunkGrid { get; }
        public float[,] HeightMap { get; }
        public float[,] PrecipitationMap { get; }
        public float[,] TemperatureMap { get; }
        public float[,] BaronMap { get; }
        public float[,] TropicalityMap { get; }
        public float[,] ObjectMap { get; }
        public Placement<IBuildable> Placement { get; }

        public WorldData(Grid<WorldNode> worldGrid, Grid<ChunkNode> chunkGrid, float[,] heightMap, float[,] precipitationMap, float[,] temperatureMap, float[,] rockyMap, float[,] tropicalityMap, float[,] objectMap, Placement<IBuildable> placement)
        {
            WorldGenerationCompleted = false;
            WorldGrid = worldGrid;
            ChunkGrid = chunkGrid;
            HeightMap = heightMap;
            PrecipitationMap = precipitationMap;
            TemperatureMap = temperatureMap;
            BaronMap = rockyMap;
            TropicalityMap = tropicalityMap;
            ObjectMap = objectMap;
            Placement = placement;
        }

        public void FinishWorldGeneration()
        {
            WorldGenerationCompleted = true;
        }

        public bool IsValidChunk(Vector2Int coords)
        {
            return ChunkGrid.IsValidCoordinate(coords);
        }

        public bool IsValidChunk(int x, int y)
        {
            return ChunkGrid.IsValidCoordinate(x, y);
        }

        public bool IsValidChunk(Vector3 worldPosition)
        {
            return ChunkGrid.IsValidCoordinate(ChunkGrid.GetCoordinate(worldPosition));
        }
    }
}