using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingMod : MonoBehaviour
{
    [SerializeField]
    private GameObject bluePrintPrefab;
    public bool IsActivated { get; private set; } = false;

    public void Activate(GameObject buildingObject, int w, int h, Tilemap validTileMap)
    {
        IsActivated = true;
        _buildingTarget = buildingObject;
        _validTileMaps = validTileMap;
        _validTileMaps.CompressBounds();

        tileOffsetX = _validTileMaps.cellBounds.min.x;
        tileOffsetY = _validTileMaps.cellBounds.min.y;

        buildingWidth = w;
        buildingHeight = h;

        _bluePrints = new();
        for (int i = 0; i < w * h; i++)
        {
            _bluePrints.Add(Instantiate(bluePrintPrefab));
        }

        map = new int[validTileMap.size.x, validTileMap.size.y];
    }

    public void Deactivate()
    {
        IsActivated = false;
        _buildingTarget = null;
        foreach (var bluePrint in _bluePrints)
        {
            Destroy(bluePrint);
        }
    }

    public void Update()
    {
        if (IsActivated)
            if (CheckIsValidTileWithDrawingBluePrint())
                if (Input.GetMouseButtonDown(0))
                    BuildOnPosition();
    }

    #region PRIVATE
    private Tilemap _validTileMaps;
    private GameObject _buildingTarget;
    private List<GameObject> _bluePrints;
    private int[,] map;
    private int tileOffsetX = 0;
    private int tileOffsetY = 0;
    private int buildingWidth = 0;
    private int buildingHeight = 0;
    private bool IsValid = false;

    private Vector3 cellCenter = Vector3.zero;
    private Vector3Int offsetPosition = Vector3Int.zero;

    private bool CheckIsValidTileWithDrawingBluePrint()
    {
        IsValid = true;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = _validTileMaps.WorldToCell(worldPoint);
        cellCenter = _validTileMaps.GetCellCenterLocal(cellPosition);
        offsetPosition = cellPosition - new Vector3Int(tileOffsetX, tileOffsetY);

        _buildingTarget.transform.position = cellCenter;

        for (int x = 0; x < buildingWidth; x++)
        {
            for (int y = 0; y < buildingHeight; y++)
            {
                if (_validTileMaps.HasTile(cellPosition + new Vector3Int(x, y)) && map[offsetPosition.x + x, offsetPosition.y + y] == 0)
                {
                    _bluePrints[x + y * buildingWidth].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.5f);
                    _bluePrints[x + y * buildingWidth].transform.position = cellCenter + new Vector3Int(x, y);
                }
                else
                {
                    _bluePrints[x + y * buildingWidth].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
                    _bluePrints[x + y * buildingWidth].transform.position = cellCenter + new Vector3Int(x, y);
                    IsValid = false;
                }
            }
        }

        return IsValid;
    }
    private void BuildOnPosition()
    {
        for (int x = 0; x < buildingWidth; x++)
        {
            for (int y = 0; y < buildingHeight; y++)
            {
                Instantiate(_buildingTarget, cellCenter, Quaternion.identity);
                map[offsetPosition.x + x, offsetPosition.y + y] = 1;
            }
        }
    }
    #endregion
}
