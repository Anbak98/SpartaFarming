
# 시연 영상
https://youtu.be/nhSb4TZOLsQ
<br>

# ⚙🖥️1. 프로젝트 소계
| 항목 | 내용 |
|------|------|
| 프로젝트 이름| SPARTA FARMING(농사 시뮬레이션) |
| 레퍼런스 게임 | 	스타듀벨리 |
| 게임 설명 | 2D 탑다운 롤플레잉, 농사 / 경영 시뮬레이션 |
| 제작 기간 | 2025.3.27~2025.4.02(약 6일)|

<br>

# 🧑‍🤝‍🧑2. 팀원 및 팀 소개

| 유승민 | 김지현 | 임석준 | 류석민 | 염기용|
|:------|------|------|------|------|
| Programmer/PL | Programmer | Programmer | Programmer | Programmer |

<br>

# 🛠️3. 역할
| 이름 | 역할 |
|------|------|
| 유승민 |  |
| 김지현 | 	<ul><li>Pooling</li><li>던전</li><li>몬스터</li></ul> |
| 임석준 | <ul><li>플레이어</li> |
| 류석민 | <ul><li>날씨</li><li>시간</li><li>광물</li></ul> |
| 염기용 | <ul><li>인벤토리</li> |

<br>

# ⚙️ 4. 개발 환경
- `C#`
- **Unity** `2022.3.17f` 

<br>

# 📌 5. 주요 기능
## 🌳 Pooling
 - Factory 패턴을 사용한 Pooling 유틸리티 제작 
 - 자세한 코드 / 설명은 하위 블로그 참고 ! 
    - [[Unity] Factory 패턴과 Object Pooling](https://youcheachae.tistory.com/69)

## 🎃 몬스터 
 - FSM 과 Brige 패턴을 사용한 몬스터 Ai 제작
 - 자세한 코드 / 설명은 하위 블로그 참고 ! 
    - [[Unity] FSM 패턴과 Brigde 패턴 섞어보기](https://youcheachae.tistory.com/71)

---

# 인벤토리 시스템 🎒

## 개요
인벤토리 시스템은 게임 내에서 플레이어가 수집하고 관리하는 아이템을 처리하는 핵심 시스템입니다. 메인 저장소와 퀵슬롯 저장소를 통해 아이템을 효율적으로 관리하며, 아이템의 획득, 사용, 분할, 스택, 드롭 등 다양한 기능을 제공합니다.

## 주요 구성 요소

### 인벤토리 (Inventory) 📦
플레이어가 소유한 아이템을 관리하는 메인 컴포넌트입니다. 메인 저장소와 퀵슬롯 저장소를 포함하고 있으며, 모든 아이템 관련 작업의 중심점입니다.

```csharp
public class Inventory : MonoBehaviour
{
    [Header("인벤토리 설정")]
    [SerializeField] private int quickStorageSize = 12;      // 퀵슬롯 저장소 크기
    [SerializeField] private int mainStorageSize = 24;       // 메인 저장소 크기 
    [SerializeField] private ItemObject itemObjectPrefab;    // 드롭된 아이템 오브젝트 프리팹
    
    // 저장소 객체
    private Storage _quickStorage;                           // 퀵슬롯 저장소
    private Storage _mainStorage;                            // 메인 저장소
    
    // 이벤트
    public event Action<ItemInstance> onItemAdded;           // 아이템 추가 이벤트
    public event Action<int, int> onItemRemoved;             // 아이템 제거 이벤트 (key, quantity)
    
    // 속성
    public Storage QuickStorage => _quickStorage;            // 퀵슬롯 저장소 접근자
    public Storage MainStorage => _mainStorage;              // 메인 저장소 접근자
    
    // 아이템 관리 메서드
    public bool AddItem(ItemInstance item)
    {
        // 1. 퀵슬롯에 동일한 아이템이 있는지 확인
        // 2. 메인 저장소에 동일한 아이템이 있는지 확인
        // 3. 빈 슬롯을 찾아 아이템 추가
        // 4. 추가 성공 시 onItemAdded 이벤트 발생
        // 5. 성공 여부 반환
    }
    
    public bool AddNewItem(int key, int quantity = 1)
    {
        // 1. 데이터 관리자에서 아이템 정보 가져오기
        // 2. 새 아이템 인스턴스 생성
        // 3. AddItem 메서드 호출하여 인벤토리에 추가
        // 4. 성공 여부 반환
    }
    
    public void DropItem(int key, int quantity = 1)
    {
        // 1. 인벤토리에서 아이템 제거
        // 2. 제거 성공 시 월드에 아이템 오브젝트 생성
        // 3. 생성된 아이템 오브젝트 초기화
    }
    
    public int RemoveItem(int key, int quantity = 1)
    {
        // 1. 퀵슬롯과 메인 저장소에서 해당 키의 아이템 탐색
        // 2. 발견한 아이템의 수량 감소
        // 3. 수량이 0이 되면 슬롯에서 아이템 제거
        // 4. onItemRemoved 이벤트 발생
        // 5. 실제로 제거된 수량 반환
    }
}
```

### 저장소 (Storage) 🗄️
아이템을 실제로 저장하고 관리하는 클래스입니다. 슬롯 기반의 아이템 저장소를 구현하며, 아이템의 추가, 제거, 교체 등의 작업을 처리합니다.

```csharp
public class Storage
{
    private ItemInstance[] _slots;                          // 아이템 슬롯 배열
    
    // 이벤트
    public event Action<Storage> onChangeStorage;           // 저장소 변경 이벤트
    
    // 속성
    public int Size => _slots.Length;                       // 저장소 크기 
    public ItemInstance[] Slots => _slots;                  // 슬롯 배열 접근자
    
    // 생성자
    public Storage(int size)
    {
        // 지정된 크기로 슬롯 배열 초기화
        _slots = new ItemInstance[size];
    }
    
    // 아이템 관리 메서드
    public bool AddItem(ItemInstance item)
    {
        // 1. 동일한 아이템이 있고 스택 가능한지 확인
        // 2. 있으면 수량 증가
        // 3. 없으면 빈 슬롯에 추가
        // 4. 변경사항 알림 및 성공 여부 반환
    }
    
    public bool TryAddItemAt(int slotIndex, ItemInstance item)
    {
        // 1. 지정된 슬롯이 비어있는지 확인
        // 2. 비어있으면 아이템 추가
        // 3. 변경사항 알림 및 성공 여부 반환
    }
    
    public void SetItemAt(int slotIndex, ItemInstance item)
    {
        // 1. 지정된 슬롯에 아이템 설정
        // 2. 변경사항 알림
    }
    
    public ItemInstance GetItemAt(int slotIndex)
    {
        // 지정된 슬롯의 아이템 반환
    }
    
    public void RemoveItemAt(int slotIndex, int quantity = -1)
    {
        // 1. 슬롯의 아이템 수량 감소 또는 완전히 제거
        // 2. 변경사항 알림
    }
    
    public void SwapItem(int slotIndex, ref ItemInstance holdingItem)
    {
        // 1. 들고 있는 아이템과 슬롯의 아이템 교환
        // 2. 변경사항 알림
    }
    
    public int FindFirstEmptySlot()
    {
        // 첫 번째 빈 슬롯 인덱스 반환, 없으면 -1 반환
    }
    
    public int FindSameItem(int itemKey)
    {
        // 동일한 키를 가진 아이템의 슬롯 인덱스 반환, 없으면 -1 반환
    }
}
```

### 아이템 인스턴스 (ItemInstance) 🧩
게임 내에서 실제로 사용되는 아이템의 구체적인 인스턴스를 나타내는 클래스입니다. 수량, 내구도, 장착 상태 등 아이템의 현재 상태를 관리합니다.

```csharp
[Serializable]
public class ItemInstance
{
    // 기본 속성
    [SerializeField] private int _quantity;                 // 아이템 수량
    [SerializeField] private float _currentDurability;      // 현재 내구도
    [SerializeField] private bool _isHolding;               // 플레이어가 들고 있는지 여부
    [SerializeField] private bool _isEquipped;              // 장착 여부

    // 읽기 전용 속성
    public ItemInfo ItemInfo { get; private set; }          // 아이템 기본 정보
    
    // 속성 접근자
    public int Quantity 
    { 
        get => _quantity; 
        set 
        {
            _quantity = value;
            if (_quantity <= 0)
                _quantity = 0;
        } 
    }
    
    public float CurrentDurability 
    { 
        get => _currentDurability; 
        set 
        {
            _currentDurability = value;
            if (_currentDurability < 0)
                _currentDurability = 0;
        } 
    }
    
    public bool IsHolding { get => _isHolding; set => _isHolding = value; }
    public bool IsEquipped { get => _isEquipped; set => _isEquipped = value; }
    
    // 생성자
    public ItemInstance(ItemInfo itemInfo, int quantity = 1)
    {
        // 아이템 정보, 수량 및 내구도 초기화
        ItemInfo = itemInfo;
        Quantity = quantity;
        CurrentDurability = itemInfo.durability;
    }
    
    // 유틸리티 메서드
    public bool IsEmpty => Quantity <= 0;                   // 아이템이 비어있는지 확인
    
    public int AddQuantity(int amount)
    {
        // 1. 아이템 최대 스택 확인
        // 2. 최대치 이상으로 추가하려고 하면 초과분 반환
        // 3. 아닌 경우 수량 증가 후 0 반환
    }
    
    public int RemoveQuantity(int amount)
    {
        // 1. amount가 음수면 전체 수량 제거
        // 2. 수량이 충분하면 지정된 양만큼 제거
        // 3. 수량이 부족하면 가능한 만큼만 제거
        // 4. 실제 제거된 수량 반환
    }
    
    public ItemInstance Clone()
    {
        // 현재 아이템의 복사본 생성 및 반환
    }
}
```

### 아이템 정보 (ItemInfo) 📋
아이템의 기본 정보를 담고 있는 데이터 클래스입니다. 이름, 설명, 유형, 최대 스택 등 아이템의 불변하는 속성을 정의합니다.

```csharp
[Serializable]
public class ItemInfo
{
    public int key;                                        // 아이템 고유 식별자
    public string name;                                    // 아이템 이름
    public string description;                             // 아이템 설명
    public DesignEnums.ItemType itemType;                  // 아이템 유형(도구, 소비품 등)
    public DesignEnums.ItemCategory itemCategory;          // 아이템 카테고리(농사, 채광 등)
    public int maxStack;                                   // 최대 스택 가능 수량
    public float durability;                               // 최대 내구도
    public string spritePath;                              // 아이템 스프라이트 경로
    public int buyPrice;                                   // 구매 가격
    public int sellPrice;                                  // 판매 가격
}
```

### 인벤토리 UI (InventoryUI) 🖥️
인벤토리의 시각적 표현을 담당하는 컴포넌트입니다. 메인 저장소와 퀵슬롯 저장소의 UI를 관리하고, 아이템 드래그 앤 드롭, 클릭 이벤트 등을 처리합니다.

```csharp
public class InventoryUI : MonoBehaviour
{
    [Header("UI 참조")]
    [SerializeField] private StorageUI _mainStorageUI;     // 메인 저장소 UI
    [SerializeField] private StorageUI _quickStorageUI;    // 퀵슬롯 저장소 UI
    [SerializeField] private HoldItemUI _holdItemUI;       // 드래그 중인 아이템 UI
    
    [Header("데이터 참조")]
    [SerializeField] private Inventory _inventory;         // 인벤토리 참조
    
    // 상태
    private ItemInstance _holdItem;                        // 현재 드래그 중인 아이템
    private bool _isHoldingItem = false;                   // 아이템 드래그 중인지 여부
    private bool _isSplitMode = false;                     // 아이템 분할 모드인지 여부
    private bool _isInventoryVisible = false;              // 인벤토리 표시 여부
    
    // 초기화
    private void Start()
    {
        // 이벤트 바인딩 및 UI 초기화
    }
    
    // 아이템 처리 메서드
    public void HandleMainStorageLeftClick(int slotIndex)
    {
        // 1. 분할 모드 확인
        // 2. 아이템을 들고 있는지 확인
        // 3. 상황에 따라 아이템 스왑, 병합, 분할 처리
    }
    
    public void HandleMainStorageRightClick(int slotIndex)
    {
        // 아이템 사용 또는 장착 처리
    }
    
    public void HandleQuickStorageLeftClick(int slotIndex)
    {
        // 메인 저장소와 유사한 로직으로 퀵슬롯 아이템 처리
    }
    
    public void HandleQuickStorageRightClick(int slotIndex)
    {
        // 퀵슬롯 아이템 사용 또는 장착 처리
    }
    
    private void UpdateHoldItemUI()
    {
        // 드래그 중인 아이템 UI 업데이트
    }
    
    private void SplitStackInHalf(ItemInstance sourceItem)
    {
        // 아이템 스택을 반으로 나누어 들고 있는 아이템으로 설정
    }
}
```

### 아이템 슬롯 UI (ItemSlotUI) 🔳
개별 아이템 슬롯의 시각적 표현을 담당하는 컴포넌트입니다. 아이템 아이콘, 수량 표시, 내구도 바 등을 관리합니다.

```csharp
public class ItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    [Header("UI 요소")]
    [SerializeField] private Image _icon;                  // 아이템 아이콘 이미지
    [SerializeField] private TextMeshProUGUI _quantityText; // 수량 텍스트
    [SerializeField] private Image _durabilityBar;         // 내구도 바 이미지
    
    [Header("슬롯 설정")]
    [SerializeField] private int _slotIndex;               // 슬롯 인덱스
    [SerializeField] private StorageUI _parentStorageUI;   // 부모 저장소 UI
    
    // 이벤트
    public event Action<int, PointerEventData> onLeftClick;  // 좌클릭 이벤트
    public event Action<int, PointerEventData> onRightClick; // 우클릭 이벤트
    
    // UI 업데이트 메서드
    public void UpdateUI(ItemInstance item)
    {
        // 1. 아이템이 비어있는지 확인
        // 2. 아이템 아이콘, 수량, 내구도 표시 업데이트
        // 3. 특수 상태(장착 등) 표시
    }
    
    public void DisableUI()
    {
        // 슬롯 UI 비활성화 (아이템 없음)
    }
    
    public void EnableUI()
    {
        // 슬롯 UI 활성화
    }
    
    // 이벤트 처리
    public void OnPointerClick(PointerEventData eventData)
    {
        // 1. 좌클릭/우클릭 구분
        // 2. 해당 이벤트 발생
    }
}
```

### 저장소 UI (StorageUI) 📱
저장소의 전체 UI를 관리하는 컴포넌트입니다. 여러 아이템 슬롯을 그리드 형태로 배치하고 관리합니다.

```csharp
public class StorageUI : MonoBehaviour
{
    [SerializeField] private ItemSlotUI _slotPrefab;       // 슬롯 UI 프리팹
    [SerializeField] private Transform _slotsParent;       // 슬롯들의 부모 트랜스폼
    
    private Storage _storage;                              // 연결된 저장소
    private List<ItemSlotUI> _slotUIs = new List<ItemSlotUI>(); // 슬롯 UI 목록
    
    // 이벤트
    public event Action<int, PointerEventData> onSlotLeftClick;   // 슬롯 좌클릭 이벤트
    public event Action<int, PointerEventData> onSlotRightClick;  // 슬롯 우클릭 이벤트
    
    // 초기화
    public void Initialize(Storage storage)
    {
        // 1. 저장소 참조 설정
        // 2. 슬롯 UI 생성 및 초기화
        // 3. 이벤트 바인딩
    }
    
    // UI 업데이트
    public void UpdateUI()
    {
        // 모든 슬롯 UI 업데이트
    }
    
    // 이벤트 핸들러
    private void OnStorageChanged(Storage storage)
    {
        // 저장소 변경 시 UI 업데이트
    }
    
    private void HandleSlotLeftClick(int slotIndex, PointerEventData eventData)
    {
        // 슬롯 좌클릭 이벤트 전달
    }
    
    private void HandleSlotRightClick(int slotIndex, PointerEventData eventData)
    {
        // 슬롯 우클릭 이벤트 전달
    }
}
```

### 퀵슬롯 시스템 (PlayerQuickslot) ⚡
자주 사용하는 아이템에 빠르게 접근할 수 있는 퀵슬롯 시스템입니다. 단축키를 통해 아이템을 빠르게 선택하고 사용할 수 있습니다.

```csharp
public class PlayerQuickslot : MonoBehaviour
{
    [SerializeField] private Inventory inventory;          // 인벤토리 참조
    [SerializeField] private int selectedSlotIndex;        // 현재 선택된 슬롯 인덱스
    [SerializeField] private Transform quickSlotHighlight; // 선택된 슬롯 하이라이트 UI
    
    // 이벤트
    public event Action<int> onSlotSelected;               // 슬롯 선택 이벤트
    
    // 초기화
    private void Start()
    {
        // 초기 슬롯 선택 및 UI 업데이트
    }
    
    // 퀵슬롯 관련 메서드
    public ItemInstance SetAndGet(int index)
    {
        // 1. 슬롯 선택
        // 2. 선택된 슬롯의 아이템 반환
    }
    
    public void SelectSlot(int index)
    {
        // 1. 유효한 인덱스인지 확인
        // 2. 선택된 슬롯 인덱스 업데이트
        // 3. 하이라이트 UI 위치 업데이트
        // 4. 이벤트 발생
    }
    
    public ItemInstance GetSelectedItem()
    {
        // 현재 선택된 슬롯의 아이템 반환
    }
    
    // 입력 처리
    public void OnHotkeyPressed(int hotkeyIndex)
    {
        // 단축키에 해당하는 슬롯 선택
    }
}
```

## 작동 방식 🔄

### 아이템 획득 과정 🏆
1. 플레이어가 월드에서 아이템을 획득하거나 적을 처치하여 보상을 얻음
2. `Inventory.AddItem` 또는 `Inventory.AddNewItem` 메서드 호출
3. 먼저 퀵슬롯 저장소에 동일한 아이템이 있는지 확인
   - 있으면 스택 가능한지 확인 후 수량 증가
4. 퀵슬롯에 추가할 수 없으면 메인 저장소 확인
   - 동일한 아이템 있으면 스택 시도
   - 없으면 빈 슬롯에 추가
5. 어디에도 추가할 수 없으면 false 반환, 아이템 드롭

### 아이템 관리 프로세스 📊
1. 인벤토리 UI를 통해 아이템을 직관적으로 관리
2. 드래그 앤 드롭으로 아이템 이동
   - 왼쪽 클릭 + 드래그: 아이템 전체 이동
   - Shift + 클릭: 아이템 수량 분할 (반으로)
3. 같은 종류의 아이템을 다른 아이템 위에 드롭하면 스택 시도
4. 인벤토리가 꽉 찼을 때 추가 아이템 획득 시 자동으로 드롭

### 아이템 사용 메커니즘 🔨
1. 퀵슬롯에서 아이템 선택 (숫자 키 1-9)
2. 사용 버튼 (기본 마우스 좌클릭)으로 아이템 사용
3. 아이템 유형에 따라 다른 효과 발생:
   - 도구: 해당 도구 상태로 플레이어 전환
   - 소비품: 즉시 사용하여 효과 적용 (체력/스태미나 회복 등)
   - 장비: 장착하여 능력치 향상
4. 내구도가 있는 아이템은 사용할 때마다 내구도 감소
   - 내구도가 0이 되면 아이템 파괴

### 아이템 분할/스택 로직 ✂️
1. **분할**:
   - Shift + 클릭: 아이템 스택을 반으로 나눔
   - 분할된 절반은 커서를 따라다니는 '홀딩 아이템'으로 표시
   - 원하는 슬롯에 클릭하여 배치
   
2. **스택**:
   - 같은 종류의 아이템 위에 드롭하면 스택 시도
   - 최대 스택을 초과하는 양은 원래 아이템에 남김
   - 서로 다른 아이템일 경우 위치 교환

## 관련 파일 🗂️

- `Assets/01.Scripts/Inventory/Inventory.cs`: 인벤토리 메인 클래스
- `Assets/01.Scripts/Inventory/Storage.cs`: 저장소 클래스
- `Assets/01.Scripts/Item/ItemInstance.cs`: 아이템 인스턴스 클래스
- `Assets/01.Scripts/Item/ItemInfo.cs`: 아이템 정보 클래스
- `Assets/01.Scripts/UI/PlayerMenuUI/UI/InventoryUI.cs`: 인벤토리 UI
- `Assets/01.Scripts/UI/PlayerMenuUI/UI/ItemSlotUI.cs`: 아이템 슬롯 UI
- `Assets/01.Scripts/UI/PlayerMenuUI/UI/StorageUI.cs`: 저장소 UI
- `Assets/01.Scripts/UI/PlayerMenuUI/UI/HoldItemUI.cs`: 드래그 중인 아이템 UI
- `Assets/01.Scripts/UI/PlayerMenuUI/PlayerQuickslot.cs`: 퀵슬롯 시스템 

---
# 상점 시스템 🏪

## 개요
상점 시스템은 게임 내에서 플레이어가 아이템을 구매할 수 있는 핵심 시스템입니다. 플레이어는 상점에서 다양한 씨앗, 도구, 가구 등을 구매하여 게임을 진행할 수 있습니다. 상점은 게임의 경제 시스템을 구성하는 중요한 요소입니다.

## 주요 구성 요소

### 상점 클래스 (Shop) 🛒
상점의 기본 동작을 관리하는 클래스입니다. 판매할 아이템 목록을 가지고 있으며, 플레이어와의 거래를 처리합니다.

```csharp
public class Shop : MonoBehaviour
{
    [field:SerializeField] public List<int> ShopItems { get; private set; }  // 상점에서 판매하는 아이템 ID 리스트

    public void Start()
    {
        UIManager.Instance.OpenShopUI(this);  // 상점 오브젝트가 활성화될 때 상점 UI 열기
    }

    public void BuyItem(Player player, int index)
    {
        if(index >= ShopItems.Count) return;  // 유효하지 않은 인덱스 확인

        ItemInfo itemInfo = DataManager.ItemLoader.GetByKey(ShopItems[index]);  // 아이템 정보 가져오기
        ItemInstance itemInstance = new ItemInstance(itemInfo);  // 아이템 인스턴스 생성
        player.Controller.Inventory.AddItem(itemInstance);  // 플레이어 인벤토리에 아이템 추가
    }
}
```

#### 주요 기능 설명
- **ShopItems**: 상점에서 판매하는 아이템의 ID 목록을 저장하는 리스트입니다. 각 ID는 `DataManager`를 통해 실제 아이템 정보로 변환됩니다.
- **Start()**: 상점 오브젝트가 활성화될 때 호출되며, `UIManager`를 통해 상점 UI를 열어 플레이어에게, 아이템 목록을 표시합니다.
- **BuyItem()**: 플레이어가 아이템을 구매할 때 호출되는 메소드로, 지정된 인덱스의 아이템을 플레이어의 인벤토리에 추가합니다.

### 상점 UI (ShopUI) 📊
상점의 UI 요소를 관리하는 클래스입니다. 판매 중인 아이템을 그리드 형태로 표시하고 구매 버튼 클릭을 처리합니다.

```csharp
public class ShopUI : MonoBehaviour
{
    private Shop _shop;  // 현재 연결된 상점 참조
    private List<ShopItemSlotUI> _itemSlots = new List<ShopItemSlotUI>();  // 아이템 슬롯 UI 목록
    [SerializeField] private ShopItemSlotUI shopItemSlotPrefab;  // 아이템 슬롯 UI 프리팹
    [SerializeField] private Transform shopItemSlotParent;  // 아이템 슬롯들의 부모 Transform
    [SerializeField] private HoldItemUI holdItemSlotUI;  // 드래그 중인 아이템 UI (구현 예정)

    private Player player;  // 플레이어 참조

    private void Awake()
    {
        player = FindObjectOfType<Player>();  // 씬에서 플레이어 찾기
    }

    public void Init(Shop shop)
    {
        _shop = shop;  // 연결된 상점 설정
        UpdateShopItems();  // 상점 아이템 UI 업데이트
    }

    public void UpdateShopItems()
    {
        DestoryShopItemSlots();  // 기존 아이템 슬롯 제거
        CreateShopItemSlots();  // 새 아이템 슬롯 생성
    }

    public void CreateShopItemSlots()
    {
        for(int i = 0; i < _shop.ShopItems.Count; i++)
        {
            ShopItemSlotUI shopItemSlot = Instantiate(shopItemSlotPrefab, shopItemSlotParent);  // 슬롯 프리팹 생성
            shopItemSlot._slotIndex = i;  // 슬롯 인덱스 설정
            shopItemSlot.UpdateUI(_shop.ShopItems[i]);  // 슬롯 UI 업데이트
            _itemSlots.Add(shopItemSlot);  // 슬롯 목록에 추가
        }
    }

    public void DestoryShopItemSlots()
    {
        foreach (var item in _itemSlots)
        {
            Destroy(item.gameObject);  // 기존 슬롯 게임 오브젝트 제거
        }

        _itemSlots.Clear();  // 슬롯 목록 초기화
    }

    public void BuyItem(int index)
    {
        _shop.BuyItem(player, index);  // 연결된 상점의 BuyItem 메소드 호출
    }
}
```

#### 주요 기능 설명
- **Init()**: 상점 UI를 초기화하는 메소드로, 연결된 상점 설정 및 아이템 UI를 업데이트합니다.
- **UpdateShopItems()**: 상점의 아이템 목록이 변경됐을 때 UI를 갱신하는 메소드입니다.
- **CreateShopItemSlots()**: 상점 아이템마다 UI 슬롯을 생성하고 초기화합니다.
- **DestoryShopItemSlots()**: 기존 UI 슬롯을 모두 제거하고 목록을 초기화합니다.
- **BuyItem()**: 특정 슬롯의 아이템 구매를 처리하는 메소드로, 상점 클래스의 구매 로직을 호출합니다.

### 상점 아이템 슬롯 UI (ShopItemSlotUI) 🏷️
상점 UI에서 각 아이템을 표시하는 슬롯 컴포넌트입니다. 아이템 아이콘, 가격 정보를 표시하고 클릭 이벤트를 처리합니다.

```csharp
public class ShopItemSlotUI : MonoBehaviour, IPointerClickHandler
{
    public Image itemIcon;  // 아이템 아이콘 이미지
    public TextMeshProUGUI itemPriceText;  // 아이템 가격 텍스트
    public int _slotIndex;  // 슬롯 인덱스

    private ShopUI shopUI;  // 부모 상점 UI 참조

    private void Awake()
    {
        shopUI = GetComponentInParent<ShopUI>();  // 부모 상점 UI 찾기
    }

    public void UpdateUI(int itemId)
    {
        ItemInfo itemInfo = DataManager.ItemLoader.GetByKey(itemId);  // 아이템 정보 가져오기
        itemIcon.sprite = Resources.Load<Sprite>(itemInfo.spritePath);  // 아이템 아이콘 설정
        itemPriceText.text = itemInfo.price.ToString();  // 아이템 가격 텍스트 설정
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shopUI.BuyItem(_slotIndex);  // 슬롯 클릭 시 구매 처리
    }
}
```

#### 주요 기능 설명
- **UpdateUI()**: 아이템 ID를 기반으로 슬롯의 아이콘과 가격 정보를 업데이트합니다.
- **OnPointerClick()**: 슬롯 클릭 시 호출되며, 부모 상점 UI의 구매 메소드를 호출하여 아이템 구매를 시도합니다.
- **내부 변수**:
  - **itemIcon**: 아이템의 시각적 표현을 위한 이미지 컴포넌트
  - **itemPriceText**: 아이템 가격을 표시하는 텍스트 컴포넌트
  - **_slotIndex**: 이 슬롯이 나타내는 아이템의 인덱스

### UI 매니저 (UIManager) 🖥️
UI 시스템을 총괄 관리하는 싱글톤 클래스입니다. 상점 UI와 플레이어 메뉴 UI의 활성화/비활성화를 제어합니다.

```csharp
public class UIManager : Singleton<UIManager>
{
    public PlayerMenuUI playerMenuUI;  // 플레이어 메뉴 UI 참조
    public ShopUI shopUI;  // 상점 UI 참조

    private void Awake()
    {
        playerMenuUI = FindObjectOfType<PlayerMenuUI>();  // 씬에서 플레이어 메뉴 UI 찾기
        shopUI = FindObjectOfType<ShopUI>();  // 씬에서 상점 UI 찾기
    }

    public void OpenPlayerMenuUI()
    {
        playerMenuUI.OpenBook();  // 플레이어 메뉴 UI 열기
    }

    public void ClosePlayerMenuUI()
    {
        playerMenuUI.CloseBook();  // 플레이어 메뉴 UI 닫기
    }

    public void OpenShopUI(Shop shop)
    {
        shopUI.Init(shop);  // 상점 UI 초기화
        shopUI.gameObject.SetActive(true);  // 상점 UI 활성화
    }

    public void CloseShopUI()
    {
        shopUI.gameObject.SetActive(false);  // 상점 UI 비활성화
    }
}
```

#### 주요 기능 설명
- **Awake()**: 초기화 단계에서 플레이어 메뉴 UI와 상점 UI를 찾아 참조를 설정합니다.
- **OpenPlayerMenuUI()** / **ClosePlayerMenuUI()**: 플레이어 메뉴 UI를 열고 닫는 메소드입니다.
- **OpenShopUI()**: 상점 UI를 초기화하고 활성화하는 메소드로, 특정 상점과 연결합니다.
- **CloseShopUI()**: 상점 UI를 비활성화하는 메소드입니다.

### 아이템 관련 클래스 📦
상점에서 구매할 수 있는 아이템의 정보와 인스턴스를 관리하는 클래스입니다.

```csharp
[Serializable]
public class ItemInfo
{
    public int key;  // 아이템 고유 식별자
    public string name;  // 아이템 이름
    public string description;  // 아이템 설명
    public DesignEnums.ItemType itemType;  // 아이템 유형(도구, 소비품 등)
    public string spritePath;  // 아이템 아이콘 경로
    public string prefabPath;  // 아이템 프리팹 경로
    public int durability;  // 내구도
    public float atk;  // 공격력
    public float speed;  // 속도
    public float hpRecovery;  // 체력 회복량
    public float energyRecovery;  // 에너지 회복량
    public int maxStack;  // 최대 스택 수
    public int price;  // 가격
}

[Serializable]
public class ItemInstance
{
    [SerializeField] private int _quantity;  // 아이템 수량
    [SerializeField] private float _currentDurability;  // 현재 내구도
    [SerializeField] private bool _isHolding;  // 플레이어가 들고 있는지 여부
    [SerializeField] private bool _isEquipped;  // 장착 여부

    public ItemInfo ItemInfo { get; private set; }  // 아이템 기본 정보
    
    public int Quantity 
    { 
        get => _quantity;
        set => _quantity = Mathf.Max(0, value);  // 수량은 최소 0 이상
    }
    
    public ItemInstance(ItemInfo itemInfo, int quantity = 1)
    {
        ItemInfo = itemInfo;  // 아이템 정보 설정
        _quantity = Mathf.Max(1, quantity);  // 수량 초기화 (최소 1)
        _currentDurability = itemInfo.durability;  // 내구도 초기화
        _isHolding = false;  // 들고 있지 않음으로 초기화
        _isEquipped = false;  // 장착하지 않음으로 초기화
    }

    // 기타 아이템 인스턴스 관련 메서드...
}
```

#### 주요 기능 설명
- **ItemInfo**: 아이템의 기본 정보를 정의하는 클래스로, 아이템의 속성과 능력치를 포함합니다.
  - **key**: 아이템을 고유하게 식별하는 ID
  - **itemType**: 도구, 소비품, 장비 등 아이템의 종류
  - **price**: 상점에서의 판매 가격
- **ItemInstance**: 게임 내에서 실제로 사용되는 아이템의 구체적인, 인스턴스를 나타내는 클래스입니다.
  - **생성자**: 아이템 정보와 수량을 받아 인스턴스를 초기화합니다.
  - **Quantity**: 아이템 수량을 관리하는 프로퍼티로, 항상 0 이상의 값을 유지합니다.

## 작동 방식 🔄

### 상점 열기 과정 🚪
1. 플레이어가 상점 NPC와 상호작용하면 상점 게임 오브젝트가 활성화됩니다.
2. `Shop.Start()` 메서드가 호출되어 `UIManager.Instance.OpenShopUI(this)`를 실행합니다.
3. `UIManager.OpenShopUI()` 메서드는 `ShopUI.Init(shop)`을 호출하여 상점 UI와 해당 상점을 연결합니다.
4. `ShopUI.Init()` 메서드는 `UpdateShopItems()`를 호출하여 상점의 아이템 목록을 UI에 표시합니다.
5. 상점 UI가 화면에 활성화되어 플레이어에게 판매 중인 아이템이 표시됩니다.

### 아이템 표시 과정 🖼️
1. `ShopUI.UpdateShopItems()` 메서드는 먼저 `DestoryShopItemSlots()`를 호출하여 기존 아이템 슬롯을 제거합니다.
2. 그런 다음 `CreateShopItemSlots()`를 호출하여 새로운 아이템 슬롯을 생성합니다.
3. 상점의 `ShopItems` 리스트에 있는 각 아이템 ID에 대해 하나의 `ShopItemSlotUI` 인스턴스가 생성됩니다.
4. 각 슬롯의 `UpdateUI()` 메서드가 호출되어 슬롯에 아이템 아이콘, 가격 등의 정보를 표시합니다.
5. 아이템 정보는 `DataManager.ItemLoader.GetByKey()`를 통해 아이템 ID로부터 가져옵니다.

### 아이템 구매 과정 💰
1. 플레이어가 상점 UI에서 아이템 슬롯을 클릭합니다.
2. `ShopItemSlotUI.OnPointerClick()` 메서드가 호출되어 `shopUI.BuyItem(_slotIndex)`를 실행합니다.
3. `ShopUI.BuyItem()` 메서드는 `_shop.BuyItem(player, index)`를 호출합니다.
4. `Shop.BuyItem()` 메서드에서는 다음 과정이 진행됩니다:
   - 인덱스가 유효한지 확인합니다.
   - `DataManager.ItemLoader.GetByKey()`를 통해 아이템 정보를 가져옵니다.
   - 아이템 정보를 바탕으로 새로운 `ItemInstance`를 생성합니다.
   - 플레이어의 인벤토리에 `AddItem()` 메서드를 통해 아이템을 추가합니다.
5. 구매가 성공하면 아이템이 플레이어의 인벤토리에 추가됩니다.

### 상점 닫기 과정 🔒
1. 플레이어가 상점 UI의 닫기 버튼을 클릭하거나 ESC 키를 누릅니다.
2. 이벤트 처리 코드에서 `UIManager.CloseShopUI()` 메서드가 호출됩니다.
3. `UIManager.CloseShopUI()` 메서드는 `shopUI.gameObject.SetActive(false)`를 실행하여 상점 UI를 비활성화합니다.
4. 상점 UI가 화면에서 사라지고 플레이어는 일반 게임플레이로 돌아갑니다.

## 향후 개선 사항 🚀

1. **화폐 시스템 추가**: 현재는 아이템 구매 시 플레이어의 화폐 보유량을 확인하지 않고 무제한 구매가 가능합니다. 향후 화폐 시스템을 추가하여 플레이어가 충분한 화폐를 보유한 경우에만 구매할 수 있도록 개선할 예정입니다.

2. **판매 기능 추가**: 현재는 구매 기능만 구현되어 있습니다. 플레이어가 소유한 아이템을 상점에 판매하는 기능을 추가할 계획입니다.

3. **아이템 카테고리 및 필터링**: 상점 아이템을 카테고리별로 구분하고 필터링할 수 있는 기능을 추가하여 사용자 경험을 개선할 예정입니다.

4. **시즌 및 특별 이벤트 상점**: 게임 내 시간이나 특별 이벤트에 따라 판매하는 아이템이 달라지는 동적 상점 시스템을 구현할 계획입니다.

## 관련 파일 📁

- `Assets/01.Scripts/Shop/Shop.cs`: 상점 클래스
- `Assets/01.Scripts/UI/ShopUI/ShopUI.cs`: 상점 UI 클래스
- `Assets/01.Scripts/UI/ShopUI/ShopItemSlotUI.cs`: 상점 아이템 슬롯 UI 클래스
- `Assets/01.Scripts/Manager/UIManager.cs`: UI 매니저 클래스
- `Assets/01.Scripts/NPC.cs`: NPC 클래스
- `Assets/DesignLoader/ItemInfo.cs`: 아이템 정보 클래스
- `Assets/01.Scripts/Item/ItemInstance.cs`: 아이템 인스턴스 클래스 
