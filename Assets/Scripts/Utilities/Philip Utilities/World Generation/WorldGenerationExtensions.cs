using System.Collections.Generic;
using UnityEngine;

namespace Philip.WorldGeneration
{
    public static class WorldGenerationExtensions
    {
        public static bool HasWaterNeighbours(this WorldNode worldNode)
        {
            foreach (KeyValuePair<WorldNode.Direction, Vector2Int> coordinate in WorldNode.DirectionByOffset)
            {
                if (!worldNode.Grid.IsValidCoordinate(worldNode.Coordinates + coordinate.Value))
                {
                    continue;
                }

                if (worldNode.GetNeighbour(coordinate.Value).IsWater)
                {
                    return true;
                }
            }

            return false;
        }
    }
}

