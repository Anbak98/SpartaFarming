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
        // 동작 실행 
        StartCoroutine(Prowl());
    }

    public void IProwl()
    {
        if (Owner.Player == null)
        {
            Debug.LogError("Base Unit의 Player가 null 상태 ");
            return;
        }

        // 플립
        // Owner.Flip();
        // 사망
        Owner.IsDie();

        // 플레이어와 일정거리 만큼 가까워지면 -> Tracking으로 변경 
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
            // 애니메이션 실행 Run 
            Owner.ChangeAnimation(EnemyAnimationState.Run);

            // 배회 움직임 
            yield return StartCoroutine(LerpProwl());

            // 애니메이션 실행 Idle
            Owner.ChangeAnimation(EnemyAnimationState.Idle);
            // 다음 이동 전 잠시 대기
            yield return new WaitForSeconds(1.5f);
        }
    }

    private IEnumerator LerpProwl()
    {
        Vector2 nextPosition;

        Vector3 currUnitPosition = transform.position;

        // 새 위치 받기 
        nextPosition = RandomPosition(currUnitPosition);

        // 플립
        Owner.Flip(nextPosition.x);

        // 시작 위치 저장
        Vector2 startPosition = currUnitPosition;

        // 이동에 걸리는 시간 계산
        float journeyLength = Vector2.Distance(startPosition, nextPosition);
        float journeyTime = journeyLength / 3f;

        // Lerp로 자연스럽게 이동
        float elapsedTime = 0;

        while (elapsedTime < journeyTime)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / journeyTime); // 0~1 사이 값으로 정규화

            transform.position = Vector2.Lerp(startPosition, nextPosition, t);
            yield return null; // 다음 프레임까지 대기
        }

        // 정확한 위치로 설정
        transform.position = nextPosition;

        // 종료 
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

            // 접근 가능한지 ?
            if(CanReach(origin, new Vector2(ranX, ranY))) 
            {
                return new Vector3 (ranX, ranY);    
            }
        }

        return origin;
    }
    
    // 장애물이 있는지 검사 
    private bool CanReach(Vector2 origin , Vector2 dest) 
    {
        // 방향 벡터
        Vector2 dir = dest - origin;

        // raycast ( 시작위치, 방향, 거리, 레이어 )
        RaycastHit2D hit = Physics2D.Raycast(origin, dir , 10f , Owner.ObstacleLayer);

        // 레이 디버그  
        Debug.DrawRay(origin, dir * 10f, Color.red, 0.1f);

        // 충돌되면 -> false
        if (hit.collider != null)
            return false;

        // 충돌 안되면 -> true 
        return true;
    }
    
}
