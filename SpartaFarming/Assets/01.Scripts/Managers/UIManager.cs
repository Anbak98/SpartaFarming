using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpartaFarming.Core;
using UnityEngine.UI;

namespace SpartaFarming.Managers
{
    /// <summary>
    /// UI 시스템을 관리하는 매니저입니다.
    /// </summary>
    public class UIManager : MonoBehaviour, IManager
    {
        #region Serialized Fields
        [Header("UI 레퍼런스")]
        [SerializeField] private Transform m_canvasRoot;
        [SerializeField] private Transform m_popupRoot;
        [SerializeField] private Transform m_tooltipRoot;
        [SerializeField] private Transform m_notificationRoot;

        [Header("UI 설정")]
        [SerializeField] private float m_fadeSpeed = 0.5f;
        [SerializeField] private float m_notificationDisplayTime = 3.0f;
        #endregion

        #region Private Fields
        private Dictionary<string, GameObject> m_uiPanels = new Dictionary<string, GameObject>();
        private Dictionary<string, GameObject> m_popups = new Dictionary<string, GameObject>();
        private Stack<GameObject> m_activePopups = new Stack<GameObject>();
        private GameObject m_currentTooltip;
        private Coroutine m_fadeCoroutine;
        private Coroutine m_notificationCoroutine;
        private bool m_isInitialized = false;
        #endregion

        #region Events
        /// <summary>
        /// UI 패널이 열릴 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<string> OnPanelOpened;

        /// <summary>
        /// UI 패널이 닫힐 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<string> OnPanelClosed;

        /// <summary>
        /// 팝업이 열릴 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<string> OnPopupOpened;

        /// <summary>
        /// 팝업이 닫힐 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<string> OnPopupClosed;
        #endregion

        #region Singleton
        private static UIManager m_instance;
        public static UIManager Instance => m_instance;
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

            // Canvas 루트가 없는 경우 생성
            if (m_canvasRoot == null)
            {
                GameObject canvasObj = new GameObject("UICanvas");
                Canvas canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
                m_canvasRoot = canvasObj.transform;
                DontDestroyOnLoad(canvasObj);

                // UI 하위 요소 생성
                m_popupRoot = CreateUIRoot("PopupRoot", m_canvasRoot);
                m_tooltipRoot = CreateUIRoot("TooltipRoot", m_canvasRoot);
                m_notificationRoot = CreateUIRoot("NotificationRoot", m_canvasRoot);
            }
        }
        #endregion

        #region IManager Implementation
        /// <summary>
        /// UI 매니저를 초기화합니다.
        /// </summary>
        public void Initialize()
        {
            if (m_isInitialized)
                return;

            // 캐시된 UI 정보 초기화
            m_uiPanels.Clear();
            m_popups.Clear();
            m_activePopups.Clear();

            // 초기 UI 프리팹 로드
            LoadUIResources();

            m_isInitialized = true;
            Debug.Log("UIManager가 초기화되었습니다.");
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
            // 활성화된 모든 UI 요소 닫기
            CloseAllPanels();
            CloseAllPopups();
            HideTooltip();

            m_isInitialized = false;
            Debug.Log("UIManager가 종료되었습니다.");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// UI 패널을 열거나 활성화합니다.
        /// </summary>
        /// <param name="panelName">패널 이름</param>
        /// <param name="fadeIn">페이드 인 효과 사용 여부</param>
        /// <returns>열린 패널 게임 오브젝트</returns>
        public GameObject OpenPanel(string panelName, bool fadeIn = false)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogWarning("열 UI 패널 이름이 비어 있습니다.");
                return null;
            }

            GameObject panel;

            // 이미 로드된 패널인 경우
            if (m_uiPanels.TryGetValue(panelName, out panel))
            {
                panel.SetActive(true);
            }
            else
            {
                // 패널 프리팹 로드
                panel = LoadUIPanel(panelName);
                if (panel == null)
                {
                    Debug.LogWarning($"UI 패널을 찾을 수 없습니다: {panelName}");
                    return null;
                }

                m_uiPanels.Add(panelName, panel);
            }

            // 페이드 인 효과
            if (fadeIn)
            {
                CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = panel.AddComponent<CanvasGroup>();
                }

                if (m_fadeCoroutine != null)
                {
                    StopCoroutine(m_fadeCoroutine);
                }

                m_fadeCoroutine = StartCoroutine(FadeIn(canvasGroup));
            }

            OnPanelOpened?.Invoke(panelName);
            Debug.Log($"UI 패널이 열렸습니다: {panelName}");

            return panel;
        }

        /// <summary>
        /// UI 패널을 닫거나 비활성화합니다.
        /// </summary>
        /// <param name="panelName">패널 이름</param>
        /// <param name="fadeOut">페이드 아웃 효과 사용 여부</param>
        public void ClosePanel(string panelName, bool fadeOut = false)
        {
            if (string.IsNullOrEmpty(panelName))
            {
                Debug.LogWarning("닫을 UI 패널 이름이 비어 있습니다.");
                return;
            }

            if (m_uiPanels.TryGetValue(panelName, out GameObject panel))
            {
                if (fadeOut)
                {
                    CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
                    if (canvasGroup == null)
                    {
                        canvasGroup = panel.AddComponent<CanvasGroup>();
                    }

                    if (m_fadeCoroutine != null)
                    {
                        StopCoroutine(m_fadeCoroutine);
                    }

                    m_fadeCoroutine = StartCoroutine(FadeOut(canvasGroup, () => panel.SetActive(false)));
                }
                else
                {
                    panel.SetActive(false);
                }

                OnPanelClosed?.Invoke(panelName);
                Debug.Log($"UI 패널이 닫혔습니다: {panelName}");
            }
            else
            {
                Debug.LogWarning($"닫을 UI 패널을 찾을 수 없습니다: {panelName}");
            }
        }

        /// <summary>
        /// 모든 UI 패널을 닫습니다.
        /// </summary>
        public void CloseAllPanels()
        {
            foreach (var panel in m_uiPanels.Values)
            {
                if (panel.activeSelf)
                {
                    panel.SetActive(false);
                }
            }

            Debug.Log("모든 UI 패널이 닫혔습니다.");
        }

        /// <summary>
        /// 팝업을 열거나 활성화합니다.
        /// </summary>
        /// <param name="popupName">팝업 이름</param>
        /// <param name="fadeIn">페이드 인 효과 사용 여부</param>
        /// <returns>열린 팝업 게임 오브젝트</returns>
        public GameObject OpenPopup(string popupName, bool fadeIn = true)
        {
            if (string.IsNullOrEmpty(popupName))
            {
                Debug.LogWarning("열 팝업 이름이 비어 있습니다.");
                return null;
            }

            GameObject popup;

            // 이미 로드된 팝업인 경우
            if (m_popups.TryGetValue(popupName, out popup))
            {
                popup.SetActive(true);
            }
            else
            {
                // 팝업 프리팹 로드
                popup = LoadPopup(popupName);
                if (popup == null)
                {
                    Debug.LogWarning($"팝업을 찾을 수 없습니다: {popupName}");
                    return null;
                }

                m_popups.Add(popupName, popup);
            }

            // 페이드 인 효과
            if (fadeIn)
            {
                CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = popup.AddComponent<CanvasGroup>();
                }

                if (m_fadeCoroutine != null)
                {
                    StopCoroutine(m_fadeCoroutine);
                }

                m_fadeCoroutine = StartCoroutine(FadeIn(canvasGroup));
            }

            m_activePopups.Push(popup);
            OnPopupOpened?.Invoke(popupName);
            Debug.Log($"팝업이 열렸습니다: {popupName}");

            return popup;
        }

        /// <summary>
        /// 팝업을 닫거나 비활성화합니다.
        /// </summary>
        /// <param name="popupName">팝업 이름</param>
        /// <param name="fadeOut">페이드 아웃 효과 사용 여부</param>
        public void ClosePopup(string popupName, bool fadeOut = true)
        {
            if (string.IsNullOrEmpty(popupName))
            {
                Debug.LogWarning("닫을 팝업 이름이 비어 있습니다.");
                return;
            }

            if (m_popups.TryGetValue(popupName, out GameObject popup))
            {
                if (m_activePopups.Contains(popup))
                {
                    m_activePopups = new Stack<GameObject>(new Stack<GameObject>(m_activePopups.Where(p => p != popup)));
                }

                if (fadeOut)
                {
                    CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
                    if (canvasGroup == null)
                    {
                        canvasGroup = popup.AddComponent<CanvasGroup>();
                    }

                    if (m_fadeCoroutine != null)
                    {
                        StopCoroutine(m_fadeCoroutine);
                    }

                    m_fadeCoroutine = StartCoroutine(FadeOut(canvasGroup, () => popup.SetActive(false)));
                }
                else
                {
                    popup.SetActive(false);
                }

                OnPopupClosed?.Invoke(popupName);
                Debug.Log($"팝업이 닫혔습니다: {popupName}");
            }
            else
            {
                Debug.LogWarning($"닫을 팝업을 찾을 수 없습니다: {popupName}");
            }
        }

        /// <summary>
        /// 최상위 팝업을 닫습니다.
        /// </summary>
        /// <param name="fadeOut">페이드 아웃 효과 사용 여부</param>
        public void CloseTopPopup(bool fadeOut = true)
        {
            if (m_activePopups.Count > 0)
            {
                GameObject popup = m_activePopups.Pop();
                string popupName = popup.name;

                if (fadeOut)
                {
                    CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
                    if (canvasGroup == null)
                    {
                        canvasGroup = popup.AddComponent<CanvasGroup>();
                    }

                    if (m_fadeCoroutine != null)
                    {
                        StopCoroutine(m_fadeCoroutine);
                    }

                    m_fadeCoroutine = StartCoroutine(FadeOut(canvasGroup, () => popup.SetActive(false)));
                }
                else
                {
                    popup.SetActive(false);
                }

                OnPopupClosed?.Invoke(popupName);
                Debug.Log($"최상위 팝업이 닫혔습니다: {popupName}");
            }
        }

        /// <summary>
        /// 모든 팝업을 닫습니다.
        /// </summary>
        public void CloseAllPopups()
        {
            while (m_activePopups.Count > 0)
            {
                GameObject popup = m_activePopups.Pop();
                popup.SetActive(false);
                OnPopupClosed?.Invoke(popup.name);
            }

            Debug.Log("모든 팝업이 닫혔습니다.");
        }

        /// <summary>
        /// 툴팁을 표시합니다.
        /// </summary>
        /// <param name="tooltipName">툴팁 이름</param>
        /// <param name="position">표시 위치</param>
        /// <returns>생성된 툴팁 게임 오브젝트</returns>
        public GameObject ShowTooltip(string tooltipName, Vector3 position)
        {
            if (string.IsNullOrEmpty(tooltipName))
            {
                Debug.LogWarning("표시할 툴팁 이름이 비어 있습니다.");
                return null;
            }

            // 기존 툴팁 닫기
            HideTooltip();

            // 툴팁 프리팹 로드
            GameObject tooltip = LoadTooltip(tooltipName);
            if (tooltip == null)
            {
                Debug.LogWarning($"툴팁을 찾을 수 없습니다: {tooltipName}");
                return null;
            }

            // 툴팁 위치 설정
            RectTransform rectTransform = tooltip.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.position = position;
            }

            m_currentTooltip = tooltip;
            Debug.Log($"툴팁이 표시되었습니다: {tooltipName}");

            return tooltip;
        }

        /// <summary>
        /// 현재 표시된 툴팁을 숨깁니다.
        /// </summary>
        public void HideTooltip()
        {
            if (m_currentTooltip != null)
            {
                Destroy(m_currentTooltip);
                m_currentTooltip = null;
            }
        }

        /// <summary>
        /// 알림을 표시합니다.
        /// </summary>
        /// <param name="message">알림 메시지</param>
        /// <param name="type">알림 타입 (0: 일반, 1: 성공, 2: 경고, 3: 오류)</param>
        /// <param name="duration">표시 시간 (초)</param>
        public void ShowNotification(string message, int type = 0, float duration = 0)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogWarning("표시할 알림 메시지가 비어 있습니다.");
                return;
            }

            // 기존 알림 타이머 취소
            if (m_notificationCoroutine != null)
            {
                StopCoroutine(m_notificationCoroutine);
            }

            // 알림 UI 생성
            GameObject notification = LoadNotification();
            if (notification == null)
            {
                Debug.LogWarning("알림 프리팹을 찾을 수 없습니다.");
                return;
            }

            // 알림 내용 설정
            Text messageText = notification.GetComponentInChildren<Text>();
            if (messageText != null)
            {
                messageText.text = message;
            }

            // 알림 타입에 따른 스타일 설정
            Image background = notification.GetComponent<Image>();
            if (background != null)
            {
                switch (type)
                {
                    case 1: // 성공
                        background.color = new Color(0.2f, 0.8f, 0.2f, 0.8f);
                        break;
                    case 2: // 경고
                        background.color = new Color(0.8f, 0.8f, 0.2f, 0.8f);
                        break;
                    case 3: // 오류
                        background.color = new Color(0.8f, 0.2f, 0.2f, 0.8f);
                        break;
                    default: // 일반
                        background.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
                        break;
                }
            }

            // 타이머 시작
            float displayTime = duration <= 0 ? m_notificationDisplayTime : duration;
            m_notificationCoroutine = StartCoroutine(HideNotificationAfterDelay(notification, displayTime));

            Debug.Log($"알림이 표시되었습니다: {message}");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// UI 리소스를 로드합니다.
        /// </summary>
        private void LoadUIResources()
        {
            // UI 프리팹 미리 로드
            ResourceManager.Instance.LoadAll<GameObject>("UI/Panels");
            ResourceManager.Instance.LoadAll<GameObject>("UI/Popups");
            ResourceManager.Instance.LoadAll<GameObject>("UI/Tooltips");

            Debug.Log("UI 리소스가 로드되었습니다.");
        }

        /// <summary>
        /// UI 패널 프리팹을 로드합니다.
        /// </summary>
        /// <param name="panelName">패널 이름</param>
        /// <returns>로드된 패널 게임 오브젝트</returns>
        private GameObject LoadUIPanel(string panelName)
        {
            string path = $"UI/Panels/{panelName}";
            GameObject panelPrefab = ResourceManager.Instance.Load<GameObject>(path);
            
            if (panelPrefab == null)
            {
                Debug.LogWarning($"UI 패널 프리팹을 찾을 수 없습니다: {path}");
                return null;
            }

            GameObject panel = Instantiate(panelPrefab, m_canvasRoot);
            panel.name = panelName;
            
            return panel;
        }

        /// <summary>
        /// 팝업 프리팹을 로드합니다.
        /// </summary>
        /// <param name="popupName">팝업 이름</param>
        /// <returns>로드된 팝업 게임 오브젝트</returns>
        private GameObject LoadPopup(string popupName)
        {
            string path = $"UI/Popups/{popupName}";
            GameObject popupPrefab = ResourceManager.Instance.Load<GameObject>(path);
            
            if (popupPrefab == null)
            {
                Debug.LogWarning($"팝업 프리팹을 찾을 수 없습니다: {path}");
                return null;
            }

            GameObject popup = Instantiate(popupPrefab, m_popupRoot);
            popup.name = popupName;
            
            return popup;
        }

        /// <summary>
        /// 툴팁 프리팹을 로드합니다.
        /// </summary>
        /// <param name="tooltipName">툴팁 이름</param>
        /// <returns>로드된 툴팁 게임 오브젝트</returns>
        private GameObject LoadTooltip(string tooltipName)
        {
            string path = $"UI/Tooltips/{tooltipName}";
            GameObject tooltipPrefab = ResourceManager.Instance.Load<GameObject>(path);
            
            if (tooltipPrefab == null)
            {
                Debug.LogWarning($"툴팁 프리팹을 찾을 수 없습니다: {path}");
                return null;
            }

            GameObject tooltip = Instantiate(tooltipPrefab, m_tooltipRoot);
            tooltip.name = tooltipName;
            
            return tooltip;
        }

        /// <summary>
        /// 알림 프리팹을 로드합니다.
        /// </summary>
        /// <returns>로드된 알림 게임 오브젝트</returns>
        private GameObject LoadNotification()
        {
            string path = "UI/Common/Notification";
            GameObject notificationPrefab = ResourceManager.Instance.Load<GameObject>(path);
            
            if (notificationPrefab == null)
            {
                Debug.LogWarning($"알림 프리팹을 찾을 수 없습니다: {path}");
                return null;
            }

            GameObject notification = Instantiate(notificationPrefab, m_notificationRoot);
            notification.name = "Notification";
            
            return notification;
        }

        /// <summary>
        /// UI 루트 객체를 생성합니다.
        /// </summary>
        /// <param name="name">루트 이름</param>
        /// <param name="parent">부모 트랜스폼</param>
        /// <returns>생성된 트랜스폼</returns>
        private Transform CreateUIRoot(string name, Transform parent)
        {
            GameObject root = new GameObject(name);
            root.transform.SetParent(parent, false);
            
            RectTransform rectTransform = root.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            
            return root.transform;
        }

        /// <summary>
        /// 페이드 인 효과를 적용합니다.
        /// </summary>
        /// <param name="canvasGroup">캔버스 그룹</param>
        /// <returns>코루틴</returns>
        private IEnumerator FadeIn(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;
            
            while (canvasGroup.alpha < 1f)
            {
                canvasGroup.alpha += Time.deltaTime * m_fadeSpeed;
                yield return null;
            }
            
            canvasGroup.alpha = 1f;
            m_fadeCoroutine = null;
        }

        /// <summary>
        /// 페이드 아웃 효과를 적용합니다.
        /// </summary>
        /// <param name="canvasGroup">캔버스 그룹</param>
        /// <param name="onComplete">완료 시 콜백</param>
        /// <returns>코루틴</returns>
        private IEnumerator FadeOut(CanvasGroup canvasGroup, Action onComplete = null)
        {
            canvasGroup.alpha = 1f;
            
            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= Time.deltaTime * m_fadeSpeed;
                yield return null;
            }
            
            canvasGroup.alpha = 0f;
            onComplete?.Invoke();
            m_fadeCoroutine = null;
        }

        /// <summary>
        /// 지정된 시간 후 알림을 숨깁니다.
        /// </summary>
        /// <param name="notification">알림 게임 오브젝트</param>
        /// <param name="delay">지연 시간</param>
        /// <returns>코루틴</returns>
        private IEnumerator HideNotificationAfterDelay(GameObject notification, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            if (notification != null)
            {
                CanvasGroup canvasGroup = notification.GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = notification.AddComponent<CanvasGroup>();
                }
                
                yield return StartCoroutine(FadeOut(canvasGroup, () => Destroy(notification)));
            }
            
            m_notificationCoroutine = null;
        }
        #endregion
    }
} 