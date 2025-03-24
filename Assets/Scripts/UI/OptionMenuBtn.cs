using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuBtn : MonoBehaviour
{
    // 메뉴
    [SerializeField] private Image _OptionMenu;

    // BGM
    [SerializeField] private Slider _BGMSlider;

    // 효과음
    [SerializeField] private Slider _EffectSlider;

    // 목소리
    [SerializeField] private Slider _VoiceSlider;

    // 설정값을 받아올거
    private JsonData.PlayerData _PlayerData;

    // 터치 소리
    [SerializeField] private AudioSource _EffectAudio;

    //  > 버튼 클릭 사운드 재생
    private void PlayEffectSound()
    {
        // 볼륨 설정
        _EffectAudio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _EffectAudio.Play();
    }

    private void OnEnable()
    {
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더 값 설정
        if (_BGMSlider != null)
        {
            _BGMSlider.value = _PlayerData.BGMValue;
            _EffectSlider.value = _PlayerData.EffectValue;
            _VoiceSlider.value = _PlayerData.VoiceValue;
        }
    }

    // X 표시를 눌렀을 때
    public void OnClickCancel()
    {
        // 터치음 재생
        PlayEffectSound();

        _OptionMenu.gameObject.SetActive(false);
    }

    // BGM 볼륨 빼기를 눌렀을 떄
    public void BGMMinusBtn()
    {
        // 최신 데이터 받아오기
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더의 값 감소
        _BGMSlider.value -= 0.1f;

        // 데이터의 소리값을 슬라이더의 값으로
        _PlayerData.BGMValue = _BGMSlider.value;
        
        // 데이터 저장
        GameManager.getJsonDataManager.playerData = _PlayerData;

        // BGM 볼륨 업데이트
        GameManager.getAudioManager.BGMVolumeUpdate();
    }

    // 효과음 볼륨 빼기를 눌렀을 떄
    public void EffectSoundMinusBtn()
    {
        // 최신 데이터 받아오기
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더의 값 감소
        _EffectSlider.value -= 0.1f;

        // 데이터의 소리값을 슬라이더의 값으로
        _PlayerData.EffectValue = _EffectSlider.value;
        
        // 데이터 저장
        GameManager.getJsonDataManager.playerData = _PlayerData;

        // 터치음 재생
        PlayEffectSound();
    }

    // 목소리 볼륨 빼기를 눌렀을 떄
    public void VoiceMinusBtn()
    {
        // 최신 데이터 받아오기
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더의 값 감소
        _VoiceSlider.value -= 0.1f;

        // 데이터의 소리값을 슬라이더의 값으로
        _PlayerData.VoiceValue = _VoiceSlider.value;

        // 데이터 저장
        GameManager.getJsonDataManager.playerData = _PlayerData;
    }

    // BGM 볼륨 더하기를 눌렀을 떄
    public void BGMPlusBtn()
    {
        // 최신 데이터 받아오기
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더의 값 증가
        _BGMSlider.value += 0.1f;

        // 데이터의 소리값을 슬라이더의 값으로
        _PlayerData.BGMValue = _BGMSlider.value;
        
        // 데이터 저장
        GameManager.getJsonDataManager.playerData = _PlayerData;

        // BGM 볼륨 업데이트
        GameManager.getAudioManager.BGMVolumeUpdate();
    }

    // 효과음 볼륨 더하기를 눌렀을 떄
    public void EffectSoundPlusBtn()
    {
        // 최신 데이터 받아오기
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더의 값 감소
        _EffectSlider.value += 0.1f;

        // 데이터의 소리값을 슬라이더의 값으로
        _PlayerData.EffectValue = _EffectSlider.value;
        
        // 데이터 저장
        GameManager.getJsonDataManager.playerData = _PlayerData;

        // 터치음 재생
        PlayEffectSound();
    }

    // 목소리 볼륨 더하기를 눌렀을 떄
    public void VoicePlusBtn()
    {
        // 최신 데이터 받아오기
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더의 값 감소
        _VoiceSlider.value += 0.1f;

        // 데이터의 소리값을 슬라이더의 값으로
        _PlayerData.VoiceValue = _VoiceSlider.value;
        
        // 데이터 저장
        GameManager.getJsonDataManager.playerData = _PlayerData;
    }

    // BGM슬라이더
    public void BGMSlider(Slider sliderValue)
    {
        // 최신 데이터 받아오기
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더 값을 볼륨값으로
        _PlayerData.BGMValue = sliderValue.value;
        
        // 데이터 저장
        GameManager.getJsonDataManager.playerData = _PlayerData;

        // BGM 볼륨 업데이트
        GameManager.getAudioManager.BGMVolumeUpdate();
    }

    // 이펙트 슬라이더
    public void EffectSlider(Slider sliderValue)
    {
        // 최신 데이터 받아오기
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더 값을 볼륨값으로
        _PlayerData.EffectValue = sliderValue.value;

        // 데이터 저장
        GameManager.getJsonDataManager.playerData = _PlayerData;

        // BGM 볼륨 업데이트
        GameManager.getAudioManager.BGMVolumeUpdate();

        // 터치음 재생
        PlayEffectSound();
    }

    // 목소리 슬라이더
    public void VoiceSlider(Slider sliderValue)
    {
        // 최신 데이터 받아오기
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 슬라이더 값을 볼륨값으로
        _PlayerData.VoiceValue = sliderValue.value;

        // 데이터 저장
        GameManager.getJsonDataManager.playerData = _PlayerData;

        // BGM 볼륨 업데이트
        GameManager.getAudioManager.BGMVolumeUpdate();
    }

}
