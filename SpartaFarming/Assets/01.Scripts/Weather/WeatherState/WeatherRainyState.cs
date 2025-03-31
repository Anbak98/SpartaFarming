
public class WeatherRainyState : WeatherBaseState
{
    public WeatherRainyState(WeatherStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        WeatherManager.Instance.WeatherSystem.WorldLight.ChangedWeatherColor(WeatherChance.RainChance);
        base.Enter();
        WeatherManager.Instance.WeatherSystem.WeatherVFX.RainEffect.OnEnable();
    }

    public override void Exit()
    {
        base.Exit();
        WeatherManager.Instance.WeatherSystem.WeatherVFX.RainEffect.OnDisable();
        TimeManager.Instance.TimeSystem.TimeChangeUpdate -= WeatherManager.Instance.WeatherSystem.WorldLight.OnTimeChangedRainy;
    }
}
