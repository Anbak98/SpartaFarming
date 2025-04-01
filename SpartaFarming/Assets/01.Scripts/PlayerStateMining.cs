using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMining : MonoBehaviour, IPlayerState
{
    public void DoAction()
    {
        GameManager.Instance.Player.Controller.onMine?.Invoke(tilePosition); // playercontroller => public Action<Vector3Int> onMine;

    }

    public void Enter()
    {
    }

    public void Exit()
    {
    }

    public void OnUpdate()
    {
        Vector3 pos = transform.position;
        Vector3Int objGridPos = GameManager.Instance.Player.Controller.objectMap.WorldToCell(pos);

        Vector3Int tilePosition = objGridPos;

        // PlayerController plLastMoveX, Y 프로퍼티 또는 퍼블릭
        if (plLastMoveX == 1) tilePosition += Vector3Int.right;
        else if (plLastMoveX == -1) tilePosition += Vector3Int.left;
        else if (plLastMoveY == 1) tilePosition += Vector3Int.up;
        else if (plLastMoveY == -1) tilePosition += Vector3Int.down;
        else return;
    }
}
