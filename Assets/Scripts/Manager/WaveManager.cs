using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
	// 전체 적을 담음
    [SerializeField] private List<EnemyBase> _EnemyList;

	// 만들어진 적을 담음
    [SerializeField] private List<EnemyBase> _CreateEnemyList;

	// 결과창
    public Image resultImage;
    
	// 게임화면 가운데 돈
	public Text coinText;

    // 현재 지역
    public uint _StageState = 1;
    [SerializeField] private GameObject _Canvas;
    [SerializeField] private StageStateImage _StageStateImage;

    // 대기 시간
    private float _AreaCount = 1.0f;
    [SerializeField] private uint _ChangeAreaCount = 20;

    // 보스가 나오는 간격
    private int _BossInterval = 10;
    
    // 보스 알리미
    [SerializeField] private GameObject _Notice;

    // 보스 퇴장시 나올 이펙트
    [SerializeField] private GameObject _BossExitEffect;
    private List<GameObject> _BossExitEffectList = new List<GameObject>();

    #region 돼지 변수
    // 돼지 스폰시간 최소, 최대
    private int _WeakPigSpawnTimeMin;
    private int _WeakPigSpawnTimeMax;

    // 돼지 생성 개수 최소, 최대
    private int _WeakPigSpawnCountMin;
    private int _WeakPigSpawnCountMax;

    // 돼지 감자가 스폰되었는지 확인
    private bool _IsPotatoPigSpawn = false;

    // 흑돼지가 스폰되었는지 확인
    private bool _IsBlackPigSpawn = false;

    // 날개 돼지가 스폰되었는지 확인
    private bool _IsAngelPigSpawn = false;

    // 묶인 돼지 스폰 시간
    private int _TiedPigSpawnTime;

    // 묶인 돼지 스폰 확률
    private int _TiedPigSpawnPer;

    // 묶인돼지가 스폰되었는지 확인
    private bool _IsTiedPigSpawn = false;

    // 오물돼지가 비활성화 후 스폰되는 시간
    private float _SoilPigSpawnDelay;

    // 오물돼지가 스폰되었는지 확인
    private bool _IsSoilPigSpawn = false;


    // 구르는 돼지 스폰 시간
    private int _RollingPigSpawnTime;

    // 구르는 돼지 스폰 확률
    private int _RollingPigSpawnPer;

    // 구르는 돼지가 스폰되었는지 확인
    private bool _IsRollingPigSpawn = false;


	// 겁쟁이돼지가 스폰되었는지 확인
	private bool _IsCowardPigSpawn = false;

	// 아기돼지 삼형제가 스폰되었는지 확인
	private bool _IsThreePigSpawn = false;

	// 돼지목의 진주목걸이가 스폰되었는지 확인
	private bool _IsPearlPigSpawn = false;

	// 진 오물돼지가 스폰되었는지 확인
	private bool _IsSludgePigSpawn = false;

	// 마법 요정돼지가 스폰되었는지 확인
	private bool _IsMagicPigSpawn = false;

	// 튀는 돼지가 스폰되었는지 확인
	private bool _IsBouncingPigSpawn = false;

	// Mr.Dangerous 가 스폰되었는지 확인
	private bool _IsMrDangerousSpawn = false;

	// 우두머리 돼지가 스폰되었는지 확인
	private bool _IsGuvPigSpawn = false;


    // 파괴자 돼지 스폰 시간
    private int _DestroyerPigSpawnTime;

    // 파괴자 돼지 스폰 확률
    private int _DestroyerPigSpawnPer;

    // 파괴자 돼지 스폰 개수 최소, 최대;
    //private int _DestroyerPigSpawnCountMin;
    //private int _DestroyerPigSpawnCountMax;

    // 파괴자 돼지가 스폰되었는지 확인
    private bool _IsDestroyerPigSpawn = false;


    // 철갑돼지 스폰 시간
    private int _IronPigSpawnTime;

    // 철갑돼지 스폰 확률
    private int _IronPigSpawnPer;

    // 철갑돼지 스폰 최소, 최대 개수
    //private int _IronPigSpawnCountMin;
    //private int _IronPigSpawnCountMax;

    // 철갑돼지가 스폰되었는지 확인
    private bool _IsIronPigSpawn = false;

    // 100인분 돼지가 스폰되었는지 확인
    private bool _IsHundredPigSpawn = false;

    // 100인분 돼지가 비활성화되었는지 확인
    private bool _IsHundredPigDisable = false;

    // 아롱이가 스폰되었는지 확인
    private bool _IsArongESpawn = false;
    
    // 아롱이가 비활성화 되었는지 확인
    private bool _IsArongEDisable = false;

    // 아기돼지집이 스폰되었는지 확인
    private bool _IsPigHouseSpawn = false;

    // 아기돼지집이 비활성화 되었는지 확인
    private bool _IsPigHouseDisable = false;

    // 까마귀가 스폰되었는지 확인
    private bool _IsCrowSpawn = false;

    // 까마귀가 비활성화 되었는지 확인
    private bool _IsCrowDisable = false;

    // 돼지왕이 스폰되었는지 확인
    private bool _IsPigKingSpawn = false;

    // 돼지왕이 비활성화 되었는지 확인
    private bool _IsPigKingDisable = false;

    // 복돼지가 스폰되었는지 확인
    private bool _IsGoldenPigSpawn = false;

    // 복돼지가 비활성화 되었는지 확인
    private bool _IsGoldenPigDisable = false;

    // 에스더가 스폰되었는지 확인
    [SerializeField] private bool _IsEstherSpawn = false;

    // 도살자가 스폰되었는지 확인
    private bool _IsButcherSpawn = false;

    #endregion

    private JsonData.PlayerData playerData;
	
    private void Start()
    {
        playerData = GameManager.getJsonDataManager.playerData;

        // 결과창 비활성화
        resultImage.gameObject.SetActive(false);

        // 몬스터 스폰
        SpawnEnemyForStart();
        
        // 지역 업데이트
        StartCoroutine(AreaUpdate());
        
        // 돼지 스폰 기본 데이터 설정
        PigSpawnDataInitialize();
        
        // 코인 상태
        CoinStateUpdate();

        // 배경음악 변경
        GameManager.getAudioManager.ChangeBGM(AudioManager.BGMSort.Game);
    }
    
    private void Update()
    {
        // 몬스터 스폰
        SpawnEnemyForUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            
        }
    }

    // 보스가 죽었을 때 알림
    public void BossDieNotice()
    {
        // 알림 설정
        BossNotice._NoticeState = BossNotice.NoticeSort.Die;

        // 보스 알리미 호출
        _Notice.gameObject.SetActive(true);
    }

    // 코인상태 업데이트
    public void CoinStateUpdate()
    {
		// 현재 얻은 은화 표시
		coinText.text = GameManager.getCharacterManager.coinCountgetset.ToString();
	}

    // 시간에 따라 바뀔몬스터 데이터, 시간에 따라 구역변경, 보스 알림
    private IEnumerator AreaUpdate() {

        while (true) {

			if (_ChangeAreaCount == 20) {
				JsonData.ItemEnchant item = GameManager.getJsonDataManager.itemEnchant;

                // 보스가 나오는 스테이지 간격
                switch (item.BossIntervalLV)
                {
                    case 0: _BossInterval = 10; break;
                    case 1: _BossInterval = 9; break;
                    case 2: _BossInterval = 8; break;
                    case 3: _BossInterval = 7; break;
                }

                // 스테이지 변경
                StartCoroutine(StageUpdate());
            }

            // 30
			else if (_ChangeAreaCount == _BossInterval * 3) {

				// 돼지 생성개수 설정
				_WeakPigSpawnCountMin = 4;
				_WeakPigSpawnCountMax = 8; // 최대 개수 + 1
			}

            // 100
			else if (_ChangeAreaCount == _BossInterval * 10) {
				// 돼지 생성개수 설정
				_WeakPigSpawnCountMin = 5;
				_WeakPigSpawnCountMax = 9; // 최대 개수 + 1
			}

            // 첫 보스 193
            else if (_ChangeAreaCount == _BossInterval * 20 - 7)
            {
                // 알림 설정
                BossNotice._NoticeState = BossNotice.NoticeSort.Appear;

                // 보스 알리미 호출
                _Notice.gameObject.SetActive(true);
            }

            // 200
			else if(_ChangeAreaCount == _BossInterval * 20) {
				// 돼지 생성개수 설정
				_WeakPigSpawnCountMin = 5;
				_WeakPigSpawnCountMax = 10; // 최대 개수 + 1
			}

            // 380
			else if(_ChangeAreaCount == _BossInterval * 38) {
				// 오물돼지 스폰시간설정
				_SoilPigSpawnDelay = 5.0f;
			}

            // 두번째 보스 393
            else if (_ChangeAreaCount == _BossInterval * 40 - 7)
            {
                // 알림 설정
                BossNotice._NoticeState = BossNotice.NoticeSort.Appear;

                // 보스 알리미 호출
                _Notice.gameObject.SetActive(true);
            }
            
            // 580
			else if(_ChangeAreaCount == _BossInterval * 58) {
				// 오물돼지 스폰시간설정
				_SoilPigSpawnDelay = 3.0f;
			}
            
            // 세번쨰 보스 593
            else if (_ChangeAreaCount == _BossInterval * 60 - 7)
            {
                // 알림 설정
                BossNotice._NoticeState = BossNotice.NoticeSort.Appear;

                // 보스 알리미 호출
                _Notice.gameObject.SetActive(true);
            }

            // 네번째 보스 793
            else if (_ChangeAreaCount == _BossInterval * 80 - 7)
            {
                // 알림 설정
                BossNotice._NoticeState = BossNotice.NoticeSort.Appear;

                // 보스 알리미 호출
                _Notice.gameObject.SetActive(true);
            }

            // 다섯 보스 993
            else if (_ChangeAreaCount == _BossInterval * 100 - 7)
            {
                // 알림 설정
                BossNotice._NoticeState = BossNotice.NoticeSort.Appear;

                // 보스 알리미 호출
                _Notice.gameObject.SetActive(true);
            }

            // 여섯 보스 1193
            else if (_ChangeAreaCount == _BossInterval * 120 - 7)
            {
                // 알림 설정
                BossNotice._NoticeState = BossNotice.NoticeSort.Appear;

                // 보스 알리미 호출
                _Notice.gameObject.SetActive(true);
            }

            // 일곱 보스 1393
            else if (_ChangeAreaCount == _BossInterval * 140 - 7)
            {
                // 알림 설정
                BossNotice._NoticeState = BossNotice.NoticeSort.Appear;

                // 보스 알리미 호출
                _Notice.gameObject.SetActive(true);
            }

            // 여덜 보스 1593
            else if (_ChangeAreaCount == _BossInterval * 160 - 7)
            {
                // 알림 설정
                BossNotice._NoticeState = BossNotice.NoticeSort.Appear;

                // 보스 알리미 호출
                _Notice.gameObject.SetActive(true);
            }

            // 카운트 증가
            yield return new WaitForSeconds(_AreaCount);
            _ChangeAreaCount++;
        }

    }

	// 시간에 따라 스테이지를 증가
	private IEnumerator StageUpdate() {

		while (true) {

            // 스테이지 이미지 활성화
            _StageStateImage.gameObject.SetActive(true);

            yield return new WaitForSeconds(20.0f);

			// 생존 보너스 추가
			GameManager.getCharacterManager.bonusgetset += 100;

			// 스테이지 증가
			_StageState++;
		}
	}

    // 현재 스테이지를 리턴
    public uint ReturnStage() {
        return _StageState;
    }

    // 몬스터 생성(스타트)
    private void SpawnEnemyForStart() {
        // 돼지 스폰
        StartCoroutine(SpawnPig());

        // 돼지인형 옷 스폰
        StartCoroutine(SpawnDollPig());
	}

    // 몬스터 생성(업데이트)
    private void SpawnEnemyForUpdate() {

        // 돼지 감자 스폰 20
        if (_StageState >= _BossInterval * 2 && !_IsPotatoPigSpawn)
        {
            StartCoroutine(SpawnPotatoPig());
            _IsPotatoPigSpawn = true;
        }

        // 흑돼지 스폰 40
         if (_StageState >= _BossInterval * 4 && !_IsBlackPigSpawn)
        {
            StartCoroutine(SpawnBlackPig());
            _IsBlackPigSpawn = true;
        }

        // 날개 돼지 스폰 60
         if (_StageState >= _BossInterval * 6 && !_IsAngelPigSpawn)
        {
            StartCoroutine(SpawnAngelPig());
            _IsAngelPigSpawn = true;
        }

        // 묶인돼지 스폰 2
         if (_StageState > 1 && !_IsTiedPigSpawn)
        {
            StartCoroutine(SpawnTiedPig());
            _IsTiedPigSpawn = true;
        }

        // 오물돼지 스폰 3
         if (_StageState > 2 && !_IsSoilPigSpawn)
        {
            StartCoroutine(SpawnSoilPig());
            _IsSoilPigSpawn = true;
        }

        // 구르는 돼지 스폰 4
         if (_StageState > 3 && !_IsRollingPigSpawn)
        {
            StartCoroutine(SpawnRollingPig());
            _IsRollingPigSpawn = true;
        }

        // 돼지목에 진주목걸이 스폰 7
         if (_StageState > 6 && !_IsPearlPigSpawn)
        {
            StartCoroutine(SpawnPearlPig());
            _IsPearlPigSpawn = true;
        }

        // 겁쟁이 돼지 스폰 12
         if (_StageState >= (_BossInterval + 2) && !_IsCowardPigSpawn)
        {
            StartCoroutine(SpawnCowardPig());
            _IsCowardPigSpawn = true;
        }

        // 아기돼지 삼형제 스폰 16
         if (_StageState >= (_BossInterval + 6) && !_IsThreePigSpawn)
        {
            StartCoroutine(SpawnThreePig());
            _IsThreePigSpawn = true;
        }

        // 진 오물돼지 스폰 30
         if (_StageState >= _BossInterval * 3 && !_IsSludgePigSpawn)
        {
            StartCoroutine(SpawnSludgePig());
            _IsSludgePigSpawn = true;
        }

        // Mr.Dangerous 스폰 30
         if (_StageState >= _BossInterval * 3 && !_IsMrDangerousSpawn)
        {
            StartCoroutine(SpawnMrDangerous());
            _IsMrDangerousSpawn = true;
        }

        // 튀는 돼지 스폰 40
         if (_StageState >= _BossInterval * 4 && !_IsBouncingPigSpawn)
        {
            StartCoroutine(SpawnBouncingPig());
            _IsBouncingPigSpawn = true;
        }

        // 마법요정 돼지 스폰 50
         if (_StageState >= _BossInterval * 5 && !_IsMagicPigSpawn)
        {
            StartCoroutine(SpawnMagicPig());
            _IsMagicPigSpawn = true;
        }

        // 파괴자 돼지 스폰 13
         if (_StageState >= _BossInterval + 3 && !_IsDestroyerPigSpawn)
        {
            StartCoroutine(SpawnDestroyerPig());
            _IsDestroyerPigSpawn = true;
        }

        // 철갑돼지 스폰 20
         if (_StageState >= _BossInterval * 2 && !_IsIronPigSpawn)
        {
            StartCoroutine(SpawnIronPig());
            _IsIronPigSpawn = true;
        }

        // 우두머리 돼지 스폰 40
         if (_StageState >= _BossInterval * 4  && !_IsGuvPigSpawn)
        {
            StartCoroutine(SpawnGuvPig());
            _IsGuvPigSpawn = true;
        }

        // 100인분(보스) 스테이지 간격 강화상태에 따라
         if (_StageState >= _BossInterval && !_IsHundredPigSpawn)
        {
            CreateHundredPig();
            _IsHundredPigSpawn = true;
        }

        // 100인분 비활성화 13
         if (_StageState >= (_BossInterval + 3) && !_IsHundredPigDisable)
        {
            for (int i = 0; i < _CreateEnemyList.Count; i++)

                // 이름이 같고 활성화 중이라면
                if (_CreateEnemyList[i].transform.name == "HundredPig(Clone)" && _CreateEnemyList[i].gameObject.activeSelf)
                {
                    // 알림 설정
                    BossNotice._NoticeState = BossNotice.NoticeSort.Run;

                    // 보스 알리미 호출
                    _Notice.gameObject.SetActive(true);

                    // 퇴장 이펙트
                    BossExitEffect(_CreateEnemyList[i].transform.position);

                    // 비활성화
                    _CreateEnemyList[i].gameObject.SetActive(false);
                }

            _IsHundredPigDisable = true;
        }

        // 아롱이 (보스) () 20
         if (_StageState >= _BossInterval * 2 && !_IsArongESpawn)
        {
            CreateArongE();
            _IsArongESpawn = true;
        }

        // 아롱이 비활성화 23
         if (_StageState >= (_BossInterval * 2) + 3 && !_IsArongEDisable)
        {
            for (int i = 0; i < _CreateEnemyList.Count; i++)

                // 이름이 같고 활성화 중이라면
                if (_CreateEnemyList[i].transform.name == "ArongE(Clone)" && _CreateEnemyList[i].gameObject.activeSelf)
                {
                    // 알림 설정
                    BossNotice._NoticeState = BossNotice.NoticeSort.Run;

                    // 보스 알리미 호출
                    _Notice.gameObject.SetActive(true);

                    // 퇴장 이펙트
                    BossExitEffect(_CreateEnemyList[i].transform.position);

                    // 비활성화
                    _CreateEnemyList[i].gameObject.SetActive(false);
                }

            _IsArongEDisable = true;
        }

        // 아기돼지집 (보스) 30
         if (_StageState >= _BossInterval * 3 && !_IsPigHouseSpawn)
        {
            CreatePigHouse();
            _IsPigHouseSpawn = true;
        }

        // 아기돼지집 비활성화 33
         if (_StageState >= _BossInterval * 3 + 3 && !_IsPigHouseDisable)
        {
            for (int i = 0; i < _CreateEnemyList.Count; i++)

                // 이름이 같고 활성화 중이라면
                if (_CreateEnemyList[i].transform.name == "PigHouse(Clone)" && _CreateEnemyList[i].gameObject.activeSelf)
                {
                    // 알림 설정
                    BossNotice._NoticeState = BossNotice.NoticeSort.Run;

                    // 보스 알리미 호출
                    _Notice.gameObject.SetActive(true);

                    // 퇴장 이펙트
                    BossExitEffect(_CreateEnemyList[i].transform.position);

                    // 비활성화
                    _CreateEnemyList[i].gameObject.SetActive(false);
                }

            _IsPigHouseDisable = true;
        }

        // 까마귀 (보스) 40
         if (_StageState >= _BossInterval * 4 && !_IsCrowSpawn)
        {
            CreateCrow();
            _IsCrowSpawn = true;
        }

        // 까마귀 비활성화 43
         if (_StageState >= _BossInterval * 4 + 3 && !_IsCrowDisable)
        {
            for (int i = 0; i < _CreateEnemyList.Count; i++)

                // 이름이 같고 활성화 중이라면
                if (_CreateEnemyList[i].transform.name == "Crow(Clone)" && _CreateEnemyList[i].gameObject.activeSelf)
                {
                    // 알림 설정
                    BossNotice._NoticeState = BossNotice.NoticeSort.Run;

                    // 보스 알리미 호출
                    _Notice.gameObject.SetActive(true);

                    // 퇴장 이펙트
                    BossExitEffect(_CreateEnemyList[i].transform.position);

                    // 비활성화
                    _CreateEnemyList[i].gameObject.SetActive(false);
                }

            _IsCrowDisable = true;
        }

        // 돼지왕 (보스) 50
         if (_StageState >= _BossInterval * 5 && !_IsPigKingSpawn)
        {
            CreatePigKing();
            _IsPigKingSpawn = true;
        }

        // 돼지왕 비활성화 53
        if (_StageState >= _BossInterval * 5 + 3 && !_IsPigKingDisable)
        {
            for (int i = 0; i < _CreateEnemyList.Count; i++)

                // 이름이 같고 활성화 중이라면
                if (_CreateEnemyList[i].transform.name == "PigKing(Clone)" && _CreateEnemyList[i].gameObject.activeSelf)
                {
                    // 알림 설정
                    BossNotice._NoticeState = BossNotice.NoticeSort.Run;

                    // 보스 알리미 호출
                    _Notice.gameObject.SetActive(true);

                    // 퇴장 이펙트
                    BossExitEffect(_CreateEnemyList[i].transform.position);

                    // 비활성화
                    _CreateEnemyList[i].gameObject.SetActive(false);
                }

            _IsPigKingDisable = true;
        }

        // 복돼지 (보스) 60
        if (_StageState >= _BossInterval * 6 && !_IsGoldenPigSpawn)
        {
            CreateGoldenPig();
            _IsGoldenPigSpawn = true;
        }

        // 복돼지 비활성화 63
        if (_StageState >= _BossInterval * 6 + 3 && !_IsGoldenPigDisable)
        {
            for (int i = 0; i < _CreateEnemyList.Count; i++)

                // 이름이 같고 활성화 중이라면
                if (_CreateEnemyList[i].transform.name == "GoldenPig(Clone)" && _CreateEnemyList[i].gameObject.activeSelf)
                {
                    // 알림 설정
                    BossNotice._NoticeState = BossNotice.NoticeSort.Run;

                    // 보스 알리미 호출
                    _Notice.gameObject.SetActive(true);

                    // 퇴장 이펙트
                    BossExitEffect(_CreateEnemyList[i].transform.position);

                    // 비활성화
                    _CreateEnemyList[i].gameObject.SetActive(false);
                }

            _IsGoldenPigDisable = true;
        }

        // 도살자 (보스) 70
        if (_StageState >= _BossInterval * 7 && !_IsButcherSpawn)
        {
            CreateButcher();
            _IsButcherSpawn = true;
        }

        // 도살자 비활성화 75
        if (_StageState >= _BossInterval * 7 + 5 && _IsButcherSpawn)
        {
            for (int i = 0; i < _CreateEnemyList.Count; i++)

                // 이름이 같고 활성화 중이라면
                if (_CreateEnemyList[i].transform.name == "Butcher(Clone)" && _CreateEnemyList[i].gameObject.activeSelf)
                {
                    // 알림 설정
                    BossNotice._NoticeState = BossNotice.NoticeSort.Run;

                    // 보스 알리미 호출
                    _Notice.gameObject.SetActive(true);

                    // 퇴장 이펙트
                    BossExitEffect(_CreateEnemyList[i].transform.position);

                    // 비활성화
                    _CreateEnemyList[i].gameObject.SetActive(false);
                }
        }

        // 에스더 80
        if (_StageState >= _BossInterval * 8 && !_IsEstherSpawn)
        {
            Debug.Log("에스더");
            CreateEsther();
            _IsEstherSpawn = true;
        }
    }

    // 결과창 띄우기
    public void OnResult()
    {
        // 결과창 활성화
        resultImage.gameObject.SetActive(true);

        // 플레이어 지갑에 돈 추가
        playerData.Coin += GameManager.getCharacterManager.coinCountgetset += GameManager.getCharacterManager.bonusgetset;

        // 최고 기록 설정
        if (playerData.Record < _StageState)
        {
            // 행동력 추가
            playerData.Vigor += (_StageState / 10) - (playerData.Record / 10);

            // 최고 기록을 현재 스테이로
            playerData.Record = _StageState;
        }

        StopAllCoroutines();

        // 플레이어 코루틴종료
        GameManager.getCharacterManager.player.StopAllCoroutines();

        // 데이터 설정
        GameManager.getJsonDataManager.playerData = playerData;
    }

	// 돼지 스폰 기본정보 설정
	private void PigSpawnDataInitialize() {

		// 1단계돼지들 스폰시간 설정
		_WeakPigSpawnTimeMin = 2;
		_WeakPigSpawnTimeMax = 5; // 최대 개수 + 1

		// 1단계돼지들 생성개수 설정
		_WeakPigSpawnCountMin = 3;
		_WeakPigSpawnCountMax = 7; // 최대 개수 + 1
        
		// 묶인 돼지 스폰 활률 설정
		_TiedPigSpawnPer = 6;

        // 묶인 돼지 스폰시간
        _TiedPigSpawnTime = 23;
        
		// 오물돼지 스폰시간
		_SoilPigSpawnDelay = 20.0f;
        
		// 구르는 돼지 스폰 확률 설정
		_RollingPigSpawnPer = 6;

        // 구르는 돼지 스폰시간
        _RollingPigSpawnTime = 25;

        // 파괴자 돼지 스폰 확률 설정
        _DestroyerPigSpawnPer = 5;
        
		// 철갑돼지 스폰 확률 설정
		_IronPigSpawnPer = 5;
	}

    #region 보스퇴장이펙트
    private void BossExitEffect(Vector2 pos)
    {
        int index = FindDisableBossExitEffect();

        if (index == -1) CreateBossExitEffect(pos);

        else EnableBossExitEffect(index, pos);
    }

    // 생성
    private void CreateBossExitEffect(Vector2 pos)
    {
        // 생성 
        GameObject effect = Instantiate(_BossExitEffect);

        // 좌표설정
        effect.transform.position = pos;

        // 리스트에 추가
        _BossExitEffectList.Add(effect);
    }

    // 비활찾기
    private int FindDisableBossExitEffect()
    {
        for (int i = 0; i < _BossExitEffectList.Count; i++)
            if (!_BossExitEffectList[i].activeSelf) return i;

        return -1;
    }

    // 활성화
    private void EnableBossExitEffect(int index, Vector2 pos)
    {
        // 활성화
        _BossExitEffectList[index].gameObject.SetActive(true);

        // 좌표설정
        _BossExitEffectList[index].transform.position = pos;
    }
    #endregion

    #region 돼지 (1단계)
    private IEnumerator SpawnPig() {

        while (_StageState <= _BossInterval * 2) {

            // 몇초에 한번씩 스폰할 것인지 뽑을 랜덤수
            int randomSpawnTime = Random.Range(_WeakPigSpawnTimeMin, _WeakPigSpawnTimeMax);

            // 몇 마리 생성할 것인지 뽑을 랜덤수
            int randomSpawnCount = Random.Range(_WeakPigSpawnCountMin, _WeakPigSpawnCountMax);
            
            for (int i = 0; i < randomSpawnCount; i++) {

                // 비활성화된 돼지가 있다면 인덱스 번호를 저장할 변수
                int index = FindDisablePig();

                // 비활성화된 돼지가 없다면 생성
                if (index == -1) CreatePig();

                // 있다면 활성화
                else EnablePig(index);

                // 딜레이 0.5초
                yield return new WaitForSeconds(0.5f);
            }

            // 딜레이
            yield return new WaitForSeconds(randomSpawnTime);
        }

        yield return null;
    }

    // 돼지 생성
    private void CreatePig() {

        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[0]);

        // X 좌표
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        // Y 좌표
        float spawnPosY = Random.Range(5.5f, 6.5f);

        //// 번호에 따라 X, Y값 설정
        //switch (locationNumber) {

        //    // 첫번째 줄
        //    case 0: spawnPosY = 6.5f; break;
        //    case 1: spawnPosY = 6.5f; break;
        //    case 2: spawnPosY = 6.5f; break;
        //    case 3: spawnPosY = 6.5f; break;
        //    case 4: spawnPosY = 6.5f; break;

        //    // 두번째 줄
        //    case 5: spawnPosY = 5.5f; break;
        //    case 6: spawnPosY = 5.5f; break;
        //    case 7: spawnPosY = 5.5f; break;
        //    case 8: spawnPosY = 5.5f; break;
        //    case 9: spawnPosY = 5.5f; break;
        //}

        // 좌표 설정
        enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

        // 리스트에 등록
        _CreateEnemyList.Add(enemy);
    }

    // 비활성화된 돼지 활성화
    private void EnablePig(int index) {

        // 활성화
        _CreateEnemyList[index].gameObject.SetActive(true);

        // X 좌표
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        // Y 좌표
        float spawnPosY = Random.Range(5.5f, 6.5f);

        // 좌표 설정
        _CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
    }

    // 비활성화된 돼지번호를 찾아서 리턴
    private int FindDisablePig() {

        for (int i = 0; i < _CreateEnemyList.Count; i++) {

            // 이름이 같고 비활성화 되어있는지 검사
            if (_CreateEnemyList[i].name == "Pig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
                return i;
        }


        // 비활성화된 돼지가 없다면 -1을 리턴
        return -1;
    }
	#endregion
	#region 뚱딴지 돼지
	private IEnumerator SpawnPotatoPig() {

		while (_StageState <= _BossInterval * 4) {

			// 몇초에 한번씩 스폰할 것인지 뽑을 랜덤수
			int randomSpawnTime = Random.Range(_WeakPigSpawnTimeMin, _WeakPigSpawnTimeMax);

			// 몇 마리 생성할 것인지 뽑을 랜덤수
			int randomSpawnCount = Random.Range(_WeakPigSpawnCountMin, _WeakPigSpawnCountMax);

			for (int i = 0; i < randomSpawnCount; i++) {

				// 비활성화된 뚱딴지가 있다면 인덱스 번호를 저장할 변수
				int index = FindDisablePotatoPig();

				// 비활성화된 뚱딴지가 없다면 생성
				if (index == -1) CreatePotatoPig();

				// 있다면 활성화
				else EnablePotatoPig(index);
				yield return new WaitForSeconds(0.5f);
			}

			// 딜레이
			yield return new WaitForSeconds(randomSpawnTime);
		}

		yield return null;
	}

	// 뚱딴지 생성
	private void CreatePotatoPig() {

		// 생성
		EnemyBase enemy = Instantiate(_EnemyList[1]);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);

		// 좌표 설정
		enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

		// 리스트에 등록
		_CreateEnemyList.Add(enemy);
	}

	// 비활성화된 뚱딴지 활성화
	private void EnablePotatoPig(int index) {

		// 활성화
		_CreateEnemyList[index].gameObject.SetActive(true);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);

		// 좌표 설정
		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
	}

	// 비활성화된 뚱딴지번호를 찾아서 리턴
	private int FindDisablePotatoPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {

			// 이름이 같고 비활성화 되어있는지 검사
			if (_CreateEnemyList[i].name == "PotatoPig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
				return i;
		}


		// 비활성화된 뚱딴지가 없다면 -1을 리턴
		return -1;
	}
	#endregion
	#region 흑돼지
	private IEnumerator SpawnBlackPig() {

		while (_StageState <= _BossInterval * 6) {

			// 몇초에 한번씩 스폰할 것인지 뽑을 랜덤수
			int randomSpawnTime = Random.Range(_WeakPigSpawnTimeMin, _WeakPigSpawnTimeMax);

			// 몇 마리 생성할 것인지 뽑을 랜덤수
			int randomSpawnCount = Random.Range(_WeakPigSpawnCountMin, _WeakPigSpawnCountMax);

			for (int i = 0; i < randomSpawnCount; i++) {

				// 비활성화된 흑돼지가 있다면 인덱스 번호를 저장할 변수
				int index = FindDisableBlackPig();

				// 비활성화된 흑돼지가 없다면 생성
				if (index == -1) CreateBlackPig();

				// 있다면 활성화
				else EnableBlackPig(index);
				yield return new WaitForSeconds(0.5f);
			}

			// 딜레이
			yield return new WaitForSeconds(randomSpawnTime);
		}

		yield return null;
	}

	// 흑돼지 생성
	private void CreateBlackPig() {

		// 생성
		EnemyBase enemy = Instantiate(_EnemyList[2]);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);
		
		// 좌표 설정
		enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

		// 리스트에 등록
		_CreateEnemyList.Add(enemy);
	}

	// 비활성화된 흑돼지 활성화
	private void EnableBlackPig(int index) {

		// 활성화
		_CreateEnemyList[index].gameObject.SetActive(true);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);

		// 좌표 설정
		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
	}

	// 비활성화된 흑돼지번호를 찾아서 리턴
	private int FindDisableBlackPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {

			// 이름이 같고 비활성화 되어있는지 검사
			if (_CreateEnemyList[i].name == "BlackPig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
				return i;
		}


		// 비활성화된 돼지가 없다면 -1을 리턴
		return -1;
	}
	#endregion
	#region 날개돼지
	private IEnumerator SpawnAngelPig() {

		while (true) {

			// 몇초에 한번씩 스폰할 것인지 뽑을 랜덤수
			int randomSpawnTime = Random.Range(_WeakPigSpawnTimeMin, _WeakPigSpawnTimeMax);

			// 몇 마리 생성할 것인지 뽑을 랜덤수
			int randomSpawnCount = Random.Range(_WeakPigSpawnCountMin, _WeakPigSpawnCountMax);

			for (int i = 0; i < randomSpawnCount; i++) {

				// 비활성화된 날개돼지가 있다면 인덱스 번호를 저장할 변수
				int index = FindDisableAngelPig();

				// 비활성화된 날개돼지가 없다면 생성
				if (index == -1) CreateAngelPig();

				// 있다면 활성화
				else EnableAngelPig(index);
				yield return new WaitForSeconds(0.5f);
			}

			// 딜레이
			yield return new WaitForSeconds(randomSpawnTime);
		}

		yield return null;
	}

	// 날개돼지 생성
	private void CreateAngelPig() {

		// 생성
		EnemyBase enemy = Instantiate(_EnemyList[3]);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);

		// 좌표 설정
		enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

		// 리스트에 등록
		_CreateEnemyList.Add(enemy);
	}

	// 비활성화된 날개돼지 활성화
	private void EnableAngelPig(int index) {

		// 활성화
		_CreateEnemyList[index].gameObject.SetActive(true);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);

		// 좌표 설정
		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
	}

	// 비활성화된 날개돼지번호를 찾아서 리턴
	private int FindDisableAngelPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {

			// 이름이 같고 비활성화 되어있는지 검사
			if (_CreateEnemyList[i].name == "AngelPig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
				return i;
		}


		// 비활성화된 날개돼지가 없다면 -1을 리턴
		return -1;
	}
	#endregion
	#region 돼지인형 옷
	private IEnumerator SpawnDollPig() {

		while (true) {

			// 생성 확률 뽑기
			int randomSpawn = Random.Range(0, 100);

			// 5%
			if (randomSpawn < 5) {

				// 돼지인형 옷의 상태에 따라 값이 바뀜
				int index = FindDisableDollPig();

                // 돼지인형 옷 생성 호출
                if (index == -1) CreateDollPig();

                // 활성화
                else EnableDollPig(index);

            }

			// 스폰딜레이 대기후 다시 뽑기
			yield return new WaitForSeconds(10.0f);
		}

		yield return null;
	}
    
	// 비활성화중인 돼지인형 옷을 반환
	private int FindDisableDollPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {
			// 이름
			if (_CreateEnemyList[i].name == "DollPig(Clone)")

				// 비활성화중인지
				if (!_CreateEnemyList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// 돼지인형 옷 생성
	private void CreateDollPig() {
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		EnemyBase enemy = Instantiate(_EnemyList[4]);

		enemy.transform.position = new Vector2(spawnPosX, 9.5f);

		_CreateEnemyList.Add(enemy);
	}

	// 돼지인형 옷 활성화
	private void EnableDollPig(int index) {
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		_CreateEnemyList[index].transform.gameObject.SetActive(true);

		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, 9.5f);
	}
	#endregion
	#region 묶인돼지 (2단계)
	private IEnumerator SpawnTiedPig() {

        while (_StageState <= _BossInterval * 3) {

            // 생성확률을 뽑을 난수
            int randomNumber = Random.Range(0, 10); // 0 ~ 9

            // 생성확률 보다 작다면 생성 (_TiedPigSpawnPer = 6)
            if (_TiedPigSpawnPer > randomNumber) {

                // 비활성화된 묶인 돼지가 있다면 인덱스 번호를 저장할 변수
                int index = FindDisableTiedPig();

                // 비활성화된 묶인 돼지가 없다면 생성
                if (index == -1) CreateTiedPig();

                // 있다면 활성화
                else EnableTiedPig(index);
            }

            // 딜레이만큼 대기
            yield return new WaitForSeconds(_TiedPigSpawnTime);
        }

        yield return null;
    }

    // 묶인 돼지 생성
    private void CreateTiedPig() {

        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[5]);

        // X 좌표
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        // Y 좌표
        float spawnPosY = Random.Range(5.5f, 6.5f);

        // 좌표 설정
        enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

        // 리스트에 등록
        _CreateEnemyList.Add(enemy);
    }

    // 비활성화된 묶인 돼지 활성화
    private void EnableTiedPig(int index) {

        // 활성화
        _CreateEnemyList[index].gameObject.SetActive(true);

        // X 좌표
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        // Y 좌표
        float spawnPosY = Random.Range(5.5f, 6.5f);

        // 좌표 설정
        _CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
    }

    // 비활성화된 묶인 돼지의 인덱스 번호를 찾아서 리턴
    private int FindDisableTiedPig() {

        for (int i = 0; i < _CreateEnemyList.Count; i++) {

            // 이름이 같고 비활성화 되어있는지 검사
            if (_CreateEnemyList[i].name == "TiedPig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
                return i;
        }


        // 비활성화된 묶인 돼지가 없다면 -1을 리턴
        return -1;
    }

    #endregion
    #region 오물돼지
    private IEnumerator SpawnSoilPig() {

        while (_StageState <= _BossInterval * 3) {

            // 스폰할 것인지 뽑을 랜덤 수
            int soilPigRandomSpawnCount = Random.Range(0, 10);
            
            // 0이라면 생성
            if (soilPigRandomSpawnCount < 6) {

                // 오물돼지가 활성화 중이라면
                if (FindEnableSoilPig() == 0) yield return null;

				// 비활성화중인 오물돼지가 있다면
				else if (FindEnableSoilPig() == 1)
					// 오물돼지 활성화 호출
					EnableSoildPig(FindDisableSoilPig());

				// 비활성화중인 오물돼지가 없다면 생성
				else if (FindEnableSoilPig() == 2)
					// 오물돼지 생성 호출
					CreateSoildPig();

			}

            // 스폰딜레이 대기후 다시 뽑기
            yield return new WaitForSeconds(_SoilPigSpawnDelay);
            
        }

		yield return null;
    }

    // 활성화중인 오물돼지가 있는지 검사
    private int FindEnableSoilPig() {

        for (int i = 0; i < _CreateEnemyList.Count; i++) {

            // 이름
            if (_CreateEnemyList[i].name == "SoilPig(Clone)") {

                // 활성화중인 오물돼지가 있다면 0을 리턴
                if (_CreateEnemyList[i].gameObject.activeSelf) return 0;
            }
        }

        // _CreateEnemyList 오물돼지가 없다면
        if (FindDisableSoilPig() == -1) return 2;

        // 오물돼지가 비활성화 중이라면
        return 1;
    }

    // 비활성화중인 오물돼지를 반환
    private int FindDisableSoilPig() {

        for (int i = 0; i < _CreateEnemyList.Count; i++) {
            // 이름
            if (_CreateEnemyList[i].name == "SoilPig(Clone)")

                // 비활성화중인지
                if (!_CreateEnemyList[i].gameObject.activeSelf) return i;
        }

        return -1;
    }

    // 오물돼지 생성
    private void CreateSoildPig()
    {
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        EnemyBase enemy = Instantiate(_EnemyList[6]);

        enemy.transform.position = new Vector2(spawnPosX, 9.5f);

        _CreateEnemyList.Add(enemy);
    }

    // 오물돼지 활성화
    private void EnableSoildPig(int index)
    {
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        _CreateEnemyList[index].transform.gameObject.SetActive(true);

        _CreateEnemyList[index].transform.position = new Vector2(spawnPosX, 9.5f);
    }
    #endregion
    #region 구르는 돼지
    private IEnumerator SpawnRollingPig() {

        while (_StageState <= _BossInterval * 4) {

            // 생성확률을 뽑을 난수
            int spawnNumber = Random.Range(0, 10);

            // 뽑은 난수가 생성확률 보다 낮다면 생성(활성화)
            if (spawnNumber < _RollingPigSpawnPer)
            {
                // 비활성화된 구르는 돼지 찾기
                int index = FindDisableRollingPig();

                // 비활성화된 구르는 돼지를 찾지못했다면 생성
                if (index == -1) CreateRollingPig();

                // 찾았다면 그 구르는 돼지를 활성화
                else EnableRollingPig(index);
            }

            // 딜레이 만큼 대기
            yield return new WaitForSeconds(_RollingPigSpawnTime);
        }

        yield return null;
    }

    // 구르는돼지 생성
    private void CreateRollingPig() {

        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[7]);

        // X 좌표
        float spawnPosX = 0.0f;

        // Y 좌표
        float spawnPosY = 5.5f;

        // 좌표 설정
        enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

        // 리스트에 등록
        _CreateEnemyList.Add(enemy);
    }

    // 비활성화된 구르는 돼지 활성화
    private void EnableRollingPig(int index) {

        // 활성화
        _CreateEnemyList[index].gameObject.SetActive(true);

        // X 좌표
        float spawnPosX = 0.0f;

        // Y 좌표
        float spawnPosY = 5.5f;

        // 좌표 설정
        _CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
    }

    // 비활성화된 구르는 돼지의 인덱스 번호를 찾아서 리턴
    private int FindDisableRollingPig() {

        for (int i = 0; i < _CreateEnemyList.Count; i++) {

            // 이름이 같고 비활성화 되어있는지 검사
            if (_CreateEnemyList[i].name == "RollingPig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
                return i;
        }


        // 비활성화된 구르는 돼지가 없다면 -1을 리턴
        return -1;
    }
	#endregion
	#region 돼지목의 진주목걸이
	private IEnumerator SpawnPearlPig() {

		while (true) {

			// 생성확률을 뽑을 난수
			int spawnNumber = Random.Range(0, 10);

			// 랜덤 스폰타이밍 뽑기
			int randomSpawnTime = Random.Range(20, 31);
            
			// 뽑은 난수가 생성확률 보다 낮다면 생성(활성화)
			if (spawnNumber < 6) {

                // 비활성화된 구르는 돼지 찾기
                int index = FindDisablePearlPig();

                // 비활성화된 구르는 돼지를 찾지못했다면 생성
                if (index == -1) CreatePearlPig();

                // 찾았다면 그 구르는 돼지를 활성화
                else EnablePearlPig(index);
            }

			// 딜레이 만큼 대기
			yield return new WaitForSeconds(randomSpawnTime);
		}

		yield return null;
	}

	// 진주 생성
	private void CreatePearlPig() {

		// 생성
		EnemyBase enemy = Instantiate(_EnemyList[10]);

		// X 좌표
		float spawnPosX = 0.0f;

		// Y 좌표
		float spawnPosY = 5.5f;

		// 좌표 설정
		enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

		// 리스트에 등록
		_CreateEnemyList.Add(enemy);
	}

	// 비활성화된 진주 돼지 활성화
	private void EnablePearlPig(int index) {

		// 활성화
		_CreateEnemyList[index].gameObject.SetActive(true);

		// X 좌표
		float spawnPosX = 0.0f;

		// Y 좌표
		float spawnPosY = 5.5f;

		// 좌표 설정
		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
	}

	// 비활성화된 진주 돼지의 인덱스 번호를 찾아서 리턴
	private int FindDisablePearlPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {

			// 이름이 같고 비활성화 되어있는지 검사
			if (_CreateEnemyList[i].name == "PearlPig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
				return i;
		}


		// 비활성화된 구르는 돼지가 없다면 -1을 리턴
		return -1;
	}
	#endregion
	#region 겁쟁이 돼지
	private IEnumerator SpawnCowardPig() {

		while (true) {

			// 스폰할 것인지 뽑을 랜덤 수
			int RandomSpawnCount = Random.Range(0, 10);

			// 랜덤 스폰타이밍
			int randomSpawnTime = Random.Range(20, 26);

			// 0이라면 생성
			if (RandomSpawnCount < 7) {

                // 활성화 중인 겁쟁이돼지번호 담음
                int index = FindDisableCowardPig();

                // 없다면 생성
                if (index == -1) CreateCowardPig();

                // 있다면 그 번호 활성화
                else EnableCowardPig(index);

            }

			// 스폰딜레이 대기후 다시 뽑기
			yield return new WaitForSeconds(randomSpawnTime);

		}

		yield return null;
	}
    
	// 비활성화중인 겁쟁이돼지를 반환
	private int FindDisableCowardPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {
			// 이름
			if (_CreateEnemyList[i].name == "CowardPig(Clone)")

				// 비활성화중인지
				if (!_CreateEnemyList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// 겁쟁이돼지 생성
	private void CreateCowardPig() {

		// 랜덤 X좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// 생성
		EnemyBase enemy = Instantiate(_EnemyList[8]);

		// 좌표설정
		enemy.transform.position = new Vector2(spawnPosX, 9.5f);

		// 리스트에 추가
		_CreateEnemyList.Add(enemy);
	}

	// 겁쟁이돼지 활성화
	private void EnableCowardPig(int index) {

		// 랜덤 X좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// 활성화
		_CreateEnemyList[index].transform.gameObject.SetActive(true);

		// 좌표설정
		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, 9.5f);
	}
	#endregion
	#region 아기돼지 삼형제
	private IEnumerator SpawnThreePig() {

		while (true) {

			// 스폰할 것인지 뽑을 랜덤 수
			int RandomSpawnCount = Random.Range(0, 10);

			// 랜덤 스폰타이밍
			int randomSpawnTime = Random.Range(20, 28);

			// 0이라면 생성
			if (RandomSpawnCount < 6) {

                // 비활 찾기
                int index = FindDisableThreePig();

                // 없다면 생성
                if (index == -1) CreateThreePig();

                // 있다면 활성화
                else EnableThreePig(index);
			}

			// 스폰딜레이 대기후 다시 뽑기
			yield return new WaitForSeconds(randomSpawnTime);

		}

		yield return null;
	}
    
	// 비활성화중인 아기돼지 삼형제를 반환
	private int FindDisableThreePig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {
			// 이름
			if (_CreateEnemyList[i].name == "ThreePig(Clone)")

				// 비활성화중인지
				if (!_CreateEnemyList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// 아기돼지 삼형제 생성
	private void CreateThreePig() {

		// 랜덤 X좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// 생성
		EnemyBase enemy = Instantiate(_EnemyList[9]);

		// 좌표설정
		enemy.transform.position = new Vector2(spawnPosX, 9.5f);

		// 리스트에 추가
		_CreateEnemyList.Add(enemy);
	}

	// 아기돼지 삼형제 활성화
	private void EnableThreePig(int index) {

		// 랜덤 X좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// 활성화
		_CreateEnemyList[index].transform.gameObject.SetActive(true);

		// 좌표설정
		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, 9.5f);
	}
	#endregion
	#region 진 오물돼지
	private IEnumerator SpawnSludgePig() {

		while (true) {

			// 스폰할 것인지 뽑을 랜덤 수
			int RandomSpawnCount = Random.Range(0, 100);

			// 0이라면 생성
			if (RandomSpawnCount < 65) {

				// 진오물돼지가 활성화 중이라면
				if (FindEnableSludgePig() == 0) yield return null;

				// 비활성화중인 진오물돼지가 있다면
				else if (FindEnableSludgePig() == 1)
					// 오물돼지 활성화 호출
					EnableSludgePig(FindDisableSludgePig());

				// 비활성화중인 오물돼지가 없다면 생성
				else if (FindEnableSludgePig() == 2)
					// 오물돼지 생성 호출
					CreateSludgePig();

			}

			// 스폰딜레이 대기후 다시 뽑기
			yield return new WaitForSeconds(20.0f);

		}

		yield return null;
	}

	// 활성화중인 오물돼지가 있는지 검사
	private int FindEnableSludgePig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {

			// 이름
			if (_CreateEnemyList[i].name == "SludgePig(Clone)") {

				// 활성화중인 오물돼지가 있다면 0을 리턴
				if (_CreateEnemyList[i].gameObject.activeSelf) return 0;
			}
		}

		// _CreateEnemyList 오물돼지가 없다면
		if (FindDisableSludgePig() == -1) return 2;

		// 오물돼지가 비활성화 중이라면
		return 1;
	}

	// 비활성화중인 오물돼지를 반환
	private int FindDisableSludgePig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {
			// 이름
			if (_CreateEnemyList[i].name == "SludgePig(Clone)")

				// 비활성화중인지
				if (!_CreateEnemyList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// 오물돼지 생성
	private void CreateSludgePig() {
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		EnemyBase enemy = Instantiate(_EnemyList[12]);

		enemy.transform.position = new Vector2(spawnPosX, 9.5f);

		_CreateEnemyList.Add(enemy);
	}

	// 오물돼지 활성화
	private void EnableSludgePig(int index) {
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		_CreateEnemyList[index].transform.gameObject.SetActive(true);

		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, 9.5f);
	}
	#endregion
	#region 마법요정 돼지
	private IEnumerator SpawnMagicPig() {

		while (true) {

			// 스폰할 것인지 뽑을 랜덤 수
			int RandomSpawnCount = Random.Range(0, 100);

            // 랜덤스폰 시간
            int randomSpawnTime = Random.Range(30, 36);

			// 0이라면 생성
			if (RandomSpawnCount < 65) {

				// 오물돼지가 활성화 중이라면
				if (FindEnableMagicPig() == 0) yield return null;

				// 비활성화중인 오물돼지가 있다면
				else if (FindEnableMagicPig() == 1)
					// 오물돼지 활성화 호출
					EnableMagicPig(FindDisableMagicPig());

				// 비활성화중인 오물돼지가 없다면 생성
				else if (FindEnableMagicPig() == 2)
					// 오물돼지 생성 호출
					CreateMagicPig();

			}

			// 스폰딜레이 대기후 다시 뽑기
			yield return new WaitForSeconds(randomSpawnTime);

		}

		yield return null;
	}

	// 활성화중인 오물돼지가 있는지 검사
	private int FindEnableMagicPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {

			// 이름
			if (_CreateEnemyList[i].name == "MagicPig(Clone)") {

				// 활성화중인 오물돼지가 있다면 0을 리턴
				if (_CreateEnemyList[i].gameObject.activeSelf) return 0;
			}
		}

		// _CreateEnemyList 오물돼지가 없다면
		if (FindDisableMagicPig() == -1) return 2;

		// 오물돼지가 비활성화 중이라면
		return 1;
	}

	// 비활성화중인 오물돼지를 반환
	private int FindDisableMagicPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {
			// 이름
			if (_CreateEnemyList[i].name == "MagicPig(Clone)")

				// 비활성화중인지
				if (!_CreateEnemyList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// 마법돼지 생성
	private void CreateMagicPig() {
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		EnemyBase enemy = Instantiate(_EnemyList[11]);

		enemy.transform.position = new Vector2(spawnPosX, 9.5f);

		_CreateEnemyList.Add(enemy);
	}

	// 마법돼지 활성화
	private void EnableMagicPig(int index) {
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		_CreateEnemyList[index].transform.gameObject.SetActive(true);

		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, 9.5f);
	}
	#endregion
	#region 튀는 돼지
	private IEnumerator SpawnBouncingPig() {

		while (true) {

			// 스폰할 것인지 뽑을 랜덤 수
			int RandomSpawnCount = Random.Range(0, 10);

            // 랜덤 스폰시간
            int randomSpawnTime = Random.Range(20, 26);

			// 0이라면 생성
			if (RandomSpawnCount < 7) {

				// 오물돼지가 활성화 중이라면
				if (FindEnableBouncingPig() == 0) yield return null;

				// 비활성화중인 오물돼지가 있다면
				else if (FindEnableBouncingPig() == 1)
					// 오물돼지 활성화 호출
				    EnableBouncingPig(FindDisableBouncingPig());

				// 비활성화중인 오물돼지가 없다면 생성
				else if (FindEnableBouncingPig() == 2)
					// 오물돼지 생성 호출
					CreateBouncingPig();

			}

			// 스폰딜레이 대기후 다시 뽑기
			yield return new WaitForSeconds(randomSpawnTime);

		}

		yield return null;
	}

	// 활성화중인 오물돼지가 있는지 검사
	private int FindEnableBouncingPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {

			// 이름
			if (_CreateEnemyList[i].name == "BouncingPig(Clone)") {

				// 활성화중인 오물돼지가 있다면 0을 리턴
				if (_CreateEnemyList[i].gameObject.activeSelf) return 0;
			}
		}

		// _CreateEnemyList 오물돼지가 없다면
		if (FindDisableBouncingPig() == -1) return 2;

		// 오물돼지가 비활성화 중이라면
		return 1;
	}

	// 비활성화중인 오물돼지를 반환
	private int FindDisableBouncingPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {
			// 이름
			if (_CreateEnemyList[i].name == "BouncingPig(Clone)")

				// 비활성화중인지
				if (!_CreateEnemyList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// 오물돼지 생성
	private void CreateBouncingPig() {
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		EnemyBase enemy = Instantiate(_EnemyList[13]);

		enemy.transform.position = new Vector2(spawnPosX, 9.5f);

		_CreateEnemyList.Add(enemy);
	}

	// 오물돼지 활성화
	private void EnableBouncingPig(int index) {
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		_CreateEnemyList[index].transform.gameObject.SetActive(true);

		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, 9.5f);
	}
	#endregion
	#region Mr.Dangerous, MissDucksec
	private IEnumerator SpawnMrDangerous() {

		while (true) {

			// 스폰할 것인지 뽑을 랜덤 수
			int RandomSpawnCount = Random.Range(0, 10);

            // 랜덤 스폰시간
            int randomSpawnTime = Random.Range(30, 36);

			// 8보다 작다면 생성
			if (RandomSpawnCount < 8) {

				// MissDucksec이 등장할 것인지 뽑을 확률
				int randomMiss = Random.Range(0, 5);

				// Miss
				if (randomMiss < 2) {

					// 비활성화 중인 Miss가 있다면담기
					int index = FindDisableMissDucksec();

					// 생성
					if (index == -1) CreateMissDucksec();

					// 활성화
					else EnableMissDucksec(index);

				}

				// Mr
				else {

					// Mr를 담음
					int index = FindDisableMrDangerous();

					// Mr가 활성화 중이라면
					if (index == -1) CreateMrDangerous();

                    // 비활성화중인 Mr가 있다면
                    else EnableMrDangerous(index);
				}

			}

			// 스폰딜레이 대기후 다시 뽑기
			yield return new WaitForSeconds(10.0f);

		}

		yield return null;
	}

	
	// 비활성화중인 Mr를 반환
	private int FindDisableMrDangerous() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {
			// 이름
			if (_CreateEnemyList[i].name == "MrDangerous(Clone)")

				// 비활성화중인지
				if (!_CreateEnemyList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// Mr 생성
	private void CreateMrDangerous() {
		float spawnPosX = Random.Range(-1.99f, 1.99f);

		EnemyBase enemy = Instantiate(_EnemyList[14]);

		enemy.transform.position = new Vector2(spawnPosX, 9.5f);

		_CreateEnemyList.Add(enemy);
	}

	// Mr 활성화
	private void EnableMrDangerous(int index) {
		float spawnPosX = Random.Range(-1.99f, 1.99f);

		_CreateEnemyList[index].transform.gameObject.SetActive(true);

		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, 9.5f);
	}

	// Miss 생성
	private void CreateMissDucksec(){
		float spawnPosX = Random.Range(-1.99f, 1.99f);

		EnemyBase enemy = Instantiate(_EnemyList[20]);

		enemy.transform.position = new Vector2(spawnPosX, 9.5f);

		_CreateEnemyList.Add(enemy);
	}

	// 비활성화된 Miss찾기
	private int FindDisableMissDucksec(){
		
		for (int i = 0; i < _CreateEnemyList.Count; i++){

			// 이름이 같고
			if (_CreateEnemyList[i].name == "MissDucksec(Clone)")

				// 비활성화 되어있다면
				if (!_CreateEnemyList[i].gameObject.activeSelf) return i;
		}

		return -1;
	}

	// Miss 활성화
	private void EnableMissDucksec(int index){
		float spawnPosX = Random.Range(-1.99f, 1.99f);

		_CreateEnemyList[index].transform.gameObject.SetActive(true);

		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, 9.5f);

	}

	#endregion
	#region 파괴자 돼지 (3단계)
	private IEnumerator SpawnDestroyerPig() {

        while (true) {

            // 랜덤 스폰확률
            int randomNumber = Random.Range(0, 10);

            // 랜덤 스폰시간
            _DestroyerPigSpawnTime = Random.Range(35, 40);

            // 생성 가능하다면
            if (randomNumber < _DestroyerPigSpawnPer) {

				// 비활성화된 돼지가 있다면 인덱스 번호를 저장할 변수
				int index = FindDisableDestroyerPig();

				// 비활성화된 돼지가 없다면 생성
				if (index == -1) CreateDestroyerPig();

				// 있다면 활성화
				else EnableDestroyerPig(index);
			}

            // 딜레이 만큼 대기
            yield return new WaitForSeconds(_DestroyerPigSpawnTime);
        }

        yield return null;
    }

    // 파괴자 돼지 생성
    private void CreateDestroyerPig() {

        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[15]);

        // X 좌표
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        // Y 좌표
        float spawnPosY = Random.Range(5.5f, 6.5f);

        // 좌표 설정
        enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

        // 리스트에 등록
        _CreateEnemyList.Add(enemy);
    }

    // 비활성화된 파괴자 돼지 활성화
    private void EnableDestroyerPig(int index) {

        // 활성화
        _CreateEnemyList[index].gameObject.SetActive(true);

        // X 좌표
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        // Y 좌표
        float spawnPosY = Random.Range(5.5f, 6.5f);

        // 좌표 설정
        _CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
    }

    // 비활성화된 파괴자 돼지의 인덱스 번호를 찾아서 리턴
    private int FindDisableDestroyerPig() {

        for (int i = 0; i < _CreateEnemyList.Count; i++) {

            // 이름이 같고 비활성화 되어있는지 검사
            if (_CreateEnemyList[i].name == "DestroyerPig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
                return i;
        }


        // 비활성화된 파괴자 돼지가 없다면 -1을 리턴
        return -1;
    }
    #endregion
    #region 철갑돼지
    private IEnumerator SpawnIronPig() {

        while (true) {

            if (true) {
                
                // 랜덤 스폰확률
                int randomNumber = Random.Range(0, 10);

                // 랜덤 스폰시간
                _IronPigSpawnTime = Random.Range(40, 46);

                // 생성 가능하다면
                if (randomNumber < _IronPigSpawnPer) {

                    // 비활성화된 철갑돼지가 있다면 인덱스 번호를 저장할 변수
                    int index = FindDisableIronPig();

                    // 비활성화된 철갑돼지가 없다면 생성
                    if (index == -1) CreateIronPig();

                    // 있다면 활성화
                    else EnableIronPig(index);
                }

                // 딜레이 만큼 대기
                yield return new WaitForSeconds(_IronPigSpawnTime);
            }

            yield return null;
        }
    }

    // 철갑돼지 생성
    private void CreateIronPig() {

        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[16]);

        // X 좌표
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        // Y 좌표
        float spawnPosY = Random.Range(5.5f, 6.5f);

        // 좌표 설정
        enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

        // 리스트에 등록
        _CreateEnemyList.Add(enemy);
    }

    // 비활성화된 철갑돼지 활성화
    private void EnableIronPig(int index) {

        // 활성화
        _CreateEnemyList[index].gameObject.SetActive(true);

        // X 좌표
        float spawnPosX = Random.Range(-2.0f, 2.0f);

        // Y 좌표
        float spawnPosY = Random.Range(5.5f, 6.5f);

        // 좌표 설정
        _CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
    }

    // 비활성화된 철갑돼지의 인덱스 번호를 찾아서 리턴
    private int FindDisableIronPig() {

        for (int i = 0; i < _CreateEnemyList.Count; i++) {

            // 이름이 같고 비활성화 되어있는지 검사
            if (_CreateEnemyList[i].name == "IronPig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
                return i;
        }


        // 비활성화된 철갑돼지가 없다면 -1을 리턴
        return -1;
    }
	#endregion
	#region 우두머리 돼지
	private IEnumerator SpawnGuvPig() {

		while (true) {

			int randomNumber = Random.Range(0, 10);

			// 랜덤적으로 스폰딜레이 설정
			int randomSpawnCount = Random.Range(40, 51);

			// 생성 가능하다면
			if (randomNumber < 5) {

				// 비활성화된 돼지가 있다면 인덱스 번호를 저장할 변수
				int index = FindDisableGuvPig();

				// 비활성화된 돼지가 없다면 생성
				if (index == -1) CreateGuvPig();

				// 있다면 활성화
				else EnableGuvPig(index);
			}

			// 딜레이 만큼 대기
			yield return new WaitForSeconds(randomSpawnCount);
		}

		yield return null;
	}

	// 파괴자 돼지 생성
	private void CreateGuvPig() {

		// 생성
		EnemyBase enemy = Instantiate(_EnemyList[17]);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);

		// 좌표 설정
		enemy.transform.position = new Vector2(spawnPosX, spawnPosY);

		// 리스트에 등록
		_CreateEnemyList.Add(enemy);
	}

	// 비활성화된 파괴자 돼지 활성화
	private void EnableGuvPig(int index) {

		// 활성화
		_CreateEnemyList[index].gameObject.SetActive(true);

		// X 좌표
		float spawnPosX = Random.Range(-2.0f, 2.0f);

		// Y 좌표
		float spawnPosY = Random.Range(5.5f, 6.5f);

		// 좌표 설정
		_CreateEnemyList[index].transform.position = new Vector2(spawnPosX, spawnPosY);
	}

	// 비활성화된 파괴자 돼지의 인덱스 번호를 찾아서 리턴
	private int FindDisableGuvPig() {

		for (int i = 0; i < _CreateEnemyList.Count; i++) {

			// 이름이 같고 비활성화 되어있는지 검사
			if (_CreateEnemyList[i].name == "GuvPig(Clone)" && !_CreateEnemyList[i].gameObject.activeSelf)
				return i;
		}


		// 비활성화된 파괴자 돼지가 없다면 -1을 리턴
		return -1;
	}
	#endregion
	#region 100인분 돼지 (보스)
    // 100인분 돼지 생성
    private void CreateHundredPig() {

        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[18]);

        // 좌표 설정
        enemy.transform.position = new Vector2(0.0f, 2.5f);

        // 리스트에 추가
        _CreateEnemyList.Add(enemy);
    }
    #endregion
    #region 아롱이
    private void CreateArongE()
    {
        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[19]);

        // 좌표 설정
        enemy.transform.position = new Vector2(0.0f, 1.0f);

        // 리스트에 추가
        _CreateEnemyList.Add(enemy);
    }
    #endregion
    #region 아기돼지집
    private void CreatePigHouse()
    {
        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[21]);

        // 좌표 설정
        enemy.transform.position = new Vector2(0.0f, 2.5f);

        // 리스트에 추가
        _CreateEnemyList.Add(enemy);
    }
    #endregion
    #region 까마귀
    private void CreateCrow()
    {
        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[22]);

        // 좌표설정
        enemy.transform.position = new Vector2(0.0f, 3.0f);

        // 리스트에 추가
        _CreateEnemyList.Add(enemy);
    }
    #endregion
    #region 돼지왕
    private void CreatePigKing()
    {
        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[23]);

        // 좌표설정
        enemy.transform.position = new Vector2(0.0f, 3.0f);

        // 리스트에 추가
        _CreateEnemyList.Add(enemy);
    }
    #endregion
    #region 복돼지
    private void CreateGoldenPig()
    {
        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[24]);

        // 좌표설정
        enemy.transform.position = new Vector2(0.0f, 3.0f);

        // 리스트에 추가
        _CreateEnemyList.Add(enemy);
    }
    #endregion
    #region 도살자
    private void CreateButcher()
    {
        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[26]);

        // 좌표설정
        enemy.transform.position = new Vector2(0.0f, 3.0f);

        // 리스트에 추가
        _CreateEnemyList.Add(enemy);
    }
    #endregion
    #region 에스더
    private void CreateEsther()
    {
        // 생성
        EnemyBase enemy = Instantiate(_EnemyList[25]);

        // 좌표설정
        enemy.transform.position = new Vector2(0.0f, 3.0f);

        // 리스트에 추가
        _CreateEnemyList.Add(enemy);
    }
    #endregion

}
