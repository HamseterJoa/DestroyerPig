using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPig : EnemyBase
{
    private bool _IsMove = false;

    private void OnEnable()
    {
        StartCoroutine(ChangeState());
    }

    private void Start()
    {
        InitializeData();
    }

    private void Update()
    {
        if (m_Moveable) Move();
    }

    // 데이터 초기화
    private void InitializeData()
    {

        // 이동속도
        m_MoveSpeed = 0.4f;

        // 접촉 데미지
        m_Damage = 20.0f;

        // 체력
        m_HP = m_MaxHP = 5;

        // 돈
        m_Money = 20;
    }

    // 아래로 이동
    protected override void Move()
    {
        if (_IsMove) transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);
    }

    private IEnumerator ChangeState()
    {
        while (m_Moveable)
        {
            int randomMoveDelay = Random.Range(3, 7);
            _IsMove = true;
            yield return new WaitForSeconds(randomMoveDelay);

            int randomStopDelay = Random.Range(1, 5);
            _IsMove = false;
            yield return new WaitForSeconds(randomStopDelay);
        }
    }

}
