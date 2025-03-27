using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SeasonType
{
    Spring = 3,
    Summer = 6,
    Fall = 9,
    Winter = 12,
}
public class WeatherSystem : MonoBehaviour
{
    private WeatherStateMachine stateMachine;

    private void Awake()
    {
        WeatherManager.Instance.WeatherSystem = this;
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
        switch(TimeManager.Instance.TimeSystem.CurrentMonth)
        {
            case (int)SeasonType.Spring:
                break;
            case (int)SeasonType.Summer: 
                break;
            case (int)SeasonType.Fall: 
                break;
            case (int)SeasonType.Winter: 
                break;
        }
    }
}
