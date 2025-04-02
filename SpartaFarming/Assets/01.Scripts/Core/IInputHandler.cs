using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 게임 내 입력 이벤트 유형
    /// </summary>
    public enum InputEventType
    {
        Move,
        Action,
        Interact,
        Inventory,
        Hotkey,
        Equip,
        Menu,
        Map,
        Sprint,
        Cancel
    }

    /// <summary>
    /// 입력 이벤트 데이터를 포함하는 클래스
    /// </summary>
    public class InputEventArgs : EventArgs
    {
        /// <summary>
        /// 입력 이벤트 유형
        /// </summary>
        public InputEventType Type { get; }

        /// <summary>
        /// 입력의 방향 값 (이동 등에 사용)
        /// </summary>
        public Vector2 DirectionValue { get; }

        /// <summary>
        /// 입력의 시작 여부
        /// </summary>
        public bool IsStarted { get; }

        /// <summary>
        /// 입력의 종료 여부
        /// </summary>
        public bool IsCanceled { get; }

        /// <summary>
        /// 원본 입력 콜백 컨텍스트
        /// </summary>
        public InputAction.CallbackContext Context { get; }

        /// <summary>
        /// 생성자
        /// </summary>
        public InputEventArgs(InputEventType type, Vector2 directionValue, bool isStarted, bool isCanceled, InputAction.CallbackContext context)
        {
            Type = type;
            DirectionValue = directionValue;
            IsStarted = isStarted;
            IsCanceled = isCanceled;
            Context = context;
        }
    }

    /// <summary>
    /// 입력 처리를 위한 인터페이스
    /// </summary>
    public interface IInputHandler
    {
        /// <summary>
        /// 입력 이벤트 발생 시 호출되는 이벤트
        /// </summary>
        event EventHandler<InputEventArgs> OnInputEvent;

        /// <summary>
        /// 이동 입력 값
        /// </summary>
        Vector2 MoveInput { get; }

        /// <summary>
        /// 바라보는 방향 (마우스 또는 조이스틱 방향)
        /// </summary>
        Vector2 LookDirection { get; }

        /// <summary>
        /// 스프린트 상태 여부
        /// </summary>
        bool IsSprinting { get; }

        /// <summary>
        /// 현재 입력 시스템을 활성화합니다.
        /// </summary>
        void Enable();

        /// <summary>
        /// 현재 입력 시스템을 비활성화합니다.
        /// </summary>
        void Disable();

        /// <summary>
        /// 특정 입력 액션을 활성화합니다.
        /// </summary>
        /// <param name="inputType">활성화할 입력 유형</param>
        void EnableInputAction(InputEventType inputType);

        /// <summary>
        /// 특정 입력 액션을 비활성화합니다.
        /// </summary>
        /// <param name="inputType">비활성화할 입력 유형</param>
        void DisableInputAction(InputEventType inputType);
    }
} 