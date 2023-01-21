using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridManager : MonoBehaviour
{
    public virtual void BakeGrid(int width, int height, float cellSize, Vector2 origin)
    {
        SetupTilesInGrid();
    }

    public abstract void SetupTilesInGrid();
}
