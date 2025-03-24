using JsonData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnchantSceneButton : MonoBehaviour
{
    // 구매 불가 창
    [SerializeField] private Image _EnoughImage;

    // 플레이어 강화 패널
    [SerializeField] private GameObject _PlayerEnchantPannel;

    // 아이템 강화 패널
    [SerializeField] private GameObject _ItemEnchantPannel;

    // 캐쉬 패널
	[SerializeField] private GameObject _CashEnchantPannel;

    // 플레이어 강화 패널 활성화 버튼의 이미지
    [SerializeField] private Image _PlayerPanelBtnImage;

    // 아이템 강화 패널 활성화 버튼의 이미지
    [SerializeField] private Image _ItemPanelBtnImage;

    // 캐쉬 패널 활성화 이미지
    [SerializeField] private Image _CashPanelBtnImage;

    // 구매 창
	[SerializeField] private GameObject _PurchaseImage;

    // 메뉴 이미지
    [SerializeField] private Image _MenuImage;

    // 에러 이미지
    [SerializeField] private Image _ErrorImage;

    // 에러 텍스트
    [SerializeField] private Text _ErrorText;

    // 메인씬 패널
    [SerializeField] private EnchantCanvas _EnchantCanvas;

    // 옵션 메뉴
    [SerializeField] private Image _OptionMenu;

    private JsonData.PlayerData _PlayerData;
    
    // 터치 소리
    [SerializeField] private AudioSource _Audio;

    // 시작 버튼 오디오
    [SerializeField] private AudioSource _StartBtnAudio;

    // 프롤로그 패널
    [SerializeField] private GameObject _PrologPannel;

    // 나가기
    [SerializeField] private Image _Quit;

    // 인트로
    [SerializeField] private Image _IntroImage;

    //  > 버튼 클릭 사운드 재생
    private void PlayClickSound()
    {
        // 볼륨 설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _Audio.Play();
    }

    // 행동력이 부족함
    private void PlayEnghoutVigorVoice()
    {
        // 볼륨 설정
        _StartBtnAudio.volume = GameManager.getJsonDataManager.playerData.VoiceValue;

        // 재생
        _StartBtnAudio.Play();
    }

    private void Start() {
        
        // 초기값 설정
        if (_ItemEnchantPannel != null) {

			// 아이템 패널 활성화
            _PlayerEnchantPannel.gameObject.SetActive(true);

			// 다른 패널 비활성화
            _ItemEnchantPannel.gameObject.SetActive(false);
			_CashEnchantPannel.gameObject.SetActive(false);

            // 다른 버튼 색깔어둡게
            _ItemPanelBtnImage.color = new Color(0.8f, 0.8f, 0.8f);
            _CashPanelBtnImage.color = new Color(0.8f, 0.8f, 0.8f);
		}
    }

    private void Update()
    {
        // 나가기 창이 널이 아니고 나가기 창이 열려 있고 뒤로가기가 눌렸다면 끄기
        if (_Quit != null && _Quit.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            _Quit.gameObject.SetActive(false);
    }

    // 구매불가 텍스트 끄기
    public void OnClickOKBtn() {
        
        // 소리 재생
        PlayClickSound();

        _EnoughImage.gameObject.SetActive(false);
    }
	
	// 아이템 패널
    public void OnClickItemEnchantPannelEnableBtn() {

        // 소리 재생
        PlayClickSound();

        // 패널 활성화
        _ItemEnchantPannel.gameObject.SetActive(true);

        // 색깔 밝게
        _ItemPanelBtnImage.color = new Color(1.0f, 1.0f, 1.0f);

        // 다른 패널 비활성화
        _PlayerEnchantPannel.gameObject.SetActive(false);
		_CashEnchantPannel.gameObject.SetActive(false);

        // 다른 버튼 색깔어둡게
        _PlayerPanelBtnImage.color = new Color(0.8f, 0.8f, 0.8f);
        _CashPanelBtnImage.color = new Color(0.8f, 0.8f, 0.8f);
    }

	// 플레이어 패널
    public void OnClickPlayerEnchantPannelEnableBtn() {

        // 소리 재생
        PlayClickSound();

        // 패널 활성화
        _PlayerEnchantPannel.gameObject.SetActive(true);

        // 색깔 밝게
        _PlayerPanelBtnImage.color = new Color(1.0f, 1.0f, 1.0f);

		// 다른 패널 비활성화
        _ItemEnchantPannel.gameObject.SetActive(false);
		_CashEnchantPannel.gameObject.SetActive(false);

        // 다른 버튼 색깔어둡게
        _ItemPanelBtnImage.color = new Color(0.8f, 0.8f, 0.8f);
        _CashPanelBtnImage.color = new Color(0.8f, 0.8f, 0.8f);
    }

	// 캐쉬 패널
	public void OnClickCashEnchantPannelEnableBtn() {

        // 소리 재생
        PlayClickSound();

        // 패널 활성화
        _CashEnchantPannel.gameObject.SetActive(true);

        // 색깔 밝게
        _CashPanelBtnImage.color = new Color(1.0f, 1.0f, 1.0f);

        // 다른 패널 비활성화
        _ItemEnchantPannel.gameObject.SetActive(false);
		_PlayerEnchantPannel.gameObject.SetActive(false);

        // 다른 버튼 색깔어둡게
        _ItemPanelBtnImage.color = new Color(0.8f, 0.8f, 0.8f);
        _PlayerPanelBtnImage.color = new Color(0.8f, 0.8f, 0.8f);
    }

	// 구매 취소 버튼
	public void OnClickCancel() {

        // 소리 재생
        PlayClickSound();

        _PurchaseImage.gameObject.SetActive(false);
        _EnoughImage.gameObject.SetActive(false);
	}

	// 메뉴켜기
	public void OnClickOption() {

        // 소리 재생
        PlayClickSound();

        _OptionMenu.gameObject.SetActive(true);
    }

    //  > 시작 버튼을 클릭했을 때 호출하도록 할 메서드입니다.
    public void OnGameStartBtnClick()
    {
        // 데이터 갱신
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 소리 재생
        PlayClickSound();

        // 시작
        LoadSceneManager.LoadScene(SceneName.GameScene);

        // 행동력이 있을 때
        //if (_PlayerData.Vigor > 0)
        //{
        //    // 데이터 새로 저장
        //    _PlayerData = GameManager.getJsonDataManager.playerData;
        //
        //    // 행동력 업데이트
        //    Vigor vigor = GameObject.Find("EnchantCanvas").transform.Find("ScreenPannel").transform.Find("Vigor").GetComponent<Vigor>();
        //
        //    // 행동력이 최대였으면 시작 시간을 설정해줌
        //    if (_PlayerData.Vigor == vigor._MaxVigor)
        //    {
        //        // 년 설정
        //        _PlayerData.StartTimeYear = System.DateTime.Now.Year.ToString();
        //
        //        // 월 설정
        //        _PlayerData.StartTimeMonth = System.DateTime.Now.Month < 10 ? "0" + System.DateTime.Now.Month.ToString() : System.DateTime.Now.Month.ToString();
        //
        //        // 일 설정
        //        _PlayerData.StartTimeDay = System.DateTime.Now.Day < 10 ? "0" + System.DateTime.Now.Day.ToString() : System.DateTime.Now.Day.ToString();
        //
        //        // 시 설정
        //        _PlayerData.StartTimeHour = System.DateTime.Now.Hour < 10 ? "0" + System.DateTime.Now.Hour.ToString() : System.DateTime.Now.Hour.ToString();
        //
        //        // 분 설정
        //        _PlayerData.StartTimeMinute = System.DateTime.Now.Minute < 10 ? "0" + System.DateTime.Now.Minute.ToString() : System.DateTime.Now.Minute.ToString();
        //
        //        // 초 설정
        //        _PlayerData.StartTimeSecond = System.DateTime.Now.Second < 10 ? "0" + System.DateTime.Now.Second.ToString() : System.DateTime.Now.Second.ToString();
        //    }
        //
        //    //행동력 소모
        //    _PlayerData.Vigor--;
        //    
        //    // 데이터 저장
        //    PlayerDataUpdate();
        //
        //    // 씬 변경
        //    LoadSceneManager.LoadScene(SceneName.GameScene);
        //}
        //
        ////없을 때
        //else
        //{
        //    _ErrorImage.gameObject.SetActive(true);
        //
        //    _ErrorText.text = "행동력이 부족합니다.";
        //
        //    // 행동력 부족
        //    PlayEnghoutVigorVoice();
        //}

    }

    //  > 메뉴 버튼을 눌렀을 때
    public void OnMenuBtnClick()
    {
        // 소리 재생
        PlayClickSound();

        _MenuImage.gameObject.SetActive(true);
    }

    // 메뉴 나가기 버튼
    public void BackToMainMenu()
    {
        // 소리 재생
        PlayClickSound();

        _MenuImage.gameObject.SetActive(false);
    }

    // 에러창 나가기
    public void ErrorOkBtn()
    {
        // 소리 재생
        PlayClickSound();

        _ErrorImage.gameObject.SetActive(false);
    }

    // 
    private void PlayerDataUpdate()
    {
        // 데이터 설정
        GameManager.getJsonDataManager.playerData = _PlayerData;
    }

    // 프롤로그 스킵버튼
    public void SkipBtn()
    {
        _PrologPannel.gameObject.SetActive(false);
    }

    // 게임 종료
    public void QuitGame()
    {
        Application.Quit();
    }

    // 안나감
    public void CancelQuit()
    {
        // 소리 재생
        PlayClickSound();

        _Quit.gameObject.SetActive(false);
    }

    // 미니게임 씬으로 이동
    public void GoMiniGame()
    {
        // 소리 재생
        PlayClickSound();

        // 씬 변경
        LoadSceneManager.LoadScene(SceneName.MiniGameGotchaScene);
    }

    // 인트로 켜기
    public void OnClickEnableIntro()
    {
        // 소리 재생
        PlayClickSound();

        _IntroImage.gameObject.SetActive(true);
    }

    // 하드 모드 켜기
    public void HardMode(Toggle toggle)
    {
        // 소리 재생
        PlayClickSound();

        PlayerData player = GameManager.getJsonDataManager.playerData;

        // 끄고 켜기
        player.Hardmode = toggle.isOn;

        // 데이터 저장
        GameManager.getJsonDataManager.playerData = player;
    }

}
