using UnityEngine;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 데미지를 받을 수 있는 엔티티를 위한 인터페이스입니다.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// 현재 체력
        /// </summary>
        float CurrentHealth { get; }

        /// <summary>
        /// 최대 체력
        /// </summary>
        float MaxHealth { get; }

        /// <summary>
        /// 물리적 데미지를 받습니다.
        /// </summary>
        /// <param name="amount">데미지 양</param>
        /// <param name="source">데미지 원천</param>
        /// <returns>실제로 적용된 데미지 양</returns>
        float TakePhysicalDamage(float amount, IEntity source = null);

        /// <summary>
        /// 마법 데미지를 받습니다.
        /// </summary>
        /// <param name="amount">데미지 양</param>
        /// <param name="source">데미지 원천</param>
        /// <returns>실제로 적용된 데미지 양</returns>
        float TakeMagicalDamage(float amount, IEntity source = null);

        /// <summary>
        /// 체력을 회복합니다.
        /// </summary>
        /// <param name="amount">회복량</param>
        /// <param name="source">회복 원천</param>
        /// <returns>실제로 회복된 양</returns>
        float Heal(float amount, IEntity source = null);

        /// <summary>
        /// 엔티티가 현재 살아있는지 확인합니다.
        /// </summary>
        bool IsAlive { get; }
    }
} 