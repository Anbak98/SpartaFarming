using System.Collections.Generic;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 인벤토리 시스템을 위한 인터페이스입니다.
    /// </summary>
    public interface IInventorySystem
    {
        /// <summary>
        /// 인벤토리에 아이템을 추가합니다.
        /// </summary>
        /// <param name="item">추가할 아이템</param>
        /// <param name="quantity">추가할 수량</param>
        /// <returns>실제로 추가된 수량</returns>
        int AddItem(ItemInstance item, int quantity = 1);

        /// <summary>
        /// 인벤토리에서 아이템을 제거합니다.
        /// </summary>
        /// <param name="itemId">제거할 아이템의 ID</param>
        /// <param name="quantity">제거할 수량</param>
        /// <returns>실제로 제거된 수량</returns>
        int RemoveItem(string itemId, int quantity = 1);

        /// <summary>
        /// 지정된 아이템을 인벤토리에서 사용합니다.
        /// </summary>
        /// <param name="itemId">사용할 아이템의 ID</param>
        /// <returns>아이템 사용 성공 여부</returns>
        bool UseItem(string itemId);

        /// <summary>
        /// 인벤토리에 있는 특정 아이템의 수량을 확인합니다.
        /// </summary>
        /// <param name="itemId">확인할 아이템의 ID</param>
        /// <returns>해당 아이템의 수량</returns>
        int GetItemQuantity(string itemId);

        /// <summary>
        /// 인벤토리에 특정 아이템이 있는지 확인합니다.
        /// </summary>
        /// <param name="itemId">확인할 아이템의 ID</param>
        /// <param name="minQuantity">필요한 최소 수량</param>
        /// <returns>충분한 수량이 있는지 여부</returns>
        bool HasItem(string itemId, int minQuantity = 1);

        /// <summary>
        /// 현재 인벤토리의 모든 아이템을 가져옵니다.
        /// </summary>
        /// <returns>아이템 목록</returns>
        IReadOnlyList<ItemInstance> GetAllItems();

        /// <summary>
        /// 인벤토리가 비어있는지 확인합니다.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 인벤토리가 가득 찼는지 확인합니다.
        /// </summary>
        bool IsFull { get; }

        /// <summary>
        /// 인벤토리의 현재 아이템 수
        /// </summary>
        int ItemCount { get; }

        /// <summary>
        /// 인벤토리의 최대 용량
        /// </summary>
        int Capacity { get; }
    }
} 