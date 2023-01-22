using Philip.Grid;
using UnityEngine;

namespace Philip.WorldGeneration
{
    public class WorldData
    {
        public bool WorldGenerationCompleted { private set; get; }
        public Grid<WorldNode> WorldGrid { private set; get; }
        public Grid<ChunkNode> ChunkGrid { private set; get; }
        public float[,] HeightMap { private set; get; }

        public WorldData(Grid<WorldNode> worldGrid, Grid<ChunkNode> chunkGrid, float[,] heightMap)
        {
            WorldGenerationCompleted = false;
            WorldGrid = worldGrid;
            ChunkGrid = chunkGrid;
            HeightMap = heightMap;
        }

        public void FinishWorldGeneration()
        {
            WorldGenerationCompleted = true;
        }

        public void SetWorldGrid(Grid<WorldNode> grid)
        {
            WorldGrid = grid;
        }

        public void SetChunkGrid(Grid<ChunkNode> grid)
        {
            ChunkGrid = grid;
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