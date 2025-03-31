using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitProwl : MonoBehaviour, IProwl<BaseUnit>
{
    [SerializeField]
    private BaseUnit Owner;
    [SerializeField]
    private float distanceToPlayer;

    public void IProwlInit(BaseUnit temp)
    {
        this.Owner = temp;
    }

    public void IProwl_Enter()
    {
        distanceToPlayer = 100f;    // �ʱ⿡ �ӽ÷� ��Ƴ��� 
        StartCoroutine(Prowl());
    }

    public void IProwl()
    {
        if (Owner.Player == null)
        {
            Debug.LogError("Base Unit�� Player�� null ���� ");
            return;
        }

        // �÷��̾�� �����Ÿ� ��ŭ ��������� -> Tracking���� ���� 
        distanceToPlayer = Vector2.Distance(transform.position , Owner.Player.position);
        if (distanceToPlayer <= Owner.UnitState.trackingTriggerRange) 
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
        Vector2 nextPosition;
        while (true) 
        { 
            Vector3 currUnitPosition = transform.position;

            // �� ��ġ �ޱ� 
            nextPosition = RandomPosition(currUnitPosition);

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

            // ���� �̵� �� ��� ���
            yield return new WaitForSeconds(1.5f);
        }
    }

    
    private Vector3 RandomPosition(Vector2 origin) 
    {
        for (int i = 0; i < 5; i++) 
        {
            float minX = origin.x - Owner.UnitState.trackingTriggerRange;
            float maxX = origin.x + Owner.UnitState.trackingTriggerRange;
            float minY = origin.y - Owner.UnitState.trackingTriggerRange;
            float maxY = origin.y + Owner.UnitState.trackingTriggerRange;

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
