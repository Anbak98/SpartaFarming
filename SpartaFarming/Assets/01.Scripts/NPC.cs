using UnityEngine;

public class NPC : MonoBehaviour
{
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
            GameManager.Instance.Player.Controller.canInteract = false;
        }
    }
}
