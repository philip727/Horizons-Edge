using Philip.AI;
using Philip.Building;
using Philip.Utilities.Extras;
using Philip.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridHandler : MonoBehaviour
{
    [Header("Debug")]public Grid<DebugNode> debugVisual;
    
    [SerializeField, Header("Setup")] private int _width = 20;
    [SerializeField] private int _height = 20;
    [SerializeField] private float _cellSize = 1f;
    [SerializeField] private Vector2 _originPosition = default;
    [SerializeField] private bool _debug = false;

    [field: SerializeField] public List<GridManager> GridManagers { private set; get; } = new List<GridManager>();

    private void Awake()
    {
        BakeGrids();
    }

    // Bakes and sets up the grids
    public void BakeGrids()
    {
        ClearTextMeshes();
        foreach (GridManager gridManager in GridManagers)
        {
            gridManager.BakeGrid(_width, _height, _cellSize, _originPosition);
        }
        debugVisual = new Grid<DebugNode>(_width, _height, _cellSize, (Grid<DebugNode> g, int x, int y) => new DebugNode(g, x, y), debug: _debug, originPosition: _originPosition);
    }

    public void ClearTextMeshes()
    {
        List<GameObject> textMeshes = debugVisual.GetGeneratedGameObjects();
        if (textMeshes.Count > 0)
        {
            foreach (GameObject textMesh in textMeshes)
            {
                DestroyImmediate(textMesh);
            }
            textMeshes.Clear();
        }
    }

    public void ClearTextMeshCache()
    {
        List<GameObject> textMeshes = debugVisual.GetGeneratedGameObjects();
        textMeshes.Clear();
    }

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Grid<PathNode> grid = Pathfinding.GetGrid();
        //    Vector2Int coords = grid.GetCoordinate(Camera.main.GetMouseWorldPosition());

        //    if (!grid.IsValidCoordinate(coords.x, coords.y)) return;

        //    List<PathNode> path = Pathfinding.FindPath(StartPoint.x, StartPoint.y, coords.x, coords.y);

        //    if (path != null)
        //    {
        //        for (int i = 0; i < path.Count - 1; i++)
        //        {
        //            Debug.DrawLine(grid.GetWorldPosition(path[i].X, path[i].Y) + new Vector3((grid.CellSize/2f), (grid.CellSize / 2f), 0f), grid.GetWorldPosition(path[i+1].X, path[i+1].Y) + new Vector3((grid.CellSize / 2f), (grid.CellSize / 2f), 0f), Color.red, 3f);
        //        }
        //    }
        //}
    }
}

