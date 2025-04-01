using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerCollectSystem : MonoBehaviour
{
    private List<GameObject> targetObjs;

    void Awake()
    {
        targetObjs = new();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (var obj in targetObjs)
            {
                CropObject crop = obj.GetComponent<CropObject>();
                crop.Harvest();
            }
        }

        foreach(var obj in targetObjs)
        {
            if(obj != null && obj.GetComponent<ICollectable>().CanHarvested == true)
            {
                obj.transform.position = Vector2.Lerp(obj.transform.position, transform.position, Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ICollectable collectable))
            if (!targetObjs.Contains(collision.gameObject))
                targetObjs.Add(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out ICollectable collectable))
        {
            targetObjs.Remove(collision.gameObject);
        }
    }
}
