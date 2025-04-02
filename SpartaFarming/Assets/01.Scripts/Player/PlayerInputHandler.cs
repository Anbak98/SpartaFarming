using System;
using UnityEngine;
using UnityEngine.InputSystem;
using SpartaFarming.Core;

namespace SpartaFarming.Player
{
    /// <summary>
    /// 플레이어의 입력을 처리하는 컴포넌트입니다.
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputHandler : MonoBehaviour, IInputHandler
    {
        #region Events
        /// <summary>
        /// 입력 이벤트가 발생했을 때 호출되는 이벤트
        /// </summary>
        public event EventHandler<InputEventArgs> OnInputEvent;
        #endregion

        #region Properties
        /// <summary>
        /// 현재 이동 입력값
        /// </summary>
        public Vector2 MoveInput { get; private set; }

        /// <summary>
        /// 현재 바라보는 방향 (마우스 위치)
        /// </summary>
        public Vector2 LookDirection { get; private set; }

        /// <summary>
        /// 현재 스프린트 중인지 여부
        /// </summary>
        public bool IsSprinting { get; private set; }
        #endregion

        #region Private Fields
        private PlayerInput m_playerInput;
        private Camera m_mainCamera;
        private bool m_isEnabled = true;
        private bool m_controlsInitialized = false;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// 컴포넌트 초기화 시 호출됩니다.
        /// </summary>
        private void Awake()
        {
            m_playerInput = GetComponent<PlayerInput>();
            m_mainCamera = Camera.main;
            
            if (m_playerInput == null)
            {
                Debug.LogError("PlayerInput 컴포넌트를 찾을 수 없습니다.");
                enabled = false;
                return;
            }
            
            // 입력 액션들을 초기화합니다.
            InitializeInputActions();
        }

        /// <summary>
        /// 컴포넌트가 활성화될 때 호출됩니다.
        /// </summary>
        private void OnEnable()
        {
            if (m_controlsInitialized)
            {
                Enable();
            }
        }

        /// <summary>
        /// 컴포넌트가 비활성화될 때 호출됩니다.
        /// </summary>
        private void OnDisable()
        {
            Disable();
        }

        /// <summary>
        /// 매 프레임 호출됩니다.
        /// </summary>
        private void Update()
        {
            if (!m_isEnabled)
                return;

            UpdateLookDirection();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 입력 처리를 활성화합니다.
        /// </summary>
        public void Enable()
        {
            if (!m_controlsInitialized)
                return;

            m_isEnabled = true;
            m_playerInput.enabled = true;
        }

        /// <summary>
        /// 입력 처리를 비활성화합니다.
        /// </summary>
        public void Disable()
        {
            m_isEnabled = false;
            if (m_playerInput != null)
            {
                m_playerInput.enabled = false;
            }
        }

        /// <summary>
        /// 특정 입력 액션을 활성화합니다.
        /// </summary>
        /// <param name="inputType">활성화할 입력 유형</param>
        public void EnableInputAction(InputEventType inputType)
        {
            if (!m_controlsInitialized)
                return;

            InputAction action = GetInputAction(inputType);
            if (action != null)
            {
                action.Enable();
            }
        }

        /// <summary>
        /// 특정 입력 액션을 비활성화합니다.
        /// </summary>
        /// <param name="inputType">비활성화할 입력 유형</param>
        public void DisableInputAction(InputEventType inputType)
        {
            if (!m_controlsInitialized)
                return;

            InputAction action = GetInputAction(inputType);
            if (action != null)
            {
                action.Disable();
            }
        }
        #endregion

        #region Input Action Callbacks
        /// <summary>
        /// 이동 입력 콜백
        /// </summary>
        /// <param name="context">입력 컨텍스트</param>
        public void OnMove(InputAction.CallbackContext context)
        {
            if (!m_isEnabled)
                return;

            MoveInput = context.ReadValue<Vector2>();
            
            bool isStarted = context.phase == InputActionPhase.Started;
            bool isCanceled = context.phase == InputActionPhase.Canceled;

            var args = new InputEventArgs(
                InputEventType.Move,
                MoveInput,
                isStarted,
                isCanceled,
                context
            );

            OnInputEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 주요 액션 입력 콜백
        /// </summary>
        /// <param name="context">입력 컨텍스트</param>
        public void OnAction(InputAction.CallbackContext context)
        {
            if (!m_isEnabled)
                return;

            bool isStarted = context.phase == InputActionPhase.Started;
            bool isCanceled = context.phase == InputActionPhase.Canceled;

            var args = new InputEventArgs(
                InputEventType.Action,
                Vector2.zero,
                isStarted,
                isCanceled,
                context
            );

            OnInputEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 상호작용 입력 콜백
        /// </summary>
        /// <param name="context">입력 컨텍스트</param>
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!m_isEnabled)
                return;

            bool isStarted = context.phase == InputActionPhase.Started;
            bool isCanceled = context.phase == InputActionPhase.Canceled;

            var args = new InputEventArgs(
                InputEventType.Interact,
                Vector2.zero,
                isStarted,
                isCanceled,
                context
            );

            OnInputEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 인벤토리 입력 콜백
        /// </summary>
        /// <param name="context">입력 컨텍스트</param>
        public void OnInventory(InputAction.CallbackContext context)
        {
            if (!m_isEnabled)
                return;

            bool isStarted = context.phase == InputActionPhase.Started;
            bool isCanceled = context.phase == InputActionPhase.Canceled;

            var args = new InputEventArgs(
                InputEventType.Inventory,
                Vector2.zero,
                isStarted,
                isCanceled,
                context
            );

            OnInputEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 핫키 입력 콜백
        /// </summary>
        /// <param name="context">입력 컨텍스트</param>
        public void OnHotkey(InputAction.CallbackContext context)
        {
            if (!m_isEnabled)
                return;

            bool isStarted = context.phase == InputActionPhase.Started;
            bool isCanceled = context.phase == InputActionPhase.Canceled;

            var args = new InputEventArgs(
                InputEventType.Hotkey,
                Vector2.zero,
                isStarted,
                isCanceled,
                context
            );

            OnInputEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 장착 입력 콜백
        /// </summary>
        /// <param name="context">입력 컨텍스트</param>
        public void OnEquip(InputAction.CallbackContext context)
        {
            if (!m_isEnabled)
                return;

            bool isStarted = context.phase == InputActionPhase.Started;
            bool isCanceled = context.phase == InputActionPhase.Canceled;

            var args = new InputEventArgs(
                InputEventType.Equip,
                Vector2.zero,
                isStarted,
                isCanceled,
                context
            );

            OnInputEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 메뉴 입력 콜백
        /// </summary>
        /// <param name="context">입력 컨텍스트</param>
        public void OnMenu(InputAction.CallbackContext context)
        {
            if (!m_isEnabled)
                return;

            bool isStarted = context.phase == InputActionPhase.Started;
            bool isCanceled = context.phase == InputActionPhase.Canceled;

            var args = new InputEventArgs(
                InputEventType.Menu,
                Vector2.zero,
                isStarted,
                isCanceled,
                context
            );

            OnInputEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 스프린트 입력 콜백
        /// </summary>
        /// <param name="context">입력 컨텍스트</param>
        public void OnSprint(InputAction.CallbackContext context)
        {
            if (!m_isEnabled)
                return;

            IsSprinting = context.phase == InputActionPhase.Started || 
                         context.phase == InputActionPhase.Performed;

            bool isStarted = context.phase == InputActionPhase.Started;
            bool isCanceled = context.phase == InputActionPhase.Canceled;

            var args = new InputEventArgs(
                InputEventType.Sprint,
                Vector2.zero,
                isStarted,
                isCanceled,
                context
            );

            OnInputEvent?.Invoke(this, args);
        }

        /// <summary>
        /// 취소 입력 콜백
        /// </summary>
        /// <param name="context">입력 컨텍스트</param>
        public void OnCancel(InputAction.CallbackContext context)
        {
            if (!m_isEnabled)
                return;

            bool isStarted = context.phase == InputActionPhase.Started;
            bool isCanceled = context.phase == InputActionPhase.Canceled;

            var args = new InputEventArgs(
                InputEventType.Cancel,
                Vector2.zero,
                isStarted,
                isCanceled,
                context
            );

            OnInputEvent?.Invoke(this, args);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 모든 입력 액션을 초기화합니다.
        /// </summary>
        private void InitializeInputActions()
        {
            if (m_playerInput == null)
                return;

            // 입력 액션들에 콜백 함수를 등록합니다.
            RegisterInputCallbacks();
            
            m_controlsInitialized = true;
        }

        /// <summary>
        /// 입력 액션에 콜백 함수를 등록합니다.
        /// </summary>
        private void RegisterInputCallbacks()
        {
            // PlayerInput 컴포넌트에서 이미 인스펙터를 통해 설정된 이벤트를 사용합니다.
            // 직접 코드에서 이벤트를 등록하려면 다음과 같이 할 수 있습니다:

            // m_playerInput.actions["Move"].performed += OnMove;
            // m_playerInput.actions["Move"].canceled += OnMove;
            // m_playerInput.actions["Action"].performed += OnAction;
            // ... 기타 입력 액션들에도 동일하게 등록

            Debug.Log("플레이어 입력 핸들러가 초기화되었습니다.");
        }

        /// <summary>
        /// 카메라를 통해 마우스 위치를 월드 좌표로 변환합니다.
        /// </summary>
        private void UpdateLookDirection()
        {
            if (m_mainCamera == null)
                return;

            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -m_mainCamera.transform.position.z;
            Vector3 worldPosition = m_mainCamera.ScreenToWorldPoint(mousePosition);
            
            LookDirection = (worldPosition - transform.position).normalized;
        }

        /// <summary>
        /// 입력 유형에 해당하는 InputAction을 가져옵니다.
        /// </summary>
        /// <param name="inputType">입력 유형</param>
        /// <returns>InputAction 또는 null</returns>
        private InputAction GetInputAction(InputEventType inputType)
        {
            if (m_playerInput == null || m_playerInput.actions == null)
                return null;
                
            string actionName = inputType.ToString();
            return m_playerInput.actions.FindAction(actionName, false);
        }
        #endregion
    }
} 