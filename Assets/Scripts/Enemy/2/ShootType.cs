using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootType : EnemyBase
{
    [SerializeField] private Bullet _BulletPrefab;
    private List<Bullet> _BulletList = new List<Bullet>();
    private float _ShootingDelay = 2.5f;
    private float _ShootingDelayCheckTime = 0.0f;

    private void Start()
    {
        InitializeData();
    }

    private void Update()
    {
        if (m_Moveable) {
            Move();
            Shooting();
        }

    }

    // 데이터 초기화
    private void InitializeData()
    {
        // 이동속도
        m_MoveSpeed = 0.0f;

        // 접촉 데미지
        m_Damage = 10.0f;

        // 체력
        m_HP = m_MaxHP = 3;

        // 돈
        m_Money = 20;
    }

    // 이동
    protected override void Move()
    {
        transform.position = Vector2.Lerp(transform.position, new Vector2(GameManager.getCharacterManager.player.transform.position.x, 3.0f), 0.8f * Time.deltaTime);
    }

    // 총질
    private void Shooting()
    {
        if (Time.time - _ShootingDelayCheckTime >= _ShootingDelay) m_Anim.SetBool("IsAttack", true);

        if (Time.time - _ShootingDelayCheckTime >= (_ShootingDelay + 0.4f))
        {
            _ShootingDelayCheckTime = Time.time;
            int index = FindDisableBulletListIndex();

            if (index == -1)
            {
                CreateBullet();
            }

            else
            {
                EnableBullet(index);
            }

            m_Anim.SetBool("IsAttack", false);
        }
    }

    // 비활성화중인 총알리턴
    private int FindDisableBulletListIndex()
    {
        for (int i = 0; i < _BulletList.Count; i++)
        {
            if (!_BulletList[i].transform.gameObject.activeSelf)
            {
                return i;
            }
        }

        // 총알을 못찾았다면 -1을 리턴
        return -1;
    }

    // 총알 생성
    private void CreateBullet()
    {
        Vector2 initPos = new Vector2(transform.position.x, transform.position.y - 1.0f);

        Quaternion initRot = Quaternion.identity;

        Bullet tempBullet = Instantiate(_BulletPrefab, initPos, initRot);

        _BulletList.Add(tempBullet);

    }

    // 총알 활성화
    private void EnableBullet(int index)
    {
        _BulletList[index].transform.gameObject.SetActive(true);

        _BulletList[index].transform.position = new Vector2(transform.position.x, transform.position.y - 1.0f);
    }
}
