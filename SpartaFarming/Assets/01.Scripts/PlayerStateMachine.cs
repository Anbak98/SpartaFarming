using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private IPlayerState currentState;
    public PlayerStateSeeding buildState;
    public PlayerStateRemovingFence removingFenceState;
    public PlayerStatePlacingFence placingFenceState;
    public PlayerStateHoeing hoeingState;

    public void ChangeState(IPlayerState playerState)
    {
        currentState?.Exit();
        currentState = playerState;
        currentState?.Enter();
    }

    public void ExitState()
    {
        currentState?.Exit();
        currentState = null;
    }

    public void Awake()
    {
        removingFenceState = GetComponent<PlayerStateRemovingFence>();
        placingFenceState = GetComponent<PlayerStatePlacingFence>();
        hoeingState = GetComponent<PlayerStateHoeing>();
    }

    public void Update()
    {
        currentState?.OnUpdate();
    }
}
