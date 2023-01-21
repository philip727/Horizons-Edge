using Philip.Building;
using Philip.Grid;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Philip.WorldGeneration
{
    [System.Serializable]
    public class WorldNode : Node<WorldNode>
    {

        public bool IsWater { private set; get; }
        public BiomeTypes Biome { private set; get; }

        public WorldNode(Grid<WorldNode> grid, int x, int y) : base(grid, x, y)
        {

        }

        public void SetIsWater(bool value)
        {
            IsWater = value;
        }

        public void SetBiome(BiomeTypes value)
        {
            Biome = value;
        }
    }
}