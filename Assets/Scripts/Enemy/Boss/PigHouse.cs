using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PigHouse : EnemyBase
{
	// 돼지 리스트
	[SerializeField] private List<EnemyBase> _PigHousePigList = new List<EnemyBase>();
	private List<EnemyBase> _CreatePigList = new List<EnemyBase>();

	// 오물
	[SerializeField] private Bullet _BulletOri;
	private List<Bullet> _BulletList = new List<Bullet>();

	// 폭탄
	[SerializeField] private GameObject _Bomb;
	private List<GameObject> _BombList = new List<GameObject>();

	// 목표지점
	[SerializeField] private GameObject _Target;
	private List<GameObject> _TargetList = new List<GameObject>();

	// 보스 체력게이지 
	private BossHPGague _BossHPGague;

    // 보스 체력게이지 이동을 위해있
    private Image bossHPBar;

    // 랜덤좌표로 이동할 떄 좌표
    private Vector2 _RandomPos;

    // 
    private bool _IsMove = false;

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

    private void Start()
    {
        InitializeData();
    }

    private void InitializeData() {

		// 이동속도
		m_MoveSpeed = 0.0f;

		// 접촉 데미지
		m_Damage = 30.0f;

		// 체력
		m_HP = m_MaxHP = 140;

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

    protected override void Move() {

        if (_IsMove)
        {
            transform.position = Vector2.MoveTowards(transform.position, _RandomPos, 1.0f * Time.deltaTime);

            if (transform.position.x == _RandomPos.x && transform.position.y == _RandomPos.y)
            {
                _IsMove = false;

                m_Anim.SetBool("Move", false);
            }
        }

        else return;

    }

    // 체력이 감소할 때 호출
    protected void HPDown(string damageType)
    {
        base.HPDown(damageType);

        // 체력바 업데이트
        _BossHPGague.HPBarUpdate(m_HP, m_MaxHP);
    }

    // 공격
    private IEnumerator Attack(){
		
		while (m_Moveable){

            // 딜레이 만크 대기
			yield return new WaitForSeconds(2.0f);

            // 어떤 행동을 할것인지 뽑음
            int randomAct = Random.Range(0, 10);

            // 공격
            if (randomAct < 5)
            {
                // 어떤 공격을 할것인지 뽑음
                int random = Random.Range(0, 10);

                // 공격 시작
                m_Anim.SetBool("Attack", true);
                yield return new WaitForSeconds(0.5f);

                // 오물 공격
                if (random < 4)
                {

                    for (int i = 0; i < 6; i++)
                    {
                        int index = FindDisableBullet();

                        // 비활성화된 오물이 없다면
                        if (index == -1) CreateBullet(i);

                        // 찾았다면
                        else EnableBullet(index, i);

                    }
                }

                // 돼지 소환
                else if (random < 7)
                {

                    // 구르는 돼지인덱스
                    int RollingIndex = FindDisableRollingPig();

                    // 없다면 생성
                    if (RollingIndex == -1) CreateRollingPig();

                    // 있다면 활성화
                    else EnablePig(RollingIndex);

                    yield return new WaitForSeconds(0.5f);

                    // 돼지를 몇마리 생성할 것인지 뽑을 수
                    int randomSpawnCount = Random.Range(1, 4);

                    for (int i = 0; i < randomSpawnCount; i++)
                    {

                        // 돼지 인덱스
                        int index = FindDisablePig();

                        // 생성
                        if (index == -1) CreatePig();

                        // 활성화
                        else EnablePig(index);

                        // 딜레이 대기
                        yield return new WaitForSeconds(0.5f);
                    }

                }

                // 폭탄 발사
                else
                {
                    // 비활성화 중인 과녁이 있다면 그과녁의 인덱스를 담음
                    int targetIndex = FindDisableTarget();

                    // 목표 위치를 보여주는 과녁 생성
                    if (targetIndex == -1) CreateTarget();

                    // 과녁 활성화
                    else EnableTarget(targetIndex);

                    // 대기
                    yield return new WaitForSeconds(2.0f);

                    // 비활성화 중인 폭탄 담음
                    int bombIndex = FindDisableBomb();

                    // 폭탄 생성
                    if (bombIndex == -1) CreateBomb(_TargetList[0].transform.position);

                    // 폭탄 활성화
                    else EnableBomb(bombIndex, _TargetList[0].transform.position);

                    // 과녁 비활성황
                    _TargetList[0].gameObject.SetActive(false);

                    yield return new WaitForSeconds(0.5f);

                    // 폭탄 비활성화
                    _BombList[0].gameObject.SetActive(false);
                }

                // 공격 끝
                m_Anim.SetBool("Attack", false);
            }

            // 이동
            else
            {
                // 랜덤 X설정
                float randomX = Random.Range(-1.5f, 1.5f);

                // 랜덤 Y설정
                float randomY = Random.Range(1.0f, 4.0f);

                // 랜덤 좌표 설정
                _RandomPos = new Vector2(randomX, randomY);

                // 움직이기 설정
                _IsMove = true;
                m_Anim.SetBool("Move", true);

                yield return new WaitForSeconds(2.0f);
            }

		}
	}

	// 오물 생성
	private void CreateBullet(int Number){

		// 생성
		Bullet bullet = Instantiate(_BulletOri);

		// 좌표설정
		switch(Number){

			case 0: bullet.transform.position = new Vector2(transform.position.x - 1.0f, transform.position.y); bullet._FireForce = Vector2.left; break;

			case 1: bullet.transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f); bullet._FireForce = new Vector2(-0.5f, -0.5f); break;

			case 2: bullet.transform.position = new Vector2(transform.position.x, transform.position.y - 1.0f); bullet._FireForce = Vector2.down; break;
																				 
			case 3: bullet.transform.position = new Vector2(transform.position.x, transform.position.y - 1.0f); bullet._FireForce = Vector2.down; break;
																				 
			case 4: bullet.transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.5f); bullet._FireForce = new Vector2(0.5f, -0.5f); break;
																				 
			case 5: bullet.transform.position = new Vector2(transform.position.x + 1.0f, transform.position.y); bullet._FireForce = Vector2.right; break;

		}

		// 리스트에 추가
		_BulletList.Add(bullet);

	}

	// 비활성화 중인 오물 찾기
	private int FindDisableBullet(){

		for (int i = 0; i < _BulletList.Count; i++){
			if (!_BulletList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// 오물 활성화
	private void EnableBullet(int index, int number){

		// 활성화
		_BulletList[index].gameObject.SetActive(true);

		// 좌표 설정
		switch(number){

			case 0: _BulletList[index].transform.position = new Vector2(transform.position.x - 1.0f, transform.position.y); _BulletList[index]._FireForce = Vector2.left; break;

			case 1: _BulletList[index].transform.position = new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f); _BulletList[index]._FireForce = new Vector2(-0.5f, -0.5f); break;

			case 2: _BulletList[index].transform.position = new Vector2(transform.position.x, transform.position.y - 1.0f); _BulletList[index]._FireForce = Vector2.down; break;

			case 3: _BulletList[index].transform.position = new Vector2(transform.position.x, transform.position.y - 1.0f); _BulletList[index]._FireForce = Vector2.down; break;

			case 4: _BulletList[index].transform.position = new Vector2(transform.position.x + 0.5f, transform.position.y - 0.5f); _BulletList[index]._FireForce = new Vector2(0.5f, -0.5f); break;

			case 5: _BulletList[index].transform.position = new Vector2(transform.position.x + 1.0f, transform.position.y); _BulletList[index]._FireForce = Vector2.right; break;

		}

	}

	// 돼지 생성
	private void CreatePig(){

		// 생성
		EnemyBase enemy = Instantiate(_PigHousePigList[0]);

		// 좌표 설정 
		enemy.transform.position = transform.position;

		// 리스트에 추가
		_CreatePigList.Add(enemy);

	}

	// 구르는 돼지 생성
	private void CreateRollingPig(){

		// 생성
		EnemyBase enemy = Instantiate(_PigHousePigList[1]);

		// 좌표 설정 
		enemy.transform.position = transform.position;

		// 리스트에 추가
		_CreatePigList.Add(enemy);
	}

	// 비활성화 중인 돼지 찾기
	private int FindDisablePig(){

		for (int i = 0; i < _CreatePigList.Count; i++){

			// 이름이 같고 비활성화 되어있는지 검사
			if (_CreatePigList[i].name == "Pig(Clone)" && !_CreatePigList[i].gameObject.activeSelf)
				return i;

		}

		return -1;
	}

	// 비활성화 중인 구르는 돼지 찾기
	private int FindDisableRollingPig(){

		for (int i = 0; i < _CreatePigList.Count; i++){

			// 이름이 같고 비활성화 되어있는지 검사
			if (_CreatePigList[i].name == "RollingPig(Clone)" && !_CreatePigList[i].gameObject.activeSelf)
				return i;
		}

		return -1;
	}

	// 돼지들 활성화
	private void EnablePig(int index) {

		// 활성화
		_CreatePigList[index].gameObject.SetActive(true);

		// 좌표설정
		_CreatePigList[index].transform.position = transform.position;

	}

	// 폭탄 생성
	private void CreateBomb(Vector2 dir){

		// 생성
		GameObject bomb = Instantiate(_Bomb);

		// 좌표 설정
		bomb.transform.position = dir;

		// 리스트에 추가
		_BombList.Add(bomb);
	}

	// 비활성화 중인 폭탄 찾기
	private int FindDisableBomb(){

		for (int i = 0; i < _BombList.Count; i++)
			if (!_BombList[i].gameObject.activeSelf) return i;

		return -1;
	}

	// 폭탄 활성화
	private void EnableBomb(int index, Vector2 dir){

		// 활성화
		_BombList[index].gameObject.SetActive(true);

		// 좌표설정
		_BombList[index].transform.position = dir;

	}

	// 목표 지점 생성
	private void CreateTarget(){

		// 과녁 생성
		GameObject target = Instantiate(_Target);
		
		// 좌표 설정
		target.transform.position = GameManager.getCharacterManager.player.transform.position;

        // 리스트에 추가
        _TargetList.Add(target);
    }

	// 비활성화 중인 과녁리턴
	private int FindDisableTarget(){

		for (int i = 0; i < _TargetList.Count; i++){
			if (!_TargetList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// 과녁 활성화
	private void EnableTarget(int index){

		// 활성화
		_TargetList[index].gameObject.SetActive(true);

		// 좌표 설정
		_TargetList[index].transform.position = GameManager.getCharacterManager.player.transform.position;
	}

    private void OnDisable()
    {
        base.OnDisable();

        // 활성화 중이라면 과녁 비활성화
        for (int i = 0; i < _TargetList.Count; i++)
            if (_TargetList[i].gameObject.activeSelf) _TargetList[i].gameObject.SetActive(false);

        
        // 활성화 중이라면 폭탄 비활성화
        for (int i = 0; i < _BombList.Count; i++)
            if (_BombList[i].gameObject.activeSelf) _BombList[0].gameObject.SetActive(false);

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
