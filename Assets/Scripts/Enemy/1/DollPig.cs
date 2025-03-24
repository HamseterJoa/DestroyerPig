using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollPig : EnemyBase
{
    private bool _Moving = false;
    private Vector2 _RandomDiraction;

    private void Start() {
        InitializeData();
    }

    private void OnEnable() {
        StartCoroutine(ChangeState());
    }

    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 0.4f;

        // 접촉 데미지
        m_Damage = 10.0f;

        // 체력
        m_HP = m_MaxHP = 1;

        // 돈
        m_Money = 5000;

    }

    private void Update() {
        if (m_Moveable) Move();
    }

    protected override void Move() {

        // 그냥 아래로 전진
        if (!_Moving) {
            transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);
        }

        // 랜방향으로 전진
        else {
            transform.Translate(_RandomDiraction * m_MoveSpeed * Time.deltaTime);

			// 가두기
			transform.position = new Vector2(Mathf.Clamp(transform.position.x, -2.0f, 2.0f), transform.position.y);
        }
        
    }

    // 상태 변경
    private IEnumerator ChangeState() {

        while (m_Moveable) {

            yield return new WaitForSeconds(1.0f);

            // 랜덤수 뽑기
            float x = Random.Range(-1.0f, 1.0f), y = Random.Range(-1.0f, 1.0f);

            // 랜덤좌표로 설정
            _RandomDiraction = new Vector2(x, y);

            // 랜덤으로 움직이기 허용
            _Moving = true;

            yield return new WaitForSeconds(2.0f);

            // 일정하게 움직이기
            _Moving = false;

        }

        yield return null;
    }

}
