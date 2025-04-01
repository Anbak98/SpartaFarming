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
        Debug.Log($"{Owner.name}이 죽었습니다. ");

        // 애니메이션 실행
        Owner.ChangeAnimation(EnemyAnimationState.Die);
        // 초기화 
        Owner.InitDie();

        ReturnToPool();
    }

    private void ReturnToPool() 
    {
        // ##TODO : 풀로 돌아가기
        UnitManager.Instance.ReturnToPool(Owner.UnitType , this.gameObject); 
    }
}
