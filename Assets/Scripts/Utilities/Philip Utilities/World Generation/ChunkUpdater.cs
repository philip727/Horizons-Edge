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
        private List<ChunkNode> loadedChunksLastUpdate = new List<ChunkNode>();

        public void Update()
        {
            if (!WorldGenerationHandler.s_worldData.Initialized) return;
            s_viewerPosition = new Vector2(Viewer.position.x, Viewer.position.y);
            UpdateVisisbleChunks();
        }

        private void UpdateVisisbleChunks()
        {
            Vector2Int currentCoords = WorldGenerationHandler.s_worldData.ChunkGrid.GetCoordinate(Viewer.position);

            for (int i = 0; i < loadedChunksLastUpdate.Count; i++)
            {
                loadedChunksLastUpdate[i].SetVisible(false);
            }

            loadedChunksLastUpdate.Clear();

            for (int yOffset = -MAX_VIEW_DISTANCE; yOffset <= MAX_VIEW_DISTANCE; yOffset++)
            {
                for (int xOffset = -MAX_VIEW_DISTANCE; xOffset <= MAX_VIEW_DISTANCE; xOffset++)
                {
                    Vector2Int viewedChunkCoord = currentCoords + new Vector2Int(xOffset, yOffset);

                    if (!WorldGenerationHandler.s_worldData.IsValidChunk(viewedChunkCoord)) continue;
                    ChunkNode chunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(viewedChunkCoord);

                    if(!chunkNode.IsVisible)
                    {
                        chunkNode.SetVisible(true);
                        loadedChunksLastUpdate.Add(chunkNode);
                    } 
                }
            }
        }
    }
}
