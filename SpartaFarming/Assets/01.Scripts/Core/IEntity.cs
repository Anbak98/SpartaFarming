using UnityEngine;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 게임 내 모든 엔티티의 기본 인터페이스입니다.
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 엔티티의 고유 식별자
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 엔티티의 게임 오브젝트
        /// </summary>
        GameObject GameObject { get; }

        /// <summary>
        /// 엔티티의 변환(Transform) 컴포넌트
        /// </summary>
        Transform Transform { get; }
    }
} 