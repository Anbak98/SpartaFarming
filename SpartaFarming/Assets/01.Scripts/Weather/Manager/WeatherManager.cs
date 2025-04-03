using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    private static WeatherManager _instance;

    public WeatherSystem WeatherSystem { get; set; }

    public static WeatherManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<WeatherManager>();
                {
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("WeatherManager");
                        _instance = obj.AddComponent<WeatherManager>();
                    }
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
        {
            Destroy(gameObject);
        }
    }

}
