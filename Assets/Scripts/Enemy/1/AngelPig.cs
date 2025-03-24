using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelPig : EnemyBase
{
    private float _MoveXSpeed;

    private void OnEnable() {
        StartCoroutine(ChnageDiraction());
    }

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
        m_Damage = 25.0f;

        // 체력
        m_HP = m_MaxHP = 6;

        // 돈
        m_Money = 25;

        int random = Random.Range(0, 2);

        if (random == 0) _MoveXSpeed = 0.8f;
        else _MoveXSpeed = -0.8f;

    }

    // 이동
    protected override void Move() {

        transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);

        if (transform.position.x >= 2.0f || transform.position.x <= -2.0f) _MoveXSpeed *= -1.0f;

        transform.Translate(Vector2.right * _MoveXSpeed * Time.deltaTime);

    }

    private IEnumerator ChnageDiraction() {

        while (m_Moveable) {

            int randomDelay = Random.Range(2, 6);

            yield return new WaitForSeconds(randomDelay);
            _MoveXSpeed *= -1.0f;
            
        }

    }

}
