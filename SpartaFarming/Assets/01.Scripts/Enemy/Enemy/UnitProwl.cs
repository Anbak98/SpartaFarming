using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitProwl : MonoBehaviour, IProwl<BaseUnit>
{
    [SerializeField]
    private BaseUnit Owner;
    [SerializeField]
    private float prowlRange;

    public void IProwlInit(BaseUnit temp)
    {
        this.Owner = temp;
    }

    public void IProwl_Enter()
    {
        // ���� ���� 
        StartCoroutine(Prowl());
    }

    public void IProwl()
    {
        if (Owner.Player == null)
        {
            Debug.LogError("Base Unit�� Player�� null ���� ");
            return;
        }

        // �ø�
        // Owner.Flip();
        // ���
        Owner.IsDie();

        // �÷��̾�� �����Ÿ� ��ŭ ��������� -> Tracking���� ���� 
        if (Owner.isInRange(Owner.UnitState.trackingTriggerRange)) 
        {
            Owner.ChageState(EnemyState.Tracking);
        }
    }

    public void IProwl_Exit()
    {
        StopAllCoroutines();
    }

    IEnumerator Prowl() 
    {
        while (true) 
        {
            // �ִϸ��̼� ���� Run 
            Owner.ChangeAnimation(EnemyAnimationState.Run);

            // ��ȸ ������ 
            yield return StartCoroutine(LerpProwl());

            // �ִϸ��̼� ���� Idle
            Owner.ChangeAnimation(EnemyAnimationState.Idle);
            // ���� �̵� �� ��� ���
            yield return new WaitForSeconds(1.5f);
        }
    }

    private IEnumerator LerpProwl()
    {
        Vector2 nextPosition;

        Vector3 currUnitPosition = transform.position;

        // �� ��ġ �ޱ� 
        nextPosition = RandomPosition(currUnitPosition);

        // �ø�
        Owner.Flip(nextPosition.x);

        // ���� ��ġ ����
        Vector2 startPosition = currUnitPosition;

        // �̵��� �ɸ��� �ð� ���
        float journeyLength = Vector2.Distance(startPosition, nextPosition);
        float journeyTime = journeyLength / 3f;

        // Lerp�� �ڿ������� �̵�
        float elapsedTime = 0;

        while (elapsedTime < journeyTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / journeyTime); // 0~1 ���� ������ ����ȭ

            transform.position = Vector2.Lerp(startPosition, nextPosition, t);
            yield return null; // ���� �����ӱ��� ���
        }

        // ��Ȯ�� ��ġ�� ����
        transform.position = nextPosition;

        // ���� 
        yield break;
    }
    
    private Vector3 RandomPosition(Vector2 origin) 
    {
        for (int i = 0; i < 5; i++) 
        {
            float minX = origin.x - Owner.UnitState.prowlRange;
            float maxX = origin.x + Owner.UnitState.prowlRange;
            float minY = origin.y - Owner.UnitState.prowlRange;
            float maxY = origin.y + Owner.UnitState.prowlRange;

            float ranX = Random.Range(minX, maxX);
            float ranY = Random.Range(minY, maxY);

            // ���� �������� ?
            if(CanReach(origin, new Vector2(ranX, ranY))) 
            {
                return new Vector3 (ranX, ranY);    
            }
        }

        return origin;
    }
    
    // ��ֹ��� �ִ��� �˻� 
    private bool CanReach(Vector2 origin , Vector2 dest) 
    {
        // ���� ����
        Vector2 dir = dest - origin;

        // raycast ( ������ġ, ����, �Ÿ�, ���̾� )
        RaycastHit2D hit = Physics2D.Raycast(origin, dir , 10f , Owner.ObstacleLayer);

        // ���� �����  
        Debug.DrawRay(origin, dir * 10f, Color.red, 0.1f);

        // �浹�Ǹ� -> false
        if (hit.collider != null)
            return false;

        // �浹 �ȵǸ� -> true 
        return true;
    }
    
}
