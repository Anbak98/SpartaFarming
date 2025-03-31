using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttack : MonoBehaviour, IAttack<BaseUnit>
{
    [SerializeField]
    private BaseUnit Owner;

    public void IAttackInit(BaseUnit temp)
    {
        this.Owner = temp;
    }

    public void IAttack_Enter()
    {
        StartCoroutine(AttackCoru());
    }

    public void IAttack()
    {
        if (Owner.Player == null)
        {
            Debug.LogError("Base Unit의 Player가 null 상태 ");
            return;
        }

        // 플립
        Owner.Flip(Owner.Player.position.x);
        // 사망
        Owner.IsDie();

        // attack 범위 나가면 -> Tracking
        if ( ! Owner.isInRange(Owner.UnitState.attackTriggerRange) )
        {
            Owner.ChageState(EnemyState.Tracking);
        }
    }

    public void IAttack_Exit()
    {
        StopAllCoroutines();  
    }

    IEnumerator AttackCoru() 
    {
        while (true) 
        {

            // 히트 범위 만큼 overlapSpere , player 레이어만
            Collider2D[] collider = Physics2D.OverlapCircleAll
                (transform.position, Owner.UnitState.hitRange, UnitManager.Instance.PlayerLayer);

            if (collider.Length != 0)
            {
                // ##TODO 플레이어에게 데미지  

            }

            yield return new WaitForSeconds(Owner.UnitState.attackCoolTime);
        }
    }
}
