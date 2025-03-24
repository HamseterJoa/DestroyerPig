using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectBtn : MonoBehaviour
{
    // 상태
    [SerializeField] public MiniGamePlayerState _State;

    // 마커
    [SerializeField] private Image _Marker;

    // 오디오
    [SerializeField] private AudioSource _Audio;

    // 눌렀을 때 호출
    public void OnClicked()
    {
        // 클릭 소리
        PlayClickSound();

        JsonData.PlayerData player = GameManager.getJsonDataManager.playerData;

        // 현재 상태가 선택된 상태와 같다면 리턴
        if (player.PlayerState == _State) return;

        // 플레이어의 상태를 현재 선택된 상태로 변경
        player.PlayerState = _State;

        // 마커의 위치 변경
        _Marker.rectTransform.anchoredPosition = new Vector2(ReturnNumber(_State) * 350.0f, 0.0f);

        // 저장
        GameManager.getJsonDataManager.playerData = player;
        GameManager.getJsonDataManager.SavePlayerData();
    }

    // 상태에 따라 번호를 리턴함
    private int ReturnNumber(MiniGamePlayerState state)
    {
        return (int)state;
    }

    // 클릭 소리
    private void PlayClickSound()
    {
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        _Audio.Play();
    }

}
