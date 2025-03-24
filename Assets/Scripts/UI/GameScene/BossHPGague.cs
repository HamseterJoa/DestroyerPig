using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPGague : MonoBehaviour
{
    [SerializeField] private Image _HPBar;
    
    // 체력 업데이트
    public void HPBarUpdate(float currentHp, float maxHP){
	
		// 보스 체력 백분율로 환산
		float bossHP = currentHp / maxHP;

		// 체력바 업데이트
		_HPBar.fillAmount = Mathf.MoveTowards(_HPBar.fillAmount, bossHP, 10.0f);
	}

}
