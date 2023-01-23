using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Settings", menuName = "Philip/World Generation/Noise Settings")]
public class NoiseSettings : ScriptableObject
{
    [field: SerializeField] public float NoiseScale { private set; get; } = 1f;
    [field: SerializeField] public int Octaves { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Persistance { private set; get; }
    [field: SerializeField] public float Lacunarity { private set; get; }

    [field: SerializeField] public Vector2 Offset { private set; get; }

    private void OnValidate()
    {
        if (Lacunarity < 1)
        {
            Lacunarity = 1;
        }

        if (Octaves < 0)
        {
            Octaves = 0;
        }
    }
}
