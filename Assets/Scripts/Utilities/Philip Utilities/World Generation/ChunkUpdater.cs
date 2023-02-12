using System.Collections.Generic;
using UnityEngine;

namespace Philip.WorldGeneration
{
    public class ChunkUpdater : MonoBehaviour
    { 
        // Viewer
        public const int MAX_VIEW_DISTANCE = 3;
        [field: SerializeField] public Transform Viewer { private set; get; }
        public static Vector2 s_viewerPosition;

        // Chunks
        private readonly List<ChunkNode> _loadedChunksLastUpdate = new List<ChunkNode>();
        private readonly List<ChunkNode> _chunksAlreadyLoadedOnFrame = new List<ChunkNode>();

        private void Start()
        {

        }

        private void Update()
        {
            if (!WorldGenerationHandler.s_worldData.WorldGenerationCompleted) return;
            s_viewerPosition = new Vector2(Viewer.position.x, Viewer.position.y);
            UpdateVisisbleChunks();
        }


        // Updates the chunk visibility
        private void UpdateVisisbleChunks()
        {
            Vector2Int currentCoords = WorldGenerationHandler.s_worldData.ChunkGrid.GetCoordinate(Viewer.position);

            _chunksAlreadyLoadedOnFrame.Clear();

            // Renders the chunks by the render distance in each direction
            for (int yOffset = -MAX_VIEW_DISTANCE; yOffset <= MAX_VIEW_DISTANCE; yOffset++)
            {
                for (int xOffset = -MAX_VIEW_DISTANCE; xOffset <= MAX_VIEW_DISTANCE; xOffset++)
                {
                    Vector2Int viewedChunkCoord = currentCoords + new Vector2Int(xOffset, yOffset);
                    if (!WorldGenerationHandler.s_worldData.IsValidChunk(viewedChunkCoord)) continue;
                    ChunkNode chunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(viewedChunkCoord);


                    if(chunkNode.IsVisible)
                    {
                        // Chunks that are already loaded, that should be kept loaded
                        _chunksAlreadyLoadedOnFrame.Add(chunkNode);
                    }
                    else
                    {
                        // If Chunk isn't loaded and should be
                        chunkNode.SetVisible(true);
                        _loadedChunksLastUpdate.Add(chunkNode);
                        _chunksAlreadyLoadedOnFrame.Add(chunkNode);
                    }
                }
            }

            // Removes previously loaded chunks, have to iterate backwards to prevent error
            for (int i = _loadedChunksLastUpdate.Count - 1; i >= 0; i--)
            {
                ChunkNode chunk = _loadedChunksLastUpdate[i];
                if (!_chunksAlreadyLoadedOnFrame.Contains(chunk))
                {
                    chunk.SetVisible(false);
                    _loadedChunksLastUpdate.Remove(chunk);
                }
            }
        }
    }
}
