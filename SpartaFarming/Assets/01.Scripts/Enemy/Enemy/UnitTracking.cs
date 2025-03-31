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
            Debug.LogError("Base Unit�� Player�� null ���� ");
            return;
        }

        targetPosition = Owner.Player.transform.position;

        // ���� ������ ��ġ
        Vector3 currentPosition = transform.position;

        // ��ǥ ��ġ������ �Ÿ�
        float distanceToTarget = Vector2.Distance(currentPosition, targetPosition);

        // �ð��� ����� �ε巯�� �̵�
        float step = Owner.UnitState.speed * Time.deltaTime;
        float t = step / distanceToTarget; // ����ȭ�� �̵� ����

        // ���� �̵� (Lerp ���)
        transform.position = Vector2.Lerp(currentPosition, targetPosition, t);

        // attack ���� �ȿ� ������ -> Attack���� ���� 
        distanceToPlayer = Vector2.Distance(transform.position, Owner.Player.position);
        if (distanceToPlayer <= Owner.UnitState.attackTriggerRange)
        {
            Owner.ChageState(EnemyState.Attack);
        }
        // ���� �÷��̾ �־����� -> Prowl ���� 
        if (Vector2.Distance(transform.position, Owner.Player.position) >= Owner.UnitState.trackingTriggerRange) 
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
}
