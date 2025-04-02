using TMPro;
using UnityEngine;

public class DateUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI yearText;
    [SerializeField] private TextMeshProUGUI monthText;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI hourText;
    [SerializeField] private TextMeshProUGUI minuteText;

    private TimeSystem timeSystem;

    private void Start()
    {
        timeSystem = TimeManager.Instance.TimeSystem;
    }

    private void Update()
    {
        UpdateDateUI();
    }

    private void UpdateDateUI()
    {
        yearText.text = $"<u>{timeSystem.CurrentYear}</u>";
        monthText.text = $"{timeSystem.CurrentMonth:D2}";
        dayText.text = $"{timeSystem.CurrentDay:D2}";
        hourText.text = $"{timeSystem.CurrentHour:D2}";
        minuteText.text = $"{timeSystem.CurrentMinute:D2}";
    }
}