using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DungeonExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌하면 
        if (collision.gameObject.layer == UnitManager.Instance.PlayerLayerInt) 
        {
            Debug.Log("플레이어가 탈출구에 도달했습니다");        
        }
    }
}
