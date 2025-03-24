using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardModeMarker : MonoBehaviour
{
    private void OnEnable()
    {
        // 하드모드가 아니라면 비활성화
        if (!GameManager.getJsonDataManager.playerData.Hardmode)
        {
            gameObject.SetActive(false);
        }
    }
}
