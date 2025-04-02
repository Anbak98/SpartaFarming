using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 계절에 따라 타일맵 표시를 관리하는 컴포넌트입니다.
/// </summary>
public class TileMapController : MonoBehaviour
{
    #region Serialized Fields
    [Header("계절별 타일맵")]
    [SerializeField] private GameObject m_summerTilemap;
    [SerializeField] private GameObject m_winterTilemap;
    [SerializeField] private GameObject m_springTilemap;
    [SerializeField] private GameObject m_fallTilemap;
    #endregion

    #region Private Fields
    private WeatherSystem m_weatherSystem;
    private SeasonData m_seasonData;
    #endregion

    #region Unity Lifecycle
    /// <summary>
    /// 첫 프레임 업데이트 전에 호출됩니다.
    /// </summary>
    private void Start()
    {
        InitializeWeatherSystem();
    }

    /// <summary>
    /// 컴포넌트가 비활성화될 때 호출됩니다.
    /// </summary>
    private void OnDisable()
    {
        if (m_weatherSystem != null)
        {
            m_weatherSystem.OnSeasonChange -= ChangeTileSprite;
        }
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// 날씨 시스템을 초기화하고 이벤트를 구독합니다.
    /// </summary>
    private void InitializeWeatherSystem()
    {
        m_weatherSystem = WeatherManager.Instance.WeatherSystem;
        m_seasonData = m_weatherSystem.CurrentSeason;
        m_weatherSystem.OnSeasonChange += ChangeTileSprite;
        
        // 초기 타일맵 상태 설정
        ChangeTileSprite();
    }

    /// <summary>
    /// 계절 변경 시 타일맵을 업데이트합니다.
    /// </summary>
    private void ChangeTileSprite()
    {
        m_seasonData = m_weatherSystem.CurrentSeason;

        // 모든 계절 타일맵 비활성화
        m_summerTilemap.SetActive(false);
        m_winterTilemap.SetActive(false);
        m_springTilemap.SetActive(false);
        m_fallTilemap.SetActive(false);

        // 현재 계절에 맞는 타일맵 활성화
        switch (m_seasonData.season)
        {
            case SeasonType.Summer:
                m_summerTilemap.SetActive(true);
                break;
            case SeasonType.Winter:
                m_winterTilemap.SetActive(true);
                break;
            case SeasonType.Spring:
                m_springTilemap.SetActive(true);
                break;
            case SeasonType.Fall:
                m_fallTilemap.SetActive(true);
                break;
        }
    }
    #endregion
}
