using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class weather_with_Enum
{
    /// <summary>
    /// ID
    /// </summary>
    public int key;

    /// <summary>
    /// 계절
    /// </summary>
    public SeasonType season;

    /// <summary>
    /// 계절 시작 월
    /// </summary>
    public int startMonth;

    /// <summary>
    /// 맑음 확률
    /// </summary>
    public int sunChance;

    /// <summary>
    /// 강수 확률
    /// </summary>
    public int rainChance;

    /// <summary>
    /// 바람 확률
    /// </summary>
    public int windChance;

    /// <summary>
    /// 눈 확률
    /// </summary>
    public int snowChance;

}
public class weather_with_EnumLoader
{
    public List<weather_with_Enum> ItemsList { get; private set; }
    public Dictionary<int, weather_with_Enum> ItemsDict { get; private set; }

    public weather_with_EnumLoader(string path = "JSON/weather_with_Enum")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, weather_with_Enum>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<weather_with_Enum> Items;
    }

    public weather_with_Enum GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public weather_with_Enum GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
