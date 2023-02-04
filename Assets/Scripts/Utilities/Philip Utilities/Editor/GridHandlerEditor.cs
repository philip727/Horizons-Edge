using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridHandler))]
public class GridHandlerEditor : Editor
{
    GridHandler _myTarget;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _myTarget = (GridHandler)target;


        if(GUILayout.Button("Bake Grids"))
        {
            _myTarget.BakeGrids();
        }

        if(GUILayout.Button("Clear Cache"))
        {
            _myTarget.ClearTextMeshCache();
        }
    }
}
