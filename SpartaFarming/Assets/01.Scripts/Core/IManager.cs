namespace SpartaFarming.Core
{
    /// <summary>
    /// 모든 매니저 클래스가 구현해야 하는 기본 인터페이스입니다.
    /// </summary>
    public interface IManager
    {
        /// <summary>
        /// 매니저 초기화
        /// </summary>
        void Initialize();

        /// <summary>
        /// 매니저가 활성화될 때
        /// </summary>
        void OnEnable();

        /// <summary>
        /// 매니저가 비활성화될 때
        /// </summary>
        void OnDisable();

        /// <summary>
        /// 매니저 종료
        /// </summary>
        void Shutdown();
    }
} 