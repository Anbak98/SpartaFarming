using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public abstract class FSM
{
    // 상태 진입 시 1회
    public abstract void FSM_Enter();

    // 상태에서 매프레임 실행
    public abstract void FSM_Excute();
    
    // 상태 종료 시 1회
    public abstract void FSM_Exit();
}
