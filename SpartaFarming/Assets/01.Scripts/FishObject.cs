using UnityEngine;

/// <summary>
/// 물고기 오브젝트의 동작을 관리하는 컴포넌트입니다.
/// </summary>
public class FishObject : MonoBehaviour
{
    #region Properties
    /// <summary>
    /// 물고기의 이동 방향 벡터를 반환합니다.
    /// </summary>
    public Vector3 PowerDirection => (m_nextMovePosition - transform.position).normalized;

    /// <summary>
    /// 물고기의 스태미나를 관리합니다.
    /// </summary>
    public int Stamina
    {
        get => m_stamina;
        set
        {
            m_stamina = Mathf.Max(0, value);
        }
    }
    #endregion

    #region Serialized Fields
    [Header("이동 설정")]
    [SerializeField] private float m_fishSpeed = 0.001f;
    [SerializeField] private float m_timeAccel = 1f;
    [SerializeField] private float m_timeInterval = 1f;
    [SerializeField] private int m_initialStamina = 100;
    #endregion

    #region Private Fields
    private Vector3 m_originPosition;
    private Vector3 m_nextMovePosition;
    private float m_elapsedTime = 0f;
    private int m_stamina;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// 컴포넌트 초기화 시 호출됩니다.
    /// </summary>
    private void Awake()
    {
        m_stamina = m_initialStamina;
    }

    /// <summary>
    /// 첫 프레임 업데이트 전에 호출됩니다.
    /// </summary>
    private void Start()
    {
        m_originPosition = transform.position;
        m_nextMovePosition = m_originPosition;
    }

    /// <summary>
    /// 매 프레임마다 호출됩니다.
    /// </summary>
    private void Update()
    {
        UpdateMovement();
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 물고기의 무작위 움직임을 업데이트합니다.
    /// </summary>
    private void UpdateMovement()
    {
        m_elapsedTime += Time.deltaTime * m_timeAccel;

        if (m_elapsedTime > m_timeInterval)
        {
            m_nextMovePosition = m_originPosition + new Vector3(
                Random.Range(-1f, 1f), 
                Random.Range(-1f, 1f), 
                0
            );
            m_elapsedTime = 0;
        }

        transform.position = Vector3.Lerp(transform.position, m_nextMovePosition, m_fishSpeed);
    }
    #endregion
}
