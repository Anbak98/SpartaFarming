using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProwlState<T, F> : FSM
{
    private T Owner;
    private IProwl<F> Iprowl;

    // »ý¼ºÀÚ
    public ProwlState(T owner, IProwl<F> iprowl = null)
    {
        this.Owner      = owner;
        this.Iprowl     = iprowl;
    }

    public override void FSM_Enter()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : ProwlState Enter");
        Iprowl.IProwl_Enter();
    }

    public override void FSM_Excute()
    {
 
    }

    public override void FSM_Exit()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : ProwlState Exit");
    }
}
