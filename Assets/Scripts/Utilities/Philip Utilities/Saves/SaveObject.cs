using System.IO;
using UnityEngine;

public abstract class SaveObject<TSaveableObject> : ScriptableObject
{
    [field: SerializeField] public TSaveableObject SaveableObject { private set; get; }
    public void Save(string fileName)
    {
        string fullSavePath = $"{Application.persistentDataPath}/{fileName}.world";
        if (!File.Exists($"{fullSavePath}"))
        {
            using FileStream file = File.Create($"{fullSavePath}");
        }

        string json = JsonUtility.ToJson(SaveableObject);
        File.WriteAllText(fullSavePath, json);
    }

    public void Load(string fileName)
    {
        string fullSavePath = $"{Application.persistentDataPath}/{fileName}.world";
        if (File.Exists(fullSavePath))
        {
            TSaveableObject saveableObject = JsonUtility.FromJson<TSaveableObject>(File.ReadAllText(fullSavePath));
            SaveableObject = saveableObject;
        }
        else
        {
            Save(fileName);
        }
    }
}
