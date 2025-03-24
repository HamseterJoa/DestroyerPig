using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HundredPig : EnemyBase
{
    // 던지는 돼지
    [SerializeField] private Bullet _BulletPrefab;
    private List<Bullet> _BulletList = new List<Bullet>();

    // 지진
    [SerializeField] private Bullet _EarthquakePrefab;
    private List<Bullet> _EarthquakeList = new List<Bullet>();

	// 보스 체력게이지 
	private BossHPGague _BossHPGague;

    private Image bossHPBar;

    // 등장 이펙트
    [SerializeField] private GameObject _AppearEffect;

    private void OnEnable()
    {
        // 등장 이펙트
        Instantiate(_AppearEffect);

        // 원거리 공격할 것인지 뽑기
        StartCoroutine(Shooting());

        // 배경음악 변경
        GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Boss);
    }

    private void Start() {
        InitializeData();
    }

    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 0.4f;

        // 접촉 데미지
        m_Damage = 40.0f;

        // 체력
        m_HP = m_MaxHP = 100;

        // 돈
        m_Money = 1000;

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

    protected override void Move() {
        
    }

    // 체력이 감소할 때 호출
    protected void HPDown(string damageType)
    {
        base.HPDown(damageType);

        // 체력바 업데이트
        _BossHPGague.HPBarUpdate(m_HP, m_MaxHP);
    }

    // 어떤 공격을 할것인지 뽑기
    private IEnumerator Shooting() {
        while (m_Moveable) {

            yield return new WaitForSeconds(2.0f);

            // 어떤 행동을 할것인지 뽑음
            int randomAct = Random.Range(0, 2);

            // 공격
            if (randomAct == 0)
            {
                // 어떤 공격을 할 것인지 뽑을 수
                int randomAttack = Random.Range(0, 100);

                // 원거리 공격
                if (randomAttack < 65)
                {
                    m_Anim.SetBool("Attack", true);
                }

                // 지진 공격
                else
                {
                    m_Anim.SetBool("Earthquake", true);
                }
            }

            // 이동
            else
            {
                // 어느 방향으로 움직일 것인지 뽑음
                int randomMove = Random.Range(0, 2);

                // 오른쪽
                if (randomMove == 0)
                {
                    m_Anim.SetBool("MoveRight", true);
                }

                // 왼쪽
                else
                {
                    m_Anim.SetBool("MoveLeft", true);
                }

            }

        }
    }

    // 오른쪽으로 이동
    private void MoveRight()
    {
        transform.position = transform.position + Vector3.right * 1.3f;

        // X좌표 가두기
        float posX = Mathf.Clamp(transform.position.x, -1.0f, 1.0f);

        // 좌표
        Vector2 pos = new Vector2(posX, transform.position.y);

        // 좌표설정
        transform.position = pos;

        m_Anim.SetBool("MoveRight", false);
    }

    // 왼쪽으로 이동
    private void MoveLeft()
    {
        transform.position = transform.position + Vector3.left * 1.3f;

        // X좌표 가두기
        float posX = Mathf.Clamp(transform.position.x, -1.0f, 1.0f);

        // 좌표
        Vector2 pos = new Vector2(posX, transform.position.y);

        // 좌표설정
        transform.position = pos;

        m_Anim.SetBool("MoveLeft", false);
    }

    // 돼지 던지기
    private void Attack() {

        int index = FindDisableBulletListIndex();

        if (index == -1) CreateBullet();

        else EnableBullet(index);

		m_Anim.SetBool("Attack", false);
    }

    // 지진
    private void Earthquake()
    {
        int index = FindDisableEarthquakeListIndex();

        if (index == -1) CreateEarthquake();

        else EnableEarthquake(index);

        m_Anim.SetBool("Earthquake", false);
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

        // 생성
        Bullet tempBullet = Instantiate(_BulletPrefab);
        
        // 좌표설정
        tempBullet.transform.position = transform.position;
        
        // 리스트에 추가
        _BulletList.Add(tempBullet);
        
        // 플레이어 방향으로 던질것 인지
        tempBullet.aim = true;
    }

    // 총알 활성화
    private void EnableBullet(int index) {

        // 활성화
        _BulletList[index].transform.gameObject.SetActive(true);

        // 좌표 설정
        _BulletList[index].transform.position = transform.position;

        // 플레이어 방향으로 던질것 인지
        _BulletList[index].aim = true;
    }

    // 비활성화중인 지진리턴
    private int FindDisableEarthquakeListIndex() {
        for (int i = 0; i < _EarthquakeList.Count; i++) {
            if (!_EarthquakeList[i].transform.gameObject.activeSelf) {
                return i;
            }
        }

        // 지진을 못찾았다면 -1을 리턴
        return -1;
    }

    // 지진 생성
    private void CreateEarthquake() {

        // 생성
        Bullet tempEarthquake = Instantiate(_EarthquakePrefab);

        // 랜덤 지진 위치 설정
        float randomEarthquake = Random.Range(-2.0f, 2.0f);

        // 방향 설정
        tempEarthquake.transform.position = new Vector2(randomEarthquake, transform.position.y);

        // 리스트에 추가
        _EarthquakeList.Add(tempEarthquake);
    }

    // 지진 활성화
    private void EnableEarthquake(int index) {

        // 활성화
        _EarthquakeList[index].transform.gameObject.SetActive(true);

        // 랜덤 지진 위치 설정
        float randomEarthquake = Random.Range(-2.0f, 2.0f);

        // 방향 설정
        _EarthquakeList[index].transform.position = new Vector2(randomEarthquake, transform.position.y);
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
