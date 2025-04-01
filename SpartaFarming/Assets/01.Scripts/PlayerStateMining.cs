using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStateMining : MonoBehaviour, IPlayerState
{
    private Vector3 pos;
    private Vector3Int tilePosition;

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
        pos = transform.position;
        tilePosition = GameManager.Instance.Player.Controller.objectMap.WorldToCell(pos);

        // PlayerController plLastMoveX, Y 프로퍼티 또는 퍼블릭
        if (plLastMoveX == 1) tilePosition += Vector3Int.right;
        else if (plLastMoveX == -1) tilePosition += Vector3Int.left;
        else if (plLastMoveY == 1) tilePosition += Vector3Int.up;
        else if (plLastMoveY == -1) tilePosition += Vector3Int.down;
        else return;
    }
}
