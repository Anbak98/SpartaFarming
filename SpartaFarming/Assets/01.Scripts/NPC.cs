using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // 범위 확인용 TriggerEnter
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("상호작용 가능!");            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {            
            GameManager.Instance.Player.Controller.canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("상호작용 가능 범위 나감!");
            GameManager.Instance.Player.Controller.canInteract = false;
        }
    }
}
