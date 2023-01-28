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

        public static Dictionary<RuleNodes, Vector2Int> s_ruleNodesByCoordinate = new Dictionary<RuleNodes, Vector2Int>()
        {
            { RuleNodes.UpLeft, new Vector2Int(-1, 1) },
            { RuleNodes.Up, new Vector2Int(0, 1) },
            { RuleNodes.UpRight, new Vector2Int(1,1) },
            { RuleNodes.Left, new Vector2Int(-1, 0) },
            { RuleNodes.Middle, new Vector2Int(0,0) },
            { RuleNodes.Right, new Vector2Int(1, 0) },
            { RuleNodes.BottomLeft, new Vector2Int(-1, -1) },
            { RuleNodes.Bottom, new Vector2Int(0, -1) },
            { RuleNodes.BottomRight, new Vector2Int(1, -1) },
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
        public TileBase GetTileFromRule(WorldNode worldNode)
        {
            RuleTile result = Array.Find(RuleTiles, tile => tile.CheckIfMeetsRequirements(worldNode));
            if (result == null)
            {
                Debug.LogError($"<color=#42ddf5>[TILEMAPS]</color> could not find tile in {RuleTiles}");
                return null;
            }

            return result.SpriteType switch
            {
                SpriteType.Default => result.Tile,
                SpriteType.Animated => result.AnimatedTile,
                _ => result.Tile,
            };
        }

        [System.Serializable]
        public class RuleTile
        {
            [field: SerializeField] public SpriteType SpriteType { private set; get; } = SpriteType.Default;
            [field: SerializeField, ConditionalField("SpriteType", SpriteType.Default)] public Tile Tile { private set; get; }
            [field: SerializeField, ConditionalField("SpriteType", SpriteType.Animated)] public AnimatedTile AnimatedTile { private set; get; }
            

            [field: SerializeField] public List<RuleNodes> RequiredLandNodes { private set; get; } = new List<RuleNodes>();
            [field: SerializeField] public List<RuleNodes> RequiredNothingNodes { private set; get; } = new List<RuleNodes>();

            // Checks through its needs, if it meets then it will be available
            public bool CheckIfMeetsRequirements(WorldNode worldNode)
            {
                foreach (RuleNodes requiredNode in RequiredLandNodes)
                {
                    s_ruleNodesByCoordinate.TryGetValue(requiredNode, out Vector2Int offset);
                    WorldNode worldNodeToCheck = worldNode.GetNeighbour(offset);

                    if(worldNodeToCheck != null && !worldNodeToCheck.IsWater)
                    {
                        continue;
                    }

                    return false;
                }

                foreach (RuleNodes requiredNothing in RequiredNothingNodes)
                {
                    s_ruleNodesByCoordinate.TryGetValue(requiredNothing, out Vector2Int offset);
                    WorldNode worldNodeToCheck = worldNode.GetNeighbour(offset);
                    if(worldNodeToCheck == null || worldNodeToCheck.IsWater)
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
