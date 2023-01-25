using Philip.Grid;
using UnityEngine;

namespace Philip.WorldGeneration
{
    public struct WorldData
    {
        public bool WorldGenerationCompleted { private set; get; }
        public readonly Grid<WorldNode> WorldGrid { get; }
        public readonly Grid<ChunkNode> ChunkGrid { get; }
        public readonly float[,] HeightMap { get; }
        public readonly float[,] PrecipitationMap { get; }
        public readonly float[,] TemperatureMap { get; }

        public WorldData(Grid<WorldNode> worldGrid, Grid<ChunkNode> chunkGrid, float[,] heightMap, float[,] precipitationMap, float[,] temperatureMap)
        {
            WorldGenerationCompleted = false;
            WorldGrid = worldGrid;
            ChunkGrid = chunkGrid;
            HeightMap = heightMap;
            PrecipitationMap = precipitationMap;
            TemperatureMap = temperatureMap;
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