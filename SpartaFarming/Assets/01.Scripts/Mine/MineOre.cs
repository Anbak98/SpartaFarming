using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class MineOre : MonoBehaviour
{
    public Tilemap oreTilemap;

    private Dictionary<Vector3Int, OreTile> destroyedTiles = new Dictionary<Vector3Int, OreTile>();

    private void Start()
    {
        oreTilemap = GetComponent<Tilemap>();
        TimeManager.Instance.TimeSystem.On8oClock += RespawnOre;
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
        GameObject dropObject = Resources.Load<GameObject>(itemInfo.prefabPath);

        Vector3 spawnPos = oreTilemap.GetCellCenterWorld(tilePosition);
        ItemObject itemObject = Instantiate(dropObject, spawnPos, Quaternion.identity).GetComponent<ItemObject>();
        if (itemObject != null)
        {
            itemObject.SetItem(itemInfo);
        }
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
