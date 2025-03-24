using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingImage : MonoBehaviour
{
    // 이미지와 텍스트
    [SerializeField] private Image _EndingImage;
    [SerializeField] private Text _EndingText;

    // 이미지를 순서대로 담을 리스트
    [SerializeField] private List<Sprite> _EndingImageList;

    // 텍스트를 순서대로 담음
    [SerializeField] private List<string> _EndingTextList;

    // 몇번눌렀는지 확인
    private int _Count;

    // 글자 효과가 나오는중 인지
    private bool _IsTyping = false;

    // 엔딩 크레딧
    [SerializeField] private GameObject _EndingCreidt;

    // 크레딧이 나왔는지 검사
    private bool _CreditIsStart = false;

    // 글자 효과
    private IEnumerator TypingEffect()
    {
        // 효과중
        _IsTyping = true;

        // 글자가 끝날때 까지 반복
        for (int i = 0; i <= _EndingTextList[_Count].Length; i++)
        {
            // 한글자씩 나옴
            _EndingText.text = _EndingTextList[_Count].Substring(0, i);

            // 0.05초 마다 한글자씩 출력되도록 딜레이
            yield return new WaitForSeconds(0.05f);
        }

        _IsTyping = false;

        yield return null;
    }

    private void Awake()
    {
        // 이미지를 첫장면으로
        _EndingImage.sprite = _EndingImageList[0];
    }

    private void Update()
    {
        // 이미지 위치가 바뀌었다면
        if (_EndingImage.rectTransform.anchoredPosition == Vector2.zero && !_CreditIsStart)
        {
            // 효과 시작
            StartCoroutine(TypingEffect());

            // 크레딧이 나왔다
            _CreditIsStart = true;
        }

        
        if (Input.GetKeyDown(KeyCode.Mouse0) && _EndingImage.rectTransform.anchoredPosition == Vector2.zero)
        {
            // 만약 끝이라면 비활성화
            if (_Count >= _EndingImageList.Count - 1) gameObject.SetActive(false);

            // 다음
            else
            {
                if (!_IsTyping) ChangeNextPage();

                // 타이핑 효과중이라면 종료후 문자 완성
                else
                {
                    StopCoroutine(TypingEffect());
                    _EndingText.text = _EndingTextList[_Count];
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
        _EndingImage.sprite = _EndingImageList[_Count];

        // 텍스트를 다음 대사로
        StartCoroutine(TypingEffect());
    }

    private void OnDisable()
    {
        // 크레딧 생성
        //_EndingCreidt.gameObject.SetActive(true);
    }
}
