using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSunnyState : WeatherBaseState
{
    public WeatherSunnyState(WeatherStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("sunny start");
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("sunny end");
    }
}
