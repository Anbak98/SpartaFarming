using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProwlState<T> : FSM
{
    private T Owner;
    private IProwl<T> Iprowl;

    // »ý¼ºÀÚ
    public ProwlState(T owner, IProwl<T> iprowl = null)
    {
        this.Owner      = owner;
        this.Iprowl     = iprowl;
    }

    public override void FSM_Enter()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : ProwlState Enter");

        if(Iprowl != null) 
            Iprowl.IProwl_Enter();
    }

    public override void FSM_Excute()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : ProwlState");

        if (Iprowl != null)
            Iprowl.IProwl();
    }

    public override void FSM_Exit()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : ProwlState Exit");

        if (Iprowl != null)
            Iprowl.IProwl_Exit();
    }
}
