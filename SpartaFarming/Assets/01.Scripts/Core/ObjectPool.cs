using System.Collections.Generic;
using UnityEngine;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 재사용 가능한 게임 오브젝트 관리를 위한 오브젝트 풀 시스템입니다.
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        #region Serialized Fields
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        [SerializeField] private List<Pool> m_pools = new List<Pool>();
        #endregion

        #region Private Fields
        private Dictionary<string, Queue<GameObject>> m_poolDictionary;
        private Dictionary<string, GameObject> m_prefabDictionary;
        private Transform m_poolParent;
        #endregion

        #region Singleton
        private static ObjectPool m_instance;
        public static ObjectPool Instance => m_instance;
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
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            Initialize();
        }

        /// <summary>
        /// 컴포넌트가 파괴될 때 호출됩니다.
        /// </summary>
        private void OnDestroy()
        {
            if (m_instance == this)
            {
                m_instance = null;
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 태그로 오브젝트를 가져옵니다.
        /// </summary>
        /// <param name="tag">오브젝트 풀 태그</param>
        /// <param name="position">생성 위치</param>
        /// <param name="rotation">생성 회전값</param>
        /// <returns>풀에서 가져온 게임 오브젝트</returns>
        public GameObject GetObject(string tag, Vector3 position, Quaternion rotation)
        {
            if (!m_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"오브젝트 풀에 '{tag}' 태그가 존재하지 않습니다.");
                return null;
            }

            Queue<GameObject> pool = m_poolDictionary[tag];

            // 풀에 오브젝트가 없으면 새로 생성
            if (pool.Count == 0)
            {
                if (!m_prefabDictionary.ContainsKey(tag))
                {
                    Debug.LogWarning($"오브젝트 풀의 '{tag}' 태그에 대한 프리팹을 찾을 수 없습니다.");
                    return null;
                }

                GameObject newObject = CreateNewObject(tag);
                return SetupObject(newObject, position, rotation);
            }

            // 풀에서 오브젝트 꺼내기
            GameObject obj = pool.Dequeue();
            return SetupObject(obj, position, rotation);
        }

        /// <summary>
        /// 오브젝트를 풀로 반환합니다.
        /// </summary>
        /// <param name="obj">반환할 게임 오브젝트</param>
        /// <param name="tag">오브젝트 풀 태그</param>
        public void ReturnObject(GameObject obj, string tag)
        {
            if (!m_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"오브젝트 풀에 '{tag}' 태그가 존재하지 않습니다.");
                Destroy(obj);
                return;
            }

            // 풀에 오브젝트 반환
            obj.SetActive(false);
            obj.transform.SetParent(m_poolParent);
            m_poolDictionary[tag].Enqueue(obj);
        }

        /// <summary>
        /// 새로운 풀을 등록합니다.
        /// </summary>
        /// <param name="tag">오브젝트 풀 태그</param>
        /// <param name="prefab">풀에 사용할 프리팹</param>
        /// <param name="size">초기 풀 크기</param>
        public void RegisterPool(string tag, GameObject prefab, int size)
        {
            if (m_poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"이미 '{tag}' 태그를 가진 오브젝트 풀이 존재합니다.");
                return;
            }

            m_poolDictionary[tag] = new Queue<GameObject>();
            m_prefabDictionary[tag] = prefab;

            // 초기 오브젝트 생성
            for (int i = 0; i < size; i++)
            {
                GameObject obj = CreateNewObject(tag);
                obj.SetActive(false);
                m_poolDictionary[tag].Enqueue(obj);
            }
        }

        /// <summary>
        /// 모든 풀을 비웁니다.
        /// </summary>
        public void ClearAllPools()
        {
            foreach (var pool in m_poolDictionary.Values)
            {
                while (pool.Count > 0)
                {
                    GameObject obj = pool.Dequeue();
                    Destroy(obj);
                }
            }

            m_poolDictionary.Clear();
            m_prefabDictionary.Clear();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 오브젝트 풀을 초기화합니다.
        /// </summary>
        private void Initialize()
        {
            m_poolDictionary = new Dictionary<string, Queue<GameObject>>();
            m_prefabDictionary = new Dictionary<string, GameObject>();

            // 풀 부모 오브젝트 생성
            m_poolParent = new GameObject("ObjectPool").transform;
            m_poolParent.SetParent(transform);

            // 미리 정의된 풀 생성
            foreach (Pool pool in m_pools)
            {
                if (string.IsNullOrEmpty(pool.tag) || pool.prefab == null)
                {
                    Debug.LogWarning("유효하지 않은 풀 설정이 있습니다.");
                    continue;
                }

                m_poolDictionary[pool.tag] = new Queue<GameObject>();
                m_prefabDictionary[pool.tag] = pool.prefab;

                // 초기 오브젝트 생성
                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = CreateNewObject(pool.tag);
                    obj.SetActive(false);
                    m_poolDictionary[pool.tag].Enqueue(obj);
                }
            }
        }

        /// <summary>
        /// 풀에 새 오브젝트를 생성합니다.
        /// </summary>
        /// <param name="tag">오브젝트 풀 태그</param>
        /// <returns>생성된 게임 오브젝트</returns>
        private GameObject CreateNewObject(string tag)
        {
            GameObject prefab = m_prefabDictionary[tag];
            GameObject obj = Instantiate(prefab);
            obj.name = $"{tag}_{m_poolDictionary[tag].Count}";
            obj.transform.SetParent(m_poolParent);
            
            // IPoolable 인터페이스가 있으면 초기화
            if (obj.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnCreatedFromPool();
            }

            return obj;
        }

        /// <summary>
        /// 오브젝트를 설정하고 활성화합니다.
        /// </summary>
        /// <param name="obj">설정할 게임 오브젝트</param>
        /// <param name="position">설정할 위치</param>
        /// <param name="rotation">설정할 회전값</param>
        /// <returns>설정된 게임 오브젝트</returns>
        private GameObject SetupObject(GameObject obj, Vector3 position, Quaternion rotation)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            // IPoolable 인터페이스가 있으면 초기화
            if (obj.TryGetComponent<IPoolable>(out var poolable))
            {
                poolable.OnGetFromPool();
            }

            return obj;
        }
        #endregion
    }

    /// <summary>
    /// 오브젝트 풀에서 관리되는 오브젝트의 인터페이스입니다.
    /// </summary>
    public interface IPoolable
    {
        /// <summary>
        /// 오브젝트가 풀에서 처음 생성될 때 호출됩니다.
        /// </summary>
        void OnCreatedFromPool();

        /// <summary>
        /// 오브젝트가 풀에서 가져와질 때 호출됩니다.
        /// </summary>
        void OnGetFromPool();

        /// <summary>
        /// 오브젝트가 풀로 반환될 때 호출됩니다.
        /// </summary>
        void OnReturnToPool();
    }
} 