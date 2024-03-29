using Philip.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Resource", menuName = "Philip/World Generation/Resource")]
public class ResourceObject : ScriptableObject
{
    [field: SerializeField] public GameObject Prefab { private set; get; }
    [field: SerializeField] public StructureObjectSettings StructureObjectSettings { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Baron { private set; get; }
    [field: SerializeField, Range(0f, 1f)] public float Tropicality { private set; get; }
}
