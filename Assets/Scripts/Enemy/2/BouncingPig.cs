using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingPig : EnemyBase
{
    private float _MoveXSpeed;

    private void Start() {
        InitializeData();
    }

    private void Update() {
        if (m_Moveable) Move();
    }

    // 데이터 초기화
    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 0.5f;

        // 접촉 데미지
        m_Damage = 40.0f;

        // 체력
        m_HP = m_MaxHP = 5;

        // 돈
        m_Money = 20;

        // 랜덤 방향
        int random = Random.Range(0, 2);

        // 랜덤한 속도
        float randomSpeed = Random.Range(0.2f, 1.0f);

        if (random == 0) _MoveXSpeed = randomSpeed;
        else _MoveXSpeed = -randomSpeed;

    }

    // 이동
    protected override void Move() {

        transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);

        if (transform.position.x >= 2.0f || transform.position.x <= -2.0f) _MoveXSpeed *= -1.0f;

        transform.Translate(Vector2.right * _MoveXSpeed * Time.deltaTime);

    }
}
