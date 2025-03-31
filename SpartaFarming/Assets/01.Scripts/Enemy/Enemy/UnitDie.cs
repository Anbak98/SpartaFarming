using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDie : MonoBehaviour, IDie<Unit>
{
    [SerializeField]
    private Unit Owner;

    public void IDieInit(Unit temp)
    {
            
    }

    public void IDie()
    {
    
    }

}
