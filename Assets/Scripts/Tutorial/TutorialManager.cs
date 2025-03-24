using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    // 튜토리얼 이미지들을 담을 리스트
    public List<Sprite> _TutorialImageList;

    // 나갈때 여기서 나가기 호출할꺼임
    public TutorialBtn tutorialBtn;

    // 이미지
    public Image tutotialImage;

    // 플레이어
    public GameObject player;

    // 리스트 번호
    private int _Count = 0;

    // 몹으로 나올 돼지
    public GameObject _Scarecrow;

    // 쓰벌 핵
    public GameObject hack;


    private void Update()
    {
        // 이미지가 비활중이면 리턴
        if (!tutotialImage.gameObject.activeSelf) return;

        // 키가 눌렸다 때지면
        if (Input.GetKeyUp(KeyCode.Mouse0)) 
        {
            // 이미지 리스트의 카운트 보다 작다면 이미지 다음으로
            if (_TutorialImageList.Count > _Count) TutorialProgressUpdate();
        }
    }

    // 튜토리얼 진행도 업데이트
    public void TutorialProgressUpdate()
    {
        // 카운트 더해주고
        _Count++;
     
        // 카운트가 넘어가면 비활 ㅅㄱ
        if (_TutorialImageList.Count <= _Count)
        {
            // 튜토리얼 이미지 비활성화
            tutotialImage.gameObject.SetActive(false);

            // 핵
            hack.gameObject.SetActive(true);

            // 플레이어 활성화
            player.gameObject.SetActive(true);

            // 허수아비
            _Scarecrow.gameObject.SetActive(true);

            return;
        }

        // 이미지를 카운트에 맞는 걸로 설정
        tutotialImage.sprite = _TutorialImageList[_Count];
    }
}
