using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SeasonData
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
    public int[] startMonth;

    /// <summary>
    /// 날씨
    /// </summary>
    public WeatherChance[] weatherChances;

    /// <summary>
    /// 날씨별 확률
    /// </summary>
    public int[] weatherChancesValues;

}
public class SeasonDataLoader
{
    public List<SeasonData> ItemsList { get; private set; }
    public Dictionary<int, SeasonData> ItemsDict { get; private set; }

    public SeasonDataLoader(string path = "Weather/SeasonData")
    {
        string jsonData;
        jsonData = Resources.Load<TextAsset>(path).text;
        ItemsList = JsonUtility.FromJson<Wrapper>(jsonData).Items;
        ItemsDict = new Dictionary<int, SeasonData>();
        foreach (var item in ItemsList)
        {
            ItemsDict.Add(item.key, item);
        }
    }

    [Serializable]
    private class Wrapper
    {
        public List<SeasonData> Items;
    }

    public SeasonData GetByKey(int key)
    {
        if (ItemsDict.ContainsKey(key))
        {
            return ItemsDict[key];
        }
        return null;
    }
    public SeasonData GetByIndex(int index)
    {
        if (index >= 0 && index < ItemsList.Count)
        {
            return ItemsList[index];
        }
        return null;
    }
}
