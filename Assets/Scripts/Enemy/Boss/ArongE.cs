using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArongE : EnemyBase
{
    // 아롱이 새끼
    [SerializeField] private List<EnemyBase> _ArongBabys = new List<EnemyBase>();
    private List<EnemyBase> _ArongBabyList = new List<EnemyBase>();

	// 아롱이 공
	[SerializeField] private Bullet _ArongBall;
	private List<Bullet> _ArongBallList = new List<Bullet>();

	// 보스 체력게이지 
	private BossHPGague _BossHPGague;

    // 체력바 위치를 바꾸기위해 필요한거
    private Image bossHPBar;

    // 움직일 것인가
    private bool _IsMove = false;

    // X
    private float randomX;

    // Y
    private float randomY;

    // 목표
    private Vector2 targetPos;

    // 등장 이펙트
    [SerializeField] private GameObject _AppearEffect;
    
    private void OnEnable() {

        // 등장 이펙트
        Instantiate(_AppearEffect);

        StartCoroutine(Attack());

        // 배경음악 변경
        GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Boss);
    }

	private void Start() {
        InitializeData();
    }

    private void InitializeData() {

        // 이동속도
        m_MoveSpeed = 0.0f;

        // 접촉 데미지
        m_Damage = 30.0f;

        // 체력
        m_HP = m_MaxHP = 70;

        // 돈
        m_Money = 813;

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

    private void Update()
    {
        if (m_Moveable) Move();
    }

    // 체력이 감소할 때 호출
    protected void HPDown(string damageType)
    {
        base.HPDown(damageType);

        // 체력바 업데이트
        _BossHPGague.HPBarUpdate(m_HP, m_MaxHP);
    }

    protected override void Move() {
        
        if (_IsMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 1.0f * Time.deltaTime);

            if (transform.position.x == targetPos.x && transform.position.y == targetPos.y)
            {
                _IsMove = false;

                m_Anim.SetBool("Move", false);
            }
        }

        else return;

    }

	// 공격
    private IEnumerator Attack() {

        while (m_Moveable) {

            yield return new WaitForSeconds(2.0f);

            // 어떤 행동을 할것인가 뽑음
            int randomAct = Random.Range(0, 10);

            // 공격
            if (randomAct < 5)
            {
                // 어떤 패턴을 할것인지 뽑음
                int random = Random.Range(0, 10);

                if (random < 3)
                {
                    // 새끼 소환
                    // 애니메이션
                    m_Anim.SetBool("Baby", true);
                    for (int i = 0; i < 4; i++)
                    {
                        // 애니메이션
                        if (i == 1) m_Anim.SetBool("Baby", false);

                        // 비활성화 중인 새끼번호를 담음
                        int index = FindDisableBaby();

                        // 생성
                        if (index == -1) CreateBaby(i);

                        // 활성화
                        else EnableBaby(index);

                        yield return new WaitForSeconds(0.5f);
                    }
                }

                else
                {
                    // 공 소환
                    m_Anim.SetBool("Attack", true);
                }

            }

            // 이동
            else
            {
                m_Anim.SetBool("Move", true);

                // 랜덤 X설정
                randomX = Random.Range(-1.5f, 1.5f);

                // 랜덤 Y설정
                randomY = Random.Range(1.0f, 4.0f);

                // 랜덤 좌표 설정
                targetPos = new Vector2(randomX, randomY);
                
                // 움직이기 설정
                _IsMove = true;
                yield return new WaitForSeconds(2.0f);
            }

		}

    }

    // 공 뱉기
    private void Ball()
    {
        for (int i = 0; i < 2; i++)
        {

            int index = FindDisableBall();

            if (index == -1) CreateBall(i);

            else EnableBall(index, i);
        }

        m_Anim.SetBool("Attack", false);
    }

    // 새끼 생성
	private void CreateBaby(int number) {

		// 생성
		EnemyBase enemy = Instantiate(_ArongBabys[number]);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);

		// 위치 설정
		enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

		// 리스트에 추가
		_ArongBabyList.Add(enemy);

	}

	// 비활성화 중인 새끼가 있는지 검사
	private int FindDisableBaby() {

		for (int i = 0; i < _ArongBabyList.Count; i++)
			if (!_ArongBabyList[i].gameObject.activeSelf) return i;

		return -1;
	}

	// 새끼 활성화
	private void EnableBaby(int index) {

		_ArongBabyList[index].gameObject.SetActive(true);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);

		_ArongBabyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
	}

	// 공 생성
	private void CreateBall(int number) {

		// 공 생성
		Bullet ball = Instantiate(_ArongBall);

		// 좌표 설정
		switch (number) {

			case 0: ball.transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y); break;

			case 1: ball.transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y); ball._XSpeed *= -1.0f; break;
		}

		// 리스트에 추가
		_ArongBallList.Add(ball);
	}

	// 비활성화 중인 공번호를 리턴함
	private int FindDisableBall() {

		for (int i = 0; i < _ArongBallList.Count; i++)
			if (!_ArongBallList[i].gameObject.activeSelf) return i;

		return -1;
	}

	private void EnableBall(int index, int number) {

		// 활성화
		_ArongBallList[index].gameObject.SetActive(true);

		// 좌표 설정
		switch (number) {

			case 0: _ArongBallList[index].transform.position = new Vector2(transform.position.x + 0.2f, transform.position.y); break;

			case 1: _ArongBallList[index].transform.position = new Vector2(transform.position.x - 0.2f, transform.position.y); _ArongBallList[index]._XSpeed *= -1.0f; break;
		}

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
