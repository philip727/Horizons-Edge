using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Settings", menuName = "Philip/World Generation/Settings")]
public class WorldGenerationSettings : ScriptableObject
{
    [field: SerializeField, Header("World Settings")] public int WorldWidth = 1024;
    [field: SerializeField] public int WorldHeight = 1024;
    [field: SerializeField] public int TileSize = 1;
    [field: SerializeField] public int ChunkSize = 16;

    [field: SerializeField, Header("Noise Settings")] public float NoiseScale { private set; get; } = 1f;
    [field: SerializeField] public int Octaves { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Persistance { private set; get; }
    [field: SerializeField] public float Lacunarity { private set; get; }

    [field: SerializeField] public Vector2 Offset { private set; get; }
    [field: SerializeField] public BiomeObject[] BiomeObjects { private set; get; } = new BiomeObject[0];


    private void OnValidate()
    {
        if(!(ChunkSize % 4 == 0)) 
        {
            for (int i = ChunkSize; i >= ChunkSize - 4; i--)
            {
                if (i % 4 == 0)
                {
                    ChunkSize = i;
                    break;
                }
            }
        }

        if(ChunkSize != 0)
        {
            if(!(WorldWidth % ChunkSize == 0))
            {
                for (int i = WorldWidth; i >= WorldWidth - ChunkSize; i--)
                {
                    if (i % 4 == 0)
                    {
                        WorldWidth = i;
                        break;
                    }
                }
            }

            if(!(WorldHeight % ChunkSize == 0))
            {
                for (int i = WorldHeight; i >= WorldHeight - ChunkSize; i--)
                {
                    if (i % 2 == 0)
                    {
                        WorldHeight = i;
                        break;
                    }
                }
            }
        }

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
