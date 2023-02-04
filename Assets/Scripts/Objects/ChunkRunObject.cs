using Philip.Building;
using Philip.WorldGeneration;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChunkRunObject : StructureObject
{
    protected bool _objectInViewersChunk = true;

    protected override void Update()
    {
        base.Update();
        ChunkNode currentChunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(transform.position);
        ChunkNode viewedChunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(ChunkUpdater.s_viewerPosition);

        _objectInViewersChunk = currentChunkNode.Coordinates == viewedChunkNode.Coordinates;

        //Debug.Log(_objectInViewersChunk);
    }
}
