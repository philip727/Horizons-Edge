using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Philip.Tilemaps
{
    [CreateAssetMenu(fileName = "New Rule Tile", menuName = "Philip/Tilemap/Rule Tile")]
    public class RuleTileObject : ScriptableObject
    {
        [field: SerializeField] public RuleTile[] RuleTiles = new RuleTile[0];

        [System.Serializable]
        public class RuleTile
        {
            [field: SerializeField] public Tile Tile { private set; get; }
            [field: SerializeField] public RuleTilePositions Rule { private set; get; }
        }

        public enum RuleTilePositions
        {
            Centre,
            CentreRight,
            CentreLeft,
            CentreUp,
            CentreDown,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
            CornerNothingRightBottom,
            CornerNothingLeftBottom,
            CornerNothingRightTop,
            CornerNothingLeftTop,
            GoingUpToSingleTile,
            GoingDownToSingleTile,
            GoingRightToSingleTile,
            GoingLeftToSingleTile,
            SingleTileUp,
            SingleTileLeft,
            SingleTileRight,
            SingleTileDown,
            TileAlone,
        }

        public Tile GetTileFromRule(RuleTilePositions ruleToLookFor)
        {
            RuleTile result = Array.Find(RuleTiles, tile => tile.Rule == ruleToLookFor);
            if(result == null)
            {
                Debug.LogError($"<color=#42ddf5>[TILEMAPS]</color> could not find {ruleToLookFor} in {RuleTiles}");
            }

            return result.Tile;
        }
    }
}
