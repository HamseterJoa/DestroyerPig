using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseBtn : MonoBehaviour
{
    // 종류
    [SerializeField] private MiniGamePlayerState _PigState;

    // 스프라이트 렌더러
    private Image _Image;

    // 원래 이미지
    [SerializeField] private Sprite _Normal;

    // 해금 하지 않았을 때 이미지
    [SerializeField] private Sprite _Unknown;

    // 오디오
    [SerializeField] private AudioSource _Audio;

    // 선택창
    [SerializeField] private SelectImage _SelectImage;

    // 데이터
    private JsonData.PlayerData _Player;

    private void Awake()
    {
        // 대이터 초기화
        Initializedata();

        // 이미지 상태설정
        SpriteUpdate();
    }

    private void OnEnable()
    {
        // 이미지 상태설정
        SpriteUpdate();
    }

    // 초기화
    private void Initializedata()
    {
        // 이미지 받고
        _Image = GetComponent<Image>();

        // 데이터 새로 받기
        _Player = GameManager.getJsonDataManager.playerData;
    }

    // 클릭 소리
    private void PlayerClickSound()
    {
        // 볼륨설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _Audio.Play();
    }

    // 스프라이트 상태를 업데이트함
    public void SpriteUpdate()
    {
        // 해금함
        if (ReturnPigState(_PigState)) _Image.sprite = _Normal;

        // 해금 안함
        else _Image.sprite = _Unknown;
    }

    // 눌렸다면 
    public void OnClicked()
    {
        // 클릭 소리
        PlayerClickSound();

        // 데이터 새로 받기
        _Player = GameManager.getJsonDataManager.playerData;

        // 언락되지 않았다면 리턴
        if (!ReturnPigState(_PigState)) return;

        // 정보창 생태 변경
        _SelectImage._State = _PigState;

        // 정보창 한번 비활성화
        _SelectImage.gameObject.SetActive(false);

        // 정보창 활성화
        _SelectImage.gameObject.SetActive(true);
    }

    // 돼지 상태에 따라 해당 해금상태를 리턴함
    private bool ReturnPigState(MiniGamePlayerState state)
    {
        // 데이터 새로 받기
        _Player = GameManager.getJsonDataManager.playerData;
        
        switch (state)
        {
            // 1날개
            case MiniGamePlayerState.Angel: return _Player.Angel;

            // 2아롱
            case MiniGamePlayerState.Arong: return _Player.Arong;

            // 3 새끼1
            case MiniGamePlayerState.Baby1: return _Player.Baby1;

            // 4새끼2
            case MiniGamePlayerState.Baby2: return _Player.Baby2;

            // 5새끼3
            case MiniGamePlayerState.Baby3: return _Player.Baby3;

            // 6새끼4
            case MiniGamePlayerState.Baby4: return _Player.Baby4;

            // 7악어새
            case MiniGamePlayerState.Bird: return _Player.Bird;

            // 8흑돼지
            case MiniGamePlayerState.Black: return _Player.Black;

            // 9튀는애
            case MiniGamePlayerState.Bouncing: return _Player.Bouncing;

            // 10황금동전
            case MiniGamePlayerState.GoldendCoin: return _Player.GoldenCoin;

            // 11겁쟁이
            case MiniGamePlayerState.Coward: return _Player.Coward;

            // 12악어
            case MiniGamePlayerState.Croco: return _Player.Croco;

            // 13악어주인
            case MiniGamePlayerState.CrocoMaster: return _Player.CrocoMaster;

            // 14까마귀
            case MiniGamePlayerState.Crow: return _Player.Crow;

            // 15파괴자
            case MiniGamePlayerState.Destroyer: return _Player.Destroyer;

            // 16돼지인형옷
            case MiniGamePlayerState.Doll: return _Player.Doll;

            // 17오리
            case MiniGamePlayerState.Duck: return _Player.Duck;

            // 18에스더
            case MiniGamePlayerState.Esther: return _Player.Esther;

            // 19복돼지
            case MiniGamePlayerState.Golden: return _Player.Golden;

            // 20우두머리
            case MiniGamePlayerState.Guv: return _Player.Guv;

            // 21햄스터
            case MiniGamePlayerState.Hamster: return _Player.Hamster;

            // 22돼지집
            case MiniGamePlayerState.House: return _Player.House;

            // 23백인분
            case MiniGamePlayerState.Hundred: return _Player.Hundred;

            // 24철갑
            case MiniGamePlayerState.Iron: return _Player.Iron;

            // 25돼지왕
            case MiniGamePlayerState.King: return _Player.King;

            // 26마법요정
            case MiniGamePlayerState.Magic: return _Player.Magic;

            // 27P
            case MiniGamePlayerState.P: return _Player.P;

            // 28진주
            case MiniGamePlayerState.Pearl: return _Player.Pearl;

            // 29뚱딴지
            case MiniGamePlayerState.Potato: return _Player.Potato;

            // 30구르는애
            case MiniGamePlayerState.Rolling: return _Player.Rolling;

            // 31하수인
            case MiniGamePlayerState.Servant: return _Player.Servant;

            // 32오물돼지 진
            case MiniGamePlayerState.Sludge: return _Player.Sludge;

            // 33오물
            case MiniGamePlayerState.Soil: return _Player.Soil;

            // 34삼형제
            case MiniGamePlayerState.Three: return _Player.Three;

            // 35 묶인
            case MiniGamePlayerState.Tied: return _Player.Tied;

            // 36 돼지
            case MiniGamePlayerState.Pig: return true;
        }

        // 기본값
        return true;
    }
}
