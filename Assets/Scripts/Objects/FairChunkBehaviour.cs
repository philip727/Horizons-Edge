using Philip.Building;
using Philip.WorldGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairChunkBehaviour : StructureObject
{
    public bool ObjectIsRunning { private set; get; } = true;
    protected override void Update()
    {
        base.Update();
        ChunkNode currentChunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(transform.position);

        ObjectIsRunning = currentChunkNode.IsVisible;

    }
}
