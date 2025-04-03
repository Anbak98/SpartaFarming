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
        Debug.Log($"{Owner.name}�� �׾����ϴ�. ");

        // �ִϸ��̼� ����
        Owner.ChangeAnimation(EnemyAnimationState.Die);
        // �ʱ�ȭ 
        Owner.InitDie();

        ReturnToPool();
    }

    private void ReturnToPool() 
    {
        // ##TODO : Ǯ�� ���ư���
        UnitManager.Instance.ReturnToPool(Owner.UnitType , this.gameObject); 
    }
}
