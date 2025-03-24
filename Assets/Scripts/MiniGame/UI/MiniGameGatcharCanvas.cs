using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameGatcharCanvas : MonoBehaviour
{
    // 화면에 표시된는 UI
    [SerializeField] private Text _Record;
    [SerializeField] private Text _PlayerCoins;

    // 나가기
    [SerializeField] private Image _Quit;

    private JsonData.PlayerData _PlayerData;
    
    private void Start()
    {
        // 기록
        RecordUpdate();

        // 돈
        CoinsUpdate();

        // 데이터 설정
        _PlayerData = GameManager.getJsonDataManager.playerData;
    }

    private void Update()
    {
        // 종료
        if (Input.GetKeyDown(KeyCode.Escape)) _Quit.gameObject.SetActive(true);
    }

    // 기록 설정
    private void RecordUpdate()
    {
        // 데이터 새로받고
        _PlayerData = GameManager.getJsonDataManager.playerData;

        _Record.text = _PlayerData.MiniGameRecord.ToString();
    }

    // 돈 업데이트
    public void CoinsUpdate()
    {
        _PlayerCoins.text = GameManager.getJsonDataManager.playerData.Coin.ToString();
    }
}
