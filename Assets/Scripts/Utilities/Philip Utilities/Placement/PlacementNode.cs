using Philip.Grid;

namespace Philip.Building
{
    [System.Serializable]
    public class PlacementNode<TBuildingObject> : Node<PlacementNode<TBuildingObject>> where TBuildingObject : IBuildable
    {
        public TBuildingObject ObjectInNode { private set; get; } = default;

        public bool IsBuildable { private set; get; } = true;
        public bool BuiltOn { private set; get; } = false;

        public PlacementNode(Grid<PlacementNode<TBuildingObject>> grid, int x, int y) : base(grid, x, y)
        {

        }

        public void SetObjectInNode(TBuildingObject item)
        {
            ObjectInNode = item;
        }

        // Set the is buildable variable
        public void SetIsBuildable(bool value)
        {
            IsBuildable = value;
        }

        // Sets the built on variable
        public void SetBuiltOn(bool value)
        {
            BuiltOn = value;
        }
    }
}
