using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherVFX : MonoBehaviour
{
    [SerializeField] private GameObject rainEffect;
    [SerializeField] private GameObject snowEffect;
    [SerializeField] private GameObject fallWindEffect;
    [SerializeField] private GameObject springWindEffect;

    public GameObject RainEffect {  get { return rainEffect; } }
    public GameObject SnowEffect { get {return snowEffect; } }
    public GameObject FallWindEffect { get { return fallWindEffect; } }
    public GameObject SpringWindEffect { get { return springWindEffect; } }

}
