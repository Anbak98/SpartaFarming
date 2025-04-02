using UnityEngine;
using SpartaFarming.Core;

namespace SpartaFarming.Tools
{
    /// <summary>
    /// 모든 도구의 기본 클래스입니다.
    /// </summary>
    public abstract class ToolBase : MonoBehaviour, ITool
    {
        #region Serialized Fields
        [Header("도구 정보")]
        [SerializeField] protected ToolType m_toolType = ToolType.None;
        [SerializeField] protected int m_toolLevel = 1;
        [SerializeField] protected float m_maxDurability = 100f;
        [SerializeField] protected bool m_consumeDurability = true;
        [SerializeField] protected float m_durabilityConsumeRate = 1f;
        [SerializeField] protected float m_durabilityRepairRate = 5f;
        [SerializeField] protected float m_upgradeMultiplier = 1.5f;
        [SerializeField] protected int m_maxUpgradeLevel = 5;

        [Header("시각적 효과")]
        [SerializeField] protected GameObject m_useEffectPrefab;
        [SerializeField] protected AudioClip m_useSound;
        [SerializeField] protected Animator m_toolAnimator;
        #endregion

        #region Private Fields
        protected float m_currentDurability;
        protected bool m_isEquipped = false;
        protected IEntity m_currentUser;
        protected Transform m_originalParent;
        #endregion

        #region Properties
        /// <summary>
        /// 도구의 종류
        /// </summary>
        public ToolType Type => m_toolType;

        /// <summary>
        /// 도구의 현재 레벨
        /// </summary>
        public int Level => m_toolLevel;

        /// <summary>
        /// 도구가 사용 가능한지 여부
        /// </summary>
        public bool IsUsable => m_isEquipped && m_currentDurability > 0f;

        /// <summary>
        /// 도구의 현재 내구도
        /// </summary>
        public float CurrentDurability => m_currentDurability;

        /// <summary>
        /// 도구의 최대 내구도
        /// </summary>
        public float MaxDurability => m_maxDurability;

        /// <summary>
        /// 애니메이터 참조
        /// </summary>
        public Animator ToolAnimator => m_toolAnimator;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// 컴포넌트 초기화 시 호출됩니다.
        /// </summary>
        protected virtual void Awake()
        {
            m_currentDurability = m_maxDurability;
            m_originalParent = transform.parent;
            
            if (m_toolAnimator == null)
            {
                m_toolAnimator = GetComponent<Animator>();
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 도구를 사용합니다.
        /// </summary>
        /// <param name="user">도구 사용자</param>
        /// <param name="target">사용 대상(선택적)</param>
        /// <param name="targetPosition">사용 위치(선택적)</param>
        /// <returns>사용 성공 여부</returns>
        public virtual bool Use(IEntity user, IEntity target = null, Vector3? targetPosition = null)
        {
            if (!IsUsable)
                return false;

            // 도구 사용 애니메이션 재생
            PlayUseAnimation();

            // 효과음 재생
            PlayUseSound();

            // 내구도 감소
            if (m_consumeDurability)
            {
                ReduceDurability(m_durabilityConsumeRate);
            }

            return true;
        }

        /// <summary>
        /// 도구를 장착합니다.
        /// </summary>
        /// <param name="user">도구 사용자</param>
        public virtual void Equip(IEntity user)
        {
            if (user == null)
                return;

            m_currentUser = user;
            m_isEquipped = true;
            
            // 시각적인 장착 표현
            OnEquipped();
        }

        /// <summary>
        /// 도구를 해제합니다.
        /// </summary>
        /// <param name="user">도구 사용자</param>
        public virtual void Unequip(IEntity user)
        {
            if (user == null || user != m_currentUser)
                return;

            m_isEquipped = false;
            m_currentUser = null;
            
            // 시각적인 해제 표현
            OnUnequipped();
        }

        /// <summary>
        /// 도구의 내구도를 감소시킵니다.
        /// </summary>
        /// <param name="amount">감소시킬 내구도 양</param>
        /// <returns>남은 내구도</returns>
        public virtual float ReduceDurability(float amount)
        {
            if (amount <= 0f)
                return m_currentDurability;

            m_currentDurability = Mathf.Max(0f, m_currentDurability - amount);
            
            // 내구도가 0이 되면 부서짐 처리
            if (m_currentDurability <= 0f)
            {
                OnBroken();
            }
            
            return m_currentDurability;
        }

        /// <summary>
        /// 도구를 수리합니다.
        /// </summary>
        /// <param name="amount">회복시킬 내구도 양</param>
        /// <returns>수리 후 내구도</returns>
        public virtual float Repair(float amount)
        {
            if (amount <= 0f)
                return m_currentDurability;

            m_currentDurability = Mathf.Min(m_maxDurability, m_currentDurability + amount);
            return m_currentDurability;
        }

        /// <summary>
        /// 도구를 업그레이드합니다.
        /// </summary>
        /// <returns>업그레이드 성공 여부</returns>
        public virtual bool Upgrade()
        {
            // 최대 레벨이면 업그레이드 불가
            if (m_toolLevel >= m_maxUpgradeLevel)
                return false;

            m_toolLevel++;
            
            // 최대 내구도 증가
            m_maxDurability *= m_upgradeMultiplier;
            m_currentDurability = m_maxDurability;
            
            // 업그레이드 효과 적용
            OnUpgraded();
            
            return true;
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// 도구 장착 시 호출되는 메서드
        /// </summary>
        protected virtual void OnEquipped()
        {
            gameObject.SetActive(true);
            
            // 장착 애니메이션 재생
            if (m_toolAnimator != null)
            {
                m_toolAnimator.SetBool("Equipped", true);
            }
        }

        /// <summary>
        /// 도구 해제 시 호출되는 메서드
        /// </summary>
        protected virtual void OnUnequipped()
        {
            gameObject.SetActive(false);
            
            // 해제 애니메이션 재생
            if (m_toolAnimator != null)
            {
                m_toolAnimator.SetBool("Equipped", false);
            }
        }

        /// <summary>
        /// 도구 내구도가 0이 되었을 때 호출되는 메서드
        /// </summary>
        protected virtual void OnBroken()
        {
            Debug.Log($"{m_toolType} 도구가 부서졌습니다!");
            
            // 부서짐 효과
            if (m_toolAnimator != null)
            {
                m_toolAnimator.SetTrigger("Break");
            }
        }

        /// <summary>
        /// 도구 업그레이드 시 호출되는 메서드
        /// </summary>
        protected virtual void OnUpgraded()
        {
            Debug.Log($"{m_toolType} 도구가 {m_toolLevel}레벨로 업그레이드되었습니다!");
            
            // 업그레이드 효과
            if (m_toolAnimator != null)
            {
                m_toolAnimator.SetTrigger("Upgrade");
            }
        }

        /// <summary>
        /// 도구 사용 애니메이션을 재생합니다.
        /// </summary>
        protected virtual void PlayUseAnimation()
        {
            if (m_toolAnimator != null)
            {
                m_toolAnimator.SetTrigger("Use");
            }
        }

        /// <summary>
        /// 도구 사용 효과음을 재생합니다.
        /// </summary>
        protected virtual void PlayUseSound()
        {
            if (m_useSound != null)
            {
                AudioSource.PlayClipAtPoint(m_useSound, transform.position);
            }
        }

        /// <summary>
        /// 사용 효과를 생성합니다.
        /// </summary>
        /// <param name="position">효과 위치</param>
        protected virtual void SpawnUseEffect(Vector3 position)
        {
            if (m_useEffectPrefab != null)
            {
                GameObject effect = Instantiate(m_useEffectPrefab, position, Quaternion.identity);
                Destroy(effect, 2f); // 2초 후 효과 제거
            }
        }
        #endregion
    }
} 