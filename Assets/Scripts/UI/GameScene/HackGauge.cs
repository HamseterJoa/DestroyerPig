using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackGauge : MonoBehaviour
{
    [SerializeField] private Image _HackGauge;
    [SerializeField] private Text _HackCount;
    private float _HackMaxCount;
    public float hack = 0;

    private void Awake()
    {
        JsonData.ItemEnchant item = GameManager.getJsonDataManager.itemEnchant;

        // 핵 최대 충전량
        switch (item.HackMaxCountLV)
        {
            case 0: _HackMaxCount = 1.0f; break;
            case 1: _HackMaxCount = 2.0f; break;
            case 2: _HackMaxCount = 3.0f; break;
            case 3: _HackMaxCount = 4.0f; break;
        }

        // 초기설정
        AddHackGauge(0.0f);
    }
    
    public void AddHackGauge(float addHack)
    {
        // 꽉찼다면 돌려보냄
        if (hack == _HackMaxCount) return;

        // 가두기
        hack = Mathf.Clamp(hack, 0, _HackMaxCount);

        // 핵 추가
        hack += addHack;

        // 게이지 상태 업데이트
        HackGaugeUpdate();
    }

    public void HackGaugeUpdate()
    {
        // 채우기
        _HackGauge.fillAmount = Mathf.MoveTowards(_HackGauge.fillAmount, hack, 10.0f);

        // 반올림
        hack = Mathf.Round(hack * 10.0f) * 0.1f;

        // 텍스트 설정
        _HackCount.text = hack.ToString();
    }

}
