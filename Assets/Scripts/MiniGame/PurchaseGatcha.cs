using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurchaseGatcha : MonoBehaviour
{
    // 결과창
    [SerializeField] private Image _GatcharResult;

    // 결과물 이미지
    [SerializeField] private Image _ResultImage;

    // 결과물 이름
    [SerializeField] private Text _ResultName;

    // 결과물 등급
    [SerializeField] private Text _ResultRate;

    // 뽑기할 돼지들을 담음(이미지)
    [SerializeField] private List<Sprite> _SpriteList;

    // 뽑은 수를 담음
    private int _Number;

    // 등급에 따라 뽑을 확률의 수
    private int _MinPer;// 최소
    private int _MaxPer;// 최대

    // 오디오
    private AudioSource _Audio;

    // 플레이어 데이터
    private JsonData.PlayerData _PlayerData;

    // 캔버스
    [SerializeField] private MiniGameGatcharCanvas _GotchaCanvas;

    // 스크롤 뷰
    [SerializeField] private GameObject _Scrollview;

    // 구매 오디오
    [SerializeField] private AudioSource _PurchaseAudio;

    // 10연차 결과창
    [SerializeField] private Image _MultiResult;

    // 10연차 내용물들
    [SerializeField] private List<Image> _MultiImageList;

    // 10연차 별들
    [SerializeField] private List<Text> _MultiStarList;

    private void Start()
    {
        // 오디오 참조
        _Audio = GetComponent<AudioSource>();

        // 플레이어 데이터 참조
        _PlayerData = GameManager.getJsonDataManager.playerData;
    }

    // 돈부족 소리
    private void PlayEnoughCoin()
    {
        _Audio.volume = GameManager.getJsonDataManager.playerData.VoiceValue;

        _Audio.Play();
    }

    // 구매 버튼 클릭 소리
    private void PlayPurchaseSound()
    {
        _PurchaseAudio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        _PurchaseAudio.Play();
    }

    // 구매
    public void Purchase()
    {
        // 돈이 부족하다면
        if (_PlayerData.Coin < 300)
        {
            PlayEnoughCoin();
        }

        else
        {
            // 클릭소리
            PlayPurchaseSound();

            // 데이터 새로 받기
            _PlayerData = GameManager.getJsonDataManager.playerData;
            
            // 돈 감소
            _PlayerData.Coin -= 300;

            // 등급 뽑기
            int randomrate = Random.Range(0, 100);
            Debug.Log("뽑은 수: " + randomrate);
            // 전설 03% 97~99
            if (randomrate > 96)
            {
                _MinPer = 23;
                _MaxPer = 35;
            }

            // 희귀 15% 82~96
            else if (randomrate < 97 && randomrate > 81)
            {
                _MinPer = 16;
                _MaxPer = 23;
            }

            // 고급 32% 50~81
            else if (randomrate < 82 && randomrate > 50)
            {
                _MinPer = 9;
                _MaxPer = 16;

            }

            // 일반 50%
            else
            {
                _MinPer = 0;
                _MaxPer = 9;
            }

            // 뽑을 수 선택(등급에 따라)
            _Number = Random.Range(_MinPer, _MaxPer);

            // 이미지 설정
            _ResultImage.sprite = ReturnSprite(_Number);

            // 이름 설정
            _ResultName.text = ReturnName(_Number);

            // 등급 설정
            _ResultRate.text = ReturnRate(_Number);

            // 뽑은 돼지 언락
            UnlockPig(_Number);

            // 결과창 활성화
            _GatcharResult.gameObject.SetActive(true);

            // 데이터 설정
            GameManager.getJsonDataManager.playerData = _PlayerData;

            // 파일 저장 
            GameManager.getJsonDataManager.SaveItemEnchantData();

            // 코인 상태 업데이트
            _GotchaCanvas.CoinsUpdate();
            
            // 스크롤 뷰 온오프
            _Scrollview.gameObject.SetActive(false);
            _Scrollview.gameObject.SetActive(true);
        }
        
    }

    // 10연차
    public void MultiPurchase()
    {
        // 돈이 부족하다면
        if (_PlayerData.Coin < 3000)
        {
            PlayEnoughCoin();
        }

        else
        {
            // 클릭소리
            PlayPurchaseSound();

            // 데이터 새로 받기
            _PlayerData = GameManager.getJsonDataManager.playerData;

            // 돈 감소
            _PlayerData.Coin -= 3000;

            // 10회 반복
            for (int i = 0; i < 10; i++)
            {
                // 등급 뽑기
                int randomrate = Random.Range(0, 100);

                // 전설 03% 97~99
                if (randomrate > 96)
                {
                    _MinPer = 23;
                    _MaxPer = 35;
                }

                // 희귀 15% 82~96
                else if (randomrate < 97 && randomrate > 81)
                {
                    _MinPer = 16;
                    _MaxPer = 23;
                }

                // 고급 32% 50~81
                else if (randomrate < 82 && randomrate > 50)
                {
                    _MinPer = 9;
                    _MaxPer = 16;

                }

                // 일반 50%
                else
                {
                    _MinPer = 0;
                    _MaxPer = 9;
                }

                // 뽑을 수 선택(등급에 따라)
                _Number = Random.Range(_MinPer, _MaxPer);

                // i번째 이미지 설정
                _MultiImageList[i].sprite = ReturnSprite(_Number);

                // i번째 별설정
                _MultiStarList[i].text = ReturnRate(_Number);

                // 뽑은 돼지 언락
                UnlockPig(_Number);
            }

            // 10연차 결과창 활성화
            _MultiResult.gameObject.SetActive(true);

            // 데이터 설정
            GameManager.getJsonDataManager.playerData = _PlayerData;

            // 파일 저장 
            GameManager.getJsonDataManager.SaveItemEnchantData();

            // 코인 상태 업데이트
            _GotchaCanvas.CoinsUpdate();

            // 스크롤 뷰 온오프
            _Scrollview.gameObject.SetActive(false);
            _Scrollview.gameObject.SetActive(true);
        }
    }

    // 뽑은 수에 따라 이름을 리턴함
    private string ReturnName(int number)
    {
        switch (number)
        {
            case 0: return "뚱딴지";
            case 1: return "흑돼지";
            case 2: return "날개돼지";
            case 3: return "오물돼지";
            case 4: return "묶인돼지";
            case 5: return "떼굴떼굴떼구르르";
            case 6: return "겁쟁이돼지";
            case 7: return "아기돼지 삼형제";
            case 8: return "돼지목에 진주 목걸이";
            case 9: return "마법요정 돼지";
            case 10: return "오물돼지(진)";
            case 11: return "쾅쾅쿠광쾅쾅쾅";
            case 12: return "미스터 데인져러스";
            case 13: return "파괴자 돼지";
            case 14: return "철갑돼지";
            case 15: return "우두머리 돼지";
            case 16: return "100인분 돼지";
            case 17: return "아롱이";
            case 18: return "아기돼지 집";
            case 19: return "괴물까마귀";
            case 20: return "돼지왕";
            case 21: return "복주머니 황금돼지";
            case 22: return "대단한 돼지 에스더";
            case 23: return "돼지 인형옷";
            case 24: return "미스 덕스엑";
            case 25: return "복실이";
            case 26: return "삼쨩";
            case 27: return "마롱";
            case 28: return "이치쨩";
            case 29: return "돼지왕의 하수인";
            case 30: return "황금돼지의 황금동전";
            case 31: return "Mr.P";
            case 32: return "악어 주인";
            case 33: return "악어 새";
            case 34: return "악어";
        }

        return "돼지";
    }

    // 9 마리 1성  0 ~  8
    // 7 마리 2성 09 ~ 15
    // 7 마리 3성 16 ~ 22
    // 12마리 4성 23 ~ 34
    // 뽑은 수에 따라 등급을 리턴함
    private string ReturnRate(int number)
    {
        if (number > 22) return "★★★★";
        else if (number > 15) return "★★★";
        else if (number > 8) return "★★";

        return "★";
    }

    // 뽑은 수에 따라 이미지를 리턴함
    private Sprite ReturnSprite(int number)
    {
        return _SpriteList[number];
    }

    // 뽑은 수에 돼지가 미확인 돼지라면 언락
    private void UnlockPig(int number)
    {
        // 해금 or 리턴
        switch (number)
        {
            case 0:
                // 해금된 상태라면 리턴
                if (_PlayerData.Potato) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Potato = true; break;

            case 1: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Black) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Black = true; break;

            case 2: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Angel) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Angel = true; break;

            case 3: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Soil) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Soil = true; break;

            case 4:
                // 해금된 상태라면 리턴
                if (_PlayerData.Tied) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Tied = true; break;

            case 5: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Rolling) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Rolling = true; break;

            case 6:
                // 해금된 상태라면 리턴
                if (_PlayerData.Coward) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Coward = true; break;

            case 7:
                // 해금된 상태라면 리턴
                if (_PlayerData.Three) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Three = true; break;

            case 8:
                // 해금된 상태라면 리턴
                if (_PlayerData.Pearl) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Pearl = true; break;

            case 9: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Magic) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Magic = true; break;

            case 10: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Sludge) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Sludge = true; break;

            case 11:
                // 해금된 상태라면 리턴
                if (_PlayerData.Bouncing) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Bouncing = true; break;

            case 12:
                // 해금된 상태라면 리턴
                if (_PlayerData.Hamster) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Hamster = true; break;

            case 13:
                // 해금된 상태라면 리턴
                if (_PlayerData.Destroyer) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Destroyer = true; break;

            case 14: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Iron) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Iron = true; break;

            case 15: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Guv) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Guv = true; break;

            case 16: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Hundred) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Hundred = true; break;

            case 17:
                // 해금된 상태라면 리턴
                if (_PlayerData.Arong) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Arong = true; break;

            case 18:
                // 해금된 상태라면 리턴
                if (_PlayerData.House) return;

                // 해금되지 않았다면 해금
                else _PlayerData.House = true; break;

            case 19: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Crow) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Crow = true; break;

            case 20: 
                // 해금된 상태라면 리턴
                if (_PlayerData.King) return;

                // 해금되지 않았다면 해금
                else _PlayerData.King = true; break;

            case 21:
                // 해금된 상태라면 리턴
                if (_PlayerData.Golden) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Golden = true; break;

            case 22:
                // 해금된 상태라면 리턴
                if (_PlayerData.Esther) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Esther = true; break;

            case 23:
                // 해금된 상태라면 리턴
                if (_PlayerData.Doll) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Doll = true; break;

            case 24:
                // 해금된 상태라면 리턴
                if (_PlayerData.Duck) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Duck = true; break;

            case 25:
                // 해금된 상태라면 리턴
                if (_PlayerData.Baby1) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Baby1 = true; break;

            case 26:
                // 해금된 상태라면 리턴
                if (_PlayerData.Baby2) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Baby2 = true; break;

            case 27:
                // 해금된 상태라면 리턴
                if (_PlayerData.Baby3) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Baby3 = true; break;

            case 28:
                // 해금된 상태라면 리턴
                if (_PlayerData.Baby4) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Baby4 = true; break;

            case 29: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Servant) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Servant = true; break;

            case 30:
                // 해금된 상태라면 리턴
                if (_PlayerData.GoldenCoin) return;

                // 해금되지 않았다면 해금
                else _PlayerData.GoldenCoin = true; break;

            case 31:
                // 해금된 상태라면 리턴
                if (_PlayerData.P) return;

                // 해금되지 않았다면 해금
                else _PlayerData.P = true; break;

            case 32: 
                // 해금된 상태라면 리턴
                if (_PlayerData.CrocoMaster) return;

                // 해금되지 않았다면 해금
                else _PlayerData.CrocoMaster = true; break;

            case 33: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Bird) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Bird = true; break;

            case 34: 
                // 해금된 상태라면 리턴
                if (_PlayerData.Croco) return;

                // 해금되지 않았다면 해금
                else _PlayerData.Croco = true; break;

        }
    }
}
