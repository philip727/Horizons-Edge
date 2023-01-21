using Philip.Grid;

namespace Philip.AI
{
    [System.Serializable]
    public class PathNode : Node<PathNode>
    {
        public int WalkingCost { private set; get; }
        public int HeuristicCost { private set; get; }
        public int FinalCost { private set; get; }
        public bool IsWalkable { private set; get; } = true;

        public PathNode PreviousNode { private set; get; }

        public PathNode(Grid<PathNode> grid, int x, int y) : base(grid, x, y)
        {

        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public void SetWalkingCost(int value)
        {
            WalkingCost = value;
        }

        public void SetHeuristicCost(int value)
        {
            HeuristicCost = value;
        }

        public void SetFinalCost(int value)
        {
            FinalCost = value;
        }

        public void UpdateLastNode(PathNode node)
        {
            PreviousNode = node;
        }
        
        public void CalculateFinalCost()
        {
            FinalCost = WalkingCost + HeuristicCost;
        }

        public void SetIsWalkable(bool value)
        {
            IsWalkable = value;
        }

    }
}
