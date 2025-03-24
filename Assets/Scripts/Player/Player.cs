using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool damageable = true;
    private float _HurtingDurationTime = 1.0f;

    private static float _PlayerMaxHP;
    public float playerHp { get; set; } = _PlayerMaxHP;
    [SerializeField] private Animator _PlayerBodyAnim;
    [SerializeField] private Animator _GunAnim;
    [SerializeField] private Animator _HatAnim;
    [SerializeField] private Animator _BarAnim;

    private bool _MoveState = false;

    public bool _Stop = false;

    // 체력감소 딜레이
    private float _HPDownDelay = 0.5f;
    private float _AddTime;

    // 돼지가 죽었을 때 회복될 체력
    private float _KillPigHeal;

    [SerializeField] private Ball _BallPrefab;
    private Ball _Ball;

    private bool _IsShootBall = false;
    private float _BallShootPower = 750.0f;
    private bool _Moveable = false;
    public bool hackable = false;

    public Camera mainCam;

    private Vector2 _MousePos = Vector2.zero;
    private bool _InputMouseButton;

    // 라인렌더러
    [SerializeField] private LineRenderer _Lr;

    // 총
    [SerializeField] private SpriteRenderer _Gun;

    // 플레이어 체력바
    [SerializeField] private PlayerHPGauge _HPGauge;

    // 결과창을 띄어줄 때 사용
    [SerializeField] private WaveManager _WaveManager;

    // 죽었을 떄 재생할 목소리
    [SerializeField] private List<AudioClip> _PlayerDieVoiceList;
    [SerializeField] private AudioSource _DieAudio;

    // 수밖시 뱉을 때
    [SerializeField] private AudioSource _ShootAudio;

    // 맞을 때 소리
    [SerializeField] private AudioSource _HurtingAudio;

    // 공 이미지
    [SerializeField] private Sprite _FireBall;
    [SerializeField] private Sprite _StandardBall;

    #region Item Variable
    [Header("---아이템---")]

    // 회복량
    private int _Heal;

    public bool isShooting = false;
    [SerializeField] private Bullet _BulletPrefab;
    private List<Bullet> _BulletList = new List<Bullet>();

    // 발사 딜레이
    private float _ShootingDelay = 0.5f;

    // 최대 발사수
    private uint _MaximumShootingCount;

    // 현재 발사수
    private uint shootingBulletCount = 0;

    // 플레이어의 총알 사이즈
    private Vector2 _PlayerBulletSize;
	
    // 커질수 있는 최대횟수
    private uint _UpgradeMaxCount;
    
    // 원래 크기
    private Vector2 _BallScale = new Vector2(0.4f, 0.4f);

    // 증가하는 크기
    private Vector2 _UpgradeAddSize = new Vector2(0.05f, 0.05f);

    // 커진횟수
    public uint upgradeCount = 0;

    public bool overload = false;
    [SerializeField] private GameObject _BallTrigger;

    // 지속시간
	private uint _OverloadDurationTime;

    // 챙
    [SerializeField] private GameObject _Bar;

    // 머리
    [SerializeField] private GameObject _Head;

    // 증가하는 크기
    private float BarScalePlus = 0.05f;

    // 최대 커지는 횟수
	private uint _BarMaxPlusCount;

    // 현재 커진횟수
    private uint _BarPlusCount = 0;

    // 원래 크기
    private Vector2 _BarStandardScale = new Vector2(0.5f, 0.5f);
	
    // 미니볼 본체
    [SerializeField] private GameObject _MiniBall;

    // 미니볼의 트렌스폼을 담을 거
    private Transform _MiniBallTransform;

    // 생성한 미니볼을 담을 리스트
    private List<GameObject> _MiniBallList = new List<GameObject>();

    // 미니볼 최대 생성량
    private uint _MiniBallMaximumCount;
    
    public bool hos = false;
    [SerializeField] private GameObject _HOS;
    private float _HosDurationTime;

    // 히오스 범위초기화
    private Vector2 _HosSize;

    #endregion

    private void Awake()
    {
        // 캐릭터매니저 플레이어 참조
        GameManager.getCharacterManager.player = this;

        // 데이터 초기화
        InitializeData();
    }

    // 데이터 초기화
    private void InitializeData()
    {
        // 공 생성
        _Ball = Instantiate(_BallPrefab);
        
        JsonData.ItemEnchant item = GameManager.getJsonDataManager.itemEnchant;

		// 최대 체력 설정
		InitializeMaxHP(item);

		// 총알 발사수
		InitializeMaximumShootingCount(item);

        // 미니볼
        InitializeMiniBallMaximumCount(item);

        // 빅바 최대증가회수
        InitializeBarMaxPlusCount(item);

        // 과부하 지속시간
        InitializeOverloadDurationTime(item);

        // 힐 증가량
        InitializeHeal(item);

        // 체력감소 딜레이시간 증가
        InitializeAddTime(item);

        // 공 크기 최대증가량
        InitializeUpradeMaxCount(item);

        // 히오스 지속시간
        InitializeHosDurationTime(item);

        // 돼지 처치시 얻는 회복량
        InitializeKealPigHeal(item);

        // 히오스 크기
        InitializeHosSize(item);

        // 총알 크기
        InitializeBulletSize(item);
    }

    #region 아이템 강화설정
    // 최대체력 설정 초기화
    private void InitializeMaxHP(JsonData.ItemEnchant item) {

		// 체력
		switch (item.PlayerMaxHPLV) {
			case 0: _PlayerMaxHP = 100; break;
			case 1: _PlayerMaxHP = 110; break;
			case 2: _PlayerMaxHP = 120; break;
			case 3: _PlayerMaxHP = 127; break;
			case 4: _PlayerMaxHP = 135; break;
			case 5: _PlayerMaxHP = 140; break;
			case 6: _PlayerMaxHP = 143; break;
			case 7: _PlayerMaxHP = 146; break;
			case 8: _PlayerMaxHP = 149; break;
			case 9: _PlayerMaxHP = 152; break;
			case 10: _PlayerMaxHP = 155; break;
			case 11: _PlayerMaxHP = 158; break;
			case 12: _PlayerMaxHP = 162; break;
			case 13: _PlayerMaxHP = 164; break;
			case 14: _PlayerMaxHP = 167; break;
			case 15: _PlayerMaxHP = 170; break;
			case 16: _PlayerMaxHP = 172; break;
			case 17: _PlayerMaxHP = 174; break;
			case 18: _PlayerMaxHP = 176; break;
			case 19: _PlayerMaxHP = 178; break;
			case 20: _PlayerMaxHP = 180; break;
			case 21: _PlayerMaxHP = 182; break;
			case 22: _PlayerMaxHP = 184; break;
			case 23: _PlayerMaxHP = 186; break;
			case 24: _PlayerMaxHP = 188; break;
			case 25: _PlayerMaxHP = 191; break;
			case 26: _PlayerMaxHP = 194; break;
			case 27: _PlayerMaxHP = 196; break;
			case 28: _PlayerMaxHP = 198; break;
			case 29: _PlayerMaxHP = 200; break;
			case 30: _PlayerMaxHP = 205; break;
		}

		// 체력 초기화
		playerHp = _PlayerMaxHP;
	}

	// 총알 발사수 설정 초기화
	private void InitializeMaximumShootingCount(JsonData.ItemEnchant item){

		switch (item.MaximumShootingCountLV) {
			case 0: _MaximumShootingCount = 4; break;
			case 1: _MaximumShootingCount = 6; break;
			case 2: _MaximumShootingCount = 8; break;
			case 3: _MaximumShootingCount = 10; break;
			case 4: _MaximumShootingCount = 12; break;
			case 5: _MaximumShootingCount = 14; break;

		}
	}

    // 미니볼 최대생성수
    private void InitializeMiniBallMaximumCount(JsonData.ItemEnchant item)
    {
        switch (item.MiniBallMaximumCountLV)
        {
            case 0: _MiniBallMaximumCount = 1; break;
            case 1: _MiniBallMaximumCount = 2; break;
            case 2: _MiniBallMaximumCount = 3; break;
            case 3: _MiniBallMaximumCount = 4; break;
            case 4: _MiniBallMaximumCount = 5; break;
            case 5: _MiniBallMaximumCount = 6; break;
            case 6: _MiniBallMaximumCount = 7; break;
        }
    }

    // 빅바 최대증가회수
    private void InitializeBarMaxPlusCount(JsonData.ItemEnchant item)
    {
        switch (item.BarMaxPlusCountLV)
        {
            case 0: _BarMaxPlusCount = 2; break;
            case 1: _BarMaxPlusCount = 3; break;
            case 2: _BarMaxPlusCount = 4; break;
            case 3: _BarMaxPlusCount = 5; break;
            case 4: _BarMaxPlusCount = 6; break;
            case 5: _BarMaxPlusCount = 7; break;
        }
    }

    // 과부하 지속시간
    private void InitializeOverloadDurationTime(JsonData.ItemEnchant item)
    {
        switch (item.OverloadLV)
        {
            case 0: _OverloadDurationTime = 6; break;
            case 1: _OverloadDurationTime = 7; break;
            case 2: _OverloadDurationTime = 8; break;
            case 3: _OverloadDurationTime = 9; break;
            case 4: _OverloadDurationTime = 10; break;
            case 5: _OverloadDurationTime = 11; break;
            case 6: _OverloadDurationTime = 12; break;
            case 7: _OverloadDurationTime = 13; break;
            case 8: _OverloadDurationTime = 14; break;
            case 9: _OverloadDurationTime = 15; break;
            case 10: _OverloadDurationTime = 16; break;
        }
    }

    // 힐 증가량
    private void InitializeHeal(JsonData.ItemEnchant item)
    {
        switch (item.HealLV)
        {
            case 0: _Heal = 20; break;
            case 1: _Heal = 23; break;
            case 2: _Heal = 25; break;
            case 3: _Heal = 27; break;
            case 4: _Heal = 29; break;
            case 5: _Heal = 31; break;
            case 6: _Heal = 33; break;
            case 7: _Heal = 35; break;
            case 8: _Heal = 37; break;
            case 9: _Heal = 39; break;
            case 10: _Heal = 41; break;
        }
    }

    // 체력감소 딜레이시간 증가
    private void InitializeAddTime(JsonData.ItemEnchant item)
    {
        switch (item.AddTimeLV)
        {
            case 0: _AddTime = 0.0f; break;
            case 1: _AddTime = 0.05f; break;
            case 2: _AddTime = 0.1f; break;
            case 3: _AddTime = 0.15f; break;
            case 4: _AddTime = 0.2f; break;
            case 5: _AddTime = 0.25f; break;
            case 6: _AddTime = 0.3f; break;
        }
    }
    
    // 공 크기 최대증가량
    private void InitializeUpradeMaxCount(JsonData.ItemEnchant item)
    {
        switch (item.UpgradeMaxCountLV)
        {
            case 0: _UpgradeMaxCount = 2; break;
            case 1: _UpgradeMaxCount = 3; break;
            case 2: _UpgradeMaxCount = 4; break;
            case 3: _UpgradeMaxCount = 5; break;
            case 4: _UpgradeMaxCount = 6; break;
            case 5: _UpgradeMaxCount = 7; break;
            case 6: _UpgradeMaxCount = 8; break;
        }
    }

    // 히오스 지속시간
    private void InitializeHosDurationTime(JsonData.ItemEnchant item)
    {
        switch (item.HosDurationTimeLV)
        {
            case 0: _HosDurationTime = 2.0f; break;
            case 1: _HosDurationTime = 2.5f; break;
            case 2: _HosDurationTime = 3.0f; break;
            case 3: _HosDurationTime = 3.5f; break;
            case 4: _HosDurationTime = 4.0f; break;
            case 5: _HosDurationTime = 4.5f; break;
            case 6: _HosDurationTime = 5.0f; break;
            case 7: _HosDurationTime = 5.5f; break;
        }
    }

    // 돼지 처치시 얻는 회복량
    private void InitializeKealPigHeal(JsonData.ItemEnchant item)
    {
        switch (item.KillPigHealLV)
        {
            case 0: _KillPigHeal = 1.5f; break;
            case 1: _KillPigHeal = 1.6f; break;
            case 2: _KillPigHeal = 1.7f; break;
            case 3: _KillPigHeal = 1.8f; break;
            case 4: _KillPigHeal = 1.9f; break;
            case 5: _KillPigHeal = 2.0f; break;
            case 6: _KillPigHeal = 2.1f; break;
            case 7: _KillPigHeal = 2.2f; break;
            case 8: _KillPigHeal = 2.3f; break;
            case 9: _KillPigHeal = 2.4f; break;
            case 10: _KillPigHeal = 2.5f; break;

        }
    }

    // 히오스 크기
    private void InitializeHosSize(JsonData.ItemEnchant item)
    {
        switch (item.HosSizeLV)
        {
            case 0: _HosSize = new Vector2(0.5f, 0.5f); break;
            case 1: _HosSize = new Vector2(0.6f, 0.6f); break;
            case 2: _HosSize = new Vector2(0.8f, 0.8f); break;
            case 3: _HosSize = new Vector2(1.0f, 1.0f); break;
            case 4: _HosSize = new Vector2(1.2f, 1.2f); break;
        }

        // 히오스 크기 설정
        _HOS.transform.localScale = _HosSize;
    }

    // 총알 크기
    private void InitializeBulletSize(JsonData.ItemEnchant item)
    {
        switch (item.BulletSizeLV)
        {
            case 0: _PlayerBulletSize = new Vector2(1.0f, 1.0f); break;
            case 1: _PlayerBulletSize = new Vector2(1.0f, 1.0f); break;
            case 2: _PlayerBulletSize = new Vector2(1.0f, 1.0f); break;
            case 3: _PlayerBulletSize = new Vector2(1.0f, 1.0f); break;
            case 4: _PlayerBulletSize = new Vector2(1.0f, 1.0f); break;
            case 5: _PlayerBulletSize = new Vector2(1.0f, 1.0f); break;
            case 6: _PlayerBulletSize = new Vector2(1.0f, 1.0f); break;
            case 7: _PlayerBulletSize = new Vector2(1.0f, 1.0f); break;
        }

        // 총알 크기 설정
        _BulletPrefab.transform.localScale = _PlayerBulletSize;
    }
    #endregion

    private void Start()
    {
        // 딜레이마다 체력감소
        StartCoroutine(HPAutoDown());
    }

    private void Update()
    {
        // 공 날리기
        ShootBall();

        // 클릭 이동
        //InputScreen();

        // 트리거가 공을 따라다니게 설정
        if (_BallTrigger.gameObject.activeSelf) FollowTrigger();

        // 보호막이 플레이어를 따라다님
        if (_HOS.gameObject.activeSelf) FollowHos();

        // 빠 따라다니기
        if (_Bar.gameObject.activeSelf) FollowBar();

		//if (Input.GetKeyDown(KeyCode.Space)) StartHos();
	}

    // 트리거가 공을 따라다니게 설정
    private void FollowTrigger()
    {
        _BallTrigger.transform.position = _Ball.transform.position;
    }

    // 보호막이 플레이어를 따라다님
    private void FollowHos()
    {
        _HOS.transform.position = transform.position;
    }

    // 빠 따라오기
    private void FollowBar()
    {
        _Bar.transform.position = new Vector2(transform.position.x, transform.position.y + 0.2f);
        _Head.transform.position = new Vector2(transform.position.x + 0.1f, transform.position.y + 0.4f);
    }

    // 플레이어 이동 드래그로
    private void OnMouseDrag()
    {
        if (_Moveable && Time.timeScale == 1.0f)
        {
            // 마우스 좌표
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            // 움직일 오브젝트
            Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePos);

            objPosition = new Vector3(objPosition.x, objPosition.y, 0.0f);

            // 오브젝트 가두기
            objPosition.x = Mathf.Clamp(objPosition.x, -2.4f, 2.4f);
            objPosition.y = Mathf.Clamp(objPosition.y, -4.8f, 3.8f);

            // 오브젝트 위치 설정
            transform.position = objPosition;
        }
        
    }

    // 사용 안함
    private void InputScreen()
    {

        // 이동할 수 있는 상태라면
        if (_Moveable && Time.timeScale == 1.0f)
        {

            // 마우스 클릭 이동
            Vector2 wp = mainCam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector3.forward, Mathf.Infinity);

            if (Input.GetMouseButtonDown(0)) _InputMouseButton = true;
            if (Input.GetMouseButtonUp(0)) _InputMouseButton = false;

            if (!_InputMouseButton) _MousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButton(0))
            {
                Vector2 playerPos = transform.position;

                playerPos = playerPos + (wp - _MousePos);
                playerPos.x = Mathf.Clamp(playerPos.x, -2.4f, 2.4f);
                playerPos.y = Mathf.Clamp(playerPos.y, -4.8f, 3.8f);

                transform.position = playerPos;
                _MousePos = wp;
            }
            
        }

    }

    // 딜레이마다 체력감소
    private IEnumerator HPAutoDown()
    {
        while (gameObject.activeSelf && _HPGauge != null)
        {
            yield return new WaitForSeconds(_HPDownDelay + _AddTime);
            playerHp--;

            // 체력바 업데이트
            _HPGauge.HPUpdate();

            // 플레이어의 체력이 0보다작다면
            if (playerHp <= 0)
            {
                // 플레이어 사망
                DiePlayerHP();

                yield return new WaitForSeconds(1.5f);

                // 비활
                DisablePlayer();
            }
        }

    }

    // 플레이어를 멈출 때 호출(마법 탄)
    public void StopPlayer()
    {
        _Stop = true;
        StartCoroutine(IsStop());
    }

    // 마법에 맞았을 때 정지
    private IEnumerator IsStop()
    {

        while (_Stop)
        {
            // 움직이지 못하게함
            _Moveable = false;

            yield return new WaitForSeconds(2.0f);

            // 움직일수 있게함
            _Moveable = true;

            _Stop = false;
        }

        yield return null;
    }
#if UNITY_EDITOR


#elif UNITY_ANDROID
   
#endif

    //공 날려주기
    private void ShootBall()
    {
        if (!_IsShootBall)
        {
            // 움직이지 못하게함
            _Moveable = false;

            // 라인렌더러 활성화
            _Lr.gameObject.SetActive(true);

            // 총 위치 설정, 그려질 순서 변경, 각도
            _Gun.transform.position = transform.position + Vector3.up * 1.0f;
            _Gun.sortingOrder = -2;
            _Gun.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 0.0f));

            // 공 활성화
            _Ball.transform.gameObject.SetActive(true);

            // 공 위치 설정
            _Ball.transform.position = transform.position + Vector3.up * 2.2f;

            // 공 가속도 설정
            _Ball.Initialize();

            // 마우스가 눌렸다면 마우스 좌표를 받아오지 않음
            if (!_InputMouseButton) _MousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            if (Input.GetMouseButtonDown(0)) _InputMouseButton = true;

            // 날려줄 방향
            Vector2 firePos;

            // 마우스가 눌린 위치에서 부터 움직인 위치만큼 빼줄 거
            Vector2 wp = mainCam.ScreenToWorldPoint(Input.mousePosition);

            firePos = wp - _MousePos;
            firePos.x = -Mathf.Clamp(firePos.x, -2.0f, 2.0f);
            firePos.y = -Mathf.Clamp(firePos.y, -2.0f, 2.0f);

            // 라인 렌더러 0번 위치 설정
            _Lr.SetPosition(0, _Ball.transform.position);

            // 라인 렌더러 1번 위치 설정
            _Lr.SetPosition(1, new Vector2(firePos.x + _Ball.transform.position.x, firePos.y + _Ball.transform.position.y));

            // 마우스떄짐
            if (Input.GetMouseButtonUp(0))
            {
                Rigidbody2D ballrigid = _Ball.GetComponent<Rigidbody2D>();

                // 공의 상태 변경
                _Ball.isHack = true;

                // 마우스 눌림 상태변경
                _InputMouseButton = false;

                // 발사 상태변경
                _IsShootBall = true;

                // 날가는 속도가 너무 느리다면 속도를 더해줌
                if (Mathf.Abs(firePos.y) < 0.3f)
                {
                    firePos.y += 0.5f;
                }

                // 공 날려주기
                ballrigid.AddForce(firePos * _BallShootPower);

                // 라인 렌더러 비활성화
                _Lr.gameObject.SetActive(false);

                // 총 위치 설정, 그려질 순서 변경, 각도
                _Gun.transform.position = transform.position;
                _Gun.sortingOrder = 2;
                _Gun.transform.rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 45.0f));

                // 움직일수 있게
                hackable = false;
                _Moveable = true;
            }

        }

        else
        {
            // 공 비활성화
            if (hackable)
            {
                _IsShootBall = false;
                _Ball.gameObject.SetActive(false);
            }

            else return;

        }

    }

	// 플레이어를 데미지를 입지않게 만듦
	private IEnumerator NoDamage() {

		while (!damageable) {

            // 소리 재생
            PlayHurtingSound();

            // 애니메이션
            _PlayerBodyAnim.SetBool("IsHurting", true);
            _HatAnim.SetBool("Hurting", true);
            _GunAnim.SetBool("Hurting", true);
            _BarAnim.SetBool("Hurting", true);

			yield return new WaitForSeconds(_HurtingDurationTime);

			damageable = true;

			// 애니메이션 설정
			_PlayerBodyAnim.SetBool("IsHurting", false);
            _HatAnim.SetBool("Hurting", false);
            _GunAnim.SetBool("Hurting", false);
            _BarAnim.SetBool("Hurting", false);
        }

		yield return null;
	}

	// 맞음
	public void HPDown(float damage) {

        // 업그레이드 데미지업 제거
        //if (upgradeCount > 0) _Ball.BallDamageDown();

        // 업그레이드 크기증가 제거
        _Ball.transform.localScale = new Vector3(0.4f, 0.4f, 1.0f);

        // 공크기 초기화
        _BallScale = new Vector2(0.4f, 0.4f);

        // 업그레이드 횟수 초기화
        upgradeCount = 0;

        // 바 크기 초기화
        _Bar.transform.localScale = new Vector2(0.5f, 0.5f);

        // 바 증가 크기 초기화
        _BarStandardScale = new Vector2(0.5f, 0.5f);

        // 바 크기 증가 회수 초기화
        _BarPlusCount = 0;

        // 체력감소
        playerHp -= damage;

		// 데미지상태 변경
		damageable = false;

        // 체력바 업데이트
        _HPGauge.HPUpdate();

        if (playerHp <= 0) {

			// 플레이어 사망
			DiePlayerHP();
		}

        // 무적 시작
        else StartCoroutine(NoDamage());
    }

    // 플레이어 죽음처리
    private void DiePlayerHP()
    {
        _Moveable = false;

        // 죽음 소리 재생
        PlayDieVoiec();

        // 플레이어 비활
        _PlayerBodyAnim.SetBool("Die", true);

		// 공 비활
		_Ball.gameObject.SetActive(false);

        // 총 비활성화
        _Gun.gameObject.SetActive(false);

        // 바 비활
        _Bar.gameObject.SetActive(false);
		_Head.gameObject.SetActive(false);

		// 활성화중인 미니볼 비활
		for (int i = 0; i < _MiniBallList.Count; i++)
		{
		    _MiniBallList[i].gameObject.SetActive(false);
		}
    }

    // 플레이어 비활성화
    private void DisablePlayer()
    {
        // 비활성화
        gameObject.SetActive(false);

        // 결과창 띄우기
        _WaveManager.OnResult();

        // 코루틴 종료
        StopAllCoroutines();
    }

    // 플레이어 최대체력을 리턴
    public float playerMaxHP { get { return _PlayerMaxHP; } }

    // 돼지 처치시 체력회복
    public void EnemyDieToHeal() { 

		// 회복
		playerHp += _KillPigHeal;

        // 가두기
        playerHp = Mathf.Clamp(playerHp, 0.0f, _PlayerMaxHP);

        // 체력바 업데이트
        _HPGauge.HPUpdate();
	}

    // 플레이어 죽는 소리 재생
    private void PlayDieVoiec()
    {
        // 재생할 클립을 랜덤으로 뽑음
        int randomNumber = Random.Range(0, _PlayerDieVoiceList.Count);

        // 볼륨 설정
        _DieAudio.volume = GameManager.getJsonDataManager.playerData.VoiceValue;

        // 재생할 클립 설정
        _DieAudio.clip = _PlayerDieVoiceList[randomNumber];

        // 재생
        _DieAudio.Play();
    }
    
    // 맞는 소리 재생
    private void PlayHurtingSound()
    {
        // 볼륨 설정
        _HurtingAudio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _HurtingAudio.Play();
    }

    // 수박씨 뱉는 소리
    private void PlayShootSound()
    {
        // 볼륨 설정
        _ShootAudio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _ShootAudio.Play();
    }

#region 아이템
    // 회복
    public void Heal() { 
	
		// 회복
		playerHp += _Heal;

		// 체력 가두기
		playerHp = Mathf.Clamp(playerHp, 0.0f, playerMaxHP);

        // 체력바 업데이트
        _HPGauge.HPUpdate();
    }

    // 아이템에서 호출해줄 메서드
    public void StartShooting()
    {
        // 발사 수 초기화
        shootingBulletCount = 0;

        // 참
        isShooting = true;

        // 코루틴 시작
        StartCoroutine(Shooting());
    }

    // 수박씨
    private IEnumerator Shooting() {

        while (isShooting) {

			// 비활성화중인 총알의 인덱스 번호를 담음
			int index = FindDisableBulletListIndex();

			// 비활성화중인 총알이 없다면
			if (index == -1) {
				CreateBullet();
				shootingBulletCount++;
			}

			// 있다면
			else {
				EnableBullet(index);
				shootingBulletCount++;
			}

            // 소리 재생
            PlayShootSound();

            // 발사후 딜레이만큼 대기
            yield return new WaitForSeconds(_ShootingDelay);

			// 모든 총알을 발싸했다면 종료
			if (shootingBulletCount == _MaximumShootingCount)
				isShooting = false;
		}

    }

    private int FindDisableBulletListIndex()
    {
        for (int i = 0; i < _BulletList.Count; i++)
        {
            if (!_BulletList[i].transform.gameObject.activeSelf)
            {
                return i;
            }
        }

        // 미사일을 못찾았다면 -1을 리턴
        return -1;
    }

    // 총알 생성
    private void CreateBullet()
    {
        Vector2 initPos = new Vector2(transform.position.x, transform.position.y + 1.0f);

        Quaternion initRot = Quaternion.identity;

        Bullet tempBullet = Instantiate(_BulletPrefab, initPos, initRot);

        _BulletList.Add(tempBullet);
    }

    // 총알 활성화
    private void EnableBullet(int index)
    {
        _BulletList[index].transform.gameObject.SetActive(true);

        _BulletList[index].transform.position = new Vector2(transform.position.x, transform.position.y + 1.0f);
    }

    // 업그레이드
    public void UpgradeBall()
    {
		// 공 크기증가
		if (upgradeCount < _UpgradeMaxCount) {

			// 크기 증가
			_BallScale.x += _UpgradeAddSize.x;
			_BallScale.y += _UpgradeAddSize.y;
			_Ball.transform.localScale = _BallScale;

			// 공 데미지 증가
			//if (upgradeCount == 0) _Ball.BallDamageUp();
			upgradeCount++;
		}
	}

    public void StartOverload()
    {
        //GameManager.getCharacterManager.ball.BallDamageUp();

        // 과부하 상태
        overload = true;

        StartCoroutine(OverloadBall());
    }

    // 과부하
    private IEnumerator OverloadBall()
    {
        while (overload) {

			// 공 레이어 변경(몬스터와 충돌X)
            _Ball.gameObject.layer = 12;

			// 공 대신 데미지 처리를 해줄 트리거 활성화
            _BallTrigger.gameObject.SetActive(true);

			// 공의 애니메이션 변경 true로
			//_Ball.BallAnimOverload();

            // 공의 스프라이트 렌더러 접근
            SpriteRenderer ballsprite = _Ball.GetComponent<SpriteRenderer>();

            // 공의 스프라이트 변경
            ballsprite.sprite = _FireBall;

            yield return new WaitForSeconds(_OverloadDurationTime);

			// 공의 레이어를 원래대로
			_Ball.gameObject.layer = 11;

			// 트리거 비활성화
			_BallTrigger.gameObject.SetActive(false);

			// 상태 변경
			overload = false;

            // 공의 스프라이트 원래대로
            ballsprite.sprite = _StandardBall;

            // 공의 애니메이션 변경 false로
            //_Ball.BallAnimOverload();

        }
    }

    // 빅바
    public void BigBar()
    {
		if (_BarPlusCount != _BarMaxPlusCount) {
			// 스케일 증가
			_BarStandardScale.x += BarScalePlus;

			_Bar.transform.localScale = _BarStandardScale;
			_BarPlusCount++;
		}
	}

    // 미니볼
    public void MiniBall()
    {
		int index = FindDisableMiniBallIndex();

		if (index == -1)
			CreateMiniBall();

		else EnableMiniBall(index);
	}

	// 비활중인 미니볼
    private int FindDisableMiniBallIndex()
    {
        for (int i = 0; i < _MiniBallList.Count; i++)
        {
            if (!_MiniBallList[i].transform.gameObject.activeSelf)
                return i;
        }

        return -1;
    }

	// 생성
    private void CreateMiniBall()
    {
        if (_MiniBallList.Count < _MiniBallMaximumCount)
        {
            GameObject newball = Instantiate(_MiniBall);

            _MiniBallTransform = newball.GetComponent<Transform>();

            _MiniBallTransform.transform.position = transform.position + Vector3.up * 1.5f;

            _MiniBallList.Add(newball);

            Rigidbody2D miniballrigid = _MiniBallTransform.GetComponent<Rigidbody2D>();

            miniballrigid.AddForce(Vector2.up * 350.0f);
            
        }

    }

	// 활성화
    private void EnableMiniBall(int index)
    {
        _MiniBallList[index].transform.gameObject.SetActive(true);

        _MiniBallList[index].transform.position = transform.position + Vector3.up * 1.2f;

        Rigidbody2D miniballrigid = _MiniBallTransform.GetComponent<Rigidbody2D>();

        miniballrigid.AddForce(Vector2.up * 300.0f);
    }

	// 히오스 실행
    public void StartHos()
    {
        hos = true;

        StartCoroutine(HOS());
    }

    // 히오스
    private IEnumerator HOS()
    {
        while (hos)
        {
            // 활성화
            _HOS.gameObject.SetActive(true);
            
            // 딜레이 만큼 대기
            yield return new WaitForSeconds(_HosDurationTime);

            // 비활성화
            _HOS.gameObject.SetActive(false);

            // 종료
            hos = false;
		}

        yield return null;

    }
#endregion

}
