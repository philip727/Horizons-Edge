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
        [SerializeField] private Tile _nonDeterminedTile;

        public void Awake()
        {
            _worldGenerationHandler.onWorldGenerationFinished += OnWorldGenerationFinished;
        }

        private void OnWorldGenerationFinished()
        {
            CreateTiles();
        }

        private RuleTile DetermineTile(int x, int y)
        {
            WorldNode worldNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(x, y);
            return DetermineTile(worldNode);
        }

        private RuleTile DetermineTile(Vector2Int coordinates)
        {
            WorldNode worldNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(coordinates.x, coordinates.y);
            return DetermineTile(worldNode);
        }

        private RuleTile DetermineTile(WorldNode worldNode)
        {
            return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(worldNode);
        }

        // Adds the required colliders
        private void CreateCollidersPerTile(RuleTile determinedTile, ChunkNode chunkNode, Vector3Int tilemapCoordinate)
        {
            // Adds colliders to the nothing nodes
            foreach (RuleNodes nothingRule in determinedTile.RequiredNothingNodes)
            {
                Vector3Int tilemapOffset = tilemapCoordinate + determinedTile.ConvertRuleToOffset(nothingRule);

                chunkNode.ColliderTilemap.SetTile(tilemapOffset, _waterTile);
            }

            // Adds colliders to the extra ones in the determined tile
            foreach (RuleNodes colliderRule in determinedTile.AddColliders)
            {
                Vector3Int tilemapOffset = tilemapCoordinate + determinedTile.ConvertRuleToOffset(colliderRule);

                chunkNode.ColliderTilemap.SetTile(tilemapOffset, _waterTile);
            }
        }

        // Renders all the tiles
        private void RenderTile(RuleTile determinedTile, ChunkNode chunkNode, Vector3Int tilemapCoordinate)
        {
            if (determinedTile != null)
            {
                // Checks if its a static or animated tile
                switch (determinedTile.SpriteType)
                {
                    case SpriteType.Default:
                        chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, determinedTile.tile);
                        break;
                    case SpriteType.Animated:
                        chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, determinedTile.animatedTile);
                        break;
                }

                // Creates the extra colliders
                CreateCollidersPerTile(determinedTile, chunkNode, tilemapCoordinate);
            }
            else
            {
                chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, _nonDeterminedTile);
            }
        }

        // Sets up all the tiles
        private void SetupTile(int x, int y)
        {
            // Gets the required objects
            WorldNode worldNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(x, y);
            Vector3 worldPosition = WorldGenerationHandler.s_worldData.WorldGrid.GetWorldPosition(x, y);
            ChunkNode chunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(worldPosition);

            Vector3Int tilemapCoordinate = new Vector3Int(x - _worldGenerationHandler.WorldGenerationSettings.ChunkSize * chunkNode.X,
                                                          y - _worldGenerationHandler.WorldGenerationSettings.ChunkSize * chunkNode.Y);

            // Checks if its water
            if (worldNode.IsWater)
            {
                chunkNode.WaterTilemap.SetTile(tilemapCoordinate, _waterTile);
                return;
            }

            // Gets the right tile
            RuleTile determinedTile = DetermineTile(worldNode);
            RenderTile(determinedTile, chunkNode, tilemapCoordinate);
        }


        // Loops through all the tiles
        private void CreateTiles()
        {
            WorldGenerationSettings worldSettings = _worldGenerationHandler.WorldGenerationSettings;

            for (int y = 0; y < worldSettings.WorldHeight; y++)
            {
                for (int x = 0; x < worldSettings.WorldWidth; x++)
                {
                    SetupTile(x, y);
                }
            }

        }
    }
}
