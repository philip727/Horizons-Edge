using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "Philip/World Generation/Biome")]
public class BiomeObject : ScriptableObject
{
    [field: SerializeField] public float HeightToSpawnAt;

    [field: SerializeField] public Biome Biome = new Biome();
}
