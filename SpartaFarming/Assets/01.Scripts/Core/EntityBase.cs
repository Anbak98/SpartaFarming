using System;
using UnityEngine;

namespace SpartaFarming.Core
{
    /// <summary>
    /// 모든 게임 엔티티의 기본 클래스입니다.
    /// </summary>
    public abstract class EntityBase : MonoBehaviour, IEntity
    {
        [SerializeField] private string m_id = string.Empty;

        /// <summary>
        /// 엔티티 고유 ID
        /// </summary>
        public string Id
        {
            get => !string.IsNullOrEmpty(m_id) ? m_id : gameObject.name;
            protected set => m_id = value;
        }

        /// <summary>
        /// 엔티티의 게임 오브젝트
        /// </summary>
        public GameObject GameObject => gameObject;

        /// <summary>
        /// 엔티티의 Transform 컴포넌트
        /// </summary>
        public Transform Transform => transform;

        /// <summary>
        /// 초기화 시 호출됩니다.
        /// </summary>
        protected virtual void Awake()
        {
            // ID가 설정되지 않은 경우 GUID 생성
            if (string.IsNullOrEmpty(m_id))
            {
                m_id = Guid.NewGuid().ToString();
            }
        }
    }
} 