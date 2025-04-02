using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// 씬 이름과 같아야함
public enum SceneState 
{
    MainScene,
    DungeonScene
}

public class SceneLoader : Singleton<SceneLoader>
{
    [Header("===Container===")]
    private Dictionary<SceneState, Action> sceneContainer;
    [SerializeField] private SceneState sceneState;

    private void Awake()
    {
        // DontDestory 설정 
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        // 컨테이너 초기화 
        sceneContainer = new Dictionary<SceneState, Action>();
        sceneContainer[SceneState.MainScene] = null;
        sceneContainer[SceneState.DungeonScene] = null;

        // 초기씬 : 메인 
        sceneState = SceneState.MainScene; 
    }

    private void OnEnable()
    {
        // sceneLoaded는 awake -> sceneLoaded -> start 순서로 실행됨 
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        Debug.Log("씬이 로드될 때 호출되는 함수 현재 씬 이름 :" + scene.name + "\n / 현재 씬 state" + sceneState);

        // 싱글톤 LoadSceneAsync 이 실행되고 난 후에 실행 
        // 다음 씬 이벤트 시작
        if (sceneContainer.ContainsKey(sceneState)) 
        {
            sceneContainer[sceneState]?.Invoke();
        }
    }

    public void RegisterSceneAction(SceneState state, Action action)
    {
        if (sceneContainer.ContainsKey(state))
        {
            sceneContainer[state] += action;
        }
    }

    public void DisRegistarerAction(SceneState state, Action action) 
    {
        if (sceneContainer.ContainsKey(state))
        {
            sceneContainer[state] -= action;
        }
    }

    // 씬 전환
    public void ChangeScene(SceneState nextState)
    {
        if (!sceneContainer.ContainsKey(nextState)) 
        {
            Debug.LogError($"씬 상태 {nextState}에 대한 동작이 딕셔너리에 없습니다.");
            return;
        }

        // 씬 로드
        StartCoroutine(LoadSceneAsync(nextState));
    }

    IEnumerator LoadSceneAsync(SceneState nextState) 
    {
        yield return new WaitForSeconds(0.02f);

        // 현재 씬 업데이트 
        sceneState = nextState;

        // 씬이름
        string sceneName = Enum.GetName(typeof(SceneState), sceneState);

        // 비동기화 로드 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // 끝날때까지 기다리기
        while(!asyncLoad.isDone) 
        {
            yield return null;
        }

        // 다음 씬 이벤트 시작
        // if (sceneContainer.ContainsKey(sceneState)) 
        // {
        //     sceneContainer[sceneState]?.Invoke();
        // }
    }    
}