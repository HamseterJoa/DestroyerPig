using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class EnchantCanvas : MonoBehaviour
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
        if (GameManager.getJsonDataManager.playerData.Record < 1) _Record.text = "뉴비";
        else if (GameManager.getJsonDataManager.playerData.Record > 72) _Record.text = "고인물";
        else _Record.text = GameManager.getJsonDataManager.playerData.Record.ToString();
    }

    // 돈 업데이트
    public void CoinsUpdate()
    {
        if (GameManager.getJsonDataManager.playerData.Coin < 999) _PlayerCoins.text = "그지";
        else _PlayerCoins.text = GameManager.getJsonDataManager.playerData.Coin.ToString();

        // 코인 설정
        _PlayerData.Coin = uint.Parse(GameManager.getJsonDataManager.playerData.Coin.ToString());
    }
    
}
