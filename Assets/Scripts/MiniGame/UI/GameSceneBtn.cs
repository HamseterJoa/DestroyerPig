using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneBtn : MonoBehaviour
{
    // 옵션 메뉴
    [SerializeField] private Image _OptionMenu;

    // 오디오
    [SerializeField] private AudioSource _Audio;

    // 공발사기
    [SerializeField] private BallShooter _BS;

    // 클릭 소리
    private void PlayClickSound()
    {
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        _Audio.Play();
    }

    // 정지
    public void Pause()
    {
        PlayClickSound();

        // 시간 정지
        Time.timeScale = 0.0f;

        // 옵션창 띄우기
        _OptionMenu.gameObject.SetActive(true);
    }

    // 재개
    public void Resume()
    {
        PlayClickSound();

        // 시간 흐름
        Time.timeScale = 1.0f;

        // 옵션창 비활성화
        _OptionMenu.gameObject.SetActive(false);
    }

    // 게임 끝내기
    public void EndGame()
    {
        PlayClickSound();

        // 끝내기 호출
        _BS.End();
    }

    // 나가기
    public void ExitGame()
    {
        PlayClickSound();

        // 시간 흐름
        Time.timeScale = 1.0f;

        // BGM변경
        GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Title);

        // 씬 이동
        LoadSceneManager.LoadScene(SceneName.MiniGameGotchaScene);
    }

}
