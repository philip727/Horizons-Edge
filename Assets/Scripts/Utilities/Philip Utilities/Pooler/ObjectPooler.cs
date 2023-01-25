using Philip.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectPooler<TClass> : MonoBehaviourSingleton<TClass> where TClass : ObjectPooler<TClass>, new()
{
    [field: SerializeField] public GameObject ObjectPrefab { private set; get; }
    [field: SerializeField] public int StartingAmountOfObjects { private set; get; }
    [field: SerializeField] public Transform ObjectParent { private set; get; }
    private readonly List<GameObject> _spawnedObjects = new List<GameObject>();

    public override void Awake()
    {
        base.Awake();
        ObjectParent = ObjectParent == null ? transform : ObjectParent;
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        for (int i = 0; i < StartingAmountOfObjects; i++)
        {
            SpawnNewObject();
        }
    }

    public GameObject SpawnNewObject()
    {
        var obj = Instantiate(ObjectPrefab, ObjectParent);
        _spawnedObjects.Add(obj);
        obj.SetActive(false);

        return obj;
    }

    // Gets the first object that isn't active
    public GameObject GetObject()
    {
        for (int i = 0; i < _spawnedObjects.Count; i++)
        {
            if (!_spawnedObjects[i].activeSelf)
            {
                return _spawnedObjects[i];
            }
        }

        // If there are no objects that aren't active then it creates a new one
        return SpawnNewObject();
    }

}
