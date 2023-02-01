using System;
using UnityEngine;

namespace Philip.Grid
{
    [System.Serializable]
    public abstract class Node<T> where T : Node<T>
    {
        public Grid<T> Grid { private set; get; }
        public int X { protected set; get; }
        public int Y { protected set; get; }

        public Vector2Int Coordinates
        {
            get
            {
                return new Vector2Int(X, Y);
            }
        }

        public virtual T GetNeighbour(Vector2Int neighbourOffset)
        {
            if(Grid.IsValidCoordinate(Coordinates + neighbourOffset))
            {
                return Grid.GetGridObject(Coordinates + neighbourOffset);
            }

            return null;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public Node(Grid<T> grid, int x, int y)
        {
            Grid = grid;
            X = x;
            Y = y;
        }
    }
}