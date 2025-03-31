using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight : MonoBehaviour
{
    [SerializeField] private Light2D lights;
    [SerializeField] private Gradient gradientSunny;
    [SerializeField] private Gradient gradientRainy;
    [SerializeField] private Gradient gradientWindy;
    [SerializeField] private Gradient gradientSnowy;

    private Coroutine colorCoroutine;
    public Light2D Lights { get { return lights; } }

    public void OnTimeChangedSunny()
    {
        lights.color = gradientSunny.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
    }

    public void OnTimeChangedRainy()
    {
        lights.color = gradientRainy.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
    }

    public void OnTimeChangedWindy()
    {
        lights.color = gradientWindy.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
    }

    public void OnTimeChangedSnowy()
    {
        lights.color = gradientSnowy.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
    }

    public void ChangedWeatherColor(WeatherChance weather)
    {
        Color targetColor = GetTargetColor(weather);
        if (colorCoroutine != null)
        {
            StopCoroutine(colorCoroutine);
        }

        colorCoroutine = StartCoroutine(ChangingColor(targetColor, () =>
        {
            switch (weather)
            {
                case WeatherChance.SunChance:
                    TimeManager.Instance.TimeSystem.TimeChangeUpdate += OnTimeChangedSunny;
                    break;
                case WeatherChance.RainChance:
                    TimeManager.Instance.TimeSystem.TimeChangeUpdate += OnTimeChangedRainy;
                    break;
                case WeatherChance.WindChance:
                    TimeManager.Instance.TimeSystem.TimeChangeUpdate += OnTimeChangedWindy;
                    break;
                case WeatherChance.SnowChance:
                    TimeManager.Instance.TimeSystem.TimeChangeUpdate += OnTimeChangedSnowy;
                    break;
            }
        }));

    }

    private IEnumerator ChangingColor(Color targetColor, Action TimeChangeupdate)
    {
        Color curColor = lights.color == null? gradientSunny.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f): lights.color; 
        float duration = 1f;
        float changeTime = 0f;

        while (changeTime < duration)
        {
            changeTime += Time.deltaTime;
            lights.color = Color.Lerp(curColor, targetColor, changeTime / duration);
            yield return null;
        }

        lights.color = targetColor;
        TimeChangeupdate?.Invoke();
    }

    Color GetTargetColor(WeatherChance weather)
    {
        switch (weather)
        {
            case WeatherChance.SunChance:
                return gradientSunny.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
            case WeatherChance.RainChance:
                return gradientRainy.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
            case WeatherChance.WindChance:
                return gradientWindy.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
            case WeatherChance.SnowChance:
                return gradientSnowy.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
            default:
                return gradientSunny.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
        }
    }
}
