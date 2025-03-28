using UnityEngine;
using UnityEngine.Tilemaps;

public class YSMTEST : MonoBehaviour
{
    [SerializeField]
    private GameObject building;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private BuildingMod buildingMod;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!buildingMod.IsActivated)
            {
                buildingMod.Activate(building, 3, 2, tilemap);
            }
            else
            {
                buildingMod.Deactivate();
            }
        }
    }
}
