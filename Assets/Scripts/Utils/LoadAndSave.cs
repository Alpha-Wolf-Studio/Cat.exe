using System.IO;
using UnityEngine;

public class LoadAndSave
{
    public static string LoadFromFile(string filename)
    {
        TextAsset textAsset = (TextAsset)Resources.Load(filename);
        string stringData = textAsset.text;
        return stringData;
    }
    public static void SaveToFile(string filename, string stringData)
    {
        string path = Application.dataPath + "/Resources/" + filename + ".txt";
        File.WriteAllText(path, stringData);
        
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#else
        File.WriteAllText(filename + ".txt", stringData);
#endif
    }
    public static void OverwriteResourceFile(string filename)
    {
        string path = filename + ".txt";
        string data = File.ReadAllText(path);

        SaveToFile(filename, data);
    }
}