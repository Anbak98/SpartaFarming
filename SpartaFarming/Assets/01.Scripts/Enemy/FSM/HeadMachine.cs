using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeadMachine<T>
{
    [Header("===Owner===")]
    [SerializeField]
    private T Owner;

    [Header("===State===")]
    [SerializeField]
    private FSM currState;
    [SerializeField]
    private FSM prevState;

    // ������
    public HeadMachine(T owner) 
    { 
        this.Owner = owner;
    }

    // �ʱ� ���� 
    public void HM_InitMachine(FSM initState) 
    {
        // ���� ���� ���� 
        currState = initState;    
    }

    // ���� ����
    public void HM_StateEnter() 
    {
        // ���� ���� Enter 
        if (currState != null)
            currState.FSM_Enter();
    }

    // ���� ���� 
    public void HM_StateExcute() 
    {
        // ���� ���� ����
        if(currState != null)
            currState.FSM_Excute();
    }

    // ���� ��ȯ
    public void HM_ChangeState(FSM nextState) 
    {
        // ������ 
        if (currState == nextState)
            return;

        // �������� = ������� 
        prevState = currState;

        // ���� ���°� null �� �ƴϸ�
        if (prevState != null)
            prevState.FSM_Exit();

        // ������� = �������� 
        currState = nextState;

        if (currState != null)
            currState.FSM_Enter();
    }

}
