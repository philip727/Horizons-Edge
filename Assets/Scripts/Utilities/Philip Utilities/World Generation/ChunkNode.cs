using Philip.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Philip.WorldGeneration
{
    public class ChunkNode : Node<ChunkNode>
    {
        public GameObject ChunkGameObject { private set; get; }
        public Tilemap WalkableTilemap { private set; get; }
        public Tilemap ColliderTilemap { private set; get; }

        public bool IsVisible
        {
            get
            {
                return ChunkGameObject.activeSelf;
            }
        }

        public ChunkNode(Grid<ChunkNode> grid, int x, int y) : base(grid, x, y)
        {

        }

        public void SetupChunk(GameObject chunkObject)
        {
            ChunkGameObject = chunkObject;
            WalkableTilemap = chunkObject.transform.GetChild(0).GetComponent<Tilemap>();
            ColliderTilemap = chunkObject.transform.GetChild(1).GetComponent<Tilemap>();
            SetVisible(false);
        }

        public void SetVisible(bool value)
        {
            ChunkGameObject.SetActive(value);
        }
    }
}
