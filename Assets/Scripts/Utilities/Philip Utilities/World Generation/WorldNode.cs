using Philip.Grid;

namespace Philip.WorldGeneration
{
    [System.Serializable]
    public class WorldNode : Node<WorldNode>
    {
        public Biome Biome { private set; get; }
        public bool IsWater { private set; get; }

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