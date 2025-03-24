using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPlayer : MonoBehaviour
{
    // 공발사기
    [SerializeField] private BallShooter _BS;

    // 스프라이트 렌더러
    private SpriteRenderer _SR;

    // 돼지들 이미지 들어있음
    [SerializeField] private List<Sprite> _SpriteList;

    private void Start()
    {
        _SR = GetComponent<SpriteRenderer>();

        // 이미지 설정
        InitializeSprite(GameManager.getJsonDataManager.playerData.PlayerState);
    }

    private void OnMouseDrag()
    {
        if (Time.timeScale == 1.0f)
        {
            // 마우스 좌표
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // 움직일 오브젝트
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);

            objPosition = new Vector3(objPosition.x, objPosition.y, 0.0f);

            // 오브젝트 가두기
            objPosition.x = Mathf.Clamp(objPosition.x, -2.2f, 2.2f);
            objPosition.y = Mathf.Clamp(objPosition.y, -4.4f, 4.4f);

            // 오브젝트 위치 설정
            transform.position = objPosition;
        }
    }

    private void InitializeSprite(MiniGamePlayerState state)
    {
        // 스프라이트를 번호에 맞게 변경
        _SR.sprite = _SpriteList[(int)state];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 공에 닿았다면
        if (collision.transform.tag == "Ball")
        {
            // 끝내기 호출
            _BS.End();

            // 몸뚱이 비활성화
            gameObject.SetActive(false);
        }
    }

}
