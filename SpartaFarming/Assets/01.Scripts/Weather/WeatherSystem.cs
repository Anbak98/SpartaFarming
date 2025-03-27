using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


public enum SeasonType
{
    Spring = 0,
    Summer = 1,
    Fall = 2,
    Winter = 3,
}

public enum WeatherChance
{
    SunChance = 0,
    RainChance = 1,
    WindChance = 2,
    SnowChance = 3,
}

public class WeatherSystem : MonoBehaviour
{
    private WeatherStateMachine stateMachine;
    private SeasonDataLoader seasons;

    private SeasonData currentSeason;

    private void Awake()
    {
        WeatherManager.Instance.WeatherSystem = this;

        stateMachine = new WeatherStateMachine(this);
        seasons = new SeasonDataLoader();
    }

    void Start()
    {
        TimeManager.Instance.TimeSystem.DateChanged += ChangeSeason;
    }

    void Update()
    {

    }

    public void ChangeSeason()
    {
        int currentMonth = TimeManager.Instance.TimeSystem.CurrentMonth;

        foreach (var season in seasons.ItemsList)
        {
            if (season.startMonth.Contains(currentMonth))
            {
                if (currentSeason == null || currentSeason != season)
                {
                    currentSeason = season;
                    ChangeToRandomWeather(currentSeason);
                }
            }
        }
    }

    void ChangeToRandomWeather(SeasonData curSeason)
    {
        int totalWeight = 0;
        int randomNum = Random.Range(0, curSeason.weatherChance.Values.Sum());
        foreach (var weight in curSeason.weatherChance)
        {
            totalWeight += weight.Value;
            if (randomNum < totalWeight)
            {
                stateMachine.ChangeState(GetStatefromKey((int)weight.Key));
            }
        }
    }

    IWeatherState GetStatefromKey(int key)
    {
        switch (key)
        {
            case 0:
                return stateMachine.SunnyState;
            case 1:
                return stateMachine.RainyState;
            case 2:
                return stateMachine.WindyState;
            case 3:
                return stateMachine.SnowyState;
            default:
                return null;
        }
    }
}
