using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainUiManager : MonoBehaviour
{
    [Header("===Component===")]
    [SerializeField]
    private TextMeshProUGUI weekText;
    [SerializeField]
    private TextMeshProUGUI timeText;
    [SerializeField]
    private Image weatherImage;
    [SerializeField]
    private Image staminaImage;

    [Header("===Sprite-===")]
    [SerializeField]
    private Sprite[] weatherSprite;

    public WeatherSystem weatherSystem;
    public TimeSystem timeSystem;

    private void Start()
    {
        weatherSystem = WeatherManager.Instance.WeatherSystem;
        timeSystem = TimeManager.Instance.TimeSystem;

        weatherSystem.OnWeatherChange += UpdateWeatherUI;
    }

    private void Update()
    {
        UpdateTime();
    }

    private void UpdateTime() 
    {
        timeText.text = $"{timeSystem.CurrentHour:D2}:{timeSystem.CurrentMinute:D2}";
        weekText.text = $"Thu, {timeSystem.CurrentDay}";
    }

    public void UpdateWeatherUI(WeatherType weatherType)
    {
        
        switch (weatherType)
        {
            case WeatherType.Rain:
                weatherImage.sprite = weatherSprite[0];
                break;
            case WeatherType.Thunder:
                weatherImage.sprite = weatherSprite[1];
                break;
            case WeatherType.ThunderStorm:
                
                break;
            case WeatherType.Sunny:
                if (timeSystem.CurrentHour < 8 || timeSystem.CurrentHour > 20)
                {
                    weatherImage.sprite = weatherSprite[2];
                }
                else
                {
                    weatherImage.sprite = weatherSprite[3];
                }
                break;
        }
        
    }
}
