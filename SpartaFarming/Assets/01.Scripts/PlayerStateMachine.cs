using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private IPlayerState currentState;
    public PlayerStateSeeding buildState;

    public void ChangeState(IPlayerState playerState)
    {
        currentState?.Exit();
        currentState = playerState;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.OnUpdate();
    }
}
