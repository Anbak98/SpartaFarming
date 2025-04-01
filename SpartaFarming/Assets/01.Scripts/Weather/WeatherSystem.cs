using System;
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
    private IWeatherState currentState;
    private WorldLight worldLight;
    private WeatherVFX weatherVFX;

    public WorldLight WorldLight { get { return worldLight; } }
    public WeatherVFX WeatherVFX {  get { return weatherVFX; } }
    public SeasonData CurrentSeason { get { return currentSeason; } }
    public IWeatherState CurrentState { get { return currentState; } }

    public bool canChangeWeather = true;

    public Action OnSeasonChange;

    private void Awake()
    {
        WeatherManager.Instance.WeatherSystem = this;
        worldLight = GetComponentInChildren<WorldLight>();
        weatherVFX = GetComponentInChildren<WeatherVFX>();

        stateMachine = new WeatherStateMachine(this);
        seasons = new SeasonDataLoader();
    }

    void Start()
    {
        TimeManager.Instance.TimeSystem.DateChanged += ChangeSeason;
        TimeManager.Instance.TimeSystem.On8oClock += FixedTimeWeatherChange;
        TimeManager.Instance.TimeSystem.On20oClock += FixedTimeWeatherChange;
        currentState = stateMachine.CurrentState;
    }

    public void GetSeason()
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
                    OnSeasonChange?.Invoke();
                }
            }
        }
    }

    public void ChangeSeason()
    {
        GetSeason();
    }

    void ChangeToRandomWeather(SeasonData curSeason)
    {
        int totalWeight = 0;
        int randomNum = UnityEngine.Random.Range(0, curSeason.weatherChancesValues.Sum());
        for(int i = 0; i < curSeason.weatherChancesValues.Length; i++)
        {
            totalWeight += curSeason.weatherChancesValues[i];
            if(randomNum < totalWeight)
            {
                stateMachine.ChangeState(GetStatefromKey(i));
                return;
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

    public void FixedTimeWeatherChange()
    {
        if(canChangeWeather)
        {
            ChangeToRandomWeather(currentSeason);
            canChangeWeather = false;
        }
    }
}
