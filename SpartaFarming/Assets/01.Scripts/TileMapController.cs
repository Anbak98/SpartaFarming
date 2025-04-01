using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapController : MonoBehaviour
{
    private WeatherSystem weatherSystem;
    private SeasonData seasonData;

    [Header("타일 참조")]
    [SerializeField] private GameObject Summer;
    [SerializeField] private GameObject Winter;
    [SerializeField] private GameObject Spring;
    [SerializeField] private GameObject Fall;

    // Start is called before the first frame update
    void Start()
    {
        weatherSystem = WeatherManager.Instance.WeatherSystem;
        seasonData = weatherSystem.CurrentSeason;
        weatherSystem.OnSeasonChange += ChangeTileSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void ChangeTileSprite()
    {
        seasonData = weatherSystem.CurrentSeason;

        Summer.SetActive(false);
        Winter.SetActive(false);
        Spring.SetActive(false);
        Fall.SetActive(false);

        switch (seasonData.season)
        {
            case SeasonType.Summer:
                Summer.SetActive(true);
                break;
            case SeasonType.Winter:
                Winter.SetActive(true);
                break;
            case SeasonType.Spring:
                Spring.SetActive(true);
                break;
            case SeasonType.Fall:
                Fall.SetActive(true);
                break;
        }
    }
}
