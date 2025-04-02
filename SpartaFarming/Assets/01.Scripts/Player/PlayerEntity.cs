using System;
using UnityEngine;
using SpartaFarming.Core;

namespace SpartaFarming.Player
{
    /// <summary>
    /// 플레이어 엔티티의 모든 컴포넌트를 관리하는 메인 클래스입니다.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerStats))]
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerEntity : EntityBase
    {
        #region Components
        /// <summary>
        /// 플레이어 상태 머신
        /// </summary>
        public PlayerStateMachine StateMachine { get; private set; }

        /// <summary>
        /// 플레이어 입력 컨트롤러
        /// </summary>
        public PlayerController Controller { get; private set; }

        /// <summary>
        /// 플레이어 스탯 및 능력치
        /// </summary>
        public PlayerStats Stats { get; private set; }

        /// <summary>
        /// 플레이어 인벤토리
        /// </summary>
        public PlayerInventory Inventory { get; private set; }

        /// <summary>
        /// 플레이어 Rigidbody2D 컴포넌트
        /// </summary>
        public Rigidbody2D Rigidbody { get; private set; }

        /// <summary>
        /// 플레이어 애니메이터
        /// </summary>
        public Animator Animator { get; private set; }
        #endregion

        #region Events
        /// <summary>
        /// 플레이어가 사망할 때 발생하는 이벤트
        /// </summary>
        public event Action OnPlayerDeath;

        /// <summary>
        /// 플레이어가 레벨업할 때 발생하는 이벤트
        /// </summary>
        public event Action<int> OnPlayerLevelUp;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// 컴포넌트 초기화 시 호출됩니다.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
            StateMachine = GetComponent<PlayerStateMachine>();
            Controller = GetComponent<PlayerController>();
            Stats = GetComponent<PlayerStats>();
            Inventory = GetComponent<PlayerInventory>();

            // 게임 매니저에 플레이어 참조 등록
            GameManager.Instance.RegisterPlayer(this);

            // 이벤트 구독
            Stats.OnDeath += HandlePlayerDeath;
            Stats.OnLevelUp += HandlePlayerLevelUp;
        }

        /// <summary>
        /// 컴포넌트가 파괴될 때 호출됩니다.
        /// </summary>
        private void OnDestroy()
        {
            // 이벤트 구독 해제
            if (Stats != null)
            {
                Stats.OnDeath -= HandlePlayerDeath;
                Stats.OnLevelUp -= HandlePlayerLevelUp;
            }

            // 게임 매니저 참조 제거
            if (GameManager.HasInstance)
            {
                GameManager.Instance.UnregisterPlayer(this);
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// 플레이어 사망 이벤트 처리
        /// </summary>
        private void HandlePlayerDeath()
        {
            Debug.Log("플레이어가 사망했습니다.");
            
            // 상태 머신의 현재 상태 클리어
            StateMachine.ClearState();
            
            // 이벤트 발생
            OnPlayerDeath?.Invoke();
        }

        /// <summary>
        /// 레벨업 이벤트 처리
        /// </summary>
        /// <param name="newLevel">새로운 레벨</param>
        private void HandlePlayerLevelUp(int newLevel)
        {
            Debug.Log($"플레이어가 {newLevel} 레벨이 되었습니다!");
            
            // 이벤트 발생
            OnPlayerLevelUp?.Invoke(newLevel);
        }
        #endregion
    }
} 