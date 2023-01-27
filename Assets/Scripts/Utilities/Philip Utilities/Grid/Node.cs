using System;
using UnityEngine;

namespace Philip.Grid
{
    [System.Serializable]
    public abstract class Node<T> where T : Node<T>
    {
        public enum NeighbourDirections
        {
            Left,
            Right,
            Up,
            Down,
            UpLeft,
            UpRight,
            DownLeft,
            DownRight,
        }

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

        public virtual T GetNeighbour(NeighbourDirections neighbourDirections)
        {
            switch (neighbourDirections)
            {
                case NeighbourDirections.Left:
                    if(!_grid.IsValidCoordinate(X - 1, Y))
                        return _grid.GetGridObject(X - 1, Y);

                    break;
                case NeighbourDirections.Right:
                    if (!_grid.IsValidCoordinate(X + 1, Y))
                        return _grid.GetGridObject(X + 1, Y);

                    break;
                case NeighbourDirections.Up:
                    if (!_grid.IsValidCoordinate(X, Y + 1))
                        return _grid.GetGridObject(X, Y + 1);

                    break;
                case NeighbourDirections.Down:
                    if (!_grid.IsValidCoordinate(X, Y - 1))
                        return _grid.GetGridObject(X, Y - 1);

                    break;
                case NeighbourDirections.UpLeft:
                    if (!_grid.IsValidCoordinate(X - 1, Y + 1))
                        return _grid.GetGridObject(X - 1, Y + 1);
                    
                    break;
                case NeighbourDirections.UpRight:
                    if (!_grid.IsValidCoordinate(X + 1, Y + 1))
                        return _grid.GetGridObject(X + 1, Y + 1);

                    break;
                case NeighbourDirections.DownLeft:
                    if (!_grid.IsValidCoordinate(X - 1, Y - 1))
                        return _grid.GetGridObject(X - 1, Y - 1);

                    break;
                case NeighbourDirections.DownRight:
                    if (!_grid.IsValidCoordinate(X + 1, Y - 1))
                        return _grid.GetGridObject(X + 1, Y - 1);

                    break;
            }

            return null;
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