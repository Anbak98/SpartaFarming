using UnityEngine;

/// <summary>
/// 플레이어 게임 오브젝트의 기본 컴포넌트입니다.
/// 다른 관련 컴포넌트에 대한 참조를 관리합니다.
/// </summary>
public class Player : MonoBehaviour
{
    #region Public Properties
    /// <summary>
    /// 플레이어 컨트롤러 컴포넌트에 대한 참조입니다.
    /// </summary>
    public PlayerController Controller { get; private set; }
    
    /// <summary>
    /// 플레이어 상태 컴포넌트에 대한 참조입니다.
    /// </summary>
    public PlayerCondition Condition { get; private set; }
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// 컴포넌트 초기화 시 호출됩니다.
    /// </summary>
    private void Awake()
    {
        // 필요한 컴포넌트 참조 초기화
        Controller = GetComponent<PlayerController>();
        Condition = GetComponent<PlayerCondition>();
        
        // 게임 매니저에 플레이어 등록
        GameManager.Instance.Player = this;
    }
    #endregion
}
