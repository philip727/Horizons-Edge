using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Philip.Tilemaps;

[CreateAssetMenu(fileName = "New Biome", menuName = "Philip/World Generation/Biome")]
public class BiomeObject : ScriptableObject
{
    [field: SerializeField, Range(0f, 1f)] public float Precipitation { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Temperature { private set; get; }
    [field: SerializeField] public RuleTileObject TileRules { private set; get; }
    [field: SerializeField] public ResourceObject[] ResourceObjects { private set; get; }

    [field: SerializeField] public Biome Biome = new Biome();
}
