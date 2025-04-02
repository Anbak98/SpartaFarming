using UnityEngine;
using SpartaFarming.Core;
using SpartaFarming.Player;

namespace SpartaFarming.NPCs
{
    /// <summary>
    /// NPC의 기본 동작을 정의하는 컴포넌트입니다.
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class NPCEntity : EntityBase, IInteractable
    {
        #region Serialized Fields
        [Header("상호작용 설정")]
        [SerializeField] private float m_interactionRange = 2f;
        [SerializeField] private string m_interactionPrompt = "E 키를 눌러 대화하기";
        [SerializeField] private bool m_canInteract = true;

        [Header("NPC 정보")]
        [SerializeField] private string m_npcName = "NPC";
        [SerializeField] private string m_npcRole = "일반 NPC";
        [SerializeField] private Sprite m_npcPortrait;
        #endregion

        #region Properties
        /// <summary>
        /// NPC 이름
        /// </summary>
        public string NPCName => m_npcName;

        /// <summary>
        /// NPC 역할
        /// </summary>
        public string NPCRole => m_npcRole;

        /// <summary>
        /// NPC 초상화
        /// </summary>
        public Sprite NPCPortrait => m_npcPortrait;

        /// <summary>
        /// 상호작용 가능 여부
        /// </summary>
        public bool CanInteract => m_canInteract;

        /// <summary>
        /// 상호작용 범위
        /// </summary>
        public float InteractionRange => m_interactionRange;

        /// <summary>
        /// 상호작용 프롬프트 텍스트
        /// </summary>
        public string InteractionPrompt => m_interactionPrompt;
        #endregion

        #region Unity Lifecycle
        /// <summary>
        /// 컴포넌트 초기화 시 호출됩니다.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // ID가 비어있으면 NPC 이름으로 설정
            if (string.IsNullOrEmpty(Id))
            {
                Id = "NPC_" + m_npcName;
            }
        }

        /// <summary>
        /// 다른 콜라이더가 트리거 영역 내에 들어왔을 때 호출됩니다.
        /// </summary>
        /// <param name="other">충돌한 콜라이더</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            // 플레이어가 접근했는지 확인
            if (other.TryGetComponent<PlayerEntity>(out var player))
            {
                ShowInteractionPrompt(true);
            }
        }

        /// <summary>
        /// 다른 콜라이더가 트리거 영역을 벗어날 때 호출됩니다.
        /// </summary>
        /// <param name="other">충돌한 콜라이더</param>
        private void OnTriggerExit2D(Collider2D other)
        {
            // 플레이어가 떠났는지 확인
            if (other.TryGetComponent<PlayerEntity>(out var player))
            {
                ShowInteractionPrompt(false);
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// 플레이어와 상호작용합니다.
        /// </summary>
        /// <param name="interactor">상호작용을 시도하는 엔티티</param>
        /// <returns>상호작용 성공 여부</returns>
        public bool Interact(IEntity interactor)
        {
            if (!m_canInteract)
                return false;

            // 플레이어인지 확인
            if (interactor is PlayerEntity player)
            {
                // 대화 시작
                StartDialogue(player);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 상호작용 가능 여부를 설정합니다.
        /// </summary>
        /// <param name="canInteract">상호작용 가능 여부</param>
        public void SetInteractable(bool canInteract)
        {
            m_canInteract = canInteract;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// 상호작용 프롬프트를 표시하거나 숨깁니다.
        /// </summary>
        /// <param name="show">표시 여부</param>
        private void ShowInteractionPrompt(bool show)
        {
            if (show && m_canInteract)
            {
                // UI 매니저를 통해 상호작용 프롬프트 표시
                UIManager.Instance?.ShowInteractionPrompt(m_interactionPrompt);
            }
            else
            {
                // UI 매니저를 통해 상호작용 프롬프트 숨김
                UIManager.Instance?.HideInteractionPrompt();
            }
        }

        /// <summary>
        /// 플레이어와 대화를 시작합니다.
        /// </summary>
        /// <param name="player">대화할 플레이어</param>
        private void StartDialogue(PlayerEntity player)
        {
            Debug.Log($"{m_npcName}(이)가 {player.Id}와 대화를 시작합니다.");
            
            // 대화 시스템 시작
            DialogueManager.Instance?.StartDialogue(this, player);
        }
        #endregion
    }
} 