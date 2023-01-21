using UnityEngine;

namespace Philip.Grid
{
    [System.Serializable]
    public abstract class Node<T>
    {
        protected Grid<T> _grid;
        public int X { protected set; get; }
        public int Y { protected set; get; }

        public Vector2Int Coordinates
        {
            get
            {
                return new Vector2Int(X, Y);
            }
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public Node(Grid<T> grid, int x, int y)
        {
            _grid = grid;
            X = x;
            Y = y;
        }
    }
}