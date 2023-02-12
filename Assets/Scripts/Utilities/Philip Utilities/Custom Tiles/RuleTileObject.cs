using Philip.Editor;
using Philip.WorldGeneration;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Philip.Tilemaps
{
    [CreateAssetMenu(fileName = "New Rule Tile", menuName = "Philip/Tilemap/Rule Tile")]
    public class RuleTileObject : ScriptableObject
    {
        [field: SerializeField] public RuleTile[] RuleTiles = new RuleTile[0];

        public static Dictionary<RuleNodes, Vector2Int> RuleNodesByCoordinate { get; } = new Dictionary<RuleNodes, Vector2Int>()
        {
            { RuleNodes.UpLeft,     new Vector2Int(-1, 1) },
            { RuleNodes.Up,         new Vector2Int(0, 1) },
            { RuleNodes.UpRight,    new Vector2Int(1,1) },
            { RuleNodes.Left,       new Vector2Int(-1, 0) },
            { RuleNodes.Middle,     new Vector2Int(0,0) },
            { RuleNodes.Right,      new Vector2Int(1, 0) },
            { RuleNodes.BottomLeft, new Vector2Int(-1, -1) },
            { RuleNodes.Bottom,     new Vector2Int(0, -1) },
            { RuleNodes.BottomRight,new Vector2Int(1, -1) },
        };

        public enum RuleNodes
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

        public enum SpriteType
        {
            Default,
            Animated,
        }

        // Gets the tile in that worldnode, if it fits its requirements
        public RuleTile GetTileFromRule(ChunkData chunkData, int x, int y)
        {
            RuleTile result = Array.Find(RuleTiles, tile => tile.CheckIfMeetsRequirements(chunkData, x, y));
            if (result == null)
            {
                Debug.LogError($"<color=#42ddf5>[TILEMAPS]</color> Could not determine tile in ({x}, {y})");
                return null;
            }

            return result;
        }

        [System.Serializable]
        public class RuleTile
        {
            [SerializeField] private SpriteType _spriteType = SpriteType.Default;
            [ConditionalField("_spriteType", SpriteType.Default)] public Tile tile;
            [ConditionalField("_spriteType", SpriteType.Animated)] public AnimatedTile animatedTile;
            public SpriteType SpriteType { get { return _spriteType; } }
            [field: SerializeField] public List<RuleNodes> RequiredLandNodes { private set; get; } = new List<RuleNodes>();
            [field: SerializeField] public List<RuleNodes> RequiredNothingNodes { private set; get; } = new List<RuleNodes>();
            [field: SerializeField] public List<RuleNodes> AddColliders { private set; get; } = new List<RuleNodes>();

            public Vector3Int ConvertRuleToOffset(RuleNodes ruleNode)
            {
                RuleNodesByCoordinate.TryGetValue(ruleNode, out Vector2Int offset);
                return (Vector3Int)offset;
            }

            // Checks through its needs, if it meets then it will be available
            public bool CheckIfMeetsRequirements(ChunkData chunkData, int x, int y)
            {
                foreach (RuleNodes requiredNode in RequiredLandNodes)
                {
                    RuleNodesByCoordinate.TryGetValue(requiredNode, out Vector2Int offset);
                    Vector2Int coordinates = chunkData.Coordinates + new Vector2Int(x, y) + offset;

                    if (!WorldGenerationHandler.Instance.IsCoordinateWater(coordinates))
                    {
                        continue;
                    }

                    return false;
                }

                foreach (RuleNodes requiredNothing in RequiredNothingNodes)
                {
                    RuleNodesByCoordinate.TryGetValue(requiredNothing, out Vector2Int offset);
                    Vector2Int coordinates = chunkData.Coordinates + new Vector2Int(x, y) + offset;
                    if (WorldGenerationHandler.Instance.IsCoordinateWater(coordinates))
                    {
                        continue;
                    }

                    return false;
                }

                return true;
            }

        }
    }
}
