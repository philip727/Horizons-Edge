using Philip.Grid;
using Philip.WorldGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldData
{
    public bool Initialized { private set; get; }
    public Grid<WorldNode> WorldGrid { private set; get; }
    public Grid<ChunkNode> ChunkGrid { private set; get; }

    public WorldData(Grid<WorldNode> worldGrid, Grid<ChunkNode> chunkGrid)
    {
        Initialized = false;
        WorldGrid = worldGrid;
        ChunkGrid = chunkGrid;
    }

    public void FinishInit()
    {
        Initialized = true;
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
