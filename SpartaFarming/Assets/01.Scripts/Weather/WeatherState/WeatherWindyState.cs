
public class WeatherWindyState : WeatherBaseState
{
    public WeatherWindyState(WeatherStateMachine stateMachine) : base(stateMachine)
    {

    }
    public override void Enter()
    {
        base.Enter();
        WeatherManager.Instance.WeatherSystem.WorldLight.ChangedWeatherColor(WeatherChance.WindChance);
        if(WeatherManager.Instance.WeatherSystem.CurrentSeason.season == SeasonType.Spring)
        {
            WeatherManager.Instance.WeatherSystem.WeatherVFX.SpringWindEffect.OnEnable();
        }
        else if(WeatherManager.Instance.WeatherSystem.CurrentSeason.season == SeasonType.Fall)
        {
            WeatherManager.Instance.WeatherSystem.WeatherVFX.FallWindEffect.OnEnable();
        }
    }

    public override void Exit()
    {
        if (WeatherManager.Instance.WeatherSystem.CurrentSeason.season == SeasonType.Summer ||
            WeatherManager.Instance.WeatherSystem.CurrentSeason.season == SeasonType.Spring)
        {
            WeatherManager.Instance.WeatherSystem.WeatherVFX.SpringWindEffect.OnDisable();
        }
        else if (WeatherManager.Instance.WeatherSystem.CurrentSeason.season == SeasonType.Winter ||
            WeatherManager.Instance.WeatherSystem.CurrentSeason.season == SeasonType.Fall)
        {
            WeatherManager.Instance.WeatherSystem.WeatherVFX.FallWindEffect.OnDisable();
        }
        base.Exit();
        TimeManager.Instance.TimeSystem.TimeChangeUpdate -= WeatherManager.Instance.WeatherSystem.WorldLight.OnTimeChangedWindy;
    }
}
