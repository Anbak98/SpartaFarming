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
        WeatherManager.Instance.WeatherSystem.WorldLight.ChangedWeatherColor(WeatherChance.SnowChance);
        base.Enter();
        Debug.Log("snowy start");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("snowy end");
        TimeManager.Instance.TimeSystem.TimeChangeUpdate -= WeatherManager.Instance.WeatherSystem.WorldLight.OnTimeChangedSnowy;
    }
}
