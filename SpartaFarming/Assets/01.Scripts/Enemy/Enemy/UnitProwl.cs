using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitProwl : MonoBehaviour, IProwl<Unit>
{
    [SerializeField]
    private Unit Owner;
    public void IProwlInit(Unit temp)
    {
        this.Owner = temp;
    }

    public void IProwl()
    {
        
    }

    public void IProwl_Enter()
    {
        
    }

    public void IProwl_Exit()
    {
        
    }
}
