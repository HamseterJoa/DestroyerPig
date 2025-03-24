using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour
{
    // 공
    [SerializeField] private GameObject _Ball;

    // 공 리스트
    private List<GameObject> _BallList = new List<GameObject>();

    // 버틴 시간
    private float _Time;

    // 버틴 시간을 알려줄 UI
    [SerializeField] private Text _Timer;

    // 결과창
    [SerializeField] private Image _ResultImage;

    // 기록이 적힐거
    [SerializeField] private Text _ResultTime;

    private void Start()
    {
        // 공발사
        StartCoroutine(AutoShootBall());

        // 타이머 온
        StartCoroutine(Timer());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            int index = FindDisableBall();

            // 찾지 못했다면 생성
            if (index == -1) CreateBall();

            // 찾았다면 그 번호를 활성화
            else EnableBall(index);
        }
    }

    // 비활성화 중인 공이 있다면 그공의 인덱스 번호를 리턴 없으면 -1
    private int FindDisableBall()
    {
        for (int i = 0; i < _BallList.Count; i++)
            if (!_BallList[i].gameObject.activeSelf) return i;

        return -1;
    }

    // 생성 x3.3 y5.5
    private void CreateBall()
    {
        // 생성
        GameObject ball = Instantiate(_Ball);

        // 랜덤 뽑기
        int randomAct = Random.Range(0, 4);

        // 좌표로 넣을거
        float x, y = 0.0f;

        if (randomAct == 0)
        {
            x = -3.3f;
            y = Random.Range(-5.5f, 5.5f);
        }

        else if (randomAct == 1)
        {
            x = 3.3f;
            y = Random.Range(-5.5f, 5.5f);
        }

        else if (randomAct == 2)
        {
            x = Random.Range(-3.3f, 3.3f);
            y = -5.5f;
        }

        else // 3
        {
            x = Random.Range(-3.3f, 3.3f);
            y = 5.5f;
        }

        // 좌표 설정
        ball.transform.position = new Vector2(x, y);

        // 리스트 추가
        _BallList.Add(ball);
    }

    // 활성화
    private void EnableBall(int index)
    {
        // 랜덤 뽑기
        int randomAct = Random.Range(0, 4);

        // 좌표로 넣을거
        float x, y = 0.0f;

        if (randomAct == 0)
        {
            x = -3.3f;
            y = Random.Range(-5.5f, 5.5f);
        }

        else if (randomAct == 1)
        {
            x = 3.3f;
            y = Random.Range(-5.5f, 5.5f);
        }

        else if (randomAct == 2)
        {
            x = Random.Range(-3.3f, 3.3f);
            y = -5.5f;
        }

        else // 3
        {
            x = Random.Range(-3.3f, 3.3f);
            y = 5.5f;
        }

        // 좌표 설정
        _BallList[index].transform.position = new Vector2(x, y);

        // 활성화
        _BallList[index].gameObject.SetActive(true);
    }

    // 공발사
    private IEnumerator AutoShootBall()
    {
        while (true)
        {
            // 랜덤으로 딜레이 설정
            float randomDelay = Random.Range(0.0f, 1.0f);

            // 비활성화 중인 공이있다면 인덱스 번호를 담음
            int index = FindDisableBall();

            // 찾지 못했다면 생성
            if (index == -1) CreateBall();

            // 찾았다면 그 번호를 활성화
            else EnableBall(index);

            // 딜레이
            yield return new WaitForSeconds(randomDelay);
        }
    }

    // 타이머
    private IEnumerator Timer()
    {
        while (true)
        {
            // 텍스트 업데이트
            _Timer.text = _Time.ToString();

            // 딜레이
            yield return new WaitForSeconds(0.1f);

            // 시간 증가
            _Time += 0.1f;

            // 반올림
            _Time = Mathf.Round(_Time * 10.0f) * 0.1f;
        }
    }

    // 공에 맞았거나 나가기 할때 호출
    public void End()
    {
        JsonData.PlayerData player = GameManager.getJsonDataManager.playerData;

        // 코루틴 종료
        StopAllCoroutines();

        // 결과창 텍스트 변경
        _ResultTime.text = (Mathf.Round(_Time * 10.0f) * 0.1f).ToString();

        // 결과창 띄우기
        _ResultImage.gameObject.SetActive(true);

        // 기록을 깻다면 기록 갱신
        if (_Time > player.MiniGameRecord) player.MiniGameRecord = Mathf.Round(_Time * 10.0f) * 0.1f;

        // 저장
        GameManager.getJsonDataManager.playerData = player;
        GameManager.getJsonDataManager.SavePlayerData();
    }

}
