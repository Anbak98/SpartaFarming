
public class WeatherBaseState : IWeatherState
{
    protected WeatherStateMachine stateMachine;

    public WeatherBaseState(WeatherStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        //���� ����
    }

    public virtual void Exit()
    {
        //���� ���� ����
    }

    public virtual void Update()
    {
    }
}
