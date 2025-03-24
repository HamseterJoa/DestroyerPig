using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameBall : MonoBehaviour
{
    // 시작지점을 저장
    private Vector2 _StartPos;

    // 날아갈 방향
    [SerializeField] private Vector2 _FirePos;

    // 속도
    private float _Speed = 3.0f;

    private void Start()
    {
        InitializeData();
    }

    private void OnEnable()
    {
        InitializeData();
    }

    private void Update()
    {
        // 이동
        Move();
    }

    // 초기화
    private void InitializeData()
    {
        // 시작위치 저장
        _StartPos = transform.position;

        // 랜덤으로 더하거나 뺴줄 수
        float randomX = Random.Range(-2.5f, 2.5f),
              randomY = Random.Range(-2.5f, 2.5f);

        // 발사방향 설정
        _FirePos = new Vector2((_StartPos.x * -1.0f) + randomX, (_StartPos.y * -1.0f) + randomY);

        // 발사방향 어쩌구화
        _FirePos.Normalize();
    }

    // 이동
    private void Move()
    {
        transform.Translate(_FirePos * _Speed * Time.deltaTime);

        if (transform.position.x > 4.0f || transform.position.x < -4.0f || transform.position.y > 6.0f || transform.position.y < -6.0f) gameObject.SetActive(false);
    }
}
