using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherBaseState : IWeatherState
{
    protected WeatherStateMachine stateMachine;

    public WeatherBaseState(WeatherStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Enter()
    {
        //³¯¾¾ ±¼¸®±â
    }

    public void Exit()
    {
        //ÇöÀç ³¯¾¾ Á¾·á
    }

    public void Update()
    {
    }
}
