using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlacementGridManager))]
public class PlacementGridManagerEditor : Editor
{
    PlacementGridManager _myTarget;
    int _placementSelectedTool = 0;
    readonly string[] _placementTools = new string[2]
    {
        "No Tool",
        "Not Placeable",
    };

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _myTarget = (PlacementGridManager)target;

        GUILayout.Label("\n\nPlacement Tools");
        _placementSelectedTool = GUILayout.Toolbar(_placementSelectedTool, _placementTools);
  
    }

    private void OnSceneGUI()
    {
        HandleMouse();
    }

    
    bool pressing = true;
    private void HandleMouse()
    {
        pressing = !pressing;
        if (pressing) return;
        Event e = Event.current;
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0) ManageTool();
                break;
            case EventType.MouseDrag:
                if (e.button == 0) ManageTool();
                break;
        }
    }

    private void ManageTool()
    {
        switch (_placementSelectedTool)
        {
            case 1:
                SetPlaceableState();
                break;

        }
    }

    private void SetPlaceableState()
    {
        Debug.Log("Ok");
        Vector2 screenPoint = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;

        Vector2Int coords = _myTarget.Placement.GetGrid().GetCoordinate(screenPoint);

        if (_myTarget.Placement.GetGrid().IsValidCoordinate(coords.x, coords.y) && _myTarget.Placement.GetGrid().IsValidCoordinate(coords.x, coords.y))
        {
            if (_myTarget.Placement.GetGrid().GetGridObject(coords.x, coords.y).CanBuildOn)
            {
                _myTarget.AddNonBuildableTiles(coords);
            }
            else
            {
                _myTarget.RemoveNonBuildableTiles(coords);
            }
        }
    }


}
