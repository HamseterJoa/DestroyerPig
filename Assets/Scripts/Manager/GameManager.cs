using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _GameManager = null;
    private CharacterManager _CharacterManager = null;
    private ItemManager _ItemManager = null;
    private JsonDataManager _JsonDataManager = null;
    private LoadSceneManager _LoadSceneManager = null;
    private AudioManager _AudioManager = null;

    private void Awake()
    {
        if (_GameManager != null && _GameManager != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public static GameManager getGameManager
    {
        get
        {
            if (_GameManager == null)
            {
                //  > 게임 매니저 초기화
                _GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

                //  > Character Manager 초기화
                _GameManager._CharacterManager = _GameManager.transform.Find("CharacterManager").
                    GetComponent<CharacterManager>();

                // ItemManager 초기화
                _GameManager._ItemManager = _GameManager.transform.Find("ItemManager").GetComponent<ItemManager>();

                //  > JsonDataManager 초기화
                _GameManager._JsonDataManager = _GameManager.transform.Find("JsonDataManager").
                    GetComponent<JsonDataManager>();

                // AudioManager 초기화
                _GameManager._AudioManager = _GameManager.transform.Find("AudioManager").
                    GetComponent<AudioManager>();

            }

            return _GameManager;
        }
    }

    public static CharacterManager getCharacterManager
    { get { return getGameManager._CharacterManager; } }

    public static ItemManager getItemManager
    { get { return getGameManager._ItemManager; } }

    public static JsonDataManager getJsonDataManager
    { get { return getGameManager._JsonDataManager; } }

    public static LoadSceneManager getLoadSceneManager
    { get { return getGameManager._LoadSceneManager; } }

    public static AudioManager getAudioManager
    { get { return getGameManager._AudioManager; } }

}
