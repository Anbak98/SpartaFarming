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
        base.Enter();
        Debug.Log("snowy start");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("snowy end");
    }
}
