using Philip.Building;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlacementGridManager : GridManager
{
    public Placement<IBuildable> Placement { private set; get; }
    [field: SerializeField, Header("Placement setup")] public List<Vector2Int> CoordinatesCantBuildOn { private set; get; } = new List<Vector2Int>();

    public override void BakeGrid(int width, int height, float cellSize, Vector2 origin)
    {
        Placement = new Placement<IBuildable>(width, height, cellSize, originPosition: origin);
        base.BakeGrid(width, height, cellSize, origin);
    }
    public override void SetupTilesInGrid()
    {
        for (int i = 0; i < CoordinatesCantBuildOn.Count; i++)
        {
            Vector2Int coords = CoordinatesCantBuildOn[i];
            SetTileBuildable(coords, false);
        }
    }
    private void SetTileBuildable(Vector2Int coords, bool value)
    {
        Placement.GetGrid().GetGridObject(coords.x, coords.y).SetIsBuildable(value);
    }

    public void AddNonBuildableTiles(Vector2Int coords)
    {
        if (!CoordinatesCantBuildOn.Contains(coords))
        {
            CoordinatesCantBuildOn.Add(coords);
            SetTileBuildable(coords, false);
        }
    }

    public void RemoveNonBuildableTiles(Vector2Int coords)
    {
        if (CoordinatesCantBuildOn.Contains(coords))
        {
            CoordinatesCantBuildOn.Remove(coords);
            SetTileBuildable(coords, true);
        }
    }

}
