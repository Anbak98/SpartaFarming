using System.Collections.Generic;
using UnityEngine;
using SpartaFarming.Core;

namespace SpartaFarming.Managers
{
    /// <summary>
    /// 게임의 모든 매니저를 관리하는 시스템입니다.
    /// </summary>
    public class ManagerSystem : MonoBehaviour
    {
        #region Serialized Fields
        [Header("매니저 프리팹")]
        [SerializeField] private GameManager m_gameManagerPrefab;
        [SerializeField] private ResourceManager m_resourceManagerPrefab;
        [SerializeField] private UIManager m_uiManagerPrefab;
        [SerializeField] private InputManager m_inputManagerPrefab;
        // 필요한 다른 매니저 프리팹들
        #endregion

        #region Private Fields
        private List<IManager> m_managers = new List<IManager>();
        private bool m_isInitialized = false;
        #endregion

        #region Singleton
        private static ManagerSystem m_instance;
        public static ManagerSystem Instance => m_instance;
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

            // 매니저 시스템 초기화
            InitializeManagers();
        }

        /// <summary>
        /// 애플리케이션 종료 전 호출됩니다.
        /// </summary>
        private void OnApplicationQuit()
        {
            ShutdownManagers();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 특정 타입의 매니저를 가져옵니다.
        /// </summary>
        /// <typeparam name="T">매니저 타입</typeparam>
        /// <returns>매니저 인스턴스 또는 null</returns>
        public T GetManager<T>() where T : IManager
        {
            foreach (var manager in m_managers)
            {
                if (manager is T typedManager)
                {
                    return typedManager;
                }
            }
            return default;
        }

        /// <summary>
        /// 모든 매니저를 초기화합니다.
        /// </summary>
        public void InitializeAllManagers()
        {
            if (!m_isInitialized)
            {
                foreach (var manager in m_managers)
                {
                    manager.Initialize();
                }
                m_isInitialized = true;
                Debug.Log("모든 매니저가 초기화되었습니다.");
            }
        }

        /// <summary>
        /// 모든 매니저를 종료합니다.
        /// </summary>
        public void ShutdownAllManagers()
        {
            if (m_isInitialized)
            {
                foreach (var manager in m_managers)
                {
                    manager.Shutdown();
                }
                m_isInitialized = false;
                Debug.Log("모든 매니저가 종료되었습니다.");
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 매니저들을 초기화합니다.
        /// </summary>
        private void InitializeManagers()
        {
            // 기존 매니저 정리
            m_managers.Clear();

            // 리소스 매니저 생성 (가장 먼저 초기화되어야 함)
            CreateManager(m_resourceManagerPrefab);

            // 게임 매니저 생성
            CreateManager(m_gameManagerPrefab);

            // UI 매니저 생성
            CreateManager(m_uiManagerPrefab);

            // 입력 매니저 생성
            CreateManager(m_inputManagerPrefab);

            // 기타 필요한 매니저 생성
            // ...

            // 모든 매니저 초기화
            InitializeAllManagers();

            Debug.Log($"{m_managers.Count}개의 매니저가 등록되었습니다.");
        }

        /// <summary>
        /// 매니저를 생성하고 등록합니다.
        /// </summary>
        /// <typeparam name="T">매니저 타입</typeparam>
        /// <param name="prefab">매니저 프리팹</param>
        /// <returns>생성된 매니저 인스턴스</returns>
        private T CreateManager<T>(T prefab) where T : MonoBehaviour, IManager
        {
            if (prefab == null)
            {
                Debug.LogWarning($"{typeof(T).Name} 프리팹이 할당되지 않았습니다.");
                return null;
            }

            // 이미 등록된 매니저인지 확인
            T existingManager = GetManager<T>();
            if (existingManager != null)
            {
                return existingManager;
            }

            // 새 매니저 인스턴스 생성
            T managerInstance = Instantiate(prefab, transform);
            managerInstance.name = prefab.name;
            
            // 매니저 등록
            m_managers.Add(managerInstance);
            
            return managerInstance;
        }

        /// <summary>
        /// 모든 매니저를 종료하고 정리합니다.
        /// </summary>
        private void ShutdownManagers()
        {
            ShutdownAllManagers();
            m_managers.Clear();
        }
        #endregion
    }
} 