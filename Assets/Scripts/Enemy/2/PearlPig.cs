using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PearlPig : EnemyBase
{
    private bool _IsAttack = false;

    // 꽃의 원본
    [SerializeField] private GameObject _FlowerOri;

    // 사용할 꽃
    private GameObject _Flower;

    private void OnEnable() {
        StartCoroutine(ChangeState());
    }

    private void Start() {

        InitializeData();
        
        // 꽃 생성
        CreateFlower();

        // 꽃 비활성화
        _Flower.gameObject.SetActive(false);
    }

    private void Update() {

        if (m_Moveable) {

            Move();

            // 공격 상태라면
            if (_IsAttack) {
                _Flower.gameObject.SetActive(true);

                // 활성화 되어있을 때 꽃 위치 설정
                _Flower.transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            }

            else _Flower.gameObject.SetActive(false);

        }
    }

    // 데이터 초기화
    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 0.4f;

        // 접촉 데미지
        m_Damage = 30.0f;

        // 체력
        m_HP = m_MaxHP = 10.0f;

        // 돈
        m_Money = 25;
    }

    // 아래로 이동
    protected override void Move() {
        transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);
    }

    private IEnumerator ChangeState() {

        while (m_Moveable) {

			// 랜덤 딜레이
            int delayTime = Random.Range(4, 11);
            yield return new WaitForSeconds(delayTime);
            _IsAttack = true;
			
			// 애니메이션 설정
			m_Anim.SetBool("Attack", _IsAttack);

            yield return new WaitForSeconds(2.0f);
            _IsAttack = false;

			// 애니메이션 설정
			m_Anim.SetBool("Attack", _IsAttack);
		}

        yield return null;
    }
    
    // 꽃 생성
    private void CreateFlower()
    {
        GameObject flower = Instantiate(_FlowerOri);

        // 사용할 꽃 지정
        _Flower = flower;
    }

    private void OnDisable()
    {
        // 사용한 꽃 비활성화
        _Flower.gameObject.SetActive(false);
    }

}
