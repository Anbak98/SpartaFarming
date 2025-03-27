using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IWeatherState
{
    public void Enter();
    public void Exit();
    public void Update();
}

public class WeatherStateMachine
{
    private IWeatherState currentState;

    public void ChangeState(IWeatherState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}
