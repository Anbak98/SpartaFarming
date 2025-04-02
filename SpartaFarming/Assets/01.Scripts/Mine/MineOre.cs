using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MineOre : MonoBehaviour
{
    public Tilemap oreTilemap;
    //public Action<Vector3Int> onMine; 플레이어에서 선언

    private Dictionary<Vector3Int, OreTile> destroyedTiles = new Dictionary<Vector3Int, OreTile>();

    private void Start()
    {
        oreTilemap = GetComponent<Tilemap>();
        //TimeManager.Instance.TimeSystem.On8oClock += RespawnOre;
        GameManager.Instance.Player.Controller.onMine += CheckOreTile;
    }

    void CheckOreTile(Vector3Int tilePosition)
    {
        TileBase targetTile = oreTilemap.GetTile(tilePosition);

        if (targetTile is OreTile oreTile)
        {
            oreTilemap.SetTile(tilePosition, null);
            SpawnOreDrop(tilePosition, oreTile.dropItemKey);

            destroyedTiles[tilePosition] = oreTile;
        }
    }

    void SpawnOreDrop(Vector3Int tilePosition, int dropItemKey)
    {
        ItemInfo itemInfo = DataManager.ItemLoader.GetByKey(dropItemKey);
        if (itemInfo == null)
        {
            Debug.LogError($"아이템 정보를 찾을 수 없습니다. Key: {dropItemKey}");
            return;
        }
        Debug.Log($"프리팹 경로: {itemInfo.prefabPath}");
        GameObject dropObject = Resources.Load<GameObject>(itemInfo.prefabPath);
        if (dropObject == null)
        {
            Debug.LogError($"프리팹을 찾을 수 없습니다. 경로: {itemInfo.prefabPath}");
            return;
        }
        Vector3 spawnPos = oreTilemap.GetCellCenterWorld(tilePosition);
        Instantiate(dropObject, spawnPos, Quaternion.identity);
        Debug.Log($"아이템 스폰 완료: {itemInfo.name} at {spawnPos}");
    }

    void RespawnOre()
    {
        if (destroyedTiles.Count == 0) return;

        foreach (Vector3Int targetPos in destroyedTiles.Keys)
        {
            oreTilemap.SetTile(targetPos, destroyedTiles[targetPos]);
        }

        destroyedTiles.Clear();
    }
}
