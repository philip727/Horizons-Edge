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

        public void SetChunkGameObject(GameObject gameObject)
        {
            ChunkGameObject = gameObject;
        }

        public void SetWalkableTilemap(Tilemap tilemap)
        {
            WalkableTilemap = tilemap;
        }

        public void SetColliderTilemap(Tilemap tilemap)
        {
            ColliderTilemap = tilemap;
        }

        public void SetVisible(bool value)
        {
            ChunkGameObject.SetActive(value);
        }
    }
}
