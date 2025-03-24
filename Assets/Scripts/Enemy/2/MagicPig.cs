using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPig : EnemyBase
{
    [SerializeField] private Bullet _BulletPrefab;
    private List<Bullet> _BulletList = new List<Bullet>();

    private void OnEnable() {
        StartCoroutine(Teleport());
        StartCoroutine(Attack());
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
        m_MoveSpeed = 0.0f;

        // 접촉 데미지
        m_Damage = 30.0f;

        // 체력
        m_HP = m_MaxHP = 5;

        // 돈
        m_Money = 25;
    }

    protected override void Move() {

    }

    // 이동
    private IEnumerator Teleport() {

        while (m_Moveable) {

            yield return new WaitForSeconds(2.0f);

            float x = Random.Range(-2.0f, 2.0f), y = Random.Range(1.5f, 3.5f);

            transform.position = new Vector2(x, y);

        }

        yield return null;
    }

    // 공격
    private IEnumerator Attack() {

        while (m_Moveable) {

            // 공격을 할확률 뽑기
            int randomAttack = Random.Range(0, 2);

            // 공격성공
            if (randomAttack == 0) {

                // 애니메이션
                m_Anim.SetBool("Attack", true);   
            }


            // 후 딜레이
            yield return new WaitForSeconds(3.0f);

        }

        yield return null;
    }

    // 애니메이션에서 던져줌
    public void Throw()
    {
        int index = FindDisableBulletListIndex();

        // 비활성화 중인 마법탄이 없다면
        if (index == -1)
        {

            // 총알 생성
            CreateBullet();
        }

        // 비활성화 중인 마법탄을 찾았다면
        else EnableBullet(index);

        m_Anim.SetBool("Attack", false);
    }

    // 비활성화중인 총알리턴
    private int FindDisableBulletListIndex() {
        for (int i = 0; i < _BulletList.Count; i++) {
            if (!_BulletList[i].transform.gameObject.activeSelf) {
                return i;
            }
        }

        // 총알을 못찾았다면 -1을 리턴
        return -1;
    }

    // 총알 생성
    private void CreateBullet() { 
        Bullet tempBullet = Instantiate(_BulletPrefab);

        tempBullet.transform.position = new Vector2(transform.position.x, transform.position.y - 1.0f);

        _BulletList.Add(tempBullet);
    }

    // 총알 활성화
    private void EnableBullet(int index) {

        // 활성화
        _BulletList[index].transform.gameObject.SetActive(true);

        // 좌표설정
        _BulletList[index].transform.position = new Vector2(transform.position.x, transform.position.y - 1.0f);
    }

}
