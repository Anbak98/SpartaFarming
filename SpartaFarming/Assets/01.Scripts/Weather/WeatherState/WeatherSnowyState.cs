using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSnowyState : WeatherBaseState
{
    public WeatherSnowyState(WeatherStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        Debug.Log("snowy start");
        base.Enter();
        WeatherManager.Instance.WeatherSystem.WorldLight.ChangedWeatherColor(WeatherChance.SnowChance);
        WeatherManager.Instance.WeatherSystem.WeatherVFX.SnowEffect.OnEnable();
    }

    public override void Exit()
    {
        Debug.Log("snowy end");
        base.Exit();
        WeatherManager.Instance.WeatherSystem.WeatherVFX.SnowEffect.OnDisable();
        TimeManager.Instance.TimeSystem.TimeChangeUpdate -= WeatherManager.Instance.WeatherSystem.WorldLight.OnTimeChangedSnowy;
    }
}
