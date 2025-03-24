using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PigKing : EnemyBase
{
	// 보스 체력게이지 
	private BossHPGague _BossHPGague;

    private Image bossHPBar;

    // 유도탄
    [SerializeField] private Bullet _Induction;
    private List<Bullet> _InductionList = new List<Bullet>();

    // 폭파
    [SerializeField] private GameObject _Bomb;
    private List<GameObject> _BombList = new List<GameObject>();

    // 신하들의 에니메이터 참조
    [SerializeField] private Animator _RightServant;
    [SerializeField] private Animator _LeftServant;

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

    private void Update()
    {
        if (m_Moveable) Move();
    }

    private void InitializeData() {

		// 이동속도
		m_MoveSpeed = 1.0f;

		// 접촉 데미지
		m_Damage = 30.0f;

		// 체력
		m_HP = m_MaxHP = 260.0f;

		// 돈
		m_Money = 2150;

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

        transform.Translate(Vector2.right * m_MoveSpeed * Time.deltaTime);

        if (transform.position.x >= 0.6f || transform.position.x <= -0.6f) m_MoveSpeed *= -1.0f;
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

			// 전체공격 딜레이
			yield return new WaitForSeconds(2.0f);

            // 어떤 공격을 할것인지 뽑음
            int randomAttack = Random.Range(0, 10);

            // 구토
            if (randomAttack < 4)
            {
                // 어떤 부하가 공격할 것인지 뽑음
                int randomServant = Random.Range(0, 2);
                
                // 오른쪽
                if (randomServant == 0)
                {
                    // 공격
                    _RightServant.SetBool("Attack", true);

                    yield return new WaitForSeconds(2.5f);

                    // 공격 종료
                    _RightServant.SetBool("Attack", false);
                }

                // 왼쪽
                else
                {
                    // 공격
                    _LeftServant.SetBool("Attack", true);

                    yield return new WaitForSeconds(2.5f);

                    // 공격 종료
                    _LeftServant.SetBool("Attack", false);
                }

            }

            // 폭발물
            else
            {
                // 터질위치
                Vector2 boomPos;

                // 비활성화중인거 있으면 담음
                int index = FindDIsableInduction();

                // 없다면 생성
                if (index == -1) CreateInduction();

                // 있다면 활성화
                else EnableInduction(index);

                // 3 ~ 6초 사이
                yield return new WaitForSeconds(Random.Range(3, 7));

                // 폭탄이 활성화중이 라면
                if (_InductionList[0].gameObject.activeSelf)
                {
                    // 터질위치 저장
                    boomPos = _InductionList[0].transform.position;

                    // 유도탄 비활성화
                    _InductionList[0].gameObject.SetActive(false);

                    // 폭발
                    int bombIndex = FindDisableBomb();

                    // 폭발 생성
                    if (bombIndex == -1) CreateBomb(boomPos);

                    // 폭발 활성화
                    else EnableBomb(bombIndex, boomPos);

                    yield return new WaitForSeconds(0.2f);

                    // 폭발 비활성화
                    _BombList[0].gameObject.SetActive(false);
                }
            }

		}
	}
    
    // 유도탄 생성
    private void CreateInduction()
    {
        // 생성
        Bullet bullet = Instantiate(_Induction);
        
        // 좌표 설정
        bullet.transform.position = transform.position;

        // 리스트에 추가
        _InductionList.Add(bullet);
    }

    // 비활성화 중인 유도탄을 찾음
    private int FindDIsableInduction()
    {
        for (int i = 0; i < _InductionList.Count; i++)
        {
            if (!_InductionList[i].gameObject.activeSelf) return i;
        }

        return -1;
    }

    // 폭탄 생성
    private void CreateBomb(Vector2 dir)
    {
        // 생성
        GameObject bomb = Instantiate(_Bomb);

        // 좌표 설정
        bomb.transform.position = dir;

        // 리스트에 추가
        _BombList.Add(bomb);
    }

    // 비활성화 중인 폭탄 찾기
    private int FindDisableBomb()
    {
        for (int i = 0; i < _BombList.Count; i++)
            if (!_BombList[i].gameObject.activeSelf) return i;

        return -1;
    }

    // 폭탄 활성화
    private void EnableBomb(int index, Vector2 dir)
    {
        // 활성화
        _BombList[index].gameObject.SetActive(true);

        // 좌표설정
        _BombList[index].transform.position = dir;
    }

    // 비활성화 중인 유도탄을 활성화함
    private void EnableInduction(int index)
    {
        // 활성화
        _InductionList[index].gameObject.SetActive(true);
        
        // 좌표 설정
        _InductionList[index].transform.position = transform.position;
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
