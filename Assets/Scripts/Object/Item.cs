using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private enum ItemSort { Portion, Shooting, Upgrade, Overload, BigBar, MiniBall, HOS}
    [SerializeField] private ItemSort _ItemSort = ItemSort.Portion;
    
    private float _MoveSpeed = 1.0f;
    
    private void Update() {
        ItemMoveDown();
    }

	// 아래로 이동
    private void ItemMoveDown() {
        transform.Translate(Vector2.down * _MoveSpeed * Time.deltaTime);

        if (transform.position.y <= -6.0f) gameObject.SetActive(false);
    }

    private void UseItem()
    {
        // 아이템 먹는 소리
        GameManager.getItemManager.PlayItemUseSound();

        // 사용아이템 추가
        GameManager.getCharacterManager.usedItemgetset++;

        switch (_ItemSort)
        {
            case ItemSort.Portion:
                GameManager.getCharacterManager.player.Heal(); break;

            case ItemSort.Shooting:

                // 시작
                // 이미 수박씨를 쏘고있다면 return
                if (GameManager.getCharacterManager.player.isShooting) return;
                GameManager.getCharacterManager.player.StartShooting();
                break;

            case ItemSort.Upgrade:

				// 공 크기 증가 실행
				GameManager.getCharacterManager.player.UpgradeBall();
                break;

            case ItemSort.Overload:

                // 과부하 시작
                // 이미 과부하 상태라면 리턴
                if (GameManager.getCharacterManager.player.overload) return;
                GameManager.getCharacterManager.player.StartOverload();

                break;

            case ItemSort.BigBar:
				GameManager.getCharacterManager.player.BigBar();
                break;

            case ItemSort.MiniBall:
				GameManager.getCharacterManager.player.MiniBall();
                break;

            case ItemSort.HOS:

                // 실행
                // 히오스가 실행중이라면 return
                if (GameManager.getCharacterManager.player.hos) return;
                GameManager.getCharacterManager.player.StartHos();
                break;

        }
    }

    private void DisableItem()
    {
        UseItem();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
            DisableItem();
    }

}
