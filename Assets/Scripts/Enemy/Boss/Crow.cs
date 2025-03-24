using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crow : EnemyBase
{
	// 회오리
	[SerializeField] private Bullet _Storm;
	private List<Bullet> _StormList = new List<Bullet>();

	// 소리
	[SerializeField] private Bullet _SoundWave;
	private List<Bullet> _SoundWaveList = new List<Bullet>();

	// 깃털
	[SerializeField] private Bullet _Feather;
	private List<Bullet> _FeatherList = new List<Bullet>();

	// 보스 체력게이지 
	private BossHPGague _BossHPGague;

    private Image bossHPBar;

    private float _MoveX = 1.0f;

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

	private void InitializeData() {

		// 이동속도
		m_MoveSpeed = 1.25f;

		// 접촉 데미지
		m_Damage = 30.0f;

		// 체력
		m_HP = m_MaxHP = 190.0f;

		// 돈
		m_Money = 2200;

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

    // 체력이 감소할 때 호출
    protected void HPDown(string damageType)
    {
        base.HPDown(damageType);

        // 체력바 업데이트
        _BossHPGague.HPBarUpdate(m_HP, m_MaxHP);
    }

    private void Update()
    {
        if (m_Moveable) Move();
    }

    protected override void Move() {

        transform.Translate(Vector2.right * _MoveX * m_MoveSpeed * Time.deltaTime);

        if (transform.position.x > 1.0f || transform.position.x < -1.0f) _MoveX *= -1.0f;
	}

	// 공격
	private IEnumerator Attack(){

		while (m_Moveable){

			// 전체공격 딜레이
			yield return new WaitForSeconds(2.0f);

            // 어떤 공격을 할것인지 뽑음
            int random = Random.Range(0, 100);

            // 공격 시작
            m_Anim.SetBool("Attack", true);
            yield return new WaitForSeconds(0.5f);

            // 회오리
            if (random < 50){

                // 어느 방향으로 쏠것인지 뽑기
                int isleft = Random.Range(0, 2);

                for (int i = 0; i < 2; i++){

					// 비활성화중인 회오리 번호를 담음
					int stormIndex = FindDisableStorm();

                    // 비활성화중인 회오리가 없다면 생성
                    if (stormIndex == -1) CreateStorm(i, isleft);

                    // 있다면 활성화
                    else EnableStorm(stormIndex, i, isleft);

				}

			}

			// 음파
			else if (random < 75){

                for (int i = 0; i < 5; i++)
                {
                    // 비활성화중인 음파를 담을 수
                    int soundWaveIndex = FindDisableSoundWave();

                    // 생성
                    if (soundWaveIndex == -1) CreateSoundWave(i);

                    // 활성화
                    else EnableSoundWave(soundWaveIndex, i);

                }

                // 랜덤으로 한개 빼기
                int minus = Random.Range(0, 5);
                _SoundWaveList[minus].gameObject.SetActive(false);

			}

			// 깃털
			else {

				for (int i = 0; i < 5; i++){

                    // 비활성화중인 깃털을 담음
                    int featherIndex = FindDisableFeather();

                    // 생성
                    if (featherIndex == -1) CreateFeather();

                    // 활성화
                    else EnableFeather(featherIndex);

					// 0.5초에 한개씩
					yield return new WaitForSeconds(0.5f);
				}

			}

            // 공격 시작
            m_Anim.SetBool("Attack", false);
        }

		yield return null;
	}

	// 회오리 생성
	private void CreateStorm(int number, int isleft) {

		// 생성
		Bullet storm = Instantiate(_Storm);

		// 위치 설정
		storm.transform.position = transform.position;

        if (isleft == 0)
            switch (number)
            {
                case 0: storm.stormtarget = new Vector2(1.0f, 1.0f); break;
                case 1: storm.stormtarget = new Vector2(2.0f, 1.0f); break;
            }
        else
            switch (number)
            {
                case 0: storm.stormtarget = new Vector2(-1.0f, 1.0f); break;
                case 1: storm.stormtarget = new Vector2(-2.0f, 1.0f); break;
            }

        // 리스트에 추가
        _StormList.Add(storm);
	}

	// 비활성화 중인 회오리가 있는지 검사
	private int FindDisableStorm() {

		for (int i = 0; i < _StormList.Count; i++)
			if (!_StormList[i].gameObject.activeSelf) return i;

		return -1;
	}

	// 회오리 활성화
	private void EnableStorm(int index, int number, int isleft) {
		
		// 활성화
		_StormList[index].gameObject.SetActive(true);

		// 좌표설정
		_StormList[index].transform.position = transform.position;

        if (isleft == 0)
            switch (number)
            {
                case 0: _StormList[index].stormtarget = new Vector2(1.0f, 1.0f);; break;
                case 1: _StormList[index].stormtarget = new Vector2(2.0f, 1.0f); ; break;
            }
        else
            switch (number)
            {
                case 0: _StormList[index].stormtarget = new Vector2(-1.0f, 1.0f); break;
                case 1: _StormList[index].stormtarget = new Vector2(-2.0f, 1.0f); break;
            }

    }

	// 음파 생성
	private void CreateSoundWave(int number) {

		// 생성
		Bullet soundWave = Instantiate(_SoundWave);

		// 좌표 설정
		switch (number) {

			case 0: soundWave.transform.position = new Vector2(-2.0f, transform.position.y); break;

			case 1: soundWave.transform.position = new Vector2(-1.0f, transform.position.y); break;

			case 2: soundWave.transform.position = new Vector2(0.0f, transform.position.y); break;

			case 3: soundWave.transform.position = new Vector2(1.0f, transform.position.y); break;

			case 4: soundWave.transform.position = new Vector2(2.0f, transform.position.y); break;
		}

		// 리스트에 추가
		_SoundWaveList.Add(soundWave);
	}

	// 비활성화 중인 음파번호를 리턴함
	private int FindDisableSoundWave() {

		for (int i = 0; i < _SoundWaveList.Count; i++)
			if (!_SoundWaveList[i].gameObject.activeSelf) return i;

		return -1;
	}

	// 음파 활성화
	private void EnableSoundWave(int index, int number) {

		// 활성화
		_SoundWaveList[index].gameObject.SetActive(true);

		// 좌표 설정
		switch (number) {

			case 0: _SoundWaveList[index].transform.position = new Vector2(-2.0f, transform.position.y); break;

			case 1: _SoundWaveList[index].transform.position = new Vector2(-1.0f, transform.position.y); break;

			case 2: _SoundWaveList[index].transform.position = new Vector2(0.0f, transform.position.y); break;

			case 3: _SoundWaveList[index].transform.position = new Vector2(1.0f, transform.position.y); break;

			case 4: _SoundWaveList[index].transform.position = new Vector2(2.0f, transform.position.y); break;
		}

	}

	// 깃털생성
	private void CreateFeather() {

		// 생성
		Bullet feather = Instantiate(_Feather);

		// 좌표 설정
		feather.transform.position = transform.position;

		// 리스트에 추가
		_FeatherList.Add(feather);

		// 플레이어의 위치로 던지기
		feather.aim = true;
	}

	// 비활성화 중인 깃털번호를 리턴함
	private int FindDisableFeather() {

		for (int i = 0; i < _FeatherList.Count; i++)
			if (!_FeatherList[i].gameObject.activeSelf) return i;

		return -1;
	}

	// 깃털 활성화
	private void EnableFeather(int index) {

		// 활성화
		_FeatherList[index].gameObject.SetActive(true);

		// 좌표 설정
		_FeatherList[index].transform.position = transform.position;

        // 플레이어 위치로 던지기
        _FeatherList[index].aim = true;
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
