using System;
using UnityEngine;

[Serializable]
public class WeatherAnimationData
{
    [SerializeField] private string dawnAnimationName = "Dawn";
    [SerializeField] private string dayAnimationName = "Day";
    [SerializeField] private string nightAnimationName = "Night";
    [SerializeField] private string noonAnimationName = "Noon";
    [SerializeField] private string rainingAnimationName = "Raining";
    [SerializeField] private string lightingAnimationName = "Lightning";
    [SerializeField] private string rainAndLightAnimationName = "Rain&Light";
    [SerializeField] private string randomParameterName = "Random";

    public int DawnHash { get; }
    public int DayHash { get; }
    public int NightHash { get; }
    public int NoonHash { get; }
    public int RainingHash { get; }
    public int LightingHash { get; }
    public int RainAndLightHash { get; }
    public int RandomHash { get; }

    public WeatherAnimationData()
    {
        DawnHash = Animator.StringToHash(dawnAnimationName);
        DayHash = Animator.StringToHash(dayAnimationName);
        NightHash = Animator.StringToHash(nightAnimationName);
        NoonHash = Animator.StringToHash(noonAnimationName);
        RainingHash = Animator.StringToHash(rainingAnimationName);
        LightingHash = Animator.StringToHash(lightingAnimationName);
        RainAndLightHash = Animator.StringToHash(rainAndLightAnimationName);
        RandomHash = Animator.StringToHash(randomParameterName);
    }
}
