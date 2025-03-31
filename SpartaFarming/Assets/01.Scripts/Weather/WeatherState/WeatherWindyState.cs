using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherWindyState : WeatherBaseState
{
    public WeatherWindyState(WeatherStateMachine stateMachine) : base(stateMachine)
    {

    }
    public override void Enter()
    {
        WeatherManager.Instance.WeatherSystem.WorldLight.ChangedWeatherColor(WeatherChance.WindChance);
        base.Enter();
        if(WeatherManager.Instance.WeatherSystem.CurrentSeason.season == SeasonType.Spring)
        {
            WeatherManager.Instance.WeatherSystem.WeatherVFX.SpringWindEffect.OnEnable();
        }
        else if(WeatherManager.Instance.WeatherSystem.CurrentSeason.season == SeasonType.Fall)
        {
            WeatherManager.Instance.WeatherSystem.WeatherVFX.FallWindEffect.OnEnable();
        }
        Debug.Log("windy start");
    }

    public override void Exit()
    {
        base.Exit();
        WeatherManager.Instance.WeatherSystem.WeatherVFX.SpringWindEffect.OnDisable();
        WeatherManager.Instance.WeatherSystem.WeatherVFX.FallWindEffect.OnDisable();
        Debug.Log("windy end");
        TimeManager.Instance.TimeSystem.TimeChangeUpdate -= WeatherManager.Instance.WeatherSystem.WorldLight.OnTimeChangedWindy;
    }
}
