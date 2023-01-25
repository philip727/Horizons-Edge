using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New World Settings", menuName = "Philip/World Generation/World Settings")]
public class WorldGenerationSettings : ScriptableObject
{
    [field: SerializeField] public int WorldWidth { private set; get; } = 1024;
    [field: SerializeField] public int WorldHeight { private set; get; } = 1024;
    [field: SerializeField] public int TileSize { private set; get; } = 1;
    [field: SerializeField] public int ChunkSize { private set; get; } = 16;
    [field: SerializeField] public BiomeObject[] BiomeObjects { private set; get; } = new BiomeObject[0];

    public BiomeObject GetBiomeObject(Biome biome)
    {
        for (int i = 0; i < BiomeObjects.Length; i++)
        {
            if (BiomeObjects[i].Biome.ID == biome.ID)
            {
                return BiomeObjects[i];
            }
        }

        return null;
    }


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
    }
}
