using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayQuickslot : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private StorageUI _quickStorageUI;
    [SerializeField] private Outline[] _outlines;

    [Header("데이터 참조")]
    [SerializeField] private Inventory _inventory;

    [Header("빌딩 참조")]
    [SerializeField] private PlayerStateSeeding _buildingMod;
    [SerializeField] private GameObject _cropPrefab;

    [Header("타일 참조")]
    [SerializeField] private Tilemap _grass;

    [Header("플레이어 참조")]
    [SerializeField] private Player _player;

    [Header("사운드")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClipSelectQuickSlot;

    private ItemInstance _selectedItem;
    private Outline _selectedOutline;

    // Start is called before the first frame update
    void Start()
    {
        // 인벤토리 이벤트 연결
        if (_inventory != null)
        {

            if (_quickStorageUI != null && _inventory.QuickStorage != null)
            {
                _inventory.QuickStorage.onChangeStorage += _quickStorageUI.UpdateUI;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Set(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            Set(2);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            Set(3);
    }

    private void Set(int index)
    {
        --index;
        if (_buildingMod.IsActivated)
            _buildingMod.Exit();
        _selectedItem = _inventory.QuickStorage.GetItemAt(index);

        if (_selectedItem == null)
            return;

        if (_selectedOutline != null)
            _selectedOutline.enabled = false;

        if (_selectedOutline == _outlines[index])
        {
            _selectedOutline = null;
            return;
        }

        _outlines[index].enabled = true;
        _selectedOutline = _outlines[index];
        _audioSource.PlayOneShot(_audioClipSelectQuickSlot);

        switch(_selectedItem?.ItemInfo.itemType)
        {
            case DesignEnums.ItemType.Seed:
                _buildingMod.Enter();
                break;
            case DesignEnums.ItemType.Weapon:
                break;
            case DesignEnums.ItemType.Tool:
                _player.Controller.selectedEquipItemIndex = _selectedItem.ItemInfo.key - 3000;
                break;
        }
    }
}
