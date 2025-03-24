using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuvPig : EnemyBase
{
    [SerializeField] private GameObject _Carrot;

    private void OnEnable()
    {
        StartCoroutine(Swing());
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
        // 당근비활성화
        _Carrot.gameObject.SetActive(false);

        // 이동속도
        m_MoveSpeed = 0.2f;

        // 접촉 데미지
        m_Damage = 30.0f;

        // 체력
        m_HP = m_MaxHP = 15;

        // 돈
        m_Money = 36;
    }

    // 아래로 이동
    protected override void Move()
    {
        transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);
    }

    // 빠따질
    private IEnumerator Swing()
    {
        while (m_Moveable)
        {
            int randomSwingCount = Random.Range(5, 11);

			// 애니메이션
			m_Anim.SetBool("Attack", true);

			// 데미지 증가
            m_Damage = 60.0f;

			// 당근 활성화
            _Carrot.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

			// 애니메이션
			m_Anim.SetBool("Attack", false);

			// 데미지 원래대로
			m_Damage = 30.0f;

			// 당근 비활성화
            _Carrot.gameObject.SetActive(false);
            yield return new WaitForSeconds(randomSwingCount);

        }

        yield return null;

    }

}
