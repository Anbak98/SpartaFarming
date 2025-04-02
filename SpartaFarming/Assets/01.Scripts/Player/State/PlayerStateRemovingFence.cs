using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerStateRemovingFence : MonoBehaviour, IPlayerState
{
    private float PlLastMoveX;
    private float PlLastMoveY;

    private Vector3 pos;
    private Vector3Int objGridPos;

    private Tilemap ObjectMap;

    public void Enter()
    {
        Debug.Log("울타리 제거 모드!");
        
        ObjectMap = GameManager.Instance.Player.Controller.objectMap;

        GameManager.Instance.Player.Controller.onRemoveFence += RemovingFence;
    }

    public void Exit()
    {
        Debug.Log("울타리 제거 모드 끝!");
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
        
    }

    public void RemovingFence()
    {
        // Axe 들고 있을 때 플레이어 이전 이동방향 따라 앞의 타일맵 오브젝트 파괴
        if (PlLastMoveX == 1) ObjectMap.SetTile(objGridPos + Vector3Int.right, null);
        else if (PlLastMoveX == -1) ObjectMap.SetTile(objGridPos + Vector3Int.left, null);
        else if (PlLastMoveY == 1) ObjectMap.SetTile(objGridPos + Vector3Int.up, null);
        else if (PlLastMoveY == -1) ObjectMap.SetTile(objGridPos + Vector3Int.down, null);
        else return;
    }

    public void HandleAction(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}