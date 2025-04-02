using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerStateHoeing : MonoBehaviour, IPlayerState
{
    private float PlLastMoveX;
    private float PlLastMoveY;

    private Vector3 pos;
    private Vector3Int objGridPos;
    private Vector3Int FlrGridPos;

    private Tilemap ObjectMap;
    private Tilemap WaterMap;
    private Tilemap FloorMap;
    private TileBase FloorTile;

    public void Enter()
    {
        Debug.Log("땅 다지기 모드!");

        ObjectMap = GameManager.Instance.Player.Controller.objectMap;
        WaterMap = GameManager.Instance.Player.Controller.waterMap;
        FloorMap = GameManager.Instance.Player.Controller.floorMap;
        FloorTile = GameManager.Instance.Player.Controller.floorTile;

        GameManager.Instance.Player.Controller.onHoeing += Hoeing;
    }

    public void Exit()
    {
        Debug.Log("땅 다지기 모드 끝!");
    }

    public void OnUpdate()
    {
        PlLastMoveX = GameManager.Instance.Player.Controller.plLastMoveX;
        PlLastMoveY = GameManager.Instance.Player.Controller.plLastMoveY;

        pos = GameManager.Instance.Player.transform.position;
        objGridPos = GameManager.Instance.Player.Controller.objectMap.WorldToCell(pos);
        FlrGridPos = GameManager.Instance.Player.Controller.floorMap.WorldToCell(pos);
    }

    public void DoAction()
    {
        
    }

    public void Hoeing()
    {
        // Hoe 들고 있을 때 플레이어 이전 이동방향 따라 앞의 땅 파기
        if (PlLastMoveX == 1 && !ObjectMap.HasTile(objGridPos + Vector3Int.right) && !WaterMap.HasTile(objGridPos + Vector3Int.right))
            FloorMap.SetTile(FlrGridPos + Vector3Int.right, FloorTile);
        else if (PlLastMoveX == -1 && !ObjectMap.HasTile(objGridPos + Vector3Int.left) && !WaterMap.HasTile(objGridPos + Vector3Int.left))
            FloorMap.SetTile(FlrGridPos + Vector3Int.left, FloorTile);
        else if (PlLastMoveY == 1 && !ObjectMap.HasTile(objGridPos + Vector3Int.up) && !WaterMap.HasTile(objGridPos + Vector3Int.up))
            FloorMap.SetTile(FlrGridPos + Vector3Int.up, FloorTile);
        else if (PlLastMoveY == -1 && !ObjectMap.HasTile(objGridPos + Vector3Int.down) && !WaterMap.HasTile(objGridPos + Vector3Int.down))
            FloorMap.SetTile(FlrGridPos + Vector3Int.down, FloorTile);
        else return;
    }
}