using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IronPig : EnemyBase
{
    private bool _isAttack = false;
    private Vector2 _TargetDiraction;

    private void OnEnable() {
        StartCoroutine(AttackDelay());
    }

    private void Start() {
        InitialzeData();
    }

    private void InitialzeData() {

        // 이동속도
        m_MoveSpeed = 0.1f;

        // 접촉 데미지
        m_Damage = 35.0f;

        // 체력
        m_HP = m_MaxHP = 15;

        // 돈
        m_Money = 33;
    }

    private void Update() {
        if (m_Moveable) Move();
    }

    // 공격 상태 변경
    private IEnumerator AttackDelay() {

        while (m_Moveable) {

            _isAttack = false;
            m_Anim.SetBool("Attack", _isAttack);

            yield return new WaitForSeconds(2.0f);

            _isAttack = true;
            m_Anim.SetBool("Attack", _isAttack);
            yield return new WaitForSeconds(0.8f);
        }

    }

    // 이동 및 공격
    protected override void Move() {

        // 공격상태가 아니라면
        if (!_isAttack) {

            // 목표 방향 설정
            _TargetDiraction = GameManager.getCharacterManager.player.transform.position - transform.position;

            // 천천히 이동
            transform.Translate(_TargetDiraction * m_MoveSpeed * Time.deltaTime);
        }

        else {
            _TargetDiraction.Normalize();

            transform.Translate(_TargetDiraction * 7.0f * Time.deltaTime);

            if (transform.position.x < -3.3f || transform.position.x > 3.3f) {
                m_HP = 0;
                Disable();
            }
        }

        
    }
}
