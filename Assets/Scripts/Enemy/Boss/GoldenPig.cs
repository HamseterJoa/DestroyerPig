using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldenPig : EnemyBase {

	[SerializeField] private Bullet Coin;
	private List<Bullet> CoinList = new List<Bullet>();

    // 보스 체력게이지 
    private BossHPGague _BossHPGague;

    // 체력바 위치를 바꾸기위해 필요한거
    private Image bossHPBar;

    // 랜덤 좌표를 저장함
    private Vector2 _RandomPos;

    // 움직임 상태
    private bool _IsMove = true;

    // 점프 상태
    private bool _IsJump = false;

    // 점프 전에 위치를 저장함
    private float _JumpY;

    // 점프할 위치를 저장함
    private Vector2 targetPos;

    // 등장 이펙트
    [SerializeField] private GameObject _AppearEffect;

    private void OnEnable()
    {
        // 등장 이펙트
        Instantiate(_AppearEffect);

        // 배경음악 변경
        GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Boss);

        StartCoroutine(Attack());
	}

	private void Start() {
		InitializeData();
	}

	private void Update() {
		if (m_Moveable) Move();
	}

	private void InitializeData() {

		// 이동속도
		m_MoveSpeed = 1.5f;

		// 접촉 데미지
		m_Damage = 30.0f;

		// 체력
		m_HP = m_MaxHP = 240.0f;

		// 돈
		m_Money = 2000;

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

        // 랜덤 x,y 설정
        float x = Random.Range(-1.5f, 1.5f);
        float y = Random.Range(1.0f, 3.5f);

        // 랜덤 좌표 설정
        _RandomPos = new Vector2(x, y);
    }

	// 이동
	protected override void Move() {

        if (_IsMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _RandomPos, m_MoveSpeed * Time.deltaTime);

            // 목표 지점에 도착하면 다시뽑기
            if (transform.position.x == _RandomPos.x && transform.position.y == _RandomPos.y)
            {
                float x = Random.Range(-1.5f, 1.5f);
                float y = Random.Range(1.0f, 3.5f);

                _RandomPos = new Vector2(x, y);
            }

        }

        if (_IsJump)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, 15.0f * Time.deltaTime);
        }
        
    }

    // 체력이 감소할 때 호출
    protected void HPDown(string damageType)
    {
        base.HPDown(damageType);

        // 체력바 업데이트
        _BossHPGague.HPBarUpdate(m_HP, m_MaxHP);
    }

    // 공격
    private IEnumerator Attack() {

		while (m_Moveable) {

			yield return new WaitForSeconds(2.0f);

            // 어떤 공격을 할것인지 뽑기
            int random = Random.Range(0, 10);

			// 돈 던진기
			if (random < 7) {

				// 몇개 던질것인지 뽑음
				int randomCount = Random.Range(3, 6);

				for (int i = 0; i < randomCount; i++){
					
					// 비활성화중인게 있다면 담기
					int index = FindDisableCoin();

					// 생성
					if (index == -1) CreateCoin(i);

					// 활성화
					else EnableCoin(index, i);
				}
				

			}

			// 뛰어서 돈 던지기
			else {

                // 움직임 상태 변경
                _IsMove = false;

                // 목표위치 설정
                targetPos = new Vector2(transform.position.x, 7.0f);

                // 애니메이션
                m_Anim.SetBool("Jump", true);

                // 1초 대기
                yield return new WaitForSeconds(1.0f);

                // 점프전에 y좌표 저장
                _JumpY = transform.position.y;

                // 점프
                _IsJump = true;

                m_Anim.SetBool("Jump", false);

                // 공중에서 대기
                yield return new WaitForSeconds(Random.Range(3, 6));

                // 새로운 목표위치 설정
                targetPos = new Vector2(transform.position.x, _JumpY);

                // 착지후 던지기
                yield return new WaitForSeconds(0.25f);

                // 몇개 던질것인지 뽑기
                int randomCount = Random.Range(8, 18);

				for (int i = 0; i < randomCount; i++){

					// 비활성화중인게 있다면 담기
					int index = FindDisableCoin();

					// 생성
					if (index == -1) CreateCoinSpread();

					// 활성화
					else EnableCoinSpread(index);

					yield return new WaitForSeconds(0.02f);
				}

                // 움직임 상태 변경
                _IsMove = true;

                _IsJump = false;
            }

		}
	}

	// 돈 생성
	private void CreateCoin(int number){

		// 생성
		Bullet coin = Instantiate(Coin);

		// 좌표설정
		coin.transform.position = transform.position;

		// 목표지점을 담음
		Vector2 targetPos = GameManager.getCharacterManager.player.transform.position - transform.position;

		// 좌표 보정
		targetPos.Normalize();

		// 발사 위치 설정
		switch (number) {
			case 0: coin._FireForce = targetPos; break;
			case 1: coin._FireForce = new Vector2(targetPos.x - 0.1f, targetPos.y + 0.2f); break;
			case 2: coin._FireForce = new Vector2(targetPos.x + 0.1f, targetPos.y + 0.2f); break;
			case 3: coin._FireForce = new Vector2(targetPos.x - 0.2f, targetPos.y + 0.1f); break;
			case 4: coin._FireForce = new Vector2(targetPos.x + 0.2f, targetPos.y + 0.1f); break;
		}

		// 리스트에 추가
		CoinList.Add(coin);
	}

	// 비활성화 중인 돈 찾기
	private int FindDisableCoin(){

		for (int i = 0; i < CoinList.Count; i++){
			if (!CoinList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// 돈 활성화
	private void EnableCoin(int index, int number){

		// 활성화
		CoinList[index].gameObject.SetActive(true);

		// 좌표설정
		CoinList[index].transform.position = transform.position;

		// 목표지점을 담음
		Vector2 targetPos = GameManager.getCharacterManager.player.transform.position - transform.position;

		// 좌표 보정
		targetPos.Normalize();

		// 발사 위치 설정
		switch (number) {
			case 0: CoinList[index]._FireForce = targetPos; break;
			case 1: CoinList[index]._FireForce = new Vector2(targetPos.x - 0.1f, targetPos.y + 0.2f); break;
			case 2: CoinList[index]._FireForce = new Vector2(targetPos.x + 0.1f, targetPos.y + 0.2f); break;
			case 3: CoinList[index]._FireForce = new Vector2(targetPos.x - 0.2f, targetPos.y + 0.1f); break;
			case 4: CoinList[index]._FireForce = new Vector2(targetPos.x + 0.2f, targetPos.y + 0.1f); break;
		}

	}

	// 돈 생성 흩뿌리며
	private void CreateCoinSpread() {

		// 생성
		Bullet coin = Instantiate(Coin);

		// 좌표설정
		coin.transform.position = transform.position;

		// 목표지점을 담음
		Vector2 targetPos = GameManager.getCharacterManager.player.transform.position - transform.position;

		// 좌표 보정
		targetPos.Normalize();

		float randomPM = Random.Range(-0.3f, 0.3f);

        // 발사 위치 설정
        coin._FireForce = new Vector2(targetPos.x + randomPM, targetPos.y + randomPM);

        // 리스트에 추가
        CoinList.Add(coin);
	}

	// 돈 활성화 흩뿌리며
	private void EnableCoinSpread(int index) {

		// 활성화
		CoinList[index].gameObject.SetActive(true);

		// 좌표설정
		CoinList[index].transform.position = transform.position;

		// 목표지점을 담음
		Vector2 targetPos = GameManager.getCharacterManager.player.transform.position - transform.position;

		// 좌표 보정
		targetPos.Normalize();

		float randomPM = Random.Range(-0.3f, 0.3f);

        // 발사 위치 설정
        CoinList[index]._FireForce = new Vector2(targetPos.x + randomPM, targetPos.y + randomPM);

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
