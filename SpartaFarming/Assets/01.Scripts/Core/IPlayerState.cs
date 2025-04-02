using UnityEngine;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 플레이어 상태 패턴에서 각 상태가 구현해야 하는 인터페이스입니다.
    /// </summary>
    public interface IPlayerState
    {
        /// <summary>
        /// 상태의 고유 식별자
        /// </summary>
        string StateId { get; }

        /// <summary>
        /// 상태가 활성화되었을 때 호출됩니다.
        /// </summary>
        /// <param name="player">플레이어 엔티티</param>
        void Enter(IEntity player);

        /// <summary>
        /// 상태가 비활성화되었을 때 호출됩니다.
        /// </summary>
        /// <param name="player">플레이어 엔티티</param>
        void Exit(IEntity player);

        /// <summary>
        /// 매 프레임 업데이트될 때 호출됩니다.
        /// </summary>
        /// <param name="player">플레이어 엔티티</param>
        void Update(IEntity player);

        /// <summary>
        /// 고정 시간 간격으로 호출되는 물리 업데이트
        /// </summary>
        /// <param name="player">플레이어 엔티티</param>
        void FixedUpdate(IEntity player);

        /// <summary>
        /// 주요 액션이 수행될 때 호출됩니다.
        /// </summary>
        /// <param name="player">플레이어 엔티티</param>
        void DoAction(IEntity player);

        /// <summary>
        /// 입력 이벤트가 발생했을 때 호출됩니다.
        /// </summary>
        /// <param name="player">플레이어 엔티티</param>
        /// <param name="args">입력 이벤트 데이터</param>
        void HandleInput(IEntity player, InputEventArgs args);

        /// <summary>
        /// 현재 상태에서 해당 입력을 처리할 수 있는지 확인합니다.
        /// </summary>
        /// <param name="inputType">확인할 입력 유형</param>
        /// <returns>입력 처리 가능 여부</returns>
        bool CanHandleInput(InputEventType inputType);

        /// <summary>
        /// 애니메이션 이벤트가 발생했을 때 호출됩니다.
        /// </summary>
        /// <param name="player">플레이어 엔티티</param>
        /// <param name="animationEvent">애니메이션 이벤트 이름</param>
        void OnAnimationEvent(IEntity player, string animationEvent);
    }
} 