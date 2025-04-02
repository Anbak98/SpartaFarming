using System.Collections.Generic;
using UnityEngine;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 플레이어의 다양한 상태를 관리하는 상태 머신 클래스입니다.
    /// </summary>
    public class PlayerStateMachine : MonoBehaviour, IStateMachine<IPlayerState, IEntity>
    {
        #region Private Fields
        private Dictionary<string, IPlayerState> m_registeredStates = new Dictionary<string, IPlayerState>();
        private IPlayerState m_currentState;
        private IPlayerState m_previousState;
        private IEntity m_owner;
        #endregion

        #region Properties
        /// <summary>
        /// 현재 활성화된 상태
        /// </summary>
        public IPlayerState CurrentState => m_currentState;

        /// <summary>
        /// 이전 상태
        /// </summary>
        public IPlayerState PreviousState => m_previousState;

        /// <summary>
        /// 상태 머신 소유자 (플레이어)
        /// </summary>
        public IEntity Owner => m_owner;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// 컴포넌트 초기화 시 호출됩니다.
        /// </summary>
        private void Awake()
        {
            m_owner = GetComponent<IEntity>();
            if (m_owner == null)
            {
                Debug.LogError("PlayerStateMachine requires an IEntity component on the same GameObject.");
                enabled = false;
                return;
            }

            // 자식 컴포넌트에서 모든 상태 검색하여 등록
            var states = GetComponentsInChildren<IPlayerState>();
            foreach (var state in states)
            {
                RegisterState(state);
            }
        }

        /// <summary>
        /// 매 프레임 호출됩니다.
        /// </summary>
        private void Update()
        {
            m_currentState?.Update(m_owner);
        }

        /// <summary>
        /// 물리 타임스텝마다 호출됩니다.
        /// </summary>
        private void FixedUpdate()
        {
            m_currentState?.FixedUpdate(m_owner);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 지정된 상태로 전환합니다.
        /// </summary>
        /// <param name="newState">새로운 상태</param>
        /// <returns>상태 전환 성공 여부</returns>
        public bool ChangeState(IPlayerState newState)
        {
            if (newState == null)
            {
                Debug.LogWarning("Attempted to change to a null state.");
                return false;
            }

            // 현재 상태와 같으면 무시
            if (m_currentState == newState)
                return true;

            // 현재 상태 종료
            m_currentState?.Exit(m_owner);

            // 이전 상태 저장
            m_previousState = m_currentState;
            m_currentState = newState;

            // 새 상태 시작
            m_currentState.Enter(m_owner);

            return true;
        }

        /// <summary>
        /// 상태 ID로 상태를 변경합니다.
        /// </summary>
        /// <param name="stateId">상태 ID</param>
        /// <returns>상태 전환 성공 여부</returns>
        public bool ChangeState(string stateId)
        {
            if (!m_registeredStates.TryGetValue(stateId, out IPlayerState state))
            {
                Debug.LogWarning($"State not found: {stateId}");
                return false;
            }

            return ChangeState(state);
        }

        /// <summary>
        /// 이전 상태로 되돌아갑니다.
        /// </summary>
        /// <returns>상태 전환 성공 여부</returns>
        public bool RevertToPreviousState()
        {
            if (m_previousState == null)
            {
                Debug.LogWarning("No previous state to revert to.");
                return false;
            }

            return ChangeState(m_previousState);
        }

        /// <summary>
        /// 현재 상태를 초기화합니다.
        /// </summary>
        public void ClearState()
        {
            m_currentState?.Exit(m_owner);
            m_previousState = m_currentState;
            m_currentState = null;
        }

        /// <summary>
        /// 새로운 상태를 등록합니다.
        /// </summary>
        /// <param name="state">등록할 상태</param>
        /// <returns>등록 성공 여부</returns>
        public bool RegisterState(IPlayerState state)
        {
            if (state == null)
            {
                Debug.LogWarning("Attempted to register a null state.");
                return false;
            }

            if (string.IsNullOrEmpty(state.StateId))
            {
                Debug.LogWarning("State ID cannot be null or empty.");
                return false;
            }

            if (m_registeredStates.ContainsKey(state.StateId))
            {
                Debug.LogWarning($"State already registered: {state.StateId}");
                return false;
            }

            m_registeredStates.Add(state.StateId, state);
            return true;
        }

        /// <summary>
        /// 상태를 제거합니다.
        /// </summary>
        /// <param name="state">제거할 상태</param>
        /// <returns>제거 성공 여부</returns>
        public bool UnregisterState(IPlayerState state)
        {
            if (state == null || string.IsNullOrEmpty(state.StateId))
                return false;

            if (state == m_currentState)
                ClearState();

            return m_registeredStates.Remove(state.StateId);
        }

        /// <summary>
        /// 상태를 가져옵니다.
        /// </summary>
        /// <typeparam name="T">가져올 상태 타입</typeparam>
        /// <returns>요청한 타입의 상태</returns>
        public T GetState<T>() where T : class, IPlayerState
        {
            foreach (var state in m_registeredStates.Values)
            {
                if (state is T typedState)
                {
                    return typedState;
                }
            }
            return null;
        }

        /// <summary>
        /// 현재 상태에서 액션을 수행합니다.
        /// </summary>
        public void DoAction()
        {
            m_currentState?.DoAction(m_owner);
        }

        /// <summary>
        /// 입력 이벤트를 처리합니다.
        /// </summary>
        /// <param name="args">입력 이벤트 데이터</param>
        public void HandleInput(InputEventArgs args)
        {
            // 현재 상태가 이 입력 유형을 처리할 수 있는 경우에만 전달
            if (m_currentState != null && m_currentState.CanHandleInput(args.Type))
            {
                m_currentState.HandleInput(m_owner, args);
            }
        }

        /// <summary>
        /// 애니메이션 이벤트를 처리합니다.
        /// </summary>
        /// <param name="animationEvent">애니메이션 이벤트 이름</param>
        public void OnAnimationEvent(string animationEvent)
        {
            m_currentState?.OnAnimationEvent(m_owner, animationEvent);
        }
        #endregion
    }
} 