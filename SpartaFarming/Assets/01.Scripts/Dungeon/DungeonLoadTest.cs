using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLoadTest : MonoBehaviour
{

    void Update()
    {
        // 던전으로 넘어가기 
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneLoader.Instance.ChangeScene(SceneState.DungeonScene);
        }
        // 메인씬 넘어가기
        else if(Input.GetKeyDown(KeyCode.K))
        {
            SceneLoader.Instance.ChangeScene(SceneState.MainScene);
        }
    }
}
