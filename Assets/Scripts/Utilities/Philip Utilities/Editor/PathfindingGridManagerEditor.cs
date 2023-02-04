using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PathfindingGridManager))]
public class PathfindingGridManagerEditor : Editor
{
    PathfindingGridManager _myTarget;
    int _pathfindingSelectedTool = 0;
    readonly string[] _pathfindingTools = new string[3]
    {
        "No Tool",
        "Not Walkable",
        "Set Test Start Point",
    };

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _myTarget = (PathfindingGridManager)target;

        GUILayout.Label("\n\nPathfinding Tools");
        _pathfindingSelectedTool = GUILayout.Toolbar(_pathfindingSelectedTool, _pathfindingTools);
    }


    private void OnSceneGUI()
    {
        if (!EditorApplication.isPlaying)
        {
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
            {

                switch (_pathfindingSelectedTool)
                {
                    // Ignore
                    case 1:
                        SetTileIgnoreState();
                        break;
                    case 2:
                        SetTestPoint();
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void SetTileIgnoreState()
    {
        Vector2 screenPoint = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

        Vector2Int coords = _myTarget.Pathfinding.GetGrid().GetCoordinate(screenPoint);
        if (_myTarget.Pathfinding.GetGrid().IsValidCoordinate(coords.x, coords.y) && _myTarget.Pathfinding.GetGrid().IsValidCoordinate(coords.x, coords.y) && !_myTarget.IsStartPoint(coords))
        {
            //Debug.Log($"{myTarget.Pathfinding.IsNodeWalkable(coords)}");
            if (_myTarget.Pathfinding.IsNodeWalkable(coords))
            {
                _myTarget.AddIgnoredTiles(coords);
            }
            else
            {
                _myTarget.RemoveIgnoredTiles(coords);
            }

        }
    }

    private void SetTestPoint()
    {
        Vector2 screenPoint = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

        Vector2Int coords = _myTarget.Pathfinding.GetGrid().GetCoordinate(screenPoint);
        if (_myTarget.Pathfinding.GetGrid().IsValidCoordinate(coords.x, coords.y) && !_myTarget.CoordinatesToIgnore.Contains(coords))
            _myTarget.SetTestPoint(coords);
    }

}