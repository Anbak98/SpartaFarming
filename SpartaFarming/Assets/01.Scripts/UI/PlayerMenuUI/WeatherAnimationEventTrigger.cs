using UnityEngine;

public class WeatherAnimationEventTrigger : MonoBehaviour
{
    private WeatherUI weatherUI;

    private void Awake()
    {
        weatherUI = GetComponentInParent<WeatherUI>();
    }

    public void OnAnimationRandomEvent()
    {
        weatherUI.animator.SetFloat(weatherUI.animationData.RandomHash, Random.Range(0f, 1f));
    }
    
}
