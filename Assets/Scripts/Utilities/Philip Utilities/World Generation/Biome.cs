using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Biome
{
    [field: SerializeField] public string Name { private set; get; }
    [field: SerializeField] public string ID { private set; get; }

    public Biome()
    {
        Name = "";
        ID = "";
    }

    public Biome(string name, string id)
    {
        Name = name;
        ID = id;
    }

}
