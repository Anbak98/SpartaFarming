using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMining : IPlayerState
{
    private Vector3 pos;
    private Vector3Int tilePosition;

    public void DoAction()
    {
        GameManager.Instance.Player.Controller.onMine?.Invoke(tilePosition);
    }

    public void Enter()
    {
        Debug.Log("enter");
    }

    public void Exit()
    {
    }

    public void OnUpdate()
    {
        pos = GameManager.Instance.Player.transform.position;
        tilePosition = GameManager.Instance.Player.Controller.objectMap.WorldToCell(pos);

        float lastMoveX = GameManager.Instance.Player.Controller.plLastMoveX;
        float lastMoveY = GameManager.Instance.Player.Controller.plLastMoveY;

        if (lastMoveX == 1) tilePosition += Vector3Int.right;
        else if (lastMoveX == -1) tilePosition += Vector3Int.left;
        else if (lastMoveY == 1) tilePosition += Vector3Int.up;
        else if (lastMoveY == -1) tilePosition += Vector3Int.down;
        else return;
    }
}
