using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private IPlayerState currentState;

    public PlayerStateSeeding seedingState;
    public PlayerStateRemovingFence removingFenceState;
    public PlayerStatePlacingFence placingFenceState;
    public PlayerStateFishing fishingState;
    public PlayerStateHoeing hoeingState;
    public PlayerStateMining miningState;

    public void ChangeState(IPlayerState playerState)
    {        
        currentState?.Exit();
        currentState = playerState;
        currentState?.Enter();
    }
    public void ClearState ()
    {
        currentState?.Exit();
        currentState = null;
    }

    public void DoAction()
    {
        currentState?.DoAction();
    }

    public void ExitState()
    {
        currentState?.Exit();
        currentState = null;
    }

    public void Update()
    {
        currentState?.OnUpdate();
    }
}
