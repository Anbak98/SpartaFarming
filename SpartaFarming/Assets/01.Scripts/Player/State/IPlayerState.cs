using UnityEngine.InputSystem;

public interface IPlayerState
{
    public void Enter();
    public void Exit();
    public void DoAction();
    public void OnUpdate();
    public void HandleAction(InputAction.CallbackContext context);
}
