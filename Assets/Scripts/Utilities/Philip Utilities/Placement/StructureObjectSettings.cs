using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure Settings", menuName = "Philip/Placement/Structure Settings")]
public class StructureObjectSettings : ScriptableObject
{
    [field: SerializeField] public Vector2Int[] CoordinatesItTakesUp { private set; get; }
}
