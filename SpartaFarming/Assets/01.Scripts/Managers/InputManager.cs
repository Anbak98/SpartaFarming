using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using SpartaFarming.Core;

namespace SpartaFarming.Managers
{
    /// <summary>
    /// 게임 입력을 관리하는 매니저입니다.
    /// </summary>
    public class InputManager : MonoBehaviour, IManager
    {
        #region Events
        /// <summary>
        /// 이동 입력이 변경될 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<Vector2> OnMoveInput;

        /// <summary>
        /// 상호작용 입력이 발생할 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action OnInteractInput;

        /// <summary>
        /// 액션 입력이 발생할 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action OnActionInput;

        /// <summary>
        /// 달리기 입력이 변경될 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<bool> OnSprintInput;

        /// <summary>
        /// 인벤토리 토글 입력이 발생할 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action OnInventoryToggleInput;

        /// <summary>
        /// 메뉴 토글 입력이 발생할 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action OnMenuToggleInput;

        /// <summary>
        /// 일시정지 토글 입력이 발생할 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action OnPauseToggleInput;

        /// <summary>
        /// 취소 입력이 발생할 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action OnCancelInput;

        /// <summary>
        /// 핫바 아이템 선택 입력이 발생할 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<int> OnHotbarSelectInput;

        /// <summary>
        /// 마우스 위치가 변경될 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<Vector2> OnMousePositionInput;

        /// <summary>
        /// 마우스 클릭 입력이 발생할 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<bool> OnMouseClickInput;
        #endregion

        #region Private Fields
        private PlayerInput m_playerInput;
        private InputActionMap m_currentActionMap;
        private bool m_isInitialized = false;
        private bool m_inputEnabled = true;

        // 입력 상태
        private Vector2 m_moveInputValue;
        private Vector2 m_mousePosition;
        private bool m_isSprintPressed;
        #endregion

        #region Properties
        /// <summary>
        /// 현재 이동 입력 값을 반환합니다.
        /// </summary>
        public Vector2 MoveInputValue => m_moveInputValue;

        /// <summary>
        /// 현재 마우스 위치를 반환합니다.
        /// </summary>
        public Vector2 MousePosition => m_mousePosition;

        /// <summary>
        /// 스프린트 입력 상태를 반환합니다.
        /// </summary>
        public bool IsSprintPressed => m_isSprintPressed;

        /// <summary>
        /// 입력이 활성화되어 있는지 여부를 반환하거나 설정합니다.
        /// </summary>
        public bool InputEnabled
        {
            get => m_inputEnabled;
            set
            {
                m_inputEnabled = value;
                if (m_inputEnabled)
                {
                    EnableInput();
                }
                else
                {
                    DisableInput();
                }
            }
        }
        #endregion

        #region Singleton
        private static InputManager m_instance;
        public static InputManager Instance => m_instance;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// 컴포넌트 초기화 시 호출됩니다.
        /// </summary>
        private void Awake()
        {
            if (m_instance == null)
            {
                m_instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // PlayerInput 컴포넌트 가져오기
            m_playerInput = GetComponent<PlayerInput>();
            if (m_playerInput == null)
            {
                m_playerInput = gameObject.AddComponent<PlayerInput>();
            }
        }

        /// <summary>
        /// 매 프레임마다 호출됩니다.
        /// </summary>
        private void Update()
        {
            if (m_isInitialized && m_inputEnabled)
            {
                // 마우스 위치 갱신
                m_mousePosition = Mouse.current.position.ReadValue();
                OnMousePositionInput?.Invoke(m_mousePosition);
            }
        }
        #endregion

        #region IManager Implementation
        /// <summary>
        /// 입력 매니저를 초기화합니다.
        /// </summary>
        public void Initialize()
        {
            if (m_isInitialized)
                return;

            // 입력 액션 설정
            SetupInputActions();

            m_isInitialized = true;
            Debug.Log("InputManager가 초기화되었습니다.");
        }

        /// <summary>
        /// 매니저가 활성화될 때 호출됩니다.
        /// </summary>
        public void OnEnable()
        {
            if (m_isInitialized && m_inputEnabled)
            {
                EnableInput();
            }
        }

        /// <summary>
        /// 매니저가 비활성화될 때 호출됩니다.
        /// </summary>
        public void OnDisable()
        {
            DisableInput();
        }

        /// <summary>
        /// 매니저를 종료합니다.
        /// </summary>
        public void Shutdown()
        {
            DisableInput();
            m_isInitialized = false;
            Debug.Log("InputManager가 종료되었습니다.");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 입력을 활성화합니다.
        /// </summary>
        public void EnableInput()
        {
            if (m_playerInput != null)
            {
                m_playerInput.enabled = true;
            }
            m_inputEnabled = true;
        }

        /// <summary>
        /// 입력을 비활성화합니다.
        /// </summary>
        public void DisableInput()
        {
            if (m_playerInput != null)
            {
                m_playerInput.enabled = false;
            }
            m_inputEnabled = false;
        }

        /// <summary>
        /// 게임플레이 입력 맵으로 전환합니다.
        /// </summary>
        public void SwitchToGameplayControls()
        {
            if (m_playerInput != null)
            {
                m_playerInput.SwitchCurrentActionMap("Gameplay");
                m_currentActionMap = m_playerInput.currentActionMap;
            }
        }

        /// <summary>
        /// UI 입력 맵으로 전환합니다.
        /// </summary>
        public void SwitchToUIControls()
        {
            if (m_playerInput != null)
            {
                m_playerInput.SwitchCurrentActionMap("UI");
                m_currentActionMap = m_playerInput.currentActionMap;
            }
        }

        /// <summary>
        /// 대화 입력 맵으로 전환합니다.
        /// </summary>
        public void SwitchToDialogueControls()
        {
            if (m_playerInput != null)
            {
                m_playerInput.SwitchCurrentActionMap("Dialogue");
                m_currentActionMap = m_playerInput.currentActionMap;
            }
        }

        /// <summary>
        /// 특정 입력 액션을 가져옵니다.
        /// </summary>
        /// <param name="actionName">액션 이름</param>
        /// <returns>입력 액션</returns>
        public InputAction GetAction(string actionName)
        {
            if (m_playerInput == null)
                return null;

            return m_playerInput.currentActionMap.FindAction(actionName, false);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 입력 액션을 설정합니다.
        /// </summary>
        private void SetupInputActions()
        {
            if (m_playerInput == null)
                return;

            // Gameplay 액션 맵 가져오기
            var gameplayMap = m_playerInput.actions.FindActionMap("Gameplay", true);
            RegisterActionCallbacks(gameplayMap);

            // UI 액션 맵 가져오기
            var uiMap = m_playerInput.actions.FindActionMap("UI", true);
            RegisterActionCallbacks(uiMap);

            // 기본 액션 맵 설정
            m_playerInput.SwitchCurrentActionMap("Gameplay");
            m_currentActionMap = m_playerInput.currentActionMap;
        }

        /// <summary>
        /// 액션 맵에 콜백을 등록합니다.
        /// </summary>
        /// <param name="actionMap">액션 맵</param>
        private void RegisterActionCallbacks(InputActionMap actionMap)
        {
            if (actionMap == null)
                return;

            // 이동 입력
            var moveAction = actionMap.FindAction("Move", false);
            if (moveAction != null)
            {
                moveAction.performed += ctx => 
                {
                    m_moveInputValue = ctx.ReadValue<Vector2>();
                    OnMoveInput?.Invoke(m_moveInputValue);
                };
                moveAction.canceled += ctx => 
                {
                    m_moveInputValue = Vector2.zero;
                    OnMoveInput?.Invoke(m_moveInputValue);
                };
            }

            // 상호작용 입력
            var interactAction = actionMap.FindAction("Interact", false);
            if (interactAction != null)
            {
                interactAction.performed += ctx => OnInteractInput?.Invoke();
            }

            // 액션 입력
            var actionInputAction = actionMap.FindAction("Action", false);
            if (actionInputAction != null)
            {
                actionInputAction.performed += ctx => OnActionInput?.Invoke();
            }

            // 달리기 입력
            var sprintAction = actionMap.FindAction("Sprint", false);
            if (sprintAction != null)
            {
                sprintAction.performed += ctx => 
                {
                    m_isSprintPressed = true;
                    OnSprintInput?.Invoke(true);
                };
                sprintAction.canceled += ctx => 
                {
                    m_isSprintPressed = false;
                    OnSprintInput?.Invoke(false);
                };
            }

            // 인벤토리 토글 입력
            var inventoryAction = actionMap.FindAction("ToggleInventory", false);
            if (inventoryAction != null)
            {
                inventoryAction.performed += ctx => OnInventoryToggleInput?.Invoke();
            }

            // 메뉴 토글 입력
            var menuAction = actionMap.FindAction("ToggleMenu", false);
            if (menuAction != null)
            {
                menuAction.performed += ctx => OnMenuToggleInput?.Invoke();
            }

            // 일시정지 토글 입력
            var pauseAction = actionMap.FindAction("TogglePause", false);
            if (pauseAction != null)
            {
                pauseAction.performed += ctx => OnPauseToggleInput?.Invoke();
            }

            // 취소 입력
            var cancelAction = actionMap.FindAction("Cancel", false);
            if (cancelAction != null)
            {
                cancelAction.performed += ctx => OnCancelInput?.Invoke();
            }

            // 핫바 선택 입력
            for (int i = 1; i <= 8; i++)
            {
                var hotbarAction = actionMap.FindAction($"Hotbar{i}", false);
                if (hotbarAction != null)
                {
                    int index = i - 1;
                    hotbarAction.performed += ctx => OnHotbarSelectInput?.Invoke(index);
                }
            }

            // 마우스 클릭 입력
            var clickAction = actionMap.FindAction("Click", false);
            if (clickAction != null)
            {
                clickAction.performed += ctx => OnMouseClickInput?.Invoke(true);
                clickAction.canceled += ctx => OnMouseClickInput?.Invoke(false);
            }
        }
        #endregion
    }
} 