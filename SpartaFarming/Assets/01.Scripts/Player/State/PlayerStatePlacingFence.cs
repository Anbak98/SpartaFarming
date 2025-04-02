using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerStatePlacingFence : MonoBehaviour, IPlayerState
{
    private float PlLastMoveX;
    private float PlLastMoveY;

    private Vector3 pos;
    private Vector3Int objGridPos;

    private Tilemap ObjectMap;
    private Tilemap WaterMap;

    private FenceToolUI FenceToolUI;
    private List<FenceToolSlot> FenceToolSlots;
    private List<TileBase> Fences;

    public void Enter()
    {
        Debug.Log("울타리 설치 모드!");

        ObjectMap = GameManager.Instance.Player.Controller.objectMap;
        WaterMap = GameManager.Instance.Player.Controller.waterMap;

        FenceToolUI = GameManager.Instance.Player.Controller.fenceToolUI;
        FenceToolSlots = GameManager.Instance.Player.Controller.fenceToolSlots;
        Fences = GameManager.Instance.Player.Controller.fences;

        GameManager.Instance.Player.Controller.onPlaceFence += PlacingFence;
    }

    public void Exit()
    {
        Debug.Log("울타리 설치 모드 끝!");
    }

    public void OnUpdate()
    {
        PlLastMoveX = GameManager.Instance.Player.Controller.plLastMoveX;
        PlLastMoveY = GameManager.Instance.Player.Controller.plLastMoveY;

        pos = GameManager.Instance.Player.transform.position;
        objGridPos = GameManager.Instance.Player.Controller.objectMap.WorldToCell(pos);
    }

    public void DoAction()
    {
        PlacingFence();
    }

    public void PlacingFence()
    {
        // 선택한 fence 플레이어 이전 이동방향 따라 앞에 타일맵에 그리기
        for (int i = 0; i < FenceToolSlots.Count; i++)
        {
            if (FenceToolSlots[i].isSelected)
            {
                if (PlLastMoveX == 1 && !ObjectMap.HasTile(objGridPos + Vector3Int.right) && !WaterMap.HasTile(objGridPos + Vector3Int.right))
                    ObjectMap.SetTile(objGridPos + Vector3Int.right, Fences[i]);
                else if (PlLastMoveX == -1 && !ObjectMap.HasTile(objGridPos + Vector3Int.left) && !WaterMap.HasTile(objGridPos + Vector3Int.left))
                    ObjectMap.SetTile(objGridPos + Vector3Int.left, Fences[i]);
                else if (PlLastMoveY == 1 && !ObjectMap.HasTile(objGridPos + Vector3Int.up) && !WaterMap.HasTile(objGridPos + Vector3Int.up))
                    ObjectMap.SetTile(objGridPos + Vector3Int.up, Fences[i]);
                else if (PlLastMoveY == -1 && !ObjectMap.HasTile(objGridPos + Vector3Int.down) && !WaterMap.HasTile(objGridPos + Vector3Int.down))
                    ObjectMap.SetTile(objGridPos + Vector3Int.down, Fences[i]);
                else return;
            }
        }
    }

    public void HandleAction(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}