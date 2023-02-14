using System.Collections.Generic;
using System.Linq;
using Unity.Burst;
using UnityEngine;

namespace Philip.WorldGeneration
{
    public class ChunkUpdater : MonoBehaviour
    { 
        // Viewer
        public const int MAX_VIEW_DISTANCE = 3;
        [field: SerializeField] public Transform Viewer { private set; get; }
        public static Vector2 s_viewerPosition;

        [SerializeField] private WorldGenerationHandler _worldGenerationHandler;
        [SerializeField] private WorldGenerationSettings _worldGenerationSettings;
        [SerializeField] private RenderTiles _tileRenderer;
        [field: SerializeField, Header("Chunk Setup")] public GameObject ChunkPrefab { private set; get; }


        // Chunks
        //private readonly List<ChunkNode> _loadedChunksLastUpdate = new List<ChunkNode>();
        //private readonly List<ChunkNode> _chunksAlreadyLoadedOnFrame = new List<ChunkNode>();

        public readonly Dictionary<Vector2Int, ChunkData> _chunksThatHaveBeenCreated = new Dictionary<Vector2Int, ChunkData>();
        public readonly Dictionary<Vector2Int, ChunkData> _chunksCurrentlyRendered = new Dictionary<Vector2Int, ChunkData>();
        public readonly Dictionary<Vector2Int, ChunkData> _chunksReceivedToRenderThisFrame = new Dictionary<Vector2Int, ChunkData>();

        private void Start()
        {

        }

        private void Update()
        {
            s_viewerPosition = new Vector2(Viewer.position.x, Viewer.position.y);
            UpdateChunks();
        }

        private void UpdateChunks()
        {
            int chunkX = Mathf.FloorToInt(s_viewerPosition.x / _worldGenerationSettings.ChunkSize);
            int chunkY = Mathf.FloorToInt(s_viewerPosition.y / _worldGenerationSettings.ChunkSize);
            Vector2Int bottomLeftOfCurrentChunk = new Vector2Int(chunkX * _worldGenerationSettings.ChunkSize, chunkY * _worldGenerationSettings.ChunkSize);

            // Clear the chunks rendered from last frame as we don't want to check them in this dict
            _chunksReceivedToRenderThisFrame.Clear();

            for (int x = -MAX_VIEW_DISTANCE; x < MAX_VIEW_DISTANCE; x++)
            {
                for (int y = -MAX_VIEW_DISTANCE; y < MAX_VIEW_DISTANCE; y++)
                {
                    // The current chunk coordinates
                    Vector2Int bottomLeftOfChunkInViewDist = new Vector2Int(bottomLeftOfCurrentChunk.x + (x * _worldGenerationSettings.ChunkSize),
                                                                            bottomLeftOfCurrentChunk.y + (y * _worldGenerationSettings.ChunkSize));

                    ChunkData chunkData;
                    if (!_chunksThatHaveBeenCreated.ContainsKey(bottomLeftOfChunkInViewDist))
                    {
                        // At this point the chunk is in the view distance but has never been created
                        chunkData = _worldGenerationHandler.RequestChunkData(bottomLeftOfChunkInViewDist.x, bottomLeftOfChunkInViewDist.y);
                        CreateChunkFromData(chunkData);
                        _chunksThatHaveBeenCreated.Add(bottomLeftOfChunkInViewDist, chunkData);
                    }
                    else
                    {
                        // The chunk has been created before and is now in the view distance
                        _chunksThatHaveBeenCreated.TryGetValue(bottomLeftOfChunkInViewDist, out chunkData);

                        // If it's not visible, then we should render it
                        if(!chunkData.IsVisible)
                        {
                            chunkData.SetVisible(true);
                        }


                    }

                    if(!_chunksReceivedToRenderThisFrame.ContainsKey(bottomLeftOfChunkInViewDist))
                    {
                        // If it's in the view distance then it must have been requested to render
                        _chunksReceivedToRenderThisFrame.Add(bottomLeftOfChunkInViewDist, chunkData);
                    }

                    if(!_chunksCurrentlyRendered.ContainsKey(bottomLeftOfChunkInViewDist))
                    {
                        // All rendered chunks will be added to this dict since we need to keep track of what needs to be unrendered
                        _chunksCurrentlyRendered.Add(bottomLeftOfChunkInViewDist, chunkData);
                    }
                }
            }

            // If it's no longer in the view distance but is still rendered, then unrender it
            for (int i = _chunksCurrentlyRendered.Count - 1; i >= 0; i--)
            {
                KeyValuePair<Vector2Int, ChunkData> chunkByCoordinate = _chunksCurrentlyRendered.ElementAt(i);
                if (!_chunksReceivedToRenderThisFrame.ContainsKey(chunkByCoordinate.Key))
                {
                    chunkByCoordinate.Value.SetVisible(false);
                    _chunksCurrentlyRendered.Remove(chunkByCoordinate.Key);
                }
            }
        }

        [BurstCompile]
        private ChunkData CreateChunkFromData(ChunkData chunkData)
        {
            GameObject newChunkPrefab = Instantiate(ChunkPrefab, new Vector3(chunkData.Coordinates.x, chunkData.Coordinates.y, 0f), Quaternion.identity, transform);
            chunkData.SetupChunk(newChunkPrefab);
            for (int x = 0; x < _worldGenerationSettings.ChunkSize; x++)
            {
                for (int y = 0; y < _worldGenerationSettings.ChunkSize; y++)
                {
                    _tileRenderer.SetupTile(chunkData, x, y);
                }
            }

            return chunkData;
        }



    }
}
