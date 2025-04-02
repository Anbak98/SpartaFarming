using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerStateRemovingFence : IPlayerState
{
    private float PlLastMoveX;
    private float PlLastMoveY;

    private Vector3 pos;
    private Vector3Int objGridPos;

    private Tilemap ObjectMap;

    public void Enter()
    {
        ObjectMap = GameManager.Instance.Player.Controller.objectMap;
    }

    public void Exit()
    {
        
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
        // Axe 플레이어 이전 이동방향 따라 앞의 타일맵 오브젝트 파괴
        if (GameManager.Instance.Player.Controller.curTool.CompareTag("Axe"))
        {
            if (PlLastMoveX == 1) ObjectMap.SetTile(objGridPos + Vector3Int.right, null);
            else if (PlLastMoveX == -1) ObjectMap.SetTile(objGridPos + Vector3Int.left, null);
            else if (PlLastMoveY == 1) ObjectMap.SetTile(objGridPos + Vector3Int.up, null);
            else if (PlLastMoveY == -1) ObjectMap.SetTile(objGridPos + Vector3Int.down, null);
            else return;
        }
    }
}