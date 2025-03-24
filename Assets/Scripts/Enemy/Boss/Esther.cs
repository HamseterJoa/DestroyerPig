using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Esther : EnemyBase
{
	// 에스더 공
	[SerializeField] private Bullet _EstherBall;
	private List<Bullet> _EstherBallList = new List<Bullet>();

	// 에스더의 하위객체를 담음
	[SerializeField] private Esther _ChildEsther;

	// 분열체들이 참조할 본체
	private Esther _Origin;

	// 에스더의 종류
	private enum EstherSort { A, B, C, D}
	[SerializeField] private EstherSort _EstherSort = EstherSort.A;
	
	private bool _IsRolling = false;

	private bool _IsUp = false;

	// 보스 체력게이지 
	private BossHPGague _BossHPGague;

    private Image bossHPBar;

    // 랜덤 좌표를 저장함
    private Vector2 _RandomPos;

    // 이동
    private bool _IsMove = true;

    // 등장 이펙트
    [SerializeField] private GameObject _AppearEffect;

    // 엔딩 이미지
    private Image _EndingImage;

    // 분열하였는지 검사
    private bool _IsSplit = false;

    // 마지막 분열체의 죽은 횟수 카운트
    private static int _DieCount = 0;

    // 죽었는지 검사
    private bool _IsDie = false;

    private void OnEnable() {

        StartCoroutine(Attack());

        // 본체가 아니라면 본체를 참조
        if (_EstherSort != EstherSort.A) _Origin = GameObject.Find("Esther(Clone)").GetComponent<Esther>();
        //Esther(Clone)
        // 본체라면 등장이펙트
        else
        {
            Instantiate(_AppearEffect);

            // 배경음악 변경
            GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Boss);

            // 엔딩이미지 참조
            _EndingImage = GameObject.Find("Canvas").transform.Find("ScreenPanel").transform.Find("EndingImage").GetComponent<Image>();
        }
    }

    private void Start() {
		InitializeData();
	}

	private void Update() {
		if (m_Moveable) Move();

        if (_EstherSort == EstherSort.A) // 카운트가 8이되면 본체 5초 뒤 사라짐
            if (_DieCount >= 8 && !_IsDie)
            {
                _IsDie = true;
                StartCoroutine(StartEnding());
            }
    }

	private void InitializeData() {

		// 이동속도
		m_MoveSpeed = 2.0f;

		// 접촉 데미지
		m_Damage = 30.0f;

		// 체력
		switch (_EstherSort){
			case EstherSort.A: m_HP = m_MaxHP = 360.0f; break;
			case EstherSort.B: m_HP = m_MaxHP = 90.0f; break;
			case EstherSort.C: m_HP = m_MaxHP = 22.5f; break;
			case EstherSort.D: m_HP = m_MaxHP = 5.625f; break;
		}

		// 돈
		m_Money = 3000;

        // 본체만 보스
        if (_EstherSort == EstherSort.A)
            m_IsBoss = true;
        
        // 체력바 참조
        _BossHPGague = GameObject.Find("Canvas").transform.Find("ScreenPanel").transform.Find("BossHPGague").transform.Find("BossHPBar").GetComponent<BossHPGague>();

        // 체력바 위치를 조정하기 위한 거
        bossHPBar = GameObject.Find("Canvas").transform.Find("ScreenPanel").transform.Find("BossHPGague").GetComponent<Image>();

        // 체력바 위치 설정
        bossHPBar.rectTransform.anchoredPosition = new Vector2();

        // 본체라면 체력바 체력 설정
        if (_EstherSort == EstherSort.A)
            _BossHPGague.HPBarUpdate(m_HP, m_MaxHP);

        // 이동 방향 랜덤 설정
        _RandomPos = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(0.0f, 4.0f));
    }

	// 체력이 감소할 때 호출
	protected void HPDown(string damageType) {

        // 데미지 받기
        switch (damageType)
        {
            case "Ball": m_HP -= GameManager.getCharacterManager.ball.BallDamage; break;
            case "MiniBall": m_HP--; break;
            case "Bullet": m_HP--; break;
            case "Hack": m_HP -= (int)_HackDamage; break;
            case "HOS": m_HP -= 0.1f; break;
        }

        // 체력 검사
        if (m_HP <= 0.0f)
        {
            // 본체가 아니라면 죽고
            if (_EstherSort != EstherSort.A)
            {
                m_Moveable = false;
                m_Anim.SetBool("Die", true);
            }
            
        }

        // 체력이 일정량 감소하면 분열
        Split();

        // 본체라면 체력바 업데이트
        if (_EstherSort == EstherSort.A) _BossHPGague.HPBarUpdate(m_HP, m_MaxHP);
    }

    // 엔딩
    private IEnumerator StartEnding()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);

            if (_EstherSort == EstherSort.A)
                gameObject.SetActive(false);
        }
    }

	// 분열
	private void Split(){

		// 첫번째 분열(분열체 체력90)
		if (m_HP <= 180 && _EstherSort == EstherSort.A && !_IsSplit) {

			for (int i = 0; i < 2; i++) {

				// 생성
				Esther esther = Instantiate(_ChildEsther);

				float random = Random.Range(-1.0f, 1.0f);

				// 좌표설정
				esther.transform.position = new Vector2(transform.position.x + random, transform.position.y - random);
			}

			// 본체 화면밖으로 이동
			transform.position = new Vector2(-5.0f, 0.0f);

			// 본체 공격이동 막기
			m_Moveable = false;

            // 더이상 분열 X
            _IsSplit = true;
        }

		// 본체가 아니라면 두번째 분열(분열체 체력22)
		else if (m_HP <= 45 && _EstherSort != EstherSort.A && _EstherSort == EstherSort.B && !_IsSplit) {

			for (int i = 0; i < 2; i++) {

				// 생성
				Esther esther = Instantiate(_ChildEsther);

				float random = Random.Range(-1.0f, 1.0f);

				// 좌표설정
				esther.transform.position = new Vector2(transform.position.x + random, transform.position.y - random);
			}

            // 더이상 분열 X
            _IsSplit = true;

            // 본체가 아니라면 비활성화
            if (_EstherSort != EstherSort.A) gameObject.SetActive(false);
		}

		// 본체가 아니라면 세번째 분열(분열체 체력 6)
		else if (m_HP <= 11.25f && _EstherSort != EstherSort.A && _EstherSort == EstherSort.C && !_IsSplit) {

			for (int i = 0; i < 2; i++) {

				// 생성
				Esther esther = Instantiate(_ChildEsther);

				float random = Random.Range(-1.0f, 1.0f);

				// 좌표설정
				esther.transform.position = new Vector2(transform.position.x + random, transform.position.y - random);
			}

            // 더이상 분열 X
            _IsSplit = true;

            // 본체가 아니라면 비활성화
            if (_EstherSort != EstherSort.A) gameObject.SetActive(false);
		}
	}

	// 본체 체력 감소
	private void OriginHPUpdate(float hp){

        // 본체 피 깍기
		_Origin.m_HP -= hp;
        
        // 체력바 업데이트
		_BossHPGague.HPBarUpdate(_Origin.m_HP, _Origin.m_MaxHP);
	}

	// 받은 데미지값을 리턴
	private float RDamage(string damageType){

		// 데미지 받기
		switch (damageType) {
			case "Ball": return GameManager.getCharacterManager.ball.BallDamage;
			case "MiniBall": return -1.0f;
			case "Bullet": return -1.0f;
			case "Hack": return (int)_HackDamage;
			case "HOS": return 0.1f;
		}

		return 0.0f;
	}

	// 이동
	protected override void Move() {

        // 통상
        if (!_IsRolling)
        {
            // 이동
            transform.position = Vector2.MoveTowards(transform.position, _RandomPos, m_MoveSpeed * Time.deltaTime);
            
        }
        
        // 공격
        else
        {
			// 위로 구르기
			if (_IsUp) {

				transform.Translate(Vector2.up * 3.0f * Time.deltaTime);

				// 천장을 찍으면
				if (transform.position.y >= 3.5f) {

					// 아래로
					_IsUp = false;

					// 상태 변경
					_IsRolling = false;

                    m_Anim.SetBool("Rolling", false);
				}
			}

			// 아래로 구르기
			else {
				transform.Translate(Vector2.down * 3.0f * Time.deltaTime);

				// 땅을 찍으면 위로
				if (transform.position.y <= -3.5f) _IsUp = true;

			}

		}

	}

	// 공격
	private IEnumerator Attack() {

		while (m_Moveable) {

			yield return new WaitForSeconds(2.0f);

            // 이동 방향 랜덤 설정
            _RandomPos = new Vector2(Random.Range(-2.0f, 2.0f), Random.Range(0.0f, 4.0f));

            // 어떤 공격을 할것인지 뽑을 수
            int random = Random.Range(0, 10);

            // 오물 던지기
            if (random < 7)
            {
                m_Anim.SetBool("Attack", true);
            }

            // 구르기?
            else
            {
                // 구르기
                _IsRolling = true;

                m_Anim.SetBool("Rolling", true);

                yield return new WaitForSeconds(4.0f);
            }

        }
	}

    private void Throw()
    {
        // 비활성화 중인 공이 있다면 담음
        int index = FindDisableBall();

        // 없다면 생성
        if (index == -1) CreateBall();

        // 있다면 활성화
        else EnableBall(index);

        m_Anim.SetBool("Attack", false);
    }

	// 공 생성
	private void CreateBall() {

		// 생성
		Bullet ball = Instantiate(_EstherBall);

		// 위치 설정
		ball.transform.position = transform.position;

		// 리스트에 추가
		_EstherBallList.Add(ball);
	}

	// 비활성화 중인 공 있는지 검사
	private int FindDisableBall() {

		for (int i = 0; i < _EstherBallList.Count; i++)
			if (!_EstherBallList[i].gameObject.activeSelf) return i;

		return -1;
	}

	// 공 활성화
	private void EnableBall(int index) {

		// 좌표설정
		_EstherBallList[index].transform.position = transform.position;

		// 활성화
		_EstherBallList[index].gameObject.SetActive(true);
	}

    protected void  OnDisable()
    {
        // 본체일 경우만
        if (_EstherSort == EstherSort.A)
        {
            base.OnDisable();

            // 체력바 위치 설정
            bossHPBar.rectTransform.anchoredPosition = new Vector2(1000.0f, 0.0f);

            // 배경음악 변경
            GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Game);

            // 결과창 띄우고
            m_Wave.OnResult();

            // 엔딩 띄우기
            _EndingImage.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
        }

        // 마지막 분신이 죽으면 카운트 증가
        else if (_EstherSort == EstherSort.D)
        {
            // 마지막 분신은 자기 최대 체력만큼 감소
            OriginHPUpdate(m_MaxHP);

            _DieCount++;
        }

        // 분신이라면 비활될때 체력바 감소
        else
        {
            // 다른 분신들은 자기 최대 체력의 절반만큼 감소
            OriginHPUpdate(m_MaxHP * 0.5f);
        }

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
