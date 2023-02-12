using UnityEngine;
using Philip.Grid;
using static Philip.Tilemaps.RuleTileObject;
using UnityEngine.Tilemaps;
using RuleTile = Philip.Tilemaps.RuleTileObject.RuleTile;
using Philip.Utilities.Math;

namespace Philip.WorldGeneration
{
    public class RenderTiles : MonoBehaviour
    {
        [SerializeField] private WorldGenerationHandler _worldGenerationHandler;
        [SerializeField] private WorldGenerationSettings _worldGenerationSettings;

        [SerializeField] private Tile _waterTile;
        [SerializeField] private Tile _nonDeterminedTile;

        private RuleTile DetermineTile(ChunkData chunkData, int x, int y)
        {
            return GetBestBiome(chunkData, x, y).TileRules.GetTileFromRule(chunkData, x, y);
        }

        private BiomeObject GetBestBiome(ChunkData chunkData, int x, int y)
        {
            Vector2Int coords = chunkData.Coordinates + new Vector2Int(x, y);
            BiomeObject bestBiomeObject = null;
            float bestBiomeDistance = 99999f;
            float precipitationHeight = Noise.GenerateHeight(coords.x, coords.y,
                _worldGenerationHandler.Seed,
                _worldGenerationHandler.PrecipitationSettings.Offset,
                _worldGenerationHandler.PrecipitationSettings.Octaves,
                _worldGenerationHandler.PrecipitationSettings.Persistance,
                _worldGenerationHandler.PrecipitationSettings.Lacunarity,
                _worldGenerationHandler.PrecipitationSettings.NoiseScale);

            float temperatureHeight = Noise.GenerateHeight(coords.x, coords.y,
                _worldGenerationHandler.Seed,
                _worldGenerationHandler.TemperatureSettings.Offset,
                _worldGenerationHandler.TemperatureSettings.Octaves,
                _worldGenerationHandler.TemperatureSettings.Persistance,
                _worldGenerationHandler.TemperatureSettings.Lacunarity,
                _worldGenerationHandler.TemperatureSettings.NoiseScale);

            for (int i = 0; i < _worldGenerationSettings.BiomeObjects.Length; i++)
            {
                BiomeObject currentBiomeObject = _worldGenerationSettings.BiomeObjects[i];

                float currentBiomeDistance = PMath.EuclidianDistance(precipitationHeight, temperatureHeight, currentBiomeObject.Precipitation, currentBiomeObject.Temperature);
                
                if (bestBiomeDistance > currentBiomeDistance)
                {
                    bestBiomeDistance = currentBiomeDistance;
                    bestBiomeObject = currentBiomeObject;
                }
            }

            return bestBiomeObject;
        }

        // Adds the required colliders
        private void CreateCollidersPerTile(RuleTile determinedTile, ChunkData chunkData, int x, int y)
        {
            // Adds colliders to the nothing nodes
            foreach (RuleNodes nothingRule in determinedTile.RequiredNothingNodes)
            {
                Vector3Int tilemapOffset =  new Vector3Int(x, y, 0) + determinedTile.ConvertRuleToOffset(nothingRule);

                chunkData.ColliderTilemap.SetTile(tilemapOffset, _waterTile);
            }

            // Adds colliders to the extra ones in the determined tile
            foreach (RuleNodes colliderRule in determinedTile.AddColliders)
            {
                Vector3Int tilemapOffset = new Vector3Int(x, y, 0) + determinedTile.ConvertRuleToOffset(colliderRule);

                chunkData.ColliderTilemap.SetTile(tilemapOffset, _waterTile);
            }
        }

        // Renders all the tiles
        private void RenderTile(RuleTile determinedTile, ChunkData chunkData, int x, int y)
        {
            if (determinedTile != null)
            {
                // Checks if its a static or animated tile
                switch (determinedTile.SpriteType)
                {
                    case SpriteType.Default:
                        chunkData.WalkableTilemap.SetTile(new Vector3Int(x, y, 0), determinedTile.tile);
                        break;
                    case SpriteType.Animated:
                        chunkData.WalkableTilemap.SetTile(new Vector3Int(x, y, 0), determinedTile.animatedTile);
                        break;
                }

                // Creates the extra colliders
                CreateCollidersPerTile(determinedTile, chunkData, x, y);
            }
            else
            {
                chunkData.WalkableTilemap.SetTile(new Vector3Int(x, y, 0), _nonDeterminedTile);
            }
        }

        // Sets up all the tiles
        public void SetupTile(ChunkData chunkData, int x, int y)
        {
            // Checks if its water
            if (_worldGenerationHandler.IsCoordinateWater(chunkData.Coordinates + new Vector2Int(x, y)))
            {
                chunkData.WaterTilemap.SetTile(new Vector3Int(x, y), _waterTile);
                return;
            }

            // Gets the right tile
            RuleTile determinedTile = DetermineTile(chunkData, x, y);
            RenderTile(determinedTile, chunkData, x, y);
        }
    }
}
