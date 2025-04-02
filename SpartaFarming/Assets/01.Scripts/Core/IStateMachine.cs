namespace SpartaFarming.Core
{
    /// <summary>
    /// 상태 머신을 위한 일반적인 인터페이스입니다.
    /// </summary>
    /// <typeparam name="TState">상태 타입</typeparam>
    /// <typeparam name="TOwner">상태 머신 소유자 타입</typeparam>
    public interface IStateMachine<TState, TOwner>
        where TState : class
        where TOwner : class
    {
        /// <summary>
        /// 현재 활성화된 상태
        /// </summary>
        TState CurrentState { get; }

        /// <summary>
        /// 이전 상태
        /// </summary>
        TState PreviousState { get; }

        /// <summary>
        /// 상태 머신 소유자
        /// </summary>
        TOwner Owner { get; }

        /// <summary>
        /// 지정된 상태로 전환합니다.
        /// </summary>
        /// <param name="newState">새로운 상태</param>
        /// <returns>상태 전환 성공 여부</returns>
        bool ChangeState(TState newState);

        /// <summary>
        /// 이전 상태로 되돌아갑니다.
        /// </summary>
        /// <returns>상태 전환 성공 여부</returns>
        bool RevertToPreviousState();

        /// <summary>
        /// 현재 상태를 초기화합니다.
        /// </summary>
        void ClearState();

        /// <summary>
        /// 새로운 상태를 등록합니다.
        /// </summary>
        /// <param name="state">등록할 상태</param>
        /// <returns>등록 성공 여부</returns>
        bool RegisterState(TState state);

        /// <summary>
        /// 상태를 제거합니다.
        /// </summary>
        /// <param name="state">제거할 상태</param>
        /// <returns>제거 성공 여부</returns>
        bool UnregisterState(TState state);

        /// <summary>
        /// 상태를 가져옵니다.
        /// </summary>
        /// <typeparam name="T">가져올 상태 타입</typeparam>
        /// <returns>요청한 타입의 상태</returns>
        T GetState<T>() where T : class, TState;
    }
} 