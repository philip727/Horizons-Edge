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

        [SerializeField] private WorldGenerationHandler _worldGenerationHandler;
        [SerializeField] private WorldGenerationSettings _worldGenerationSettings;
        [SerializeField] private RenderTiles _tileRenderer;
        [field: SerializeField, Header("Chunk Setup")] public GameObject ChunkPrefab { private set; get; }


        // Chunks
        //private readonly List<ChunkNode> _loadedChunksLastUpdate = new List<ChunkNode>();
        //private readonly List<ChunkNode> _chunksAlreadyLoadedOnFrame = new List<ChunkNode>();

        private readonly Dictionary<Vector2Int, ChunkData> chunksLoaded = new Dictionary<Vector2Int, ChunkData>();

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
            for (int x = -MAX_VIEW_DISTANCE; x < MAX_VIEW_DISTANCE; x++)
            {
                for (int y = -MAX_VIEW_DISTANCE; y < MAX_VIEW_DISTANCE; y++)
                {
                    Vector2Int bottomLeftOfChunkInViewDist = new Vector2Int(bottomLeftOfCurrentChunk.x + (x * _worldGenerationSettings.ChunkSize),
                                                                            bottomLeftOfCurrentChunk.y + (y * _worldGenerationSettings.ChunkSize));
                    ChunkData chunkData = _worldGenerationHandler.RequestChunkData(bottomLeftOfChunkInViewDist.x, bottomLeftOfChunkInViewDist.y);
                    if (chunksLoaded.ContainsKey(bottomLeftOfChunkInViewDist)) continue;
                    CreateChunkFromData(chunkData);
                    chunksLoaded.Add(bottomLeftOfChunkInViewDist, chunkData);
                }

            }
        }

        private void CreateChunkFromData(ChunkData chunkData)
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
        }
    }
}
