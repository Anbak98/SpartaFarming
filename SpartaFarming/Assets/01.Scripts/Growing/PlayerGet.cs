using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGet : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if(collision.TryGetComponent(out ICollectable collectable))
        {
            if (collectable.CanHarvested)
            {
                inventory.AddNewItem(3001);
                Destroy(collision.gameObject);
            }
        }
    }
}
