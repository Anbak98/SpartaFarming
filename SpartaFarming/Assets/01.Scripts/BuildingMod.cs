using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingMod : MonoBehaviour
{
    [SerializeField]
    private GameObject bluePrintPrefab;
    public bool IsActivated { get; private set; } = false;

    public void Activate(GameObject buildingObject, int buildingKey, int w, int h, Tilemap validTileMap)
    {
        IsActivated = true;
        _buildingTarget = buildingObject;
        _validTileMaps = validTileMap;
        _validTileMaps.CompressBounds();

        _tileOffsetX = _validTileMaps.cellBounds.min.x;
        _tileOffsetY = _validTileMaps.cellBounds.min.y;

        _buildingWidth = w;
        _buildingHeight = h;

        _buildingKey = buildingKey;

        _bluePrints = new();
        for (int i = 0; i < w * h; i++)
        {
            _bluePrints.Add(Instantiate(bluePrintPrefab));
        }

        _map = new int[validTileMap.size.x, validTileMap.size.y];
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

    #region PRIVATE
    private Tilemap _validTileMaps;
    private GameObject _buildingTarget;
    private List<GameObject> _bluePrints;
    private int[,] _map;
    private int _buildingKey = 0;
    private int _tileOffsetX = 0;
    private int _tileOffsetY = 0;
    private int _buildingWidth = 0;
    private int _buildingHeight = 0;
    private bool _IsValid = false;

    private Vector3 _cellCenter = Vector3.zero;
    private Vector3Int _offsetPosition = Vector3Int.zero;

    private void Update()
    {
        if (IsActivated)
            if (CheckIsValidTileWithDrawingBluePrint())
                if (Input.GetMouseButtonDown(0))
                    BuildOnPosition();
    }

    private bool CheckIsValidTileWithDrawingBluePrint()
    {
        _IsValid = true;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = _validTileMaps.WorldToCell(worldPoint);
        _cellCenter = _validTileMaps.GetCellCenterLocal(cellPosition);
        _offsetPosition = cellPosition - new Vector3Int(_tileOffsetX, _tileOffsetY);

        _buildingTarget.transform.position = _cellCenter;

        for (int x = 0; x < _buildingWidth; x++)
        {
            for (int y = 0; y < _buildingHeight; y++)
            {
                if (_validTileMaps.HasTile(cellPosition + new Vector3Int(x, y)) && _map[_offsetPosition.x + x, _offsetPosition.y + y] == 0)
                {
                    _bluePrints[x + y * _buildingWidth].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.5f);
                    _bluePrints[x + y * _buildingWidth].transform.position = _cellCenter + new Vector3Int(x, y);
                }
                else
                {
                    _bluePrints[x + y * _buildingWidth].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
                    _bluePrints[x + y * _buildingWidth].transform.position = _cellCenter + new Vector3Int(x, y);
                    _IsValid = false;
                }
            }
        }

        return _IsValid;
    }
    private void BuildOnPosition()
    {
        for (int x = 0; x < _buildingWidth; x++)
        {
            for (int y = 0; y < _buildingHeight; y++)
            {
                GameObject obj = Instantiate(_buildingTarget, _cellCenter, Quaternion.identity);
                if(obj.TryGetComponent(out IBuildable build))
                {
                    build.Init(_buildingKey);
                }

                _map[_offsetPosition.x + x, _offsetPosition.y + y] = 1;
            }
        }
    }
    #endregion
}
