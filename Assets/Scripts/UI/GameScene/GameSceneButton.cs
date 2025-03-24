using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneButton : MonoBehaviour
{
    //  > Main Scene Canvas 를 참조할 변수
    private Canvas _Canvas = null;
    [SerializeField] private WaveManager wave;
    [SerializeField] private HackGauge hackGauge;

    // 터치 소리
    [SerializeField] private AudioSource _Audio;

    //  > 버튼 클릭 사운드 재생
    private void PlayClickSound()
    {
        // 볼륨 설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _Audio.Play();
    }

    private void Start()
    {
        _Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
    }

    // 멈춤
    public void OnPauseBtnClick()
    {
        // 터치음 재생
        PlayClickSound();

        _Canvas.SwitchingPage(Canvas.GameSceneState.Pause);
    }

    // 홈키가 눌렸다면 호출
    private void OnApplicationPause()
    {
        OnPauseBtnClick();
    }

    // 진행
    public void OnResumeBtnClick()
    {
        // 터치음 재생
        PlayClickSound();

        _Canvas.SwitchingPage(Canvas.GameSceneState.Playing);
    }

    // 나가기 버튼을 눌렀다면
    public void OnExitBtnClick()
    {
        // 터치음 재생
        PlayClickSound();

        wave.OnResult();
        GameManager.getCharacterManager.bonusgetset = 0;
    }

    // 나가기
    public void OnExitBtn()
    {
        // 터치음 재생
        PlayClickSound();

        LoadSceneManager.LoadScene(SceneName.EnchantScene);

        // 배경음악 변경
        GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Title);

        if (Time.timeScale != 1.0f)
            Time.timeScale = 1.0f;
    }

    // 핵
    public void OnHackBtn()
    {
        if (hackGauge.hack >= 1.0f)
        {
            // 터치음 재생
            PlayClickSound();
            
            // 빼고
            hackGauge.hack -= 1.0f;
            
            // 상태 업데이트
            hackGauge.HackGaugeUpdate();
            GameManager.getCharacterManager.player.hackable = true;
        }
    }

}
