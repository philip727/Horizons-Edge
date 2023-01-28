using UnityEngine;
using Philip.Grid;
using static Philip.Tilemaps.RuleTileObject;
using UnityEngine.Tilemaps;

namespace Philip.WorldGeneration
{
    public class RenderTiles : MonoBehaviour
    {
        [SerializeField] private WorldGenerationHandler _worldGenerationHandler;

        [SerializeField] private Tile _waterTile;

        public void Awake()
        {
            _worldGenerationHandler.onWorldGenerationFinished += OnWorldGenerationFinished;
        }

        private void OnWorldGenerationFinished()
        {
            DisplayTiles();
        }

        private TileBase DetermineTile(int x, int y)
        {
            WorldNode worldNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(x, y);
            return DetermineTile(worldNode);
        }

        private TileBase DetermineTile(WorldNode worldNode)
        {
            return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(worldNode);
        }

        private void DisplayTiles()
        {
            WorldGenerationSettings worldSettings = _worldGenerationHandler.WorldGenerationSettings;

            for (int y = 0; y < worldSettings.WorldHeight; y++)
            {
                for (int x = 0; x < worldSettings.WorldWidth; x++)
                {
                    // Need to determine tile
                    WorldNode worldNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(x, y);
                    Vector3 worldPosition = WorldGenerationHandler.s_worldData.WorldGrid.GetWorldPosition(x, y);
                    ChunkNode chunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(worldPosition);
                    Vector3Int tilemapCoordinate = new Vector3Int(x - _worldGenerationHandler.WorldGenerationSettings.ChunkSize * chunkNode.X,
                                                                  y - _worldGenerationHandler.WorldGenerationSettings.ChunkSize * chunkNode.Y);
                    if (worldNode.IsWater)
                    {
                        chunkNode.ColliderTilemap.SetTile(tilemapCoordinate, _waterTile);
                        continue;
                    }

                    TileBase determinedTile = DetermineTile(worldNode);
                    if(determinedTile != null)
                    {
                        chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, determinedTile);
                    }
                }
            }

        }
    }
}
