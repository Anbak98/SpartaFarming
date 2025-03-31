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
    private FSM currState;
    private FSM prevState;

    // 생성자
    public HeadMachine(T owner) 
    { 
        this.Owner = owner;
    }

    // 초기 세팅 
    public void HM_InitMachine(FSM initState) 
    {
        // 현재 상태 지정 
        currState = initState;    
    }

    // 상태 시작
    public void HM_StateEnter() 
    {
        // 현재 상태 Enter 
        if (currState != null)
            currState.FSM_Enter();
    }

    // 상태 실행 
    public void HM_StateExcute() 
    {
        // 현재 상태 실행
        if(currState != null)
            currState.FSM_Excute();
    }

    // 상태 변환
    public void HM_ChangeState(FSM nextState) 
    {
        // null 이면 
        if (nextState == null)
        {
            Debug.Log($"{nextState} 의 상태가 null");
            return;
        }

        // 같으면 
        if (currState == nextState)
        {
            Debug.Log($"{nextState}가 {currState}와 같음");
            return;
        }

        // 이전상태 = 현재상태 
        prevState = currState;

        // 이전 상태가 null 이 아니면
        if (prevState != null)
            prevState.FSM_Exit();

        // 현재상태 = 다음상태 
        currState = nextState;

        if (currState != null)
            currState.FSM_Enter();
    }

}
