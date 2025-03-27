using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [Header("시간")]
    [SerializeField][Range(0f, 24f)] private float initialGameTime = 8;
    [SerializeField] private float timeMultiplier = 1f;

    private float oneSecond = 1/3600f;
    public float currentGameTime = 0f;
    private int currentHour;
    private int currentMinute;

    [Header("날짜")]
    [SerializeField] private int initialDay = 1;
    [SerializeField] private int initialMonth = 8;
    [SerializeField] private int initialYear = 2025;

    private int[] daysInMonth = new int[] { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
    private int currentDay;
    private int currentMonth;
    private int currentYear;
    private int currentDaysInMonth;
    public Action DateChanged;

    public int CurrentHour { get { return currentHour; } }
    public int CurrentMinute { get { return currentMinute; } }
    public int CurrentDay { get { return currentDay; } }
    public int CurrentMonth { get { return currentMonth; } }
    public int CurrentYear { get { return currentYear; } }

    private void Awake()
    {
        TimeManager.Instance.TimeSystem = this;
    }
    void Start()
    {
        Init();
    }

    void Init()
    {
        currentGameTime = initialGameTime;
        currentDay = initialDay;
        currentMonth = initialMonth;
        currentYear = initialYear;
        currentDaysInMonth = daysInMonth[currentMonth - 1];
        DateChanged += CheckDate;
        DateChanged.Invoke();
    }

    void Update()
    {
        CountTime();
        //Debug.Log($"{currentHour}시{currentMinute}분 10분 단위");
        //Debug.Log($"{currentMonth}{currentDay}");
    }

    private void CountTime()
    {
        currentGameTime += Time.deltaTime * timeMultiplier * oneSecond;
        if (currentGameTime >= 24f)
        {
            currentGameTime -= 24f;
            currentDay++;
            if (currentDay > currentDaysInMonth)
            {
                DateChanged.Invoke();
            }
        }

        TimeCountDisplay();
    }

    private void TimeCountDisplay()
    {
        currentHour = Mathf.FloorToInt(currentGameTime);
        int min = Mathf.FloorToInt((currentGameTime - currentHour) * 60);
        currentMinute = Mathf.FloorToInt(min / 10f) * 10;
    }

    private void CheckDate()
    {
        currentDay = 1;
        currentMonth++;
        if (currentMonth > 12)
        {
            currentMonth = 1;
            currentYear++;
        }
    }

}
