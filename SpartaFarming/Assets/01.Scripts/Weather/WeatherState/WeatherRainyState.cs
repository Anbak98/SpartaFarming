using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherRainyState : WeatherBaseState
{
    public WeatherRainyState(WeatherStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        WeatherManager.Instance.WeatherSystem.WorldLight.ChangedWeatherColor(WeatherChance.RainChance);
        base.Enter();
        WeatherManager.Instance.WeatherSystem.WeatherVFX.RainEffect.OnEnable();
        Debug.Log("rainy start");
    }

    public override void Exit()
    {
        base.Exit();
        WeatherManager.Instance.WeatherSystem.WeatherVFX.RainEffect.OnDisable();
        Debug.Log("rainy end");
        TimeManager.Instance.TimeSystem.TimeChangeUpdate -= WeatherManager.Instance.WeatherSystem.WorldLight.OnTimeChangedRainy;
    }
}
