using TMPro;
using UnityEngine;

public class WeatherUI : MonoBehaviour
{
    public Animator animator { get; private set; }
    private WeatherSystem weatherSystem;
    private TimeSystem timeSystem;
    [field: SerializeField] public WeatherAnimationData animationData { get; private set; }
    [SerializeField] private TextMeshProUGUI WeatherText;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        weatherSystem = WeatherManager.Instance.WeatherSystem;
        timeSystem = TimeManager.Instance.TimeSystem;

        weatherSystem.OnWeatherChange += UpdateWeatherUI;
        timeSystem.On8oClock += On8oClock;
        timeSystem.On20oClock += On20oClock;
    }

    private void OnDisable()
    {
        weatherSystem.OnWeatherChange -= UpdateWeatherUI;
        timeSystem.On8oClock -= On8oClock;
        timeSystem.On20oClock -= On20oClock;
    }

    public void On8oClock()
    {
        if(weatherSystem.CurrentWeather == WeatherType.Sunny)
        {
            animator.Play(animationData.DayHash);
        }
    }

    public void On20oClock()
    {
        if(weatherSystem.CurrentWeather == WeatherType.Sunny)
        {
            animator.Play(animationData.NightHash);
        }
    }

    public void UpdateWeatherUI(WeatherType weatherType)
    {
        WeatherText.text = weatherType.ToString();

        switch (weatherType)
        {
            case WeatherType.Rain:
                animator.Play(animationData.RainingHash);
                break;
            case WeatherType.Thunder:
                animator.Play(animationData.LightingHash);
                break;
            case WeatherType.ThunderStorm:
                animator.Play(animationData.RainAndLightHash);
                break;
            case WeatherType.Sunny:
                if(timeSystem.CurrentHour < 8 || timeSystem.CurrentHour > 20)
                {
                    animator.Play(animationData.NightHash);
                }
                else
                {
                    animator.Play(animationData.DayHash);
                }
                break;
        }
    }
}
