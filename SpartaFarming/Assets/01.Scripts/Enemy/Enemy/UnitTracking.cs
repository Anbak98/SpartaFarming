using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTracking : MonoBehaviour, ITraking<Unit>
{
    [SerializeField]
    private Unit Owner;

    public void ITrakingInit(Unit temp)
    {
        this.Owner = temp;
    }

    public void ITraking()
    {
            
    }


    public void ITraking_Enter()
    {

    }

    public void ITraking_Exit()
    {

    }
}
