using UnityEngine;

public class WeatherVFX : MonoBehaviour
{
    [SerializeField] private ParticleController rainEffect;
    [SerializeField] private ParticleController snowEffect;
    [SerializeField] private ParticleController fallWindEffect;
    [SerializeField] private ParticleController springWindEffect;

    public ParticleController RainEffect {  get { return rainEffect; } }
    public ParticleController SnowEffect { get {return snowEffect; } }
    public ParticleController FallWindEffect { get { return fallWindEffect; } }
    public ParticleController SpringWindEffect { get { return springWindEffect; } }

    private void Update()
    {
        this.transform.position = GameManager.Instance.Player.transform.position;
    }
}
