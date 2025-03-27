using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Example_Without_Enum
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// Name
    /// </summary>
    public string name;

    /// <summary>
    /// Age
    /// </summary>
    public int age;

    /// <summary>
    /// Scores
    /// </summary>
    public List<int> scores;

}
public class Example_Without_EnumLoader
{
    public List<Example_Without_Enum> ItemsList { get; private set; }
    public Dictionary<int, Example_Without_Enum> ItemsDict { get; private set; }

    public Example_Without_EnumLoader(string path = "default/path/Example_Without_Enum")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, Example_Without_Enum>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<Example_Without_Enum> Items;
    }
}
