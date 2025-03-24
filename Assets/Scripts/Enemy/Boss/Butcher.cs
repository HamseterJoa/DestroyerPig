using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Butcher : EnemyBase
{
    // 던지는 칼
    [SerializeField] private Bullet _KnifePrefab;
    private List<Bullet> _KnifeList = new List<Bullet>();

    // 보스 체력게이지 
    private BossHPGague _BossHPGague;

    private Image bossHPBar;

    // 등장 이펙트
    [SerializeField] private GameObject _AppearEffect;

    // 회전할 중심좌표
    private Vector3 _Point = new Vector3(0.0f, 3.0f, 0.0f);

    // 회전 방향
    private Vector3 _RotateDir = Vector3.back;

    // 랜덤이동할 좌표 에스더꺼
    private Vector2 _RandomPos;

    // 움직일때 쓸 불형
    private bool _Moving = false;

    private void OnEnable()
    {
        // 등장 이펙트
        Instantiate(_AppearEffect);

        // 원거리 공격할 것인지 뽑기
        StartCoroutine(Shooting());

        // 배경음악 변경
        GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Boss);
    }

    private void Start()
    {
        InitializeData();
    }

    private void Update()
    {
        if (_Moving)
        {
            transform.position = Vector2.MoveTowards(transform.position, _RandomPos, m_MoveSpeed * Time.deltaTime);

            // 도착 했다면
            if (transform.position.x == _RandomPos.x && transform.position.y == _RandomPos.y) _Moving = false;
        }
    }

    private void InitializeData()
    {
        // 이동속도
        m_MoveSpeed = 2.0f;

        // 접촉 데미지
        m_Damage = 40.0f;

        // 체력
        m_HP = m_MaxHP = 550;

        // 돈
        m_Money = 5500;

        // 보스
        m_IsBoss = true;

        // 체력바 참조
        _BossHPGague = GameObject.Find("Canvas").transform.Find("ScreenPanel").transform.Find("BossHPGague").transform.Find("BossHPBar").GetComponent<BossHPGague>();

        // 체력바 위치를 조정하기 위한 거
        bossHPBar = GameObject.Find("Canvas").transform.Find("ScreenPanel").transform.Find("BossHPGague").GetComponent<Image>();

        // 체력바 위치 설정
        bossHPBar.rectTransform.anchoredPosition = new Vector2();

        // 체력바 체력 설정
        _BossHPGague.HPBarUpdate(m_HP, m_MaxHP);
    }

    protected override void Move()
    {
        // 움직이기 호출(애니메이션)
        _Moving = true;
    }

    // 체력이 감소할 때 호출
    protected void HPDown(string damageType)
    {
        base.HPDown(damageType);

        // 체력바 업데이트
        _BossHPGague.HPBarUpdate(m_HP, m_MaxHP);
    }

    // 어떤 공격을 할것인지 뽑기
    private IEnumerator Shooting()
    {
        while (m_Moveable)
        {
            // 던지기
            m_Anim.SetTrigger("Attack");

            // 이동 방향 랜덤 설정
            _RandomPos = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(0.0f, 4.0f));

            yield return new WaitForSeconds(3.5f);
        }
    }

    // 칼 던지기 애니메이션에서 던짐
    public void Throw()
    {
        // 번호잠시 담을거
        int num = FindDisableKnife();

        // 칼을 못찾았다면 생성
        if (num == -1) CreateKnife();

        // 못찾았다면 활성화
        else EnableKnife(num);
    }

    // 비활성화되어있는거를 찾음
    private int FindDisableKnife()
    {
        for(int i = 0; i < _KnifeList.Count; i++)
        {
            // 있으면 그거에 리스트 번호를 리턴
            if (!_KnifeList[i].gameObject.activeSelf) return i;
        }

        // 없으면 -1리턴
        return -1;
    }

    // 칼 생성
    private void CreateKnife()
    {
        // 생성
        Bullet knife = Instantiate(_KnifePrefab);

        // 좌표 설정
        knife.transform.position = transform.position;

        // 리스트에 추가
        _KnifeList.Add(knife);
    }

    // 칼 활성화
    private void EnableKnife(int index)
    {
        // 받은 번호의 칼 활성화
        _KnifeList[index].gameObject.SetActive(true);

        // 좌표설정
        _KnifeList[index].transform.position = transform.position;
    }

    private void OnDisable()
    {
        base.OnDisable();

        // 체력바 위치 설정
        bossHPBar.rectTransform.anchoredPosition = new Vector2(1000.0f, 0.0f);

        // 배경음악 변경
        GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Game);
    }

    #region 맞음처리
    protected void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.transform.CompareTag("Ball"))
        {
            HPDown("Ball");
        }

        if (collision.transform.tag == "MiniBall")
        {
            HPDown("MiniBall");
        }

        // 플레이어와 접촉시 플레이어가 대미지를 받을수 있는지 검사하고 받을수 있다면 플레이어 체력감소
        if (collision.transform.tag == "Player")
            if (GameManager.getCharacterManager.player.damageable)
                GameManager.getCharacterManager.player.HPDown(m_Damage);

        // 옥수수 지역에 도착하면 플레이어에게 대미지
        if (collision.transform.tag == "CornArea")
        {

            // 보스가 아니라면 대미지를 주고 비활성화
            if (!m_IsBoss)
            {
                GameManager.getCharacterManager.player.playerHp -= 10;
                Disable();
            }

        }

    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 총알에 맞았다면
        if (collision.transform.tag == "Bullet") HPDown("Bullet");

        // 공에 맞았다면
        if (collision.transform.CompareTag("Ball"))
        {
            HPDown("Ball");
            GameManager.getCharacterManager.ball.AttackEnemy();
        }

        // 핵에 맞았다면
        if (collision.transform.tag == "Hack") HPDown("Hack");
    }

    protected void OnTriggerStay2D(Collider2D collision)
    {
        // 히오스에 닿았다면
        if (collision.transform.tag == "HOS") HPDown("HOS");
    }
    #endregion
}
