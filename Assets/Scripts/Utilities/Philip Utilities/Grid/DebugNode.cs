namespace Philip.Grid { 
    public class DebugNode : Node<DebugNode>
    {

        public string Display { private set; get; }
        public DebugNode(Grid<DebugNode> grid, int x, int y) : base(grid, x, y)
        {

        }

        public void SetDisplay(string text)
        {
            Display = text;
        }

        public override string ToString()
        {
            return Display;
        }
    }

}

