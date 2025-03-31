using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitTracking : MonoBehaviour, ITraking<BaseUnit>
{
    [SerializeField]
    private BaseUnit Owner;
    [SerializeField]
    private Vector2 targetPosition;
    [SerializeField]
    private float distanceToPlayer;

    public void ITrakingInit(BaseUnit temp)
    {
        this.Owner = temp;
    }

    public void ITraking()
    {
        if (Owner.Player == null)
        { 
            Debug.LogError("Base Unit의 Player가 null 상태 ");
            return;
        }

        // 추적 
        LerpTracking();

        // attack 범위 안에 들어오면 -> Attack으로 변경 
        if (Owner.isInRange(Owner.UnitState.attackTriggerRange))
        {
            Owner.ChageState(EnemyState.Attack);
        }
        // 만약 플레이어가 멀어지면 -> Prowl 변경 
        if ( ! Owner.isInRange(Owner.UnitState.trackingTriggerRange) ) 
        {
            Owner.ChageState(EnemyState.Prowl);
        }
    }

    public void ITraking_Enter()
    {
                   
    }

    public void ITraking_Exit()
    {

    }

    private void LerpTracking() 
    {
        targetPosition = Owner.Player.transform.position;

        // 현재 유닛의 위치
        Vector3 currentPosition = transform.position;

        // 목표 위치까지의 거리
        float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

        // 시간에 기반한 부드러운 이동
        float step = Owner.UnitState.speed * Time.deltaTime;
        float t = step / distanceToTarget; // 정규화된 이동 비율

        // 실제 이동 (Lerp 사용)
        transform.position = Vector2.Lerp(currentPosition, targetPosition, t);
    }
}
