using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap oreTilemap;

    private void Awake()
    {
        if (oreTilemap == null)
        {
            Debug.LogError("oreTilemap이 할당되지 않았습니다!");
            return;
        }
    }

    void SpawnOreDrop(Vector3Int tilePosition, int dropItemKey)
    {
        if (oreTilemap == null)
        {
            Debug.LogError("oreTilemap이 null입니다!");
            return;
        }

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
} 