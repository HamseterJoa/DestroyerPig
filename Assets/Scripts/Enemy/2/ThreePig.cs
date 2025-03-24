using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreePig : EnemyBase
{

    private void Start() {
        InitializeData();
    }

    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 0.4f;

        // 접촉 데미지
        m_Damage = 30.0f;

        // 체력
        m_HP = m_MaxHP = 20;

        // 돈
        m_Money = 23;

    }

    private void Update() {
        if (m_Moveable) Move();
    }

    protected override void Move() {
        transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);
    }
}
