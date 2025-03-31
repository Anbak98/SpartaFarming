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
        Debug.Log("windy start");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("windy end");
        TimeManager.Instance.TimeSystem.TimeChangeUpdate -= WeatherManager.Instance.WeatherSystem.WorldLight.OnTimeChangedWindy;
    }
}
