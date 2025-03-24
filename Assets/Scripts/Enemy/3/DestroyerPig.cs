using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerPig : EnemyBase
{
    [SerializeField] private Bullet _BulletPrefab;
    private List<Bullet> _BulletList = new List<Bullet>();

    private void Start() {
        InitializeData();
    }

    // 활성화될때 실행
    private void OnEnable() {
        // 원거리 공격
        StartCoroutine(Shooting());
    }

    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 0.3f;

        // 접촉 데미지
        m_Damage = 25.0f;

        // 체력
        m_HP = m_MaxHP = 10;

        // 돈
        m_Money = 37;

    }

    private void Update() {
        if (m_Moveable)
        {
            Move();
        }
    }

    protected override void Move() {
        transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);
    }

    // 원거리 공격
    private IEnumerator Shooting()
    {
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                m_Moveable = false;
                m_Anim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.5f);
            }

            m_Moveable = true;
            yield return new WaitForSeconds(4.0f);
        }
    }

    private void Attack() {

        int index = FindDisableBulletListIndex();

        if (index == -1) CreateBullet();

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
        tempBullet.transform.position = transform.position;
        _BulletList.Add(tempBullet);

        // 플레이어 방향으로 던질것 인지
        tempBullet.aim = true;
    }

    // 총알 활성화
    private void EnableBullet(int index) {
        _BulletList[index].transform.gameObject.SetActive(true);

        _BulletList[index].transform.position = transform.position;

        // 플레이어 방향으로 던질것 인지
        _BulletList[index].aim = true;
    }
}
