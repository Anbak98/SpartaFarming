
public class WeatherBaseState : IWeatherState
{
    protected WeatherStateMachine stateMachine;

    public WeatherBaseState(WeatherStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter()
    {
        //날씨 시작
    }

    public virtual void Exit()
    {
        //현재 날씨 종료
    }

    public virtual void Update()
    {
    }
}
