using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowardPig : EnemyBase
{
    [SerializeField] private bool _IsNear = false;

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
        m_Damage = 0.0f;

        // 체력
        m_HP = m_MaxHP = 5;

        // 돈
        m_Money = 15;
    }

    // 아래로 이동
    protected override void Move()
    {
        if (!_IsNear) transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);
    }

    // 체력이 감소할 때 호출
    protected void HPDown(string damageType)
    {
        //m_Anim.SetBool("Hurting", true);]

        switch (damageType)
        {
            case "Ball":
                if (!_IsNear) m_HP -= GameManager.getCharacterManager.ball.BallDamage;
                else m_HP -= GameManager.getCharacterManager.ball.BallDamage * 0.1f; break;

            case "MiniBall":
                if (!_IsNear) m_HP--; 
                else m_HP -= 0.1f; break;

            case "Bullet":
                if (!_IsNear) m_HP--;
                else m_HP -= 0.1f; break;

            case "Hack":
				if (!_IsNear) m_HP -= _HackDamage;
				else m_HP -= _HackDamage * 0.1f; break;

            case "HOS":
                if (!_IsNear) m_HP -= 0.1f;
                else m_HP -= 0.01f; break;
        }

		// 체력 검사
		if (m_HP <= 0.0f) {
			m_Moveable = false;
			m_Anim.SetBool("Die", true);
		}

	}
	
	protected void OnTriggerExit2D(Collider2D collision) {

		if (collision.transform.tag == "Player") {

			_IsNear = false;

			// 애니메이션 설정
			m_Anim.SetBool("Def", _IsNear);
		}

	}

	protected void OnCollisionEnter2D(Collision2D collision) {

		if (collision.transform.CompareTag("Ball")) {
			HPDown("Ball");
		}

		if (collision.transform.tag == "MiniBall") {
			HPDown("MiniBall");
		}

		// 플레이어와 접촉시 플레이어가 대미지를 받을수 있는지 검사하고 받을수 있다면 플레이어 체력감소
		if (collision.transform.tag == "Player")
			if (GameManager.getCharacterManager.player.damageable)
				GameManager.getCharacterManager.player.HPDown(m_Damage);

		// 옥수수 지역에 도착하면 플레이어에게 대미지
		if (collision.transform.tag == "CornArea") {

			// 보스가 아니라면 대미지를 주고 비활성화
			if (!m_IsBoss) {
				GameManager.getCharacterManager.player.playerHp -= 10;
				Disable();
			}

		}

	}

	protected void OnTriggerEnter2D(Collider2D collision) {

		if (collision.transform.tag == "Player") {

			_IsNear = true;

			// 애니메이션 설정
			m_Anim.SetBool("Def", _IsNear);
		}

		// 플레이어 총알에 맞았다면
		if (collision.transform.tag == "Bullet") HPDown("Bullet");

		// 공에 맞았다면
		if (collision.transform.CompareTag("Ball")) {
			HPDown("Ball");
			GameManager.getCharacterManager.ball.AttackEnemy();
		}

		// 핵에 맞았다면
		if (collision.transform.tag == "Hack") HPDown("Hack");
	}

	protected void OnTriggerStay2D(Collider2D collision) {
		// 히오스에 닿았다면
		if (collision.transform.tag == "HOS") HPDown("HOS");
	}

}
