using System;
using System.Collections.Generic;
using UnityEngine;
using SpartaFarming.Core;

namespace SpartaFarming.Managers
{
    /// <summary>
    /// 리소스 로딩 및 관리를 담당하는 매니저입니다.
    /// </summary>
    public class ResourceManager : MonoBehaviour, IManager
    {
        #region Private Fields
        private Dictionary<string, UnityEngine.Object> m_resources = new Dictionary<string, UnityEngine.Object>();
        private bool m_isInitialized = false;
        #endregion

        #region Singleton
        private static ResourceManager m_instance;
        public static ResourceManager Instance => m_instance;
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
        }
        #endregion

        #region IManager Implementation
        /// <summary>
        /// 리소스 매니저를 초기화합니다.
        /// </summary>
        public void Initialize()
        {
            if (m_isInitialized)
                return;

            m_resources.Clear();
            
            // 자주 사용되는 리소스 미리 로드
            PreloadResources();
            
            m_isInitialized = true;
            Debug.Log("ResourceManager가 초기화되었습니다.");
        }

        /// <summary>
        /// 매니저가 활성화될 때 호출됩니다.
        /// </summary>
        public void OnEnable()
        {
            // 필요한 경우 구현
        }

        /// <summary>
        /// 매니저가 비활성화될 때 호출됩니다.
        /// </summary>
        public void OnDisable()
        {
            // 필요한 경우 구현
        }

        /// <summary>
        /// 매니저를 종료합니다.
        /// </summary>
        public void Shutdown()
        {
            ClearCache();
            m_isInitialized = false;
            Debug.Log("ResourceManager가 종료되었습니다.");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Resources 폴더에서 리소스를 로드합니다.
        /// </summary>
        /// <typeparam name="T">로드할 리소스 타입</typeparam>
        /// <param name="path">리소스 경로</param>
        /// <param name="cache">캐시 사용 여부</param>
        /// <returns>로드된 리소스</returns>
        public T Load<T>(string path, bool cache = true) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("로드할 리소스 경로가 비어 있습니다.");
                return null;
            }

            // 캐시에서 리소스 찾기
            string key = $"{typeof(T).Name}_{path}";
            if (m_resources.TryGetValue(key, out UnityEngine.Object resource))
            {
                return resource as T;
            }

            // 리소스 로드
            T loadedResource = Resources.Load<T>(path);
            if (loadedResource == null)
            {
                Debug.LogWarning($"리소스를 찾을 수 없습니다: {path}");
                return null;
            }

            // 캐시에 저장
            if (cache)
            {
                m_resources.Add(key, loadedResource);
            }

            return loadedResource;
        }

        /// <summary>
        /// Resources 폴더에서 여러 리소스를 로드합니다.
        /// </summary>
        /// <typeparam name="T">로드할 리소스 타입</typeparam>
        /// <param name="path">리소스 폴더 경로</param>
        /// <param name="cache">캐시 사용 여부</param>
        /// <returns>로드된 리소스 배열</returns>
        public T[] LoadAll<T>(string path, bool cache = true) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("로드할 리소스 경로가 비어 있습니다.");
                return Array.Empty<T>();
            }

            // 리소스 로드
            T[] loadedResources = Resources.LoadAll<T>(path);
            if (loadedResources == null || loadedResources.Length == 0)
            {
                Debug.LogWarning($"리소스를 찾을 수 없습니다: {path}");
                return Array.Empty<T>();
            }

            // 캐시에 저장
            if (cache)
            {
                foreach (T resource in loadedResources)
                {
                    string key = $"{typeof(T).Name}_{path}/{resource.name}";
                    if (!m_resources.ContainsKey(key))
                    {
                        m_resources.Add(key, resource);
                    }
                }
            }

            return loadedResources;
        }

        /// <summary>
        /// 프리팹을 인스턴스화합니다.
        /// </summary>
        /// <param name="path">프리팹 경로</param>
        /// <param name="parent">부모 트랜스폼(선택적)</param>
        /// <returns>인스턴스화된 게임 오브젝트</returns>
        public GameObject Instantiate(string path, Transform parent = null)
        {
            GameObject prefab = Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"인스턴스화할 프리팹을 찾을 수 없습니다: {path}");
                return null;
            }

            return Instantiate(prefab, parent);
        }

        /// <summary>
        /// 프리팹을 인스턴스화합니다.
        /// </summary>
        /// <param name="prefab">프리팹 오브젝트</param>
        /// <param name="parent">부모 트랜스폼(선택적)</param>
        /// <returns>인스턴스화된 게임 오브젝트</returns>
        public GameObject Instantiate(GameObject prefab, Transform parent = null)
        {
            if (prefab == null)
            {
                Debug.LogWarning("인스턴스화할 프리팹이 null입니다.");
                return null;
            }

            GameObject instance = UnityEngine.Object.Instantiate(prefab, parent);
            instance.name = prefab.name;
            return instance;
        }

        /// <summary>
        /// 지정된 위치에 프리팹을 인스턴스화합니다.
        /// </summary>
        /// <param name="path">프리팹 경로</param>
        /// <param name="position">생성 위치</param>
        /// <param name="rotation">생성 회전값</param>
        /// <returns>인스턴스화된 게임 오브젝트</returns>
        public GameObject Instantiate(string path, Vector3 position, Quaternion rotation)
        {
            GameObject prefab = Load<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"인스턴스화할 프리팹을 찾을 수 없습니다: {path}");
                return null;
            }

            GameObject instance = UnityEngine.Object.Instantiate(prefab, position, rotation);
            instance.name = prefab.name;
            return instance;
        }

        /// <summary>
        /// 게임 오브젝트를 파괴합니다.
        /// </summary>
        /// <param name="gameObject">파괴할 게임 오브젝트</param>
        public void Destroy(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            UnityEngine.Object.Destroy(gameObject);
        }

        /// <summary>
        /// 게임 오브젝트를 지정된 시간 후 파괴합니다.
        /// </summary>
        /// <param name="gameObject">파괴할 게임 오브젝트</param>
        /// <param name="delay">지연 시간(초)</param>
        public void Destroy(GameObject gameObject, float delay)
        {
            if (gameObject == null)
                return;

            UnityEngine.Object.Destroy(gameObject, delay);
        }

        /// <summary>
        /// 리소스 캐시를 비웁니다.
        /// </summary>
        public void ClearCache()
        {
            m_resources.Clear();
            Resources.UnloadUnusedAssets();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 자주 사용되는 리소스를 미리 로드합니다.
        /// </summary>
        private void PreloadResources()
        {
            // UI 관련 리소스
            LoadAll<Sprite>("UI/Icons");
            
            // 효과음
            LoadAll<AudioClip>("Sounds/SFX");
            
            // 기타 필요한 리소스
            // ...
        }
        #endregion
    }
} 