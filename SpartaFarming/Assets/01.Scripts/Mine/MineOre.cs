using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MineOre : MonoBehaviour
{
    public Tilemap oreTilemap;
    //public Action<Vector3Int> onMine; 플레이어에서 선언
    //플레이어에서 광질 시 onMine?.Invoke(tilePosition); 
    /*
     *                 if (curTool.CompareTag("Axe"))
                {
                    if (plLastMoveX == 1) objectMap.SetTile(objGridPos + Vector3Int.right, null);
                    else if (plLastMoveX == -1) objectMap.SetTile(objGridPos + Vector3Int.left, null);
                    else if (plLastMoveY == 1) objectMap.SetTile(objGridPos + Vector3Int.up, null);
                    else if (plLastMoveY == -1) objectMap.SetTile(objGridPos + Vector3Int.down, null);
                    else return;
                }

    이부분을
                    if (curTool.CompareTag("Axe"))
                {
                    Vector3Int tilePosition = objGridPpos;

                    if (plLastMoveX == 1) targetPosition += Vector3Int.right
                    else if (plLastMoveX == -1) targetPosition += Vector3Int.left
                    else if (plLastMoveY == 1) targetPosition += Vector3Int.up
                    else if (plLastMoveY == -1) targetPosition += Vector3Int.down
                    else return;

                    onMine?.Invoke(tilePosition);
                }

     */

    private Dictionary<Vector3Int, OreTile> destroyedTiles = new Dictionary<Vector3Int, OreTile>();

    private void Start()
    {
        oreTilemap = GetComponent<Tilemap>();
        TimeManager.Instance.TimeSystem.On8oClock += RespawnOre;
        //GameManager.Instance.Player.onMine += CheckOreTile;
    }

    void CheckOreTile(Vector3Int tilePosition)
    {
        TileBase targetTile = oreTilemap.GetTile(tilePosition);

        if (targetTile is OreTile oreTile)
        {
            oreTilemap.SetTile(tilePosition, null);
            SpawnOreDrop(tilePosition, oreTile.oreDropPrefab);

            destroyedTiles[tilePosition] = oreTile;
        }
    }

    void SpawnOreDrop(Vector3Int tilePosition, GameObject oreDropPrefab)
    {
        if (oreDropPrefab != null)
        {
            Vector3 spawnPos = oreTilemap.GetCellCenterWorld(tilePosition);
            Instantiate(oreDropPrefab, spawnPos, Quaternion.identity);
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
