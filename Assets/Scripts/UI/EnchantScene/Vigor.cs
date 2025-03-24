using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Vigor : MonoBehaviour
{
    // 화면에 표시된는 UI
    [SerializeField] private Text _VigorText;
    [SerializeField] private Text _VigorTimer;

    private JsonData.PlayerData _PlayerData;

    // 시작 시간
    private DateTime _StartTime;

    // 시간차
    private TimeSpan _TimeDifference;

    // 최대 행동력
    public uint _MaxVigor;

    // 행동력 충전 딜레이 시간
    private readonly double _AddVigorDelay = 10.0; // 분

    private void Start()
    {
        // 데이터 설정
        //_PlayerData = GameManager.getJsonDataManager.playerData;
        //
        //// 최대 행동력 업데이트
        //InitializeMaxVigor();
        //
        //// 시작 시간 설정
        //InitializeStartTime();
        //
        //// 행동력 검사
        //VigorCheck();
        //
        //// 행동력이 최대치가 아닐때 실행
        //if (_PlayerData.Vigor < _MaxVigor)
        //
        //    // 타이머 작동
        //    StartCoroutine(VigorTimer());
        //
        //// 행동력 업데이트
        //VigorUpdate();
    
    }

    // 행동력 업데이트
    public void VigorUpdate()
    {
        // 최대 행동력 설정
        InitializeMaxVigor();

        StringBuilder vigorState = new StringBuilder();

        vigorState.Append(" ");
        vigorState.Append(_PlayerData.Vigor.ToString());
        vigorState.Append(" / ");
        vigorState.Append(_MaxVigor.ToString());

        // 현재 행동력 설정
        _VigorText.text = vigorState.ToString();
    }


    // 최대 행동력을 설정해줌
    private void InitializeMaxVigor()
    {
        _MaxVigor = 5 + (_PlayerData.Record / 10) + GameManager.getJsonDataManager.itemEnchant.AddVigorLV;
    }

    // 행동력 검사
    private void VigorCheck()
    {
        // 시간차를 설정
        _TimeDifference = DateTime.Now - _StartTime;

        // 검사했는데 시간차가 딜레이보다 크다면
        if (_TimeDifference.TotalMinutes >= _AddVigorDelay)
        {
            // 행동력이 최대가 아니라면
            if (_MaxVigor > _PlayerData.Vigor)
            {
                Debug.Log("전");
                Debug.Log(_StartTime);
                // 행동력 추가
                _PlayerData.Vigor++;
                VigorUpdate();
                // 10분 추가
                _StartTime.AddMinutes(10.0d);

                Debug.Log("후");
                Debug.Log(_StartTime);
                // 시간 데이터들 설정
                TimeDatasUpdate(_StartTime);
            }

            // 최대라면 타이머 오프
            else _VigorTimer.gameObject.SetActive(false);

            // 데이터 저장
            GameManager.getJsonDataManager.playerData = _PlayerData;

            // 저장
            GameManager.getJsonDataManager.SavePlayerData();
        }
    }

    // 타이머 시간 설정
    private void VigorTimerUpdate()
    {
        // 시간차 설정
        _TimeDifference = DateTime.Now - _StartTime;

        // 타이머 시간 설정
        _VigorTimer.text = (_AddVigorDelay - _TimeDifference.TotalMinutes).ToString();

        // 타이머가 0보다 작다면 행동력 추가
        if (_AddVigorDelay - _TimeDifference.TotalMinutes < 0.0d) VigorCheck();
    }

    // 행동력 딜레이 시간을 세어줄 타이머를 작동 시켜줄 코루틴
    private IEnumerator VigorTimer()
    {
        // 행동력이 최대가 아니라면 반복
        while (_MaxVigor != _PlayerData.Vigor)
        {
            // 1초의 딜레이
            yield return new WaitForSecondsRealtime(1.0f);

            // 타이머 시간 설정
            VigorTimerUpdate();
        }

        yield return null;
    }

    // 시작시간을 설정해줌
    private void InitializeStartTime()
    {
        // 데이터 최신화
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 스트링 빌더 초기화 하고 담기
        StringBuilder startTime = new StringBuilder();
        startTime.Append(_PlayerData.StartTimeYear);
        startTime.Append("/");
        startTime.Append(_PlayerData.StartTimeMonth);
        startTime.Append("/");
        startTime.Append(_PlayerData.StartTimeDay);
        startTime.Append(" ");
        startTime.Append(_PlayerData.StartTimeHour);
        startTime.Append(":");
        startTime.Append(_PlayerData.StartTimeMinute);
        startTime.Append(":");
        startTime.Append(_PlayerData.StartTimeSecond);

        // 시작 시간 설정
        _StartTime = Convert.ToDateTime(startTime.ToString());
    }

    // 데이터에 시간들을 입력한 시간으로 설정
    private void TimeDatasUpdate(DateTime startTime)
    {
        // 년 설정
        _PlayerData.StartTimeYear = startTime.Year.ToString();

        // 월 설정
        _PlayerData.StartTimeMonth = startTime.Month < 10 ? "0" + startTime.Month.ToString() : startTime.Month.ToString();

        // 일 설정
        _PlayerData.StartTimeDay = startTime.Day < 10 ? "0" + startTime.Day.ToString() : startTime.Day.ToString();

        // 시 설정
        _PlayerData.StartTimeHour = startTime.Hour < 10 ? "0" + startTime.Hour.ToString() : startTime.Hour.ToString();

        // 분 설정
        _PlayerData.StartTimeMinute = startTime.Minute < 10 ? "0" + startTime.Minute.ToString() : startTime.Minute.ToString();

        // 초 설정
        _PlayerData.StartTimeSecond = startTime.Second < 10 ? "0" + startTime.Second.ToString() : startTime.Second.ToString();

        // 데이터 저장
        _PlayerData = GameManager.getJsonDataManager.playerData;

        // 저장
        GameManager.getJsonDataManager.SavePlayerData();
    }
}
