using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // 배경음악들을 담을 리스트
    [SerializeField] private List<AudioClip> _BGMList = new List<AudioClip>();

    // 배경음악의 상태의 종류
    public enum BGMSort { Title, Game, Boss}

    // 출력할떄 사용할 오디오쏘쓰
    public static AudioSource _Audio;

    private void Start()
    {
        // 오디오 소스참조
        _Audio = GetComponent<AudioSource>();

        // 오디오 클립 설정
        _Audio.clip = _BGMList[0];

        // 오디오 볼륨 설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.BGMValue;

        // 오디오 재생
        _Audio.Play();
    }

    public void BGMVolumeUpdate()
    {
        // 오디오 볼륨 설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.BGMValue;
    }

    public void ChangeBGM(BGMSort bgm)
    {
        int bgmType = 0;

        switch (bgm)
        {
            case BGMSort.Title: bgmType = 0; break;
            case BGMSort.Game: bgmType = 1; break;
            case BGMSort.Boss: bgmType = 2; break;
        }

        // 배경음악 변경
        _Audio.clip = _BGMList[bgmType];

        // 재생
        _Audio.Play();
    }

}
