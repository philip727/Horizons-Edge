using System.IO;
using UnityEngine;

public abstract class SaveObject<TSaveableObject> : ScriptableObject
{
    public delegate void OnSaveUpdate(TSaveableObject saveableObject);
    [field: SerializeField] public TSaveableObject SaveableObject { private set; get; }
    public OnSaveUpdate onLoad;
    public OnSaveUpdate onSave;

    public void Save(string fileName)
    {
        string fullSavePath = $"{Application.persistentDataPath}/{fileName}.world";
        if (!File.Exists($"{fullSavePath}"))
        {
            using FileStream file = File.Create($"{fullSavePath}");
        }

        string json = JsonUtility.ToJson(SaveableObject);
        File.WriteAllText(fullSavePath, json);
        onSave?.Invoke(SaveableObject);
    }

    public virtual void Load(string fileName)
    {
        string fullSavePath = $"{Application.persistentDataPath}/{fileName}.world";
        if (File.Exists(fullSavePath))
        {
            TSaveableObject saveableObject = JsonUtility.FromJson<TSaveableObject>(File.ReadAllText(fullSavePath));
            SaveableObject = saveableObject;
            onLoad?.Invoke(SaveableObject);
        }
        else
        {
            Save(fileName);
        }
    }
}
