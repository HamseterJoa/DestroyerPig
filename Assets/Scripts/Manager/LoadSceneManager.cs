using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneName { StartScene, LoadingScene, EnchantScene, GameScene, MiniGameScene, MiniGameGotchaScene, Tutorial }

public class LoadSceneManager : MonoBehaviour
{
    // 씬을 저장함
    private static SceneName _LoadSceneName = SceneName.StartScene;

    // 로딩 상태
    private static bool _IsLoading = false;

    // 로딩시 딜레이
    private float _LoadingDelay = 3.0f;

    private void Start()
    {
        StartLoding();
    }

    public static void LoadScene(SceneName loadScene)
    {
        // 로딩씬으로 이동
        SceneManager.LoadScene(SceneName.LoadingScene.ToString());
        
        // 이동할 씬 저장
        _LoadSceneName = loadScene;

        // 씬이 변경될 때 데이터 저장
        GameManager.getJsonDataManager.SaveItemEnchantData();
        GameManager.getJsonDataManager.SavePlayerData();

        // 씬이 변경될때 GameScene이 아니라면 아이템 리스트 초기화
        if (loadScene != SceneName.GameScene) GameManager.getItemManager.ClearItemList();
    }

    private void StartLoding()
    {
        // 로딩상태 변경
        _IsLoading = true;

        // 씬 로딩시작
        StartCoroutine(Loading());
    }
    //public SceneName getSceneState { get { return _SceneState; } }

    private IEnumerator Loading()
    {
        while (_IsLoading)
        { 
            yield return new WaitForSeconds(_LoadingDelay);
            
            // 로딩이 끝나면 씬이동
            SceneManager.LoadScene(_LoadSceneName.ToString());

            _IsLoading = false;
        }

        yield return null;
    }

}
