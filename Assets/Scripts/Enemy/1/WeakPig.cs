using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPig : EnemyBase
{
    private enum PigType { TiedPig, Pig}
    [SerializeField] private PigType _Pig = PigType.TiedPig;

    [SerializeField] private Bullet _BulletPrefab;
    private List<Bullet> _BulletList = new List<Bullet>();
    
    private float _MoveXSpeed = 0.7f;

    private void OnEnable() {
        if (_Pig == PigType.TiedPig)
            StartCoroutine(Shooting());
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
        m_Damage = 10.0f;

        // 체력
        m_HP = m_MaxHP = 2;

        if (_Pig == PigType.Pig) m_Money = 10;

        else
            // 돈
            m_Money = 15;
    }

    // 아래로 이동
    protected override void Move() {
        transform.Translate(Vector2.down * m_MoveSpeed * Time.deltaTime);

        if (_Pig == PigType.TiedPig)
        {
            transform.Translate(Vector2.right * _MoveXSpeed * Time.deltaTime);

            if (transform.position.x >= 2.0f) _MoveXSpeed = -0.7f;
            else if (transform.position.x <= -2.0f) _MoveXSpeed = 0.7f;
        }
        
    }

    // 총질
    private IEnumerator Shooting() {

        while (m_Moveable) {

            float randomNumber = Random.Range(2.0f, 4.0f);

            yield return new WaitForSeconds(randomNumber);

            int index = FindDisableBulletListIndex();

            if (index == -1) {
                CreateBullet();
            }

            else {
                EnableBullet(index);
            }

        }
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
    }

    // 총알 활성화
    private void EnableBullet(int index) {
        _BulletList[index].transform.gameObject.SetActive(true);

        _BulletList[index].transform.position = transform.position;
    }

}
