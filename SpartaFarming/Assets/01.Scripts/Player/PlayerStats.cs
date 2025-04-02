using System;
using UnityEngine;
using SpartaFarming.Core;

namespace SpartaFarming.Player
{
    /// <summary>
    /// 플레이어의 스탯과 상태를 관리하는 컴포넌트입니다.
    /// </summary>
    public class PlayerStats : MonoBehaviour, IDamageable, IStaminaUser
    {
        #region Serialized Fields
        [Header("체력 설정")]
        [SerializeField] private float m_maxHealth = 100f;
        [SerializeField] private float m_healthRegenRate = 1f;
        
        [Header("스태미나 설정")]
        [SerializeField] private float m_maxStamina = 100f;
        [SerializeField] private float m_staminaRegenRate = 5f;
        
        [Header("레벨 및 경험치")]
        [SerializeField] private int m_level = 1;
        [SerializeField] private float m_experience = 0f;
        [SerializeField] private float m_experienceToNextLevel = 100f;
        [SerializeField] private float m_experienceMultiplier = 1.5f;
        
        [Header("기술 스탯")]
        [SerializeField] private float m_farmingLevel = 1f;
        [SerializeField] private float m_fishingLevel = 1f;
        [SerializeField] private float m_miningLevel = 1f;
        [SerializeField] private float m_combatLevel = 1f;
        
        [Header("제한 설정")]
        [SerializeField] private float m_maxSkillLevel = 100f;
        #endregion

        #region Private Fields
        private float m_currentHealth;
        private float m_currentStamina;
        private bool m_isDead = false;
        #endregion

        #region Events
        /// <summary>
        /// 체력이 변경될 때 발생하는 이벤트 (현재 체력, 최대 체력)
        /// </summary>
        public event Action<float, float> OnHealthChanged;

        /// <summary>
        /// 스태미나가 변경될 때 발생하는 이벤트 (현재 스태미나, 최대 스태미나)
        /// </summary>
        public event Action<float, float> OnStaminaChanged;

        /// <summary>
        /// 레벨이 변경될 때 발생하는 이벤트 (새 레벨)
        /// </summary>
        public event Action<int> OnLevelUp;

        /// <summary>
        /// 경험치가 변경될 때 발생하는 이벤트 (현재 경험치, 다음 레벨까지 필요 경험치)
        /// </summary>
        public event Action<float, float> OnExperienceChanged;

        /// <summary>
        /// 스킬 레벨이 변경될 때 발생하는 이벤트 (스킬 이름, 새 레벨)
        /// </summary>
        public event Action<string, float> OnSkillLevelChanged;

        /// <summary>
        /// 플레이어가 사망할 때 발생하는 이벤트
        /// </summary>
        public event Action OnDeath;

        /// <summary>
        /// 플레이어가 데미지를 받을 때 발생하는 이벤트 (데미지 양, 현재 체력)
        /// </summary>
        public event Action<float, float> OnDamaged;
        #endregion

        #region Public Properties
        /// <summary>
        /// 현재 체력
        /// </summary>
        public float CurrentHealth 
        { 
            get => m_currentHealth;
            private set
            {
                float oldValue = m_currentHealth;
                m_currentHealth = Mathf.Clamp(value, 0f, m_maxHealth);
                
                if (oldValue != m_currentHealth)
                {
                    OnHealthChanged?.Invoke(m_currentHealth, m_maxHealth);
                    
                    // 사망 체크
                    if (m_currentHealth <= 0f && !m_isDead)
                    {
                        m_isDead = true;
                        OnDeath?.Invoke();
                    }
                }
            }
        }

        /// <summary>
        /// 최대 체력
        /// </summary>
        public float MaxHealth => m_maxHealth;

        /// <summary>
        /// 체력 재생 속도
        /// </summary>
        public float HealthRegenRate => m_healthRegenRate;

        /// <summary>
        /// 현재 스태미나
        /// </summary>
        public float CurrentStamina
        {
            get => m_currentStamina;
            private set
            {
                float oldValue = m_currentStamina;
                m_currentStamina = Mathf.Clamp(value, 0f, m_maxStamina);
                
                if (oldValue != m_currentStamina)
                {
                    OnStaminaChanged?.Invoke(m_currentStamina, m_maxStamina);
                }
            }
        }

        /// <summary>
        /// 최대 스태미나
        /// </summary>
        public float MaxStamina => m_maxStamina;

        /// <summary>
        /// 스태미나 회복 속도
        /// </summary>
        public float StaminaRecoveryRate => m_staminaRegenRate;

        /// <summary>
        /// 현재 레벨
        /// </summary>
        public int Level => m_level;

        /// <summary>
        /// 현재 경험치
        /// </summary>
        public float Experience => m_experience;

        /// <summary>
        /// 다음 레벨에 필요한 경험치
        /// </summary>
        public float ExperienceToNextLevel => m_experienceToNextLevel;

        /// <summary>
        /// 농사 스킬 레벨
        /// </summary>
        public float FarmingLevel => m_farmingLevel;

        /// <summary>
        /// 낚시 스킬 레벨
        /// </summary>
        public float FishingLevel => m_fishingLevel;

        /// <summary>
        /// 채굴 스킬 레벨
        /// </summary>
        public float MiningLevel => m_miningLevel;

        /// <summary>
        /// 전투 스킬 레벨
        /// </summary>
        public float CombatLevel => m_combatLevel;

        /// <summary>
        /// 플레이어가 살아있는지 여부
        /// </summary>
        public bool IsAlive => !m_isDead;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// 컴포넌트 초기화 시 호출됩니다.
        /// </summary>
        private void Awake()
        {
            // 초기 상태 설정
            m_currentHealth = m_maxHealth;
            m_currentStamina = m_maxStamina;
        }

        /// <summary>
        /// 매 프레임 호출됩니다.
        /// </summary>
        private void Update()
        {
            if (!IsAlive) 
                return;

            // 자연 회복
            RegenerateHealthAndStamina();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 물리적 데미지를 받습니다.
        /// </summary>
        /// <param name="amount">데미지 양</param>
        /// <param name="source">데미지 원천</param>
        /// <returns>실제로 적용된 데미지</returns>
        public float TakePhysicalDamage(float amount, IEntity source = null)
        {
            if (!IsAlive || amount <= 0f)
                return 0f;

            float initialHealth = CurrentHealth;
            CurrentHealth -= amount;
            
            float actualDamage = initialHealth - CurrentHealth;
            OnDamaged?.Invoke(actualDamage, CurrentHealth);
            
            return actualDamage;
        }

        /// <summary>
        /// 마법 데미지를 받습니다.
        /// </summary>
        /// <param name="amount">데미지 양</param>
        /// <param name="source">데미지 원천</param>
        /// <returns>실제로 적용된 데미지</returns>
        public float TakeMagicalDamage(float amount, IEntity source = null)
        {
            // 이 게임에서는 마법과 물리 데미지를 동일하게 처리
            return TakePhysicalDamage(amount, source);
        }

        /// <summary>
        /// 체력을 회복합니다.
        /// </summary>
        /// <param name="amount">회복량</param>
        /// <param name="source">회복 원천</param>
        /// <returns>실제로 회복된 양</returns>
        public float Heal(float amount, IEntity source = null)
        {
            if (!IsAlive || amount <= 0f)
                return 0f;

            float initialHealth = CurrentHealth;
            CurrentHealth += amount;
            
            return CurrentHealth - initialHealth;
        }

        /// <summary>
        /// 스태미나를 소비합니다.
        /// </summary>
        /// <param name="amount">소비할 스태미나 양</param>
        /// <returns>실제로 소비된 스태미나 양</returns>
        public float UseStamina(float amount)
        {
            if (!IsAlive || amount <= 0f)
                return 0f;

            float initialStamina = CurrentStamina;
            CurrentStamina -= amount;
            
            return initialStamina - CurrentStamina;
        }

        /// <summary>
        /// 스태미나를 회복합니다.
        /// </summary>
        /// <param name="amount">회복할 스태미나 양</param>
        /// <returns>실제로 회복된 스태미나 양</returns>
        public float RecoverStamina(float amount)
        {
            if (!IsAlive || amount <= 0f)
                return 0f;

            float initialStamina = CurrentStamina;
            CurrentStamina += amount;
            
            return CurrentStamina - initialStamina;
        }

        /// <summary>
        /// 특정 행동에 필요한 스태미나가 충분한지 확인합니다.
        /// </summary>
        /// <param name="requiredAmount">필요한 스태미나 양</param>
        /// <returns>스태미나가 충분한지 여부</returns>
        public bool HasEnoughStamina(float requiredAmount)
        {
            return IsAlive && CurrentStamina >= requiredAmount;
        }

        /// <summary>
        /// 경험치를 추가합니다.
        /// </summary>
        /// <param name="amount">추가할 경험치</param>
        public void AddExperience(float amount)
        {
            if (!IsAlive || amount <= 0f)
                return;

            m_experience += amount;
            OnExperienceChanged?.Invoke(m_experience, m_experienceToNextLevel);
            
            // 레벨업 체크
            CheckLevelUp();
        }

        /// <summary>
        /// 특정 스킬의 레벨을 높입니다.
        /// </summary>
        /// <param name="skillName">스킬 이름</param>
        /// <param name="amount">증가할 양</param>
        public void IncreaseSkillLevel(string skillName, float amount)
        {
            if (!IsAlive || amount <= 0f)
                return;

            switch (skillName.ToLower())
            {
                case "farming":
                    m_farmingLevel = Mathf.Min(m_farmingLevel + amount, m_maxSkillLevel);
                    OnSkillLevelChanged?.Invoke("Farming", m_farmingLevel);
                    break;
                case "fishing":
                    m_fishingLevel = Mathf.Min(m_fishingLevel + amount, m_maxSkillLevel);
                    OnSkillLevelChanged?.Invoke("Fishing", m_fishingLevel);
                    break;
                case "mining":
                    m_miningLevel = Mathf.Min(m_miningLevel + amount, m_maxSkillLevel);
                    OnSkillLevelChanged?.Invoke("Mining", m_miningLevel);
                    break;
                case "combat":
                    m_combatLevel = Mathf.Min(m_combatLevel + amount, m_maxSkillLevel);
                    OnSkillLevelChanged?.Invoke("Combat", m_combatLevel);
                    break;
                default:
                    Debug.LogWarning($"Unknown skill name: {skillName}");
                    break;
            }
        }

        /// <summary>
        /// 플레이어를 부활시킵니다.
        /// </summary>
        public void Resurrect()
        {
            if (IsAlive)
                return;
                
            m_isDead = false;
            CurrentHealth = m_maxHealth * 0.5f; // 50% 체력으로 부활
            CurrentStamina = m_maxStamina * 0.5f; // 50% 스태미나로 부활
            
            Debug.Log("플레이어가 부활했습니다.");
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 체력과 스태미나를 자연 회복합니다.
        /// </summary>
        private void RegenerateHealthAndStamina()
        {
            // 체력 자연 회복
            if (CurrentHealth < MaxHealth)
            {
                CurrentHealth += HealthRegenRate * Time.deltaTime;
            }
            
            // 스태미나 자연 회복
            if (CurrentStamina < MaxStamina)
            {
                CurrentStamina += StaminaRecoveryRate * Time.deltaTime;
            }
        }

        /// <summary>
        /// 레벨업 조건을 확인하고 적용합니다.
        /// </summary>
        private void CheckLevelUp()
        {
            while (m_experience >= m_experienceToNextLevel)
            {
                m_experience -= m_experienceToNextLevel;
                m_level++;
                
                // 다음 레벨에 필요한 경험치 증가
                m_experienceToNextLevel = Mathf.Round(m_experienceToNextLevel * m_experienceMultiplier);
                
                // 레벨업으로 체력과 스태미나 모두 회복
                CurrentHealth = MaxHealth;
                CurrentStamina = MaxStamina;
                
                OnLevelUp?.Invoke(m_level);
            }
            
            OnExperienceChanged?.Invoke(m_experience, m_experienceToNextLevel);
        }
        #endregion
    }
} 