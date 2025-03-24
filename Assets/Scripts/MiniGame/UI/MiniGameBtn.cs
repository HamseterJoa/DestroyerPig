using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameBtn : MonoBehaviour
{
    [SerializeField] private GameObject _OptionMenu;

    // 오디오
    [SerializeField] private AudioSource _Audio;

    // 종료 메뉴
    [SerializeField] private GameObject _QuitMenu;

    // 돼지 선택창
    [SerializeField] private GameObject _SelectImage;

    // 터치음
    private void PlayTouchSound()
    {
        // 볼륨 설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _Audio.Play();
    }

    // 게임 시작
    public void StartMiniGame()
    {
        PlayTouchSound();

        // BGM변경
        GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Game);

        // 씬 변경
        LoadSceneManager.LoadScene(SceneName.MiniGameScene);
    }

    // 옵션 띄우기
    public void OpenOption()
    {
        PlayTouchSound();

        _OptionMenu.gameObject.SetActive(true);
    }

    // 게임 나가기
    public void Quit()
    {
        Application.Quit();
    }

    // 게임 안나가기
    public void CancelQuit()
    {
        PlayTouchSound();

        _QuitMenu.gameObject.SetActive(false);
    }

    // 본 게임 메뉴로
    public void BackToMenu()
    {
        PlayTouchSound();

        // 씬 변경
        LoadSceneManager.LoadScene(SceneName.EnchantScene);
    }

    // 선택창 나가기
    public void CancelSelectImage()
    {
        PlayTouchSound();

        _SelectImage.gameObject.SetActive(false);
    }
    
}
