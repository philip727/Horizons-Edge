using UnityEngine;
using Philip.Grid;
using static Philip.Tilemaps.RuleTileObject;
using UnityEngine.Tilemaps;
using RuleTile = Philip.Tilemaps.RuleTileObject.RuleTile;

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

        private RuleTile DetermineTile(int x, int y)
        {
            WorldNode worldNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(x, y);
            return DetermineTile(worldNode);
        }

        private RuleTile DetermineTile(WorldNode worldNode)
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
                        chunkNode.WaterTilemap.SetTile(tilemapCoordinate, _waterTile);
                        continue;
                    }

                    RuleTile determinedTile = DetermineTile(worldNode);
                    if(determinedTile != null)
                    {
                        switch (determinedTile.SpriteType)
                        {
                            case SpriteType.Default:
                                chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, determinedTile.tile);
                                break;
                            case SpriteType.Animated:
                                chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, determinedTile.animatedTile);
                                break;
                        }

                        foreach (RuleNodes colliderRule in determinedTile.RequiredNothingNodes)
                        {
                            Vector3Int tilemapOffset = tilemapCoordinate + determinedTile.ConvertRuleToOffset(colliderRule);

                            chunkNode.ColliderTilemap.SetTile(tilemapOffset, _waterTile);
                        }
                    }
                }
            }

        }
    }
}
