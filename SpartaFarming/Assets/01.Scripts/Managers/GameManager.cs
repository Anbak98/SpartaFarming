using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpartaFarming.Core;
using SpartaFarming.Player;

namespace SpartaFarming.Managers
{
    /// <summary>
    /// 게임의 전반적인 상태와 흐름을 관리하는 매니저입니다.
    /// </summary>
    public class GameManager : MonoBehaviour, IManager
    {
        #region Enums
        /// <summary>
        /// 게임 상태를 나타내는 열거형입니다.
        /// </summary>
        public enum GameState
        {
            None,
            Loading,
            MainMenu,
            InGame,
            Paused,
            GameOver
        }

        /// <summary>
        /// 게임 시간대를 나타내는 열거형입니다.
        /// </summary>
        public enum TimeOfDay
        {
            Morning,
            Afternoon,
            Evening,
            Night
        }
        #endregion

        #region Events
        /// <summary>
        /// 게임 상태가 변경될 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<GameState, GameState> OnGameStateChanged;

        /// <summary>
        /// 게임 시간이 변경될 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<TimeOfDay> OnTimeOfDayChanged;

        /// <summary>
        /// 게임 날짜가 변경될 때 발생하는 이벤트입니다.
        /// </summary>
        public event Action<int, int, int> OnDateChanged; // 년, 계절, 일

        /// <summary>
        /// 게임 시작 시 발생하는 이벤트
        /// </summary>
        public event Action OnGameStart;

        /// <summary>
        /// 게임 일시 정지 시 발생하는 이벤트
        /// </summary>
        public event Action<bool> OnGamePause;

        /// <summary>
        /// 게임 종료 시 발생하는 이벤트
        /// </summary>
        public event Action OnGameEnd;

        /// <summary>
        /// 플레이어가 등록되었을 때 발생하는 이벤트
        /// </summary>
        public event Action<PlayerEntity> OnPlayerRegistered;
        #endregion

        #region Serialized Fields
        [Header("시간 설정")]
        [SerializeField] private float m_realTimePerGameDay = 20.0f; // 현실 20초 = 게임 내 1일
        [SerializeField] private float m_morningStart = 6.0f;
        [SerializeField] private float m_afternoonStart = 12.0f;
        [SerializeField] private float m_eveningStart = 18.0f;
        [SerializeField] private float m_nightStart = 22.0f;

        [Header("계절 설정")]
        [SerializeField] private int m_daysPerSeason = 28;
        #endregion

        #region Private Fields
        private GameState m_currentState = GameState.None;
        private GameState m_previousState = GameState.None;

        private float m_gameTime = 8.0f; // 게임 시작 시간 (08:00)
        private float m_timeMultiplier = 1.0f;
        private TimeOfDay m_currentTimeOfDay = TimeOfDay.Morning;

        private int m_currentYear = 1;
        private int m_currentSeason = 0; // 0: 봄, 1: 여름, 2: 가을, 3: 겨울
        private int m_currentDay = 1;

        private bool m_isTimeFlowing = true;
        private bool m_isInitialized = false;
        #endregion

        #region Properties
        /// <summary>
        /// 현재 게임 상태를 반환합니다.
        /// </summary>
        public GameState CurrentGameState => m_currentState;

        /// <summary>
        /// 이전 게임 상태를 반환합니다.
        /// </summary>
        public GameState PreviousGameState => m_previousState;

        /// <summary>
        /// 현재 게임 내 시간을 반환합니다. (0-24 범위)
        /// </summary>
        public float GameTime => m_gameTime;

        /// <summary>
        /// 현재 게임 내 시간대를 반환합니다.
        /// </summary>
        public TimeOfDay CurrentTimeOfDay => m_currentTimeOfDay;

        /// <summary>
        /// 현재 게임 내 년도를 반환합니다.
        /// </summary>
        public int CurrentYear => m_currentYear;

        /// <summary>
        /// 현재 게임 내 계절을 반환합니다. (0: 봄, 1: 여름, 2: 가을, 3: 겨울)
        /// </summary>
        public int CurrentSeason => m_currentSeason;

        /// <summary>
        /// 현재 게임 내 계절 이름을 반환합니다.
        /// </summary>
        public string CurrentSeasonName
        {
            get
            {
                return m_currentSeason switch
                {
                    0 => "봄",
                    1 => "여름",
                    2 => "가을",
                    3 => "겨울",
                    _ => "알 수 없음"
                };
            }
        }

        /// <summary>
        /// 현재 게임 내 일을 반환합니다.
        /// </summary>
        public int CurrentDay => m_currentDay;

        /// <summary>
        /// 현재 등록된 플레이어
        /// </summary>
        public PlayerEntity Player { get; private set; }

        /// <summary>
        /// 게임이 일시 정지되었는지 여부
        /// </summary>
        public bool IsPaused { get; private set; }

        /// <summary>
        /// 게임이 진행 중인지 여부
        /// </summary>
        public bool IsGameRunning { get; private set; }
        #endregion

        #region Singleton
        private static GameManager m_instance;
        public static GameManager Instance => m_instance;
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

        /// <summary>
        /// 매 프레임마다 호출됩니다.
        /// </summary>
        private void Update()
        {
            if (m_isInitialized && m_isTimeFlowing && m_currentState == GameState.InGame)
            {
                UpdateGameTime();
            }
        }
        #endregion

        #region IManager Implementation
        /// <summary>
        /// 게임 매니저를 초기화합니다.
        /// </summary>
        public void Initialize()
        {
            if (m_isInitialized)
                return;

            // 초기 게임 상태 설정
            ChangeGameState(GameState.MainMenu);

            // 초기 게임 시간 설정
            m_gameTime = m_morningStart + 2.0f; // 아침 8시 시작
            UpdateTimeOfDay();

            m_isInitialized = true;
            Debug.Log("GameManager가 초기화되었습니다.");
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
            m_isInitialized = false;
            Debug.Log("GameManager가 종료되었습니다.");
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 플레이어를 등록합니다.
        /// </summary>
        /// <param name="player">등록할 플레이어</param>
        public void RegisterPlayer(PlayerEntity player)
        {
            if (player == null)
                return;

            Player = player;
            OnPlayerRegistered?.Invoke(player);
            Debug.Log("플레이어가 게임 매니저에 등록되었습니다.");
        }

        /// <summary>
        /// 플레이어 등록을 해제합니다.
        /// </summary>
        /// <param name="player">해제할 플레이어</param>
        public void UnregisterPlayer(PlayerEntity player)
        {
            if (Player == player)
            {
                Player = null;
            }
        }

        /// <summary>
        /// 게임을 시작합니다.
        /// </summary>
        public void StartGame()
        {
            IsGameRunning = true;
            OnGameStart?.Invoke();
            Debug.Log("게임이 시작되었습니다.");
        }

        /// <summary>
        /// 게임을 일시 정지합니다.
        /// </summary>
        /// <param name="isPaused">일시 정지 상태</param>
        public void SetPauseState(bool isPaused)
        {
            if (IsPaused == isPaused)
                return;

            IsPaused = isPaused;
            Time.timeScale = isPaused ? 0f : 1f;
            OnGamePause?.Invoke(isPaused);
            Debug.Log($"게임 일시정지: {isPaused}");
        }

        /// <summary>
        /// 게임을 종료합니다.
        /// </summary>
        public void EndGame()
        {
            IsGameRunning = false;
            OnGameEnd?.Invoke();
            Debug.Log("게임이 종료되었습니다.");
        }

        /// <summary>
        /// 게임 상태를 변경합니다.
        /// </summary>
        /// <param name="newState">새로운 게임 상태</param>
        public void ChangeGameState(GameState newState)
        {
            if (m_currentState == newState)
                return;

            m_previousState = m_currentState;
            m_currentState = newState;

            OnGameStateChanged?.Invoke(m_previousState, m_currentState);

            Debug.Log($"게임 상태가 변경되었습니다: {m_previousState} -> {m_currentState}");

            // 상태에 따른 시간 흐름 제어
            switch (newState)
            {
                case GameState.InGame:
                    m_isTimeFlowing = true;
                    break;
                case GameState.Paused:
                case GameState.GameOver:
                    m_isTimeFlowing = false;
                    break;
            }
        }

        /// <summary>
        /// 이전 게임 상태로 돌아갑니다.
        /// </summary>
        public void ReturnToPreviousState()
        {
            ChangeGameState(m_previousState);
        }

        /// <summary>
        /// 게임을 일시정지합니다.
        /// </summary>
        public void PauseGame()
        {
            if (m_currentState == GameState.InGame)
            {
                ChangeGameState(GameState.Paused);
            }
        }

        /// <summary>
        /// 게임을 재개합니다.
        /// </summary>
        public void ResumeGame()
        {
            if (m_currentState == GameState.Paused)
            {
                ChangeGameState(GameState.InGame);
            }
        }

        /// <summary>
        /// 게임 시간 배속을 설정합니다.
        /// </summary>
        /// <param name="multiplier">시간 배속 (1.0f: 일반, 2.0f: 2배속)</param>
        public void SetTimeMultiplier(float multiplier)
        {
            m_timeMultiplier = Mathf.Max(0.0f, multiplier);
            Debug.Log($"게임 시간 배속이 {m_timeMultiplier}배로 설정되었습니다.");
        }

        /// <summary>
        /// 게임 시간을 특정 시간으로 설정합니다.
        /// </summary>
        /// <param name="hour">시간 (0-24)</param>
        public void SetGameTime(float hour)
        {
            m_gameTime = Mathf.Clamp(hour, 0.0f, 24.0f);
            UpdateTimeOfDay();
        }

        /// <summary>
        /// 다음 날로 넘어갑니다.
        /// </summary>
        public void AdvanceToNextDay()
        {
            m_gameTime = m_morningStart;
            m_currentDay++;

            if (m_currentDay > m_daysPerSeason)
            {
                m_currentDay = 1;
                m_currentSeason = (m_currentSeason + 1) % 4;

                if (m_currentSeason == 0)
                {
                    m_currentYear++;
                }
            }

            UpdateTimeOfDay();
            OnDateChanged?.Invoke(m_currentYear, m_currentSeason, m_currentDay);

            Debug.Log($"날짜가 변경되었습니다: {m_currentYear}년 {CurrentSeasonName} {m_currentDay}일");
        }

        /// <summary>
        /// 일시적으로 시간 흐름을 중지합니다.
        /// </summary>
        public void StopTimeFlow()
        {
            m_isTimeFlowing = false;
        }

        /// <summary>
        /// 시간 흐름을 재개합니다.
        /// </summary>
        public void ResumeTimeFlow()
        {
            m_isTimeFlowing = true;
        }

        /// <summary>
        /// 게임을 저장합니다.
        /// </summary>
        public void SaveGame()
        {
            // TODO: 저장 시스템 구현
            Debug.Log("게임 저장 기능이 호출되었습니다.");
        }

        /// <summary>
        /// 게임을 로드합니다.
        /// </summary>
        public void LoadGame()
        {
            // TODO: 로드 시스템 구현
            Debug.Log("게임 로드 기능이 호출되었습니다.");
        }

        /// <summary>
        /// 게임을 새로 시작합니다.
        /// </summary>
        public void StartNewGame()
        {
            // 초기 상태로 리셋
            m_currentYear = 1;
            m_currentSeason = 0;
            m_currentDay = 1;
            m_gameTime = m_morningStart + 2.0f;
            
            UpdateTimeOfDay();
            ChangeGameState(GameState.InGame);
            
            OnDateChanged?.Invoke(m_currentYear, m_currentSeason, m_currentDay);
            
            Debug.Log("새 게임이 시작되었습니다.");
        }

        /// <summary>
        /// 게임을 종료합니다.
        /// </summary>
        public void QuitGame()
        {
            Debug.Log("게임을 종료합니다.");
            
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 게임 시간을 업데이트합니다.
        /// </summary>
        private void UpdateGameTime()
        {
            // 시간 흐름 계산
            float timeIncrement = (Time.deltaTime / m_realTimePerGameDay) * 24.0f * m_timeMultiplier;
            m_gameTime += timeIncrement;

            // 하루가 지나면 다음 날로 넘어감
            if (m_gameTime >= 24.0f)
            {
                m_gameTime -= 24.0f;
                AdvanceToNextDay();
            }

            // 시간대 업데이트
            UpdateTimeOfDay();
        }

        /// <summary>
        /// 현재 시간에 따라 시간대를 업데이트합니다.
        /// </summary>
        private void UpdateTimeOfDay()
        {
            TimeOfDay newTimeOfDay;

            if (m_gameTime >= m_morningStart && m_gameTime < m_afternoonStart)
            {
                newTimeOfDay = TimeOfDay.Morning;
            }
            else if (m_gameTime >= m_afternoonStart && m_gameTime < m_eveningStart)
            {
                newTimeOfDay = TimeOfDay.Afternoon;
            }
            else if (m_gameTime >= m_eveningStart && m_gameTime < m_nightStart)
            {
                newTimeOfDay = TimeOfDay.Evening;
            }
            else
            {
                newTimeOfDay = TimeOfDay.Night;
            }

            if (m_currentTimeOfDay != newTimeOfDay)
            {
                m_currentTimeOfDay = newTimeOfDay;
                OnTimeOfDayChanged?.Invoke(m_currentTimeOfDay);
                
                Debug.Log($"시간대가 변경되었습니다: {m_currentTimeOfDay}");
            }
        }
        #endregion
    }
} 