
public class WeatherSnowyState : WeatherBaseState
{
    public WeatherSnowyState(WeatherStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        WeatherManager.Instance.WeatherSystem.WorldLight.ChangedWeatherColor(WeatherChance.SnowChance);
        WeatherManager.Instance.WeatherSystem.WeatherVFX.SnowEffect.OnEnable();
    }

    public override void Exit()
    {
        WeatherManager.Instance.WeatherSystem.WeatherVFX.SnowEffect.OnDisable();
        base.Exit();
        TimeManager.Instance.TimeSystem.TimeChangeUpdate -= WeatherManager.Instance.WeatherSystem.WorldLight.OnTimeChangedSnowy;
    }
}
