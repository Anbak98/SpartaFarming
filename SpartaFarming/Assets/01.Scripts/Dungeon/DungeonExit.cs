using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DungeonExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾�� �浹�ϸ� 
        if (collision.gameObject.layer == UnitManager.Instance.PlayerLayerInt) 
        {
            DungeonManager.Instance.dungeonUi.OnOffPanel();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �÷��̾�� �浹�ϸ� 
        if (collision.gameObject.layer == UnitManager.Instance.PlayerLayerInt)
        {
            DungeonManager.Instance.dungeonUi.OnOffPanel();
        }
    }
}
