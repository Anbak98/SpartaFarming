using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonLoadTest : MonoBehaviour
{

    void Update()
    {
        // �������� �Ѿ�� 
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneLoader.Instance.ChangeScene(SceneState.DungeonScene);
        }
        // ���ξ� �Ѿ��
        else if(Input.GetKeyDown(KeyCode.K))
        {
            SceneLoader.Instance.ChangeScene(SceneState.MainScene);
        }
    }
}
