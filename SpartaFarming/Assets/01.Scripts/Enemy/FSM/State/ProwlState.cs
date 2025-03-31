using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProwlState<T> : FSM
{
    private T Owner;
    private IProwl Iprowl;

    // »ý¼ºÀÚ
    public ProwlState(T owner, IProwl iprowl = null)
    {
        this.Owner      = owner;
        this.Iprowl     = iprowl;
    }

    public override void FSM_Enter()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : ProwlState Enter");

    }

    public override void FSM_Excute()
    {
 
    }

    public override void FSM_Exit()
    {
        //Debug.Log($"Owner : {Owner.GetType().Name} : ProwlState Exit");
    }
}
