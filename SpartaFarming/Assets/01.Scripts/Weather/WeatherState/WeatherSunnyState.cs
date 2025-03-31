
public class WeatherSunnyState : WeatherBaseState
{
    public WeatherSunnyState(WeatherStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        WeatherManager.Instance.WeatherSystem.WorldLight.ChangedWeatherColor(WeatherChance.SunChance);
    }

    public override void Exit()
    {
        base.Exit();
        TimeManager.Instance.TimeSystem.TimeChangeUpdate -= WeatherManager.Instance.WeatherSystem.WorldLight.OnTimeChangedSunny;
    }
}
