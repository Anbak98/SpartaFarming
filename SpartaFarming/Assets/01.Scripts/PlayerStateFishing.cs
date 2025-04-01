using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerStateFishing : IPlayerState
{
    public FishObject Fish;
    public GameObject UI;
    public Image LeftSignal;
    public Image RightSignal;
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
    }

    public void OnUpdate()
    {
        if(IsActivated && Fish != null)
        {
            if(Fish.PowerDirection.x > 0)
            {
                LeftSignal.color = Color.white;
                RightSignal.color = Color.red;
            }
            else
            {
                LeftSignal.color = Color.red;
                RightSignal.color = Color.white;
            }
            
            //if (Input.GetMouseButtonDown(0))
            //{
            //    if(Struggle(new Vector3(1, 0, 0)))
            //    {
            //        Destroy(Fish.gameObject);
            //        UI.SetActive(false);
            //    }
            //}
            //else if (Input.GetMouseButtonDown(1))
            //{
            //    Struggle(new Vector3(-1, 0, 0));
            //}
        }
    }
}
