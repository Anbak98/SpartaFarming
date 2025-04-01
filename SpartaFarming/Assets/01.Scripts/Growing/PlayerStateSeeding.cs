using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerStateSeeding : MonoBehaviour, IPlayerState
{
    public bool IsActivated { get; private set; } = false;
    public Tilemap ValidTileMaps;
    public GameObject BuildingTarget;
    public int BuildingWidth = 0;
    public int BuildingHeight = 0;
    public int BuildingKey = 0;

    public void Enter()
    {
        bluePrintPrefab = Resources.Load<GameObject>("Prefab/Blueprint");
        IsActivated = true;
        ValidTileMaps.CompressBounds();

        _tileOffsetX = ValidTileMaps.cellBounds.min.x;
        _tileOffsetY = ValidTileMaps.cellBounds.min.y;

        _bluePrints = new();
        for (int i = 0; i < BuildingWidth * BuildingHeight; i++)
        {
            _bluePrints.Add(Instantiate(bluePrintPrefab));
        }

        _map ??= new int[ValidTileMaps.size.x, ValidTileMaps.size.y];
    }

    public void Exit()
    {
        IsActivated = false;
        BuildingTarget = null;
        foreach (var bluePrint in _bluePrints)
        {
            Destroy(bluePrint);
        }
    }

    public void DoAction()
    {
        if (IsActivated)
            if (CheckIsValidTileWithDrawingBluePrint())
                    BuildOnPosition();
    }

    public void OnUpdate()
    {
        if (IsActivated)
            CheckIsValidTileWithDrawingBluePrint();
    }

    #region PRIVATE
    private List<GameObject> _bluePrints;
    private GameObject bluePrintPrefab;
    private int[,] _map;
    private int _tileOffsetX = 0;
    private int _tileOffsetY = 0;
    private bool _IsValid = false;

    private Vector3 _cellCenter = Vector3.zero;
    private Vector3Int _offsetPosition = Vector3Int.zero;

    private bool CheckIsValidTileWithDrawingBluePrint()
    {
        _IsValid = true;
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = ValidTileMaps.WorldToCell(worldPoint);
        _cellCenter = ValidTileMaps.GetCellCenterLocal(cellPosition);
        _offsetPosition = cellPosition - new Vector3Int(_tileOffsetX, _tileOffsetY);

        BuildingTarget.transform.position = _cellCenter;

        for (int x = 0; x < BuildingWidth; x++)
        {
            for (int y = 0; y < BuildingHeight; y++)
            {
                if (ValidTileMaps.HasTile(cellPosition + new Vector3Int(x, y)) && _map[_offsetPosition.x + x, _offsetPosition.y + y] == 0)
                {
                    _bluePrints[x + y * BuildingWidth].GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 0.5f);
                    _bluePrints[x + y * BuildingWidth].transform.position = _cellCenter + new Vector3Int(x, y);
                }
                else
                {
                    _bluePrints[x + y * BuildingWidth].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);
                    _bluePrints[x + y * BuildingWidth].transform.position = _cellCenter + new Vector3Int(x, y);
                    _IsValid = false;
                }
            }
        }

        return _IsValid;
    }
    private void BuildOnPosition()
    {
        for (int x = 0; x < BuildingWidth; x++)
        {
            for (int y = 0; y < BuildingHeight; y++)
            {
                GameObject obj = Instantiate(BuildingTarget, _cellCenter, Quaternion.identity);
                if (obj.TryGetComponent(out IBuildable build))
                {
                    build.Init(BuildingKey);
                }

                _map[_offsetPosition.x + x, _offsetPosition.y + y] = 1;
            }
        }
    }
    #endregion
}
