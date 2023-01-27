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

        private Tile DetermineTile(int x, int y)
        {
            WorldNode worldNode = WorldGenerationHandler.s_worldData.WorldGrid.GetGridObject(x, y);
            return DetermineTile(worldNode);
        }

        private Tile DetermineTile(WorldNode worldNode)
        {
            WorldNode leftNode = worldNode.GetNeighbour(Node<WorldNode>.NeighbourDirections.Left);
            WorldNode aboveNode = worldNode.GetNeighbour(Node<WorldNode>.NeighbourDirections.Up);
            WorldNode rightNode = worldNode.GetNeighbour(Node<WorldNode>.NeighbourDirections.Right);
            WorldNode belowNode = worldNode.GetNeighbour(Node<WorldNode>.NeighbourDirections.Down);
            WorldNode topLeftNode = worldNode.GetNeighbour(Node<WorldNode>.NeighbourDirections.UpLeft);
            WorldNode topRightNode = worldNode.GetNeighbour(Node<WorldNode>.NeighbourDirections.UpRight);
            WorldNode bottomLeftNode = worldNode.GetNeighbour(Node<WorldNode>.NeighbourDirections.DownLeft);
            WorldNode bottomRightNode = worldNode.GetNeighbour(Node<WorldNode>.NeighbourDirections.DownRight);

            //Debug.Log($"{leftNode}, {aboveNode}, {rightNode}, {belowNode}, {topLeftNode}, {topRightNode}, {bottomLeftNode}, {bottomRightNode}");

            // Centre Tile
            if(leftNode != null && aboveNode != null && rightNode != null 
                && belowNode != null && topLeftNode != null && topRightNode != null
                && bottomLeftNode != null && bottomRightNode != null)
            {
                if(!leftNode.IsWater && !aboveNode.IsWater && !rightNode.IsWater && !belowNode.IsWater
                    && !topLeftNode.IsWater && !topRightNode.IsWater && !bottomLeftNode.IsWater !&& !bottomRightNode.IsWater)
                {
                    //Debug.Log("Centre Tile");
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.Centre);
                }
            }

            // Top Edge Tile
            if(leftNode != null && rightNode != null && belowNode != null && bottomLeftNode != null && bottomRightNode != null)
            {
                if(!leftNode.IsWater && !rightNode.IsWater && !belowNode.IsWater 
                    && !bottomLeftNode.IsWater && !bottomRightNode.IsWater && (aboveNode == null || aboveNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CentreUp);
                }
            }

            // Left Edge Tile
            if(rightNode != null && aboveNode != null && belowNode != null && topRightNode != null && bottomRightNode != null)
            {
                if(!rightNode.IsWater && !aboveNode.IsWater && !belowNode.IsWater && !topRightNode.IsWater && !bottomRightNode.IsWater
                    && (leftNode == null || leftNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CentreLeft);
                }
            }

            // Right Edge Tile
            if(leftNode != null && aboveNode != null && belowNode != null && topLeftNode != null && bottomLeftNode != null)
            {
                if (!leftNode.IsWater && !aboveNode.IsWater && !belowNode.IsWater && !topLeftNode.IsWater && !bottomLeftNode.IsWater 
                    && (rightNode == null || rightNode.IsWater)) 
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CentreRight);
                }
            }

            // Bottom Edge Tile
            if(leftNode != null && rightNode != null && aboveNode != null && topLeftNode != null && topRightNode != null)
            {
                if(!leftNode.IsWater && !aboveNode.IsWater && !rightNode.IsWater 
                    && !topLeftNode.IsWater && !topRightNode.IsWater && (belowNode == null || belowNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CentreDown);
                }
            }

            // Top Left Corner Tile
            if(rightNode != null && belowNode != null && bottomRightNode != null)
            {
                if (!rightNode.IsWater && !belowNode.IsWater && !bottomRightNode.IsWater 
                    && (leftNode == null || leftNode.IsWater) && (aboveNode == null || aboveNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.TopLeft);
                }
            }

            // Top Right Corner Tile
            if(leftNode != null && belowNode != null && bottomLeftNode != null)
            {
                if (!leftNode.IsWater && !belowNode.IsWater && !bottomLeftNode.IsWater
                    && (rightNode == null || rightNode.IsWater) && (aboveNode == null || aboveNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.TopRight);
                }
            }

            // Bottom Left Corner Tile
            if(rightNode != null && aboveNode != null && topRightNode != null)
            {
                if (!rightNode.IsWater && !aboveNode.IsWater && !topRightNode.IsWater && (leftNode == null || leftNode.IsWater)
                    && (belowNode == null || belowNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.BottomLeft);
                }
            }

            // Bottom Right Corner Tile
            if(leftNode != null && aboveNode != null && topLeftNode != null)
            {
                if (!leftNode.IsWater && !aboveNode.IsWater && !topLeftNode.IsWater && (rightNode == null || rightNode.IsWater)
                    && (belowNode == null || belowNode.IsWater)) 
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.BottomRight);
                }
            }

            // Corner Leading Up Right
            if(rightNode != null && belowNode != null && aboveNode != null && leftNode != null && bottomLeftNode != null && topRightNode != null)
            {
                if(!rightNode.IsWater && !belowNode.IsWater && !aboveNode.IsWater && !leftNode.IsWater && !bottomLeftNode.IsWater
                    && !topRightNode.IsWater && (bottomRightNode == null || bottomRightNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CornerNothingRightBottom);
                }
            }

            // Corner Leading Up Left
            if (rightNode != null && belowNode != null && aboveNode != null && leftNode != null && bottomRightNode != null && topLeftNode != null)
            {
                if (!rightNode.IsWater && !belowNode.IsWater && !aboveNode.IsWater && !leftNode.IsWater && !bottomRightNode.IsWater
                    && !topLeftNode.IsWater && (bottomLeftNode == null || bottomLeftNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CornerNothingLeftBottom);
                }
            }

            // Corner Leading Down Left
            if (rightNode != null && belowNode != null && aboveNode != null && leftNode != null && topRightNode != null && bottomLeftNode != null)
            {
                if (!rightNode.IsWater && !belowNode.IsWater && !aboveNode.IsWater && !leftNode.IsWater && !topRightNode.IsWater
                    && !bottomLeftNode.IsWater && (topLeftNode == null || topLeftNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CornerNothingLeftTop);
                }
            }

            // Corner Leading Down Right
            if (rightNode != null && belowNode != null && aboveNode != null && leftNode != null && topLeftNode != null && bottomRightNode != null)
            {
                if (!rightNode.IsWater && !belowNode.IsWater && !aboveNode.IsWater && !leftNode.IsWater && !topLeftNode.IsWater
                    && !bottomRightNode.IsWater && (topRightNode == null || topRightNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CornerNothingRightTop);
                }
            }

            // Single Block Up Corner
            if (rightNode != null && leftNode != null && aboveNode != null && belowNode != null && bottomLeftNode != null && bottomRightNode != null)
            {
                if(!aboveNode.IsWater && !rightNode.IsWater && !leftNode.IsWater && !belowNode.IsWater && !bottomLeftNode.IsWater && !bottomRightNode.IsWater
                    && (topLeftNode == null || topLeftNode.IsWater) && (topRightNode == null || topRightNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.GoingUpToSingleTile);
                }
            }

            // Single Block Down Corner
            if(rightNode != null && leftNode != null && aboveNode != null && belowNode != null && topRightNode != null && topLeftNode != null)
            {
                if (!rightNode.IsWater && !leftNode.IsWater && !aboveNode.IsWater && !belowNode.IsWater && !topRightNode.IsWater && !topLeftNode.IsWater
                    && (bottomLeftNode == null || bottomLeftNode.IsWater) && (bottomRightNode == null || bottomRightNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.GoingDownToSingleTile);
                }
            }

            // Single Block Left Corner
            //if (aboveNode != null && belowNode != null && leftNode != null)
            //{
            //    if (!aboveNode.IsWater && !belowNode.IsWater && !leftNode.IsWater && (topLeftNode == null || topLeftNode.IsWater)
            //        && (bottomLeftNode == null || bottomLeftNode.IsWater))
            //    {

            //    }
            //}

            // Single Width Walk Ways


            // Alone Tiles Each Direction
            // Alone Tile Up
            if(bottomLeftNode != null && belowNode != null && bottomRightNode != null)
            {
                if (!bottomLeftNode.IsWater && !belowNode.IsWater && !bottomRightNode.IsWater
                    && (leftNode == null || leftNode.IsWater) && (rightNode == null || rightNode.IsWater) && (aboveNode == null || aboveNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.SingleTileUp);
                }
            }

            if ((leftNode == null || leftNode.IsWater) && (rightNode == null || rightNode.IsWater) 
                && (aboveNode == null || aboveNode.IsWater) && (belowNode == null || belowNode.IsWater))
            {
                return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.TileAlone);
            }

            //Debug.LogError($"<color=#36ffc3>[WORLD GENERATION]</color> could not determine tile");

            return null;
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

                    Tile determinedTile = DetermineTile(worldNode);
                    if(determinedTile != null)
                    {
                        

                        chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, determinedTile);
                    }
                }
            }

        }
    }
}
