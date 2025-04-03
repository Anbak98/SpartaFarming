using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerStateFishing : MonoBehaviour, IPlayerState
{
    public FishObject Fish;
    public GameObject UI;
    public Vector3 powerDirection;

    private bool IsActivated = false;

    public void Enter()
    {
        IsActivated = true;
        UI.transform.SetParent(Fish.transform, false);
    }

    public void Exit()
    {
        IsActivated = false ;
        UI.transform.SetParent(null, false);
    }

    public void DoAction()
    {
        float ValidPower = Vector3.Dot(powerDirection, Fish.PowerDirection);
        if (ValidPower > 0)
        {
            Fish.Stamina -= (int)(ValidPower * 10);
            Debug.Log(Fish.Stamina);
        }
        else
        {

        }
    }

    public void HandleAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {

        }
    }

    public void Struggle()
    {

    }

    public void OnUpdate()
    {
        if(IsActivated && Fish != null)
        {
            
        }
    }
}
