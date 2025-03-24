using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingPig : EnemyBase
{
    private bool _IsAttack = false;
    private Vector2 _TargetPos;

    private void OnEnable() {
        StartCoroutine(AttackDelay());
    }

    private void Start() {
        InitializeData();
    }

    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 4.0f;

        // 접촉 데미지
        m_Damage = 10.0f;

        // 체력
        m_HP = m_MaxHP = 1;

        // 돈
        m_Money = 10;
    }

    private void Update() {
        if (m_Moveable) Move();
    }

    protected override void Move() {

        // 목표 지점 설정
        _TargetPos = new Vector2(GameManager.getCharacterManager.player.transform.position.x, 3.0f);

        // 플레이어 쪽으로 이동
        if (_IsAttack) transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);

        else transform.position = Vector2.MoveTowards(transform.position, _TargetPos, 1.0f * Time.deltaTime);
    }

    private IEnumerator AttackDelay() {

        while (true) {

            if (Mathf.Approximately(transform.position.x, _TargetPos.x) && !_IsAttack) {

                m_Moveable = false;

                // 공격상태
                _IsAttack = true;

                // 데미지 설정
                m_Damage = 40;

                // 애니메이션재생
                m_Anim.SetBool("Attack", true);

                yield return new WaitForSeconds(1.0f);

                m_Moveable = true;
            }

            yield return null;
        }
    }

    private void OnDisable() {
        _IsAttack = false;
    }


}
