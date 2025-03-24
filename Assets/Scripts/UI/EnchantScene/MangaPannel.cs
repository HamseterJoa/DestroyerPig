using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MangaPannel : MonoBehaviour
{
    // 이미지와 텍스트
    [SerializeField] private Image _PrologImage;
    [SerializeField] private Text _PrologText;

    // 이미지를 순서대로 담을 리스트
    [SerializeField] private List<Sprite> _PrologImageList;

    // 텍스트를 순서대로 담음
    [SerializeField] private List<string> _PrologTextList;

    // 몇번눌렀는지 확인
    private int _Count = 0;

    // 타이핑 효과가 진행중인지 검사함
    private bool _IsTyping = false;

    // 한글자씩 나오는 효과
    private IEnumerator TypingEffect()
    {
        // 타이핑중으로
        _IsTyping = true;

        // i는 텍스트의 길이만큼 증가
        for (int i = 0; i <= _PrologTextList[_Count].Length; i++)
        {
            // 표시되는 텍스트를 한글자씩 나오게함
            _PrologText.text = _PrologTextList[_Count].Substring(0, i);
            
            // 0.05초 마다 한글자씩 출력되도록 딜레이
            yield return new WaitForSeconds(0.05f);

            // 멈추질 않아 쓰바
            //if (Input.GetKeyDown(KeyCode.Mouse0))
            //{
            //    Debug.Log("쒸바 끝나라고");
            //    // 타이핑 효과 종료
            //    _IsTyping = false;

            //    // 표시되는 텍스트를 한글자씩 나오게함
            //    _PrologText.text = _PrologTextList[_Count].Substring(0, _PrologTextList[_Count].Length);

            //    break;
            //}
        }

        // 타이핑 효과 종료
        _IsTyping = false;

        yield return null;
    }

    private void Awake()
    {
        // 프롤로그를 본적이있다면 비활성화
        if (GameManager.getJsonDataManager.playerData.Prolog) gameObject.SetActive(false);

        // 이미지를 첫장면으로
        _PrologImage.sprite = _PrologImageList[0];
        
        // 텍스트를 첫 대사로
        _PrologText.text = _PrologTextList[0];

        _PrologTextList[4] = "덩치큰 돼지가 손을 들고 말했습니다.\n관중 돼지 : 소시지를 만들기 위해서인가요?";
    }

    private void OnEnable()
    {
        // 이미지를 첫장면으로
        _PrologImage.sprite = _PrologImageList[0];

        // 텍스트를 첫 대사로
        _PrologText.text = _PrologTextList[0];

        // 카운트 초기화
        _Count = 0;

        // 타이핑 효과 시작
        StartCoroutine(TypingEffect());
    }

    //private void Start()
    //{
    //    // 타이핑 효과 시작
    //    StartCoroutine(TypingEffect());
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            // 튜토리얼 씬으로
            if (_Count >= _PrologImageList.Count - 1) LoadSceneManager.LoadScene(SceneName.Tutorial);//gameObject.SetActive(false);

            // 다음
            else
            {
                // 타이핑 효과중이 아닐때만 실행
                if (!_IsTyping) ChangeNextPage();

                // 타이핑 효과중이라면 종료후 문자 완성
                else
                {
                    StopCoroutine(TypingEffect());
                    //_PrologText.text = _PrologTextList[_Count];
                }
            }
        }
    }

    // 만화 페이지 넘겨주기
    private void ChangeNextPage()
    {
        // 다음순서
        _Count++;

        // 이미지를 다음장면
        _PrologImage.sprite = _PrologImageList[_Count];

        // 글자가 필요없는 장면이라면실행안함
        if (_Count < 9)

            // 타이핑 효과
            StartCoroutine(TypingEffect());

        // 실행안하고 글자 지움
        else _PrologText.text = "";
    }

    private void OnDisable()
    {
        JsonData.PlayerData player = GameManager.getJsonDataManager.playerData;

        if (!player.Prolog)
        {
            player.Prolog = true;

            GameManager.getJsonDataManager.playerData = player;
        }
    }

}
