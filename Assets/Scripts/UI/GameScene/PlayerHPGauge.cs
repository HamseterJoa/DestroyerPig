using UnityEngine;
using UnityEngine.UI;

public class PlayerHPGauge : MonoBehaviour
{
    [SerializeField] private Image _HPbar1; // 빨강
    [SerializeField] private Image _HPbar2; // 보라
    [SerializeField] private Image _HPbar3; // 노랑

    private float playerHP1, playerHP2, playerHP3;
    
    // 플레이어 쪽에서 맞거나 체력이 감소할 때 마다 호출
    public void HPUpdate() {

        // 빨간줄 받아오기
        if (GameManager.getCharacterManager.player.playerHp <= 100) playerHP1 = GameManager.getCharacterManager.player.playerHp * 0.01f;

        // 다른줄이 없다면 리턴
        else if (_HPbar2 == null) return;

        // 보라색 줄
        else if (GameManager.getCharacterManager.player.playerHp <= 200) playerHP2 = (GameManager.getCharacterManager.player.playerHp - 100) * 0.01f;

        // 노란색 줄
        else if (GameManager.getCharacterManager.player.playerHp <= 300) playerHP3 = (GameManager.getCharacterManager.player.playerHp - 200) * 0.01f;

        // 보라색줄 삭제
        if (GameManager.getCharacterManager.player.playerHp <= 100) _HPbar2.fillAmount = 0.0f;
        
        // 보라색줄이 있을 떄 빨간줄 최대로
        if (GameManager.getCharacterManager.player.playerHp > 100) _HPbar1.fillAmount = 1.0f;
        
        // 노란색줄 있을 때 보라색줄 최대로
        if (GameManager.getCharacterManager.player.playerHp > 200) _HPbar2.fillAmount = 1.0f;
        
        // 노란색줄 삭제
        if (GameManager.getCharacterManager.player.playerHp <= 200) _HPbar3.fillAmount = 0.0f;

        // 빨간줄 UI
        if (GameManager.getCharacterManager.player.playerHp <= 100) _HPbar1.fillAmount = Mathf.MoveTowards(_HPbar1.fillAmount, playerHP1, 10.0f);

        // 보라색 줄
        else if (GameManager.getCharacterManager.player.playerHp <= 200) _HPbar2.fillAmount = Mathf.MoveTowards(_HPbar2.fillAmount, playerHP2, 10.0f);

        // 노란색 줄
        else if (GameManager.getCharacterManager.player.playerHp <= 300) _HPbar3.fillAmount = Mathf.MoveTowards(_HPbar3.fillAmount, playerHP3, 10.0f);
    }

}
