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
        //���� ������
    }

    public void Exit()
    {
        //���� ���� ����
    }

    public void Update()
    {
    }
}
