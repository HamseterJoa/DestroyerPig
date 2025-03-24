using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Marker : MonoBehaviour
{
    private Image _Self;
    
    private void Start()
    {
        JsonData.PlayerData player = GameManager.getJsonDataManager.playerData;

        // 본인 참조
        _Self = GetComponent<Image>();

        // 마커 위치 초기화
        _Self.rectTransform.anchoredPosition = new Vector2((int)player.PlayerState * 350.0f, 0.0f);
    }
}
