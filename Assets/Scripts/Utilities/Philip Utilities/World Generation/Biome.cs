using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Biome
{
    [field: SerializeField] public string Name { private set; get; }
    [field: SerializeField] public string ID { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Precipitation { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Temperature { private set; get; }
    [field: SerializeField] public Tile BiomeTile { private set; get; }

    public Biome()
    {
        Name = "";
        ID = "";
        Precipitation = 0f;
        Temperature = 0f;
        BiomeTile = null;
    }

    public Biome(string name, string id, float precipitation, float temperature, Tile biomeTile)
    {
        Name = name;
        ID = id;
        Precipitation = precipitation;
        Temperature = temperature;
        BiomeTile = biomeTile;
    }

}
