using UnityEngine;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 도구의 종류를 정의하는 열거형입니다.
    /// </summary>
    public enum ToolType
    {
        None,
        Axe,
        Hoe,
        Pickaxe,
        FishingRod,
        WateringCan,
        Sword,
        Seed
    }

    /// <summary>
    /// 모든 도구가 구현해야 하는 인터페이스입니다.
    /// </summary>
    public interface ITool
    {
        /// <summary>
        /// 도구의 종류
        /// </summary>
        ToolType Type { get; }

        /// <summary>
        /// 도구의 현재 레벨
        /// </summary>
        int Level { get; }

        /// <summary>
        /// 도구를 사용합니다.
        /// </summary>
        /// <param name="user">도구 사용자</param>
        /// <param name="target">도구 사용 대상(선택적)</param>
        /// <param name="targetPosition">도구 사용 위치(선택적)</param>
        /// <returns>도구 사용 성공 여부</returns>
        bool Use(IEntity user, IEntity target = null, Vector3? targetPosition = null);

        /// <summary>
        /// 도구를 장착합니다.
        /// </summary>
        /// <param name="user">도구 사용자</param>
        void Equip(IEntity user);

        /// <summary>
        /// 도구를 해제합니다.
        /// </summary>
        /// <param name="user">도구 사용자</param>
        void Unequip(IEntity user);

        /// <summary>
        /// 도구의 내구도를 감소시킵니다.
        /// </summary>
        /// <param name="amount">감소시킬 내구도 양</param>
        /// <returns>남은 내구도</returns>
        float ReduceDurability(float amount);

        /// <summary>
        /// 도구를 수리합니다.
        /// </summary>
        /// <param name="amount">회복시킬 내구도 양</param>
        /// <returns>수리 후 내구도</returns>
        float Repair(float amount);

        /// <summary>
        /// 도구가 사용 가능한지 확인합니다.
        /// </summary>
        bool IsUsable { get; }

        /// <summary>
        /// 도구의 현재 내구도
        /// </summary>
        float CurrentDurability { get; }

        /// <summary>
        /// 도구의 최대 내구도
        /// </summary>
        float MaxDurability { get; }

        /// <summary>
        /// 도구를 업그레이드합니다.
        /// </summary>
        /// <returns>업그레이드 성공 여부</returns>
        bool Upgrade();
    }
} 