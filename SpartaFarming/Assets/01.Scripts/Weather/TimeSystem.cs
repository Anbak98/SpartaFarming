using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [SerializeField][Range(0f, 24f)] private float initialGameTime;
    [SerializeField] private float timeMultiplier = 1f;
    private float oneSecond;
    public float currentGameTime = 0f;

    public int currentHour;
    public int currentMinute;

    private void Awake()
    {
        TimeManager.Instance.TimeSystem = this;
    }
    void Start()
    {
        currentGameTime = initialGameTime;
        oneSecond = 1 / 3600f;
    }

    void Update()
    {
        CountTime();
        Debug.Log($"{currentHour}시{currentMinute}분 10분 단위");
    }

    void CountTime()
    {
        currentGameTime += Time.deltaTime * timeMultiplier * oneSecond;
        if(currentGameTime >= 24f)
        {
            currentGameTime -= 24f;
        }

        TimeCountDisplay();
    }

    void TimeCountDisplay()
    {
        currentHour = Mathf.FloorToInt(currentGameTime);
        int min = Mathf.FloorToInt((currentGameTime - currentHour) * 60);
        currentMinute = Mathf.FloorToInt(min / 10f) * 10;
    }

}
