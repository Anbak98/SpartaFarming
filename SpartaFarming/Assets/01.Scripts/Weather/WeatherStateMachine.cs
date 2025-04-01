
public interface IWeatherState
{
    public void Enter();
    public void Exit();
    public void Update();
}

public class WeatherStateMachine
{
    private IWeatherState currentState;

    public IWeatherState CurrentState { get { return currentState; } }
    public WeatherSystem WeatherSystem { get; }
    public WeatherSunnyState SunnyState { get; }
    public WeatherRainyState RainyState { get; }
    public WeatherWindyState WindyState { get; }
    public WeatherSnowyState SnowyState { get; }

    public WeatherStateMachine(WeatherSystem weatherSystem)
    {
        this.WeatherSystem = weatherSystem;

        SunnyState = new WeatherSunnyState(this);
        RainyState = new WeatherRainyState(this);
        WindyState = new WeatherWindyState(this);
        SnowyState = new WeatherSnowyState(this);
    }
    public void ChangeState(IWeatherState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}