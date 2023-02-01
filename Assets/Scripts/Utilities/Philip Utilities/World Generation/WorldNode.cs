using Philip.Grid;
using static Philip.Tilemaps.RuleTileObject;
using System.Collections.Generic;
using UnityEngine;
using Philip.Tilemaps;

namespace Philip.WorldGeneration
{
    [System.Serializable]
    public class WorldNode : Node<WorldNode>
    {
        public Biome Biome { private set; get; }
        public bool IsWater { private set; get; }

        public enum Direction
        {
            UpLeft,
            Up,
            UpRight,
            Left,
            Middle,
            Right,
            BottomLeft,
            Bottom,
            BottomRight,
        }

        public static Dictionary<Direction, Vector2Int> DirectionByOffset { get; } = new Dictionary<Direction, Vector2Int>()
        {
            { Direction.UpLeft,     new Vector2Int(-1, 1) },
            { Direction.Up,         new Vector2Int(0, 1) },
            { Direction.UpRight,    new Vector2Int(1,1) },
            { Direction.Left,       new Vector2Int(-1, 0) },
            { Direction.Middle,     new Vector2Int(0,0) },
            { Direction.Right,      new Vector2Int(1, 0) },
            { Direction.BottomLeft, new Vector2Int(-1, -1) },
            { Direction.Bottom,     new Vector2Int(0, -1) },
            { Direction.BottomRight,new Vector2Int(1, -1) },
        };

        public WorldNode(Grid<WorldNode> grid, int x, int y) : base(grid, x, y)
        {

        }

        public void SetIsWater(bool value)
        {
            IsWater = value;
        }

        public void SetBiome(Biome biome)
        {
            Biome = biome;
        }
    }
}