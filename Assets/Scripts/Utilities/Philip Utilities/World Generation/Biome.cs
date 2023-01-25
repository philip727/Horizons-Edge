using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Biome
{
    [field: SerializeField] public string Name { private set; get; }
    [field: SerializeField] public string ID { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Precipitation { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Temperature { private set; get; }

    [field: SerializeField] public RuleTile BiomeTile { private set; get; }

}
