using Philip.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGridManager : GridManager
{
    public Pathfinding Pathfinding { private set; get; }
    [field: SerializeField, Header("Nav setup")] public List<Vector2Int> CoordinatesToIgnore { private set; get; } = new List<Vector2Int>();

    [field: SerializeField] public Vector2Int StartPoint { private set; get; } = new Vector2Int(0, 0);

    public override void BakeGrid(int width, int height, float cellSize, Vector2 origin)
    {
        Pathfinding = new Pathfinding(width, height, cellSize, originPosition: origin);
        base.BakeGrid(width, height, cellSize, origin);
    }

    public override void SetupTilesInGrid()
    {
        for (int i = 0; i < CoordinatesToIgnore.Count; i++)
        {
            Vector2Int coords = CoordinatesToIgnore[i];
            SetTileWalkable(coords, false);
        }
    }
    public void AddIgnoredTiles(Vector2Int coords)
    {
        if (!CoordinatesToIgnore.Contains(coords))
        {
            CoordinatesToIgnore.Add(coords);
            SetTileWalkable(coords, false);
        }
    }

    public void RemoveIgnoredTiles(Vector2Int coords)
    {
        if (CoordinatesToIgnore.Contains(coords))
        {
            CoordinatesToIgnore.Remove(coords);
            SetTileWalkable(coords, true);
        }
    }

    public bool IsStartPoint(Vector2Int coords)
    {
        return coords == StartPoint;
    }

    private void SetTileWalkable(Vector2Int coords, bool value)
    {
        Pathfinding.GetGrid().GetGridObject(coords.x, coords.y).SetIsWalkable(value);
    }

    public void SetTestPoint(Vector2Int coords)
    {
        StartPoint = coords;
    }

}
