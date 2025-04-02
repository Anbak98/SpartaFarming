using UnityEngine;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 게임 내에서 상호작용이 가능한 객체의 인터페이스입니다.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// 상호작용 가능 여부
        /// </summary>
        bool CanInteract { get; }

        /// <summary>
        /// 상호작용 범위
        /// </summary>
        float InteractionRange { get; }

        /// <summary>
        /// 상호작용할 때 표시될 텍스트
        /// </summary>
        string InteractionPrompt { get; }

        /// <summary>
        /// 상호작용을 수행합니다.
        /// </summary>
        /// <param name="interactor">상호작용을 시도하는 객체</param>
        /// <returns>상호작용 성공 여부</returns>
        bool Interact(IEntity interactor);
    }
} 