using Philip.Building;
using Philip.WorldGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StrictChunkBehaviour : StructureObject
{
    public bool ObjectIsRunning { private set; get; } = true;

    protected override void Update()
    {
        base.Update();
        ChunkNode currentChunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(transform.position);
        ChunkNode viewedChunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(ChunkUpdater.s_viewerPosition);

        ObjectIsRunning = currentChunkNode.Coordinates == viewedChunkNode.Coordinates;

    }
}
