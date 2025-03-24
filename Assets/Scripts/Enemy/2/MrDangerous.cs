using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MrDangerous : EnemyBase
{
    // 멈췄었는지 검사
    private bool _IsStop = false;

    private void OnEnable() {
        StartCoroutine(IsStop());
    }

    private void OnDisable() {
        _IsStop = false;
    }

    private void Start() {
        InitializeData();
    }

    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 0.8f;

        // 접촉 데미지
        m_Damage = 30.0f;

        // 체력
        m_HP = m_MaxHP = 8;

        // 돈
        m_Money = 25;

    }

    private void Update() {
        if (m_Moveable) Move();
    }

    protected override void Move() {
        transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);
    }

    private IEnumerator IsStop() {

        while (m_Moveable) {

            if (transform.position.y <= 0.0 && !_IsStop) {

                m_Anim.SetBool("Waiting", true);
                m_Moveable = false;
                yield return new WaitForSeconds(2.0f);
                m_Anim.SetBool("Waiting", false);
                _IsStop = true;
                m_Moveable = true;
            }

            yield return null;
        }

    }

}
