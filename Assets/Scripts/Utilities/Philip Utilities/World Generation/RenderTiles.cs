using UnityEngine;
using Philip.Grid;
using static Philip.Tilemaps.RuleTileObject;
using UnityEngine.Tilemaps;

namespace Philip.WorldGeneration
{
    public class RenderTiles : MonoBehaviour
    {
        [SerializeField] private WorldGenerationHandler _worldGenerationHandler;

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
            if(leftNode != null && rightNode != null && belowNode != null)
            {
                if(!leftNode.IsWater && !rightNode.IsWater && !belowNode.IsWater && (aboveNode == null || aboveNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CentreUp);
                }
            }

            // Left Edge Tile
            if(rightNode != null && aboveNode != null && belowNode != null)
            {
                if(!rightNode.IsWater && !aboveNode.IsWater && !belowNode.IsWater && (leftNode == null || leftNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CentreLeft);
                }
            }

            // Right Edge Tile
            if(leftNode != null && aboveNode != null && belowNode != null)
            {
                if (!leftNode.IsWater && !aboveNode.IsWater && !belowNode.IsWater && (rightNode == null || rightNode.IsWater)) 
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CentreRight);
                }
            }

            // Bottom Edge Tile
            if(leftNode != null && rightNode != null && aboveNode != null)
            {
                if(!leftNode.IsWater && !aboveNode.IsWater && !rightNode.IsWater && (belowNode == null || belowNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.CentreDown);
                }
            }

            // Top Left Corner Tile
            if(rightNode != null && belowNode != null && bottomRightNode != null)
            {
                if (!rightNode.IsWater && !belowNode.IsWater && !bottomRightNode.IsWater 
                    && (leftNode == null || leftNode.IsWater) && (aboveNode == null || aboveNode.IsWater)
                    && (topLeftNode == null || topLeftNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.TopLeft);
                }
            }

            // Top Right Corner Tile
            if(leftNode != null && belowNode != null && bottomLeftNode != null)
            {
                if (!leftNode.IsWater && !belowNode.IsWater && !bottomLeftNode.IsWater
                    && (rightNode == null || rightNode.IsWater) && (aboveNode == null || aboveNode.IsWater)
                    && (topRightNode == null || topRightNode.IsWater))
                {
                    return _worldGenerationHandler.WorldGenerationSettings.GetBiomeObject(worldNode.Biome).TileRules.GetTileFromRule(RuleTilePositions.TopRight);
                }
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

                    if(worldNode.IsWater)
                    {
                        continue;
                    }

                    Tile determinedTile = DetermineTile(worldNode);
                    if(determinedTile != null)
                    {
                        Vector3 worldPosition = WorldGenerationHandler.s_worldData.WorldGrid.GetWorldPosition(x, y);
                        ChunkNode chunkNode = WorldGenerationHandler.s_worldData.ChunkGrid.GetGridObject(worldPosition);

                        // Makes sure the tile is in the right position of its current chunk tilemap
                        Vector3Int tilemapCoordinate = new Vector3Int(x - _worldGenerationHandler.WorldGenerationSettings.ChunkSize * chunkNode.X,
                                                                      y - _worldGenerationHandler.WorldGenerationSettings.ChunkSize * chunkNode.Y);

                        chunkNode.WalkableTilemap.SetTile(tilemapCoordinate, determinedTile);
                    }
                }
            }

        }
    }
}
