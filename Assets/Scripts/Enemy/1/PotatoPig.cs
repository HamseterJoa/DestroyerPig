using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoPig : EnemyBase
{
    private void Start() {
        InitializeData();
    }

    private void Update() {
        if (m_Moveable) Move();
    }

    // 데이터 초기화
    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 0.4f;

        // 접촉 데미지
        m_Damage = 15.0f;

        // 체력
        m_HP = m_MaxHP = 3;

        // 돈
        m_Money = 15;
    }

    // 아래로 이동
    protected override void Move() {
        transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);
    }
}
