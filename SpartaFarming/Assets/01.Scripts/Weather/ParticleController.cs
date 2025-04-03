using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        particle.Stop();
    }

    public void OnEnable()
    {
        this.gameObject.SetActive(true);
        particle.Play();
    }

    public void OnDisable()
    {
        particle.Stop();
        this.gameObject.SetActive(false);
    }
}
