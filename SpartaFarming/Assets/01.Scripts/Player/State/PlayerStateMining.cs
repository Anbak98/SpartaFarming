using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMining : MonoBehaviour, IPlayerState
{
    private Vector3 pos;
    private Vector3Int tilePosition;

    public void DoAction()
    {
        GameManager.Instance.Player.Controller.onMine?.Invoke(tilePosition);
    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void HandleAction(InputAction.CallbackContext context)
    {
    }

    public void OnUpdate()
    {
        pos = GameManager.Instance.Player.transform.position;
        tilePosition = GameManager.Instance.Player.Controller.oreMap.WorldToCell(pos);

        float lastMoveX = GameManager.Instance.Player.Controller.plLastMoveX;
        float lastMoveY = GameManager.Instance.Player.Controller.plLastMoveY;

        if (lastMoveX == 1) tilePosition += Vector3Int.right;
        else if (lastMoveX == -1) tilePosition += Vector3Int.left;
        else if (lastMoveY == 1) tilePosition += Vector3Int.up;
        else if (lastMoveY == -1) tilePosition += Vector3Int.down;
        else return;
    }
}
