using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour, IAttack<Unit>
{
    [SerializeField]
    private Unit Owner;

    public void IAttackInit(Unit temp)
    {
        this.Owner = temp;
    }

    public void IAttack()
    {

    }

    public void IAttack_Enter()
    {

    }

    public void IAttack_Exit()
    {

    }
}
