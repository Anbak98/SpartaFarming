using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;
    public TimeSystem TimeSystem { get; set; }

    public static TimeManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<TimeManager>();
                {
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject("TimeManager");
                        _instance = obj.AddComponent<TimeManager>();
                    }
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
}
