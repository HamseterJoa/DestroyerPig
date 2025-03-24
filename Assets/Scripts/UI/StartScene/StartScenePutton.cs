using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScenePutton : MonoBehaviour
{
    [SerializeField] private Animator _Anim;

    // 배경
    [SerializeField] private Image _Background;

    // 이미지
    [SerializeField] private Image _Image;

    // 제목
    [SerializeField] private Image _Title;

    // 스타트
    [SerializeField] private Image _Start;

    // 소리
    [SerializeField] private AudioSource _Audio;

    // 속도
    private float _Speed = 1.0f;

	// 클릭이 되었는지 검사
	private bool _OnClick = false;

    private void Awake()
    {
        // 처음 이라면
        if (!GameManager.getJsonDataManager.playerData.Prolog)
        {
            JsonData.PlayerData player = GameManager.getJsonDataManager.playerData;

            // 기본값 설정
            player.Vigor = 5;
            player.BGMValue = 0.5f;
            player.EffectValue = 0.5f;
            player.VoiceValue = 0.5f;

            GameManager.getJsonDataManager.playerData = player;
        }
        
    }

    private void Update()
    {
        // 마우스가 눌렸다면
        if (Input.GetMouseButtonDown(0))
        {
            // 음량 설정
            _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

            // 소리 재생
            _Audio.Play();

            _OnClick = true;
        }

		// 시작
		if (_OnClick) StartGame();

	}

    // 화면을 검게해주고 완전히 검어지면 시작
    private void StartGame(){

        _Anim.SetBool("Down", true);

        // 배경의 r
        _Background.color = new Color(Mathf.MoveTowards(_Background.color.r, 0.0f, _Speed * Time.deltaTime),
            // g
            Mathf.MoveTowards(_Background.color.g, 0.0f, _Speed * Time.deltaTime),
            // b
            Mathf.MoveTowards(_Background.color.b, 0.0f, _Speed * Time.deltaTime));

        // 이미지의 r
        _Image.color = new Color(Mathf.MoveTowards(_Image.color.r, 0.0f, _Speed * Time.deltaTime),
			// g
			Mathf.MoveTowards(_Image.color.g, 0.0f, _Speed * Time.deltaTime),
			// b
			Mathf.MoveTowards(_Image.color.b, 0.0f, _Speed * Time.deltaTime));

		// 제목의 r
		_Title.color = new Color(Mathf.MoveTowards(_Title.color.r, 0.0f, _Speed * Time.deltaTime),
			// g
			Mathf.MoveTowards(_Title.color.g, 0.0f, _Speed * Time.deltaTime),
			// b
			Mathf.MoveTowards(_Title.color.b, 0.0f, _Speed * Time.deltaTime));

        // 스타트의 r
        _Start.color = new Color(Mathf.MoveTowards(_Start.color.r, 0.0f, _Speed * Time.deltaTime),
            // g
            Mathf.MoveTowards(_Start.color.g, 0.0f, _Speed * Time.deltaTime),
            // b
            Mathf.MoveTowards(_Start.color.b, 0.0f, _Speed * Time.deltaTime));

        if (Mathf.Approximately(_Image.color.r, 0.0f)) LoadSceneManager.LoadScene(SceneName.EnchantScene);

	}

}
