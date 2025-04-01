using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class CropGrowingInfo
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
    /// Description
    /// </summary>
    public string description;

    /// <summary>
    /// EXP
    /// </summary>
    public float expRequirement;

    /// <summary>
    /// GrowthSpeed
    /// </summary>
    public float Spring;

    /// <summary>
    /// GrowthSpeed
    /// </summary>
    public float Summer;

    /// <summary>
    /// GrowthSpeed
    /// </summary>
    public float Fall;

    /// <summary>
    /// GrowthSpeed
    /// </summary>
    public float Winter;

    /// <summary>
    /// GrowthSpeed
    /// </summary>
    public float SunChance;

    /// <summary>
    /// GrowthSpeed
    /// </summary>
    public float RainChance;

    /// <summary>
    /// GrowthSpeed
    /// </summary>
    public float WindChance;

    /// <summary>
    /// GrowthSpeed
    /// </summary>
    public float SnowChance;

}
public class CropGrowingInfoLoader
{
    public List<CropGrowingInfo> ItemsList { get; private set; }
    public Dictionary<int, CropGrowingInfo> ItemsDict { get; private set; }

    public CropGrowingInfoLoader(string path = "JSON/CropGrowingInfo")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, CropGrowingInfo>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<CropGrowingInfo> Items;
    }

    public CropGrowingInfo GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
}
