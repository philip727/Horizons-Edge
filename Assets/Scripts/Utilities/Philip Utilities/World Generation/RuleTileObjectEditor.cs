using Philip.Tilemaps;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RuleTileObject))]
public class RuleTileObjectEditor : Editor
{
    RuleTileObject _ruleTileObject;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        _ruleTileObject = (RuleTileObject)target;

        foreach (RuleTileObject.RuleTile ruleTile in _ruleTileObject.RuleTiles)
        {
            if(ruleTile.tile == null)
            {
                continue;
            }

            Texture2D texture = AssetPreview.GetAssetPreview(ruleTile.tile.sprite);

            GUILayout.Label(ruleTile.tile.name, GUILayout.Height(64), GUILayout.Width(64));

            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        }
    }
}
