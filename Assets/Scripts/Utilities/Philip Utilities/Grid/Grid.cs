using Philip.Utilities.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Philip.Grid
{
    [System.Serializable]
    public class Grid<T> {

        public delegate void OnGridValueChanged(int x, int y);
        [field:SerializeField, Header("Setup")] public int Width { private set; get; }
        [field: SerializeField] public int Height { private set; get; }
        [field:SerializeField] public float CellSize { private set; get; }
        [field: SerializeField] public bool Debug { private set; get; } = false;
        [SerializeField] private Vector3 _originPosition;

        // Debug
        [SerializeField] private List<GameObject> generatedGameObjects = new List<GameObject>();


        // Delegates
        public OnGridValueChanged afterGridValueChanged;

        // Arrays
        [SerializeField] private T[,] _gridArray;

        public Grid(int width, int height, float cellSize, Func<Grid<T>, int, int, T> createObject, bool debug=false, Vector3 originPosition=default) 
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            Debug = debug;
            _originPosition = originPosition;

            Init(createObject);
        }

        // Initializes the grid and creates it
        public void Init(Func<Grid<T>, int, int, T> createObject)
        {
            _gridArray = new T[Width, Height];
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)
                {
                    _gridArray[x, y] = createObject(this, x, y);
                }
            }
            // If we are debugging then this.
            if (Debug)
            {
                DrawDebugLines();    
            }

            afterGridValueChanged += GridValueChanged;
        }

        // Returns all the game objects that have been created
        public List<GameObject> GetGeneratedGameObjects()
        {
            return generatedGameObjects;
        }


        // Delegate that is called when a grid value is changed
        public void GridValueChanged(int x, int y)
        {

        }


        // Draws the debug lines of the grid
        public void DrawDebugLines()
        {
#if UNITY_EDITOR
            if (!Debug) return;
            for (int x = 0; x < _gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < _gridArray.GetLength(1); y++)   
                {
                    UnityEngine.Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, Mathf.Infinity);
                    UnityEngine.Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, Mathf.Infinity);
                }
            }

            // Outer lines
            UnityEngine.Debug.DrawLine(GetWorldPosition(0, Height), GetWorldPosition(Width, Height), Color.white, Mathf.Infinity);
            UnityEngine.Debug.DrawLine(GetWorldPosition(Width, 0), GetWorldPosition(Width, Height), Color.white, Mathf.Infinity);
    #endif
        }

        // Gets the world position of a coordinate
        public Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * CellSize + _originPosition;
        }

        // Gets the world position of a coordinate
        public Vector3 GetWorldPosition(Vector2Int coords)
        {
            return new Vector3(coords.x, coords.y) * CellSize + _originPosition;
        }

        // Gets the coordinates from a world position
        public Vector2Int GetCoordinate(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt((worldPosition - _originPosition).x / CellSize);
            int y = Mathf.FloorToInt((worldPosition - _originPosition).y / CellSize);

            return new Vector2Int(x, y);
        }

        // Checks if the coordinate given is a valid coordinate on the grid
        public bool IsValidCoordinate(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < Width && y < Height);
        }

        public bool IsValidCoordinate(Vector2Int coords)
        {
            return (coords.x >= 0 && coords.y >= 0 && coords.x < Width && coords.y < Height);
        }

        // Gets the value of a coordinate
        public T GetGridObject(int x, int y)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                return _gridArray[x, y];
            } 
            else
            {
                return default;
            }
        }

        public T GetGridObject(Vector2Int coords)
        {
            if (coords.x >= 0 && coords.y >= 0 && coords.x < Width && coords.y < Height)
            {
                return _gridArray[coords.x, coords.y];
            }
            else
            {
                return default;
            }
        }

        // Gets the value of a coordinate from a world position
        public T GetGridObject(Vector3 worldPosition)
        {
            Vector2Int coordinates = GetCoordinate(worldPosition);
            return GetGridObject(coordinates.x, coordinates.y);
        }

        // Sets the value of a coordinate
        public void SetGridObject(int x, int y, T value)
        {
            if (x >= 0 && y >= 0 && x < Width && y < Height)
            {
                _gridArray[x, y] = value;
                TriggerGridObjectChanged(x, y);
            }
            else
            {
                UnityEngine.Debug.LogWarning($"{x}, {y} is not a valid coordinate in the _gridArray of {this}");
            }
        }

        // Sets the value of a coordinate from world position
        public void SetGridObject(Vector3 worldPosition, T value)
        {
            Vector2Int coordinates = GetCoordinate(worldPosition);
            SetGridObject(coordinates.x, coordinates.y, value);
        }

        // Triggers the after grid value changed delegate
        public void TriggerGridObjectChanged(int x, int y)
        {
            afterGridValueChanged?.Invoke(x, y);
        }
    }
}

