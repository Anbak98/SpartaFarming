using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitDie : MonoBehaviour, IDie<BaseUnit>
{
    [SerializeField]
    private BaseUnit Owner;

    public void IDieInit(BaseUnit temp)
    {
        this.Owner = temp;
    }

    public void IDie()
    {
    
    }

}
