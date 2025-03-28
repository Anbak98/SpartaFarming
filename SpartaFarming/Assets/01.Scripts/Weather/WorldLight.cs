using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class WorldLight : MonoBehaviour
{
    [SerializeField]private Light2D worldLight;
    [SerializeField] private Gradient gradient;

    private void Start()
    {
        TimeManager.Instance.TimeSystem.TimeChangeUpdate += OnTimeChanged;
    }

    private void OnTimeChanged()
    {
        worldLight.color = gradient.Evaluate(TimeManager.Instance.TimeSystem.currentGameTime / 24f);
    }

}
