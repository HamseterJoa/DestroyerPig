using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnchantButton : MonoBehaviour
{
	// 스크립트를 넣어준 오브젝트의 종류
	private enum ObjectState { Icon, purchaseBtn }
	[SerializeField] private ObjectState _ObjectState = ObjectState.Icon;

	// 강화할 아이템의 종류
	public enum EnchantState { MaxHP, HackMaxCount, HackPlus, AddMoney, AddTime, HackDamage, BallDamage, KillPigHeal, HackSize, _BossInterval, AddVigor }
    public EnchantState _EnchantState = EnchantState.MaxHP;
    #region 강화진행도
    // 최대체력 강화진행도(게이지)
    [SerializeField] private Image _MaxHPEnchantStateBar;

    // 최대체력 강화진행도(숫자)
    [SerializeField] private Text _MaxHPEnchantStateText;

    // 핵최대치 강화진행도(게이지)
    [SerializeField] private Image _MaxHackEnchantStateBar;

    // 핵최대치 강화진행도(숫자)
    [SerializeField] private Text _MaxHackEnchantStateText;
    
    // 추가돈 강화진행도(게이지)
    [SerializeField] private Image _AddMoneyEnchantStateBar;

    // 추가돈 강화진행도(숫자)
    [SerializeField] private Text _AddMoneyEnchantStateText;

    // 추가시간 강화진행도(게이지)
    [SerializeField] private Image _AddTimeEnchantStateBar;

    // 추가시간 강화진행도(숫자)
    [SerializeField] private Text _AddTimeEnchantStateText;

    // 핵데미지 강화진행도(게이지)
    [SerializeField] private Image _HackDamageEnchantStateBar;

    // 핵데미지 강화진행도(숫자)
    [SerializeField] private Text _HackDamageEnchantStateText;

    // 공데미지 강화진행도(게이지)
    [SerializeField] private Image _BallDamageEnchantStateBar;

    // 공데미지 강화진행도(숫자)
    [SerializeField] private Text _BallDamageEnchantStateText;

    // 돼지처치힐 강화진행도(게이지)
    [SerializeField] private Image _KillPigHealEnchantStateBar;

    // 돼지처치힐 강화진행도(숫자)
    [SerializeField] private Text _KillPigHealEnchantStateText;

    // 핵크기 강화진행도(게이지)
    [SerializeField] private Image _HackSizeEnchantStateBar;

    // 핵크기 강화진행도(숫자)
    [SerializeField] private Text _HackSizeEnchantStateText;

    // 건너띌시간 강화진행도(게이지)
    [SerializeField] private Image _StartTimeEnchantStateBar;

    // 건너띌시간 강화진행도(숫자)
    [SerializeField] private Text _StartTimeEnchantStateText;

    // 추가행동력 강화진행도(게이지)
    [SerializeField] private Image _AddVigorEnchantStateBar;

    // 추가행동력 강화진행도(숫자)
    [SerializeField] private Text _AddVigorEnchantStateText;
    #endregion
    // 구매불가 텍스트
    [SerializeField] private Text _EnoughText;

    // 구매불가 이미지
    [SerializeField] private Image _EnoughImage;

    // 구매 이미지
    [SerializeField] private Image _PurchaseImage;

    // 능력치 변경점
    [SerializeField] private Text _Stat;

    // 아이템의 정보
    [SerializeField] private Text _Info;

    // 구매 조건
    [SerializeField] private Text _PurchaseableStage;

    // 구매 가격
    [SerializeField] private Text _PurchaseablePrice;

    // 아이템 구매 버튼
    [SerializeField] private GameObject _ItemBtn;

    // 플레이어 구매 버튼
    [SerializeField] private PlayerEnchantButton _PlayerBtn;

    // 캔버스
    [SerializeField] private EnchantCanvas _EnchantCanvas;

    // 가격
    private uint _Price;

    // 구매 조건
    private uint _Stage;

    // 최대레벨
    private uint _EnchantMaxLv;

    private JsonData.PlayerData playerData;
    private JsonData.ItemEnchant itemEnchant;

    // 코인, 기록 부족시 나올 목소리
    [SerializeField] private AudioSource _EnoughtCoinAudio;
    [SerializeField] private AudioSource _EnoughtRecordAudio;

    // 설명 버튼
    [SerializeField] private GameObject _ExplanationBtn;

    // 코인 부족
    private void PlayEnoughtCoin()
    {
        // 볼륨 설정
        _EnoughtCoinAudio.volume = GameManager.getJsonDataManager.playerData.VoiceValue;

        // 재생
        _EnoughtCoinAudio.Play();
    }

    // 기록 부족
    private void PlayEnoughtRecord()
    {
        // 볼륨 설정
        _EnoughtRecordAudio.volume = GameManager.getJsonDataManager.playerData.VoiceValue;

        // 재생
        _EnoughtRecordAudio.Play();
    }

    // 터치 소리
    [SerializeField] private AudioSource _Audio;

    //  > 버튼 클릭 사운드 재생
    private void PlayClickSound()
    {
        // 볼륨 설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _Audio.Play();
    }

    private void OnEnable()
    {
		// 최대 강화횟수 설정
		EnchantMaxLV();

		// 가격 구매조건
		EnchantPriceUpdate();
		EnchantStageUpdate();

        // 아이템, 플레이어 정보 설정
        playerData = GameManager.getJsonDataManager.playerData;
        itemEnchant = GameManager.getJsonDataManager.itemEnchant;

        if (_MaxHPEnchantStateBar != null)
        {
            // 강화진행도 설정
            EnchantStateGaugeUpdate();
            EnchantStateTextUpdate();
        }
    }
    
	// 최대 강화 횟수
	private void EnchantMaxLV(){
		switch (_EnchantState) {
			case EnchantState.MaxHP: _EnchantMaxLv = 30; break;
			case EnchantState.HackMaxCount: _EnchantMaxLv = 3; break;
			//case EnchantState.HackPlus: _EnchantMaxLv = 3; break;
			case EnchantState.AddMoney: _EnchantMaxLv = 30; break;
			case EnchantState.AddTime: _EnchantMaxLv = 6; break;
			case EnchantState.HackDamage: _EnchantMaxLv = 6; break;
			case EnchantState.BallDamage: _EnchantMaxLv = 2; break;
			case EnchantState.KillPigHeal: _EnchantMaxLv = 10; break;
			case EnchantState.HackSize: _EnchantMaxLv = 2; break;
			case EnchantState._BossInterval: _EnchantMaxLv = 3; break;
			case EnchantState.AddVigor: _EnchantMaxLv = 4; break;
		}
	}

	// 강화레벨별 가격
	private void EnchantPriceUpdate(){

		// 가격 구매조건
		switch (_EnchantState) {

			// 최대체력
			case EnchantState.MaxHP:
				switch (GameManager.getJsonDataManager.itemEnchant.PlayerMaxHPLV) {
					case 0: _Price = 1000; break; //1
					case 1: _Price = 2000; break; //2
					case 2: _Price = 3000; break; //3
					case 3: _Price = 4000; break; //4
					case 4: _Price = 4000; break; //5
					case 5: _Price = 6000; break; //6
					case 6: _Price = 6000; break; //7
					case 7: _Price = 8000; break; //8
					case 8: _Price = 10000; break; //9
					case 9: _Price = 12000; break; //10
					case 10: _Price = 14000; break; //11
					case 11: _Price = 17000; break; //12
					case 12: _Price = 19000; break; //13
					case 13: _Price = 25000; break; //14
					case 14: _Price = 27000; break; //15
					case 15: _Price = 32000; break; //16
					case 16: _Price = 40000; break; //17
					case 17: _Price = 41000; break; //18
					case 18: _Price = 42000; break; //19
					case 19: _Price = 44000; break; //20
					case 20: _Price = 50000; break;
					case 21: _Price = 52000; break;
					case 22: _Price = 53000; break;
					case 23: _Price = 60000; break;
					case 24: _Price = 61000; break;
					case 25: _Price = 62000; break;
					case 26: _Price = 64000; break;
					case 27: _Price = 65000; break;
					case 28: _Price = 67000; break;
					case 29: _Price = 68000; break;
				}

				break;

			// 핵최대치
			case EnchantState.HackMaxCount:
				switch (GameManager.getJsonDataManager.itemEnchant.HackMaxCountLV) {
					case 0: _Price = 30000; break;
					case 1: _Price = 50000; break;
					case 2: _Price = 60000; break;
				}

				break;

			// 핵충전량
			//case EnchantState.HackPlus:
			//    switch (GameManager.getJsonDataManager.itemEnchant.HackPlusLV) {
			//        case 0: _Price = 60000; _Stage = 3; break;
			//        case 1: _Price = 120000; _Stage = 15; break;
			//        case 2: _Price = 240000; _Stage = 20; break;
			//    }
			//    break;

			// 코인추가증가량
			case EnchantState.AddMoney:
				switch (GameManager.getJsonDataManager.itemEnchant.AddMoneyLV) {
					case 0: _Price = 2000; break;
					case 1: _Price = 4000; break;
					case 2: _Price = 6000; break;
					case 3: _Price = 8000; break;
					case 4: _Price = 10000; break;
					case 5: _Price = 12000; break;
					case 6: _Price = 14000; break;
					case 7: _Price = 16000; break;
					case 8: _Price = 18000; break;
					case 9: _Price = 20000; break;
					case 10: _Price = 22000; break;
					case 11: _Price = 24000; break;
					case 12: _Price = 26000; break;
					case 13: _Price = 28000; break;
					case 14: _Price = 30000; break;
					case 15: _Price = 32000; break;
					case 16: _Price = 34000; break;
					case 17: _Price = 36000; break;
					case 18: _Price = 38000; break;
					case 19: _Price = 40000; break;
					case 20: _Price = 42000; break;
					case 21: _Price = 44000; break;
					case 22: _Price = 46000; break;
					case 23: _Price = 48000; break;
					case 24: _Price = 50000; break;
					case 25: _Price = 52000; break;
					case 26: _Price = 54000; break;
					case 27: _Price = 56000; break;
					case 28: _Price = 58000; break;
					case 29: _Price = 60000; break;
				}

				break;

			// 체력감소딜레이증가
			case EnchantState.AddTime:
				switch (GameManager.getJsonDataManager.itemEnchant.AddTimeLV) {
					case 0: _Price = 5000; break;
					case 1: _Price = 10000; break;
					case 2: _Price = 18000; break;
					case 3: _Price = 22000; break;
					case 4: _Price = 25000; break;
					case 5: _Price = 26000; break;
				}

				break;

			// 핵데미지증가
			case EnchantState.HackDamage:
				switch (GameManager.getJsonDataManager.itemEnchant.HackDamageLV) {
					case 0: _Price = 5000; break;
					case 1: _Price = 9500; break;
					case 2: _Price = 12000; break;
					case 3: _Price = 16000; break;
					case 4: _Price = 22000; break;
					case 5: _Price = 27500; break;
				}

				break;

			// 공 데미지
			case EnchantState.BallDamage:
				switch (GameManager.getJsonDataManager.itemEnchant.BallDamageLV) {
					case 0: _Price = 24000; break;
					case 1: _Price = 72000; break;
				}

				break;

			// 돼지 처치시 얻는 회복량
			case EnchantState.KillPigHeal:
				switch (GameManager.getJsonDataManager.itemEnchant.KillPigHealLV) {
					case 0: _Price = 1000; break;
					case 1: _Price = 3000; break;
					case 2: _Price = 5000; break;
					case 3: _Price = 7000; break;
					case 4: _Price = 10000; break;
					case 5: _Price = 12000; break;
					case 6: _Price = 15000; break;
					case 7: _Price = 17000; break;
					case 8: _Price = 22000; break;
					case 9: _Price = 25000; break;
				}

				break;

			// 핵 범위
			case EnchantState.HackSize:
				switch (GameManager.getJsonDataManager.itemEnchant.HackSizeLV) {
					case 0: _Price = 45000; break;
					case 1: _Price = 80000; break;
				}

				break;

			// 스테이지 간격
			case EnchantState._BossInterval:
				switch (GameManager.getJsonDataManager.itemEnchant.BossIntervalLV) {
					case 0: _Price = 70000; break;
					case 1: _Price = 90000; break;
					case 2: _Price = 110000; break;
				}

				break;

			// 추가 행동력
			case EnchantState.AddVigor:
				switch (GameManager.getJsonDataManager.itemEnchant.AddVigorLV) {
					case 0: _Price = 2000; break;
					case 1: _Price = 5000; break;
					case 2: _Price = 10000; break;
					case 3: _Price = 20000; break;
				}

				break;
		}
	}

	// 강화레벨별 구매조건
	private void EnchantStageUpdate(){
		switch (_EnchantState) {
			// 최대체력
			case EnchantState.MaxHP:
				switch (GameManager.getJsonDataManager.itemEnchant.PlayerMaxHPLV) {
					case 0: _Stage = 0; break; //1
					case 1: _Stage = 0; break; //2
					case 2: _Stage = 0; break; //3
					case 3: _Stage = 3; break; //4
					case 4: _Stage = 5; break; //5
					case 5: _Stage = 7; break; //6
					case 6: _Stage = 8; break; //7
					case 7: _Stage = 9; break; //8
					case 8: _Stage = 10; break; //9
					case 9: _Stage = 10; break; //10
					case 10: _Stage = 10; break; //11
					case 11: _Stage = 10; break; //12
					case 12: _Stage = 10; break; //13
					case 13: _Stage = 20; break; //14
					case 14: _Stage = 20; break; //15
					case 15: _Stage = 20; break; //16
					case 16: _Stage = 25; break; //17
					case 17: _Stage = 25; break; //18
					case 18: _Stage = 25; break; //19
					case 19: _Stage = 25; break; //20
					case 20: _Stage = 30; break;
					case 21: _Stage = 30; break;
					case 22: _Stage = 30; break;
					case 23: _Stage = 35; break;
					case 24: _Stage = 35; break;
					case 25: _Stage = 35; break;
					case 26: _Stage = 40; break;
					case 27: _Stage = 40; break;
					case 28: _Stage = 40; break;
					case 29: _Stage = 40; break;
				}

				break;

			// 핵최대치
			case EnchantState.HackMaxCount:
				switch (GameManager.getJsonDataManager.itemEnchant.HackMaxCountLV) {
					case 0: _Stage = 20; break;
					case 1: _Stage = 40; break;
					case 2: _Stage = 60; break;
				}

				break;

			// 핵충전량
			//case EnchantState.HackPlus:
			//    switch (GameManager.getJsonDataManager.itemEnchant.HackPlusLV) {
			//        case 0: _Price = 60000; _Stage = 3; break;
			//        case 1: _Price = 120000; _Stage = 15; break;
			//        case 2: _Price = 240000; _Stage = 20; break;
			//    }
			//    break;

			// 코인추가증가량
			case EnchantState.AddMoney:
				switch (GameManager.getJsonDataManager.itemEnchant.AddMoneyLV) {
					case 0: _Stage = 0; break;
					case 1: _Stage = 0; break;
					case 2: _Stage = 0; break;
					case 3: _Stage = 5; break;
					case 4: _Stage = 5; break;
					case 5: _Stage = 5; break;
					case 6: _Stage = 10; break;
					case 7: _Stage = 10; break;
					case 8: _Stage = 10; break;
					case 9: _Stage = 15; break;
					case 10: _Stage = 15; break;
					case 11: _Stage = 15; break;
					case 12: _Stage = 15; break;
					case 13: _Stage = 20; break;
					case 14: _Stage = 20; break;
					case 15: _Stage = 20; break;
					case 16: _Stage = 25; break;
					case 17: _Stage = 25; break;
					case 18: _Stage = 25; break;
					case 19: _Stage = 30; break;
					case 20: _Stage = 30; break;
					case 21: _Stage = 30; break;
					case 22: _Stage = 35; break;
					case 23: _Stage = 35; break;
					case 24: _Stage = 35; break;
					case 25: _Stage = 40; break;
					case 26: _Stage = 40; break;
					case 27: _Stage = 40; break;
					case 28: _Stage = 45; break;
					case 29: _Stage = 50; break;
				}

				break;

			// 체력감소딜레이증가
			case EnchantState.AddTime:
				switch (GameManager.getJsonDataManager.itemEnchant.AddTimeLV) {
					case 0: _Stage = 3; break;
					case 1: _Stage = 3; break;
					case 2: _Stage = 12; break;
					case 3: _Stage = 12; break;
					case 4: _Stage = 20; break;
					case 5: _Stage = 20; break;
				}

				break;

			// 핵데미지증가
			case EnchantState.HackDamage:
				switch (GameManager.getJsonDataManager.itemEnchant.HackDamageLV) {
					case 0: _Stage = 0; break;
					case 1: _Stage = 7; break;
					case 2: _Stage = 10; break;
					case 3: _Stage = 15; break;
					case 4: _Stage = 20; break;
					case 5: _Stage = 25; break;
				}

				break;

			// 공 데미지
			case EnchantState.BallDamage:
				switch (GameManager.getJsonDataManager.itemEnchant.BallDamageLV) {
					case 0: _Stage = 20; break;
					case 1: _Stage = 60; break;
				}

				break;

			// 돼지 처치시 얻는 회복량
			case EnchantState.KillPigHeal:
				switch (GameManager.getJsonDataManager.itemEnchant.KillPigHealLV) {
					case 0: _Stage = 3; break;
					case 1: _Stage = 5; break;
					case 2: _Stage = 5; break;
					case 3: _Stage = 10; break;
					case 4: _Stage = 10; break;
					case 5: _Stage = 15; break;
					case 6: _Stage = 15; break;
					case 7: _Stage = 25; break;
					case 8: _Stage = 25; break;
					case 9: _Stage = 30; break;
				}

				break;

			// 핵 범위
			case EnchantState.HackSize:
				switch (GameManager.getJsonDataManager.itemEnchant.HackSizeLV) {
					case 0: _Stage = 40; break;
					case 1: _Stage = 70; break;
				}

				break;

			// 스테이지 간격
			case EnchantState._BossInterval:
				switch (GameManager.getJsonDataManager.itemEnchant.BossIntervalLV) {
					case 0: _Stage = 80; break;
					case 1: _Stage = 80; break;
					case 2: _Stage = 80; break;
				}

				break;

			// 추가 행동력
			case EnchantState.AddVigor:
				switch (GameManager.getJsonDataManager.itemEnchant.AddVigorLV) {
					case 0: _Stage = 5; break;
					case 1: _Stage = 15; break;
					case 2:  _Stage = 25; break;
					case 3:  _Stage = 35; break;
				}

				break;
		}
	}

	// 구매창 업데이트
	private void PurchaseImageUpdate(){

        // 능력치 설명을 담을 거
        StringBuilder stat;

        // 구매 조건을 담을 거
        StringBuilder stage;

        switch (_EnchantState) {
			// 최대체력
			case EnchantState.MaxHP:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("최대체력\n");
                stat.Append(RPlayerMaxHP(itemEnchant.PlayerMaxHPLV).ToString());
                stat.Append(" → ");
                stat.Append(RPlayerMaxHP(itemEnchant.PlayerMaxHPLV + 1).ToString());

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "최대체력의 량을 늘려주는 강화야!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.PlayerMaxHPLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RPlayerMaxHP(itemEnchant.PlayerMaxHPLV).ToString());
                    stat.Append(")");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;

			// 핵최대치
			case EnchantState.HackMaxCount:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("최대 돼지코 충전량\n");
                stat.Append(RHackMaxCount(itemEnchant.HackMaxCountLV).ToString());
                stat.Append("개 → ");
                stat.Append(RHackMaxCount(itemEnchant.HackMaxCountLV + 1).ToString());
                stat.Append("개");

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "[돼지코]의 충전량을 더욱 늘려줘!!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.HackMaxCountLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RHackMaxCount(itemEnchant.HackMaxCountLV).ToString());
                    stat.Append("개)");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;

			// 핵충전량
			//case EnchantState.HackPlus:

			//    break;

			// 코인추가증가량
			case EnchantState.AddMoney:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("추가로 얻는 은화\n");
                stat.Append(RAddMoney(itemEnchant.AddMoneyLV).ToString());
                stat.Append("배 → ");
                stat.Append(RAddMoney(itemEnchant.AddMoneyLV + 1).ToString());
                stat.Append("배");

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "돼지를 잡을때마다 얻는 은화의 량을 늘려준단다!!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.AddMoneyLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RAddMoney(itemEnchant.AddMoneyLV).ToString());
                    stat.Append("배)");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;

			// 체력감소딜레이증가
			case EnchantState.AddTime:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("체력감소 딜레이 증가시간\n");
                stat.Append(RAddTime(itemEnchant.AddTimeLV).ToString());
                stat.Append("초 → ");
                stat.Append(RAddTime(itemEnchant.AddTimeLV + 1).ToString());
                stat.Append("초");

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "체력이 감소하는 시간을 뎌디게 해주는 거야!!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.AddTimeLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RAddTime(itemEnchant.AddTimeLV).ToString());
                    stat.Append("초)");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;

			// 핵데미지증가
			case EnchantState.HackDamage:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("핵 데미지\n");
                stat.Append(RHackDamage(itemEnchant.HackDamageLV).ToString());
                stat.Append(" → ");
                stat.Append(RHackDamage(itemEnchant.HackDamageLV + 1).ToString());

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "[돼지코]로 발사하는 \n공의 데미지를 더욱 상승시키는 거야!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.HackDamageLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RHackDamage(itemEnchant.HackDamageLV).ToString());
                    stat.Append(")");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;

			// 공 데미지
			case EnchantState.BallDamage:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("공 기본데미지\n");
                stat.Append(RBallDamage(itemEnchant.BallDamageLV).ToString());
                stat.Append(" → ");
                stat.Append(RBallDamage(itemEnchant.BallDamageLV + 1).ToString());

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "기본적인 공의 데미지를 증가시켜줘!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.BallDamageLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RBallDamage(itemEnchant.BallDamageLV).ToString());
                    stat.Append(")");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;

			// 돼지 처치시 얻는 회복량
			case EnchantState.KillPigHeal:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("돼지 처치시 얻는 회복량\n");
                stat.Append(RKillPigHeal(itemEnchant.KillPigHealLV).ToString());
                stat.Append(" → ");
                stat.Append(RKillPigHeal(itemEnchant.KillPigHealLV + 1).ToString());

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "돼지를 죽일 때 얻는 \n회복량을 더욱 증가 시킬 수 있어!!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.KillPigHealLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RKillPigHeal(itemEnchant.KillPigHealLV).ToString());
                    stat.Append(")");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;

			// 핵 범위
			case EnchantState.HackSize:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("핵 폭발 범위\n");
                stat.Append(RHackSize(itemEnchant.HackSizeLV).ToString());
                stat.Append("배 → ");
                stat.Append(RHackSize(itemEnchant.HackSizeLV + 1).ToString());
                stat.Append("배");

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "[돼지코]로 발사한 공의 폭발범위가 더욱 늘어나!!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.HackSizeLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RHackSize(itemEnchant.HackSizeLV).ToString());
                    stat.Append("배)");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;

			// 시작 스테이지
			case EnchantState._BossInterval:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("시간을 절약한다\n");
                stat.Append(RBossInterval(itemEnchant.BossIntervalLV).ToString());
                stat.Append(" 스테이지 → ");
                stat.Append(RBossInterval(itemEnchant.BossIntervalLV + 1).ToString());
                stat.Append(" 스테이지");

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "게임의 전체 스테이지가 줄어들어!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.BossIntervalLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RBossInterval(itemEnchant.BossIntervalLV).ToString());
                    stat.Append(")");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;

			// 추가 행동력
			case EnchantState.AddVigor:

                // 능력치 설명을 담음
                stat = new StringBuilder();
                stat.Append("추가 행동력\n");
                stat.Append(RAddVigor(itemEnchant.AddVigorLV).ToString());
                stat.Append(" → ");
                stat.Append(RAddVigor(itemEnchant.AddVigorLV + 1).ToString());

                // 구매 조건을 담음
                stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "너의 [행동력]의 \n최대 충전량을 증가시킬 수 있단다!! ";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.AddVigorLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RAddVigor(itemEnchant.AddVigorLV).ToString());
                    stat.Append(")");

                    // 능력치 설명
                    _Stat.text = stat.ToString();

					// 구매가능 스테이지
					_PurchaseableStage.text = "최대레벨 입니다.";

					// 구매가능 가격
					_PurchaseablePrice.text = "최대레벨 입니다.";
				}

				break;
		}

	}
    
    // 강화진행도 게이지
    private void EnchantStateGaugeUpdate() {
        
        switch (_EnchantState)
        {
            // 최대체력
            case EnchantState.MaxHP:

                _MaxHPEnchantStateBar.fillAmount = Mathf.MoveTowards(_MaxHPEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.PlayerMaxHPLV / (float)_EnchantMaxLv, 10.0f);
                
                break;

            // 핵최대치
            case EnchantState.HackMaxCount:

                _MaxHackEnchantStateBar.fillAmount = Mathf.MoveTowards(_MaxHackEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.HackMaxCountLV / (float)_EnchantMaxLv, 10.0f);
                
                break;

            // 핵충전량
            //case EnchantState.HackPlus:
            //    
            //    _EnchantStateBar.fillAmount = Mathf.MoveTowards(_EnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.HackPlusLV / (float)_EnchantMaxLv, 10.0f);
            //    _EnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.HackPlusLV.ToString();
            //    _EnchantPrice.text = _Price.ToString();
            //    if (GameManager.getJsonDataManager.itemEnchant.HackPlusLV == _EnchantMaxLv) _EnchantPrice.text = "최대레벨";
            //    break;

            // 코인추가증가량
            case EnchantState.AddMoney:

                _AddMoneyEnchantStateBar.fillAmount = Mathf.MoveTowards(_AddMoneyEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.AddMoneyLV / (float)_EnchantMaxLv, 10.0f);
                
                break;

            // 체력감소딜레이증가
            case EnchantState.AddTime:

                _AddTimeEnchantStateBar.fillAmount = Mathf.MoveTowards(_AddTimeEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.AddTimeLV / (float)_EnchantMaxLv, 10.0f);
                
                break;

            // 핵데미지증가
            case EnchantState.HackDamage:

                _HackDamageEnchantStateBar.fillAmount = Mathf.MoveTowards(_HackDamageEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.HackDamageLV / (float)_EnchantMaxLv, 10.0f);
                
                break;

            // 공 데미지
            case EnchantState.BallDamage:

                _BallDamageEnchantStateBar.fillAmount = Mathf.MoveTowards(_BallDamageEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.BallDamageLV / (float)_EnchantMaxLv, 10.0f);
                
                break;

            // 돼지 처치시 얻는 회복량
            case EnchantState.KillPigHeal:

                _KillPigHealEnchantStateBar.fillAmount = Mathf.MoveTowards(_KillPigHealEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.KillPigHealLV / (float)_EnchantMaxLv, 10.0f);
               
                break;

            // 핵 범위
            case EnchantState.HackSize:

                _HackSizeEnchantStateBar.fillAmount = Mathf.MoveTowards(_HackSizeEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.HackSizeLV / (float)_EnchantMaxLv, 10.0f);
                
                break;

            // 시작 스테이지
            case EnchantState._BossInterval:

                _StartTimeEnchantStateBar.fillAmount = Mathf.MoveTowards(_StartTimeEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.BossIntervalLV / (float)_EnchantMaxLv, 10.0f);
                
                break;

            // 추가 행동력
            case EnchantState.AddVigor:

                _AddVigorEnchantStateBar.fillAmount = Mathf.MoveTowards(_AddVigorEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.AddVigorLV / (float)_EnchantMaxLv, 10.0f);
                
                break;
        }

    }

	// 강화진행도 텍스트
	private void EnchantStateTextUpdate()
    {
        switch (_EnchantState) {

			// 최대체력
			case EnchantState.MaxHP:

				_MaxHPEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.PlayerMaxHPLV.ToString();

				// 만약 최고 레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.PlayerMaxHPLV == _EnchantMaxLv) _MaxHPEnchantStateText.text = "최대레벨";
				break;

			// 핵최대치
			case EnchantState.HackMaxCount:

				_MaxHackEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.HackMaxCountLV.ToString();

				// 만약 최고 레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.HackMaxCountLV == _EnchantMaxLv) _MaxHackEnchantStateText.text = "최대레벨";
				break;

			// 핵충전량
			//case EnchantState.HackPlus:
			//    
			//    _EnchantStateBar.fillAmount = Mathf.MoveTowards(_EnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.HackPlusLV / (float)_EnchantMaxLv, 10.0f);
			//    _EnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.HackPlusLV.ToString();
			//    _EnchantPrice.text = _Price.ToString();
			//    if (GameManager.getJsonDataManager.itemEnchant.HackPlusLV == _EnchantMaxLv) _EnchantPrice.text = "최대레벨";
			//    break;

			// 코인추가증가량
			case EnchantState.AddMoney:
			
				_AddMoneyEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.AddMoneyLV.ToString();

				// 만약 최고 레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.AddMoneyLV == _EnchantMaxLv) _AddMoneyEnchantStateText.text = "최대레벨";
				break;

			// 체력감소딜레이증가
			case EnchantState.AddTime:

				_AddTimeEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.AddTimeLV.ToString();

				// 만약 최고 레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.AddTimeLV == _EnchantMaxLv) _AddTimeEnchantStateText.text = "최대레벨";
				break;

			// 핵데미지증가
			case EnchantState.HackDamage:

				_HackDamageEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.HackDamageLV.ToString();

				// 만약 최고 레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.HackDamageLV == _EnchantMaxLv) _HackDamageEnchantStateText.text = "최대레벨";
				break;

			// 공 데미지
			case EnchantState.BallDamage:

				_BallDamageEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.BallDamageLV.ToString();

				// 만약 최고 레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.BallDamageLV == _EnchantMaxLv) _BallDamageEnchantStateText.text = "최대레벨";
				break;

			// 돼지 처치시 얻는 회복량
			case EnchantState.KillPigHeal:

				_KillPigHealEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.KillPigHealLV.ToString();

				// 만약 최고 레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.KillPigHealLV == _EnchantMaxLv) _KillPigHealEnchantStateText.text = "최대레벨";
				break;

			// 핵 범위
			case EnchantState.HackSize:
			
				_HackSizeEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.HackSizeLV.ToString();

				// 만약 최고 레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.HackSizeLV == _EnchantMaxLv) _HackSizeEnchantStateText.text = "최대레벨";
				break;

			// 시작 스테이지
			case EnchantState._BossInterval:

				_StartTimeEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.BossIntervalLV.ToString();
				if (GameManager.getJsonDataManager.itemEnchant.BossIntervalLV == _EnchantMaxLv) _StartTimeEnchantStateText.text = "최대레벨";
				break;

			// 추가 행동력
			case EnchantState.AddVigor:

				_AddVigorEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.AddVigorLV.ToString();

				// 만약 최고 레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.AddVigorLV == _EnchantMaxLv) _AddVigorEnchantStateText.text = "최대레벨";

				break;
		}

	}

    // 아이콘을 눌렀을 때 구매 창을 띄웁니다.
    public void OnClickIcon()
    {
        // 터치음 재생
        PlayClickSound();

        // 아이템 종류 별로
        switch (_EnchantState)
        {
            // 최대체력
            case EnchantState.MaxHP:

                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState.MaxHP;
                
                break;

            // 핵최대치
            case EnchantState.HackMaxCount:
                
                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState.HackMaxCount;

                break;

            // 코인추가증가량
            case EnchantState.AddMoney:
                
                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState.AddMoney;

                break;

            // 체력감소딜레이증가
            case EnchantState.AddTime:
                
                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState.AddTime;

                break;

            // 핵데미지증가
            case EnchantState.HackDamage:
                
                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState.HackDamage;

                break;

            // 공 데미지
            case EnchantState.BallDamage:
                
                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState.BallDamage;

                break;

            // 돼지 처치시 얻는 회복량
            case EnchantState.KillPigHeal:
                
                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState.KillPigHeal;

                break;

            // 핵 범위
            case EnchantState.HackSize:
                
                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState.HackSize;

                break;

            // 시작 스테이지
            case EnchantState._BossInterval:
                
                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState._BossInterval;

                break;

            // 추가 행동력
            case EnchantState.AddVigor:
                
                // 구매버튼 상태 설정
                _PlayerBtn._EnchantState = EnchantState.AddVigor;

                break;
        }
        
		// 가격 구매조건
		EnchantPriceUpdate();
		EnchantStageUpdate();

		// 구매창 업데이트
		PurchaseImageUpdate();

		// 한번 비활성화
		_PurchaseImage.gameObject.SetActive(false);

        // 구매 창 활성화
        _PurchaseImage.gameObject.SetActive(true);

        // 플레이어 구매 버튼 활성화
        _PlayerBtn.gameObject.SetActive(true);

        // 아이템 구매 버튼 비활성화
        _ItemBtn.gameObject.SetActive(false);

        // 설명 버튼 비활성화
        _ExplanationBtn.gameObject.SetActive(false);
    }

    // 구매를 눌렀을시 호출
    public void OnClickPurchase() {

        // 터치음 재생
        PlayClickSound();

        switch (_EnchantState) {

            // 최대체력
            case EnchantState.MaxHP:

                // 최대레벨이라면 리턴
                if (itemEnchant.PlayerMaxHPLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨 증가
                    itemEnchant.PlayerMaxHPLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 핵최대치
            case EnchantState.HackMaxCount:

                // 최대레벨이라면 리턴
                if (itemEnchant.HackMaxCountLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨증가
                    itemEnchant.HackMaxCountLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;
                
            // 추가코인량
            case EnchantState.AddMoney:

                // 최대레벨이라면 리턴
                if (itemEnchant.AddMoneyLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨증가
                    itemEnchant.AddMoneyLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 체력감소딜레이증가
            case EnchantState.AddTime:

                // 최대레벨이라면 리턴
                if (itemEnchant.AddTimeLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨증가
                    itemEnchant.AddTimeLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 핵데미지증가
            case EnchantState.HackDamage:

                // 최대레벨이라면 리턴
                if (itemEnchant.HackDamageLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage)
                {

                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨증가
                    itemEnchant.HackDamageLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 공 데미지
            case EnchantState.BallDamage:

                // 최대레벨이라면 리턴
                if (itemEnchant.BallDamageLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage)
                {

                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨증가
                    itemEnchant.BallDamageLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 돼지 처치시 얻는 회복량
            case EnchantState.KillPigHeal:

                // 최대레벨이라면 리턴
                if (itemEnchant.KillPigHealLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage)
                {

                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨증가
                    itemEnchant.KillPigHealLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 핵 범위
            case EnchantState.HackSize:

                // 최대레벨이라면 리턴
                if (itemEnchant.HackSizeLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage)
                {
                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨증가
                    itemEnchant.HackSizeLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 스테이지 간격
            case EnchantState._BossInterval:

                // 최대레벨이라면 리턴
                if (itemEnchant.BossIntervalLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage)
                {
                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨증가
                    itemEnchant.BossIntervalLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 추가 행동력
            case EnchantState.AddVigor:

                // 최대레벨이라면 리턴
                if (itemEnchant.AddVigorLV == _EnchantMaxLv) return;

                // 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage)
                {
                    // 코인 차감
                    playerData.Coin -= _Price;

                    // 레벨증가
                    itemEnchant.AddVigorLV++;

                    // 행동력 +
                    playerData.Vigor++;

                    // 행동력 업데이트
                    Vigor vigor = GameObject.Find("EnchantCanvas").transform.Find("ScreenPannel").transform.Find("Vigor").GetComponent<Vigor>();
                    vigor.VigorUpdate();
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;
        }

        // 데이터 설정
        GameManager.getJsonDataManager.playerData = playerData;
        GameManager.getJsonDataManager.itemEnchant = itemEnchant;

        // 파일 저장 
        GameManager.getJsonDataManager.SaveItemEnchantData();
        GameManager.getJsonDataManager.SavePlayerData();

        // 가격 구매조건
        EnchantPriceUpdate();
        EnchantStageUpdate();

        // 구매창 업데이트
        PurchaseImageUpdate();

        // 코인 업데이트
        _EnchantCanvas.CoinsUpdate();

        // 강화진행도 설정
        EnchantStateGaugeUpdate();
        EnchantStateTextUpdate();
    }
    
    // 구매 실패시 호출함
    private void PuchaseFailed(uint price, uint stage)
    {
        // 구매 불가창 활성화
        _EnoughImage.gameObject.SetActive(true);

        // 담기
        StringBuilder text = new StringBuilder();

        // 돈이 없다면
        if (GameManager.getJsonDataManager.playerData.Coin < 999) text.Append(" 그지는 구매할 수 없습니다.\n");

        // 돈이 부족하다면
        else if (price > GameManager.getJsonDataManager.playerData.Coin)
        {
            text.Append("코인이 부족합니다.\n필요한 최소 코인 : ");
            text.Append(price.ToString());

            // 코인 부족
            PlayEnoughtCoin();
        }

        // 구매조건이 충족되지 않았다면
        else if (GameManager.getJsonDataManager.playerData.Record < stage)
        {
            text.Append("스테이지가 너무 낮습니다.\n구매가능 최소 스테이지 : ");
            text.Append(stage.ToString());

            // 기록 부족
            PlayEnoughtRecord();
        }

        // 텍스트 설정
        _EnoughText.text = text.ToString();
    }
    
    #region 아이템 능력치 리턴
    // 플레이어 최대체력의 레벨에 따른 능력치를 리턴
    public float RPlayerMaxHP(uint level)
    {
        switch (level)
        {
            case 0: return 100;
            case 1: return 110;
            case 2: return 120;
            case 3: return 127;
            case 4: return 135;
            case 5: return 140;
            case 6: return 143;
            case 7: return 146;
            case 8: return 149;
            case 9: return 152;
            case 10: return 155;
            case 11: return 158;
            case 12: return 162;
            case 13: return 164;
            case 14: return 167;
            case 15: return 170;
            case 16: return 172;
            case 17: return 174;
            case 18: return 176;
            case 19: return 178;
            case 20: return 180;
            case 21: return 182;
            case 22: return 184;
            case 23: return 186;
            case 24: return 188;
            case 25: return 191;
            case 26: return 194;
            case 27: return 196;
            case 28: return 198;
            case 29: return 200;
            case 30: return 205;
        }

        return 0.0f;
    }

    // 총알을 발싸하는 아이템의 레벨에 따른 능력치를 리턴
    public uint RShootingCount(uint level)
    {
        switch (level)
        {
            case 0: return 4;
            case 1: return 6;
            case 2: return 8;
            case 3: return 10;
            case 4: return 12;
            case 5: return 14;
        }

        return 0;
    }

    // 미니볼 아이템의 레벨에 따른 능력치를 리턴
    public uint RMiniballMaxCount(uint level)
    {
        switch (level)
        {
            case 0: return 1;
            case 1: return 2;
            case 2: return 3;
            case 3: return 4;
            case 4: return 5;
            case 5: return 6;
            case 6: return 7;
        }

        return 0;
    }

    // 빅바 아이템의 레벨에 따른 능력치를 리턴
    public uint RBigBarMaxCount(uint level)
    {
        switch (level)
        {
            case 0: return 2;
            case 1: return 3;
            case 2: return 4;
            case 3: return 5;
            case 4: return 6;
            case 5: return 7;
        }

        return 0;
    }

    // 과부하 아이템의 레벨에 따른 능력치를 리턴
    public uint ROverload(uint level)
    {
        switch (level)
        {
            case 0: return 6;
            case 1: return 7;
            case 2: return 8;
            case 3: return 9;
            case 4: return 10;
            case 5: return 11;
            case 6: return 12;
            case 7: return 13;
            case 8: return 14;
            case 9: return 15;
            case 10: return 16;
        }

        return 0;
    }

    // 힐 아이템의 레벨에 따른 능력치를 리턴
    public int RHeal(uint level)
    {
        switch (level)
        {
            case 0: return 20;
            case 1: return 23;
            case 2: return 25;
            case 3: return 27;
            case 4: return 29;
            case 5: return 31;
            case 6: return 33;
            case 7: return 35;
            case 8: return 37;
            case 9: return 39;
            case 10: return 41;
        }

        return 0;
    }

    // 체력감소 딜레이증가의 레벨에 따른 능력치를 리턴
    public float RAddTime(uint level)
    {
        switch (level)
        {
            case 0: return 0.5f;
            case 1: return 0.55f;
            case 2: return 0.6f;
            case 3: return 0.65f;
            case 4: return 0.7f;
            case 5: return 0.75f;
            case 6: return 0.8f;
        }

        return 0.0f;
    }

    // 업그레이드 아이템의레벨에 따른 능력치를 리턴
    public int RUpgradeMaxCount(uint level)
    {
        switch (level)
        {
            case 0: return 2;
            case 1: return 3;
            case 2: return 4;
            case 3: return 5;
            case 4: return 6;
            case 5: return 7;
            case 6: return 8;
        }

        return 0;
    }

    // 히오스 지속시간 강화의 레벨에 따른 능력치를 리턴
    public float RHosDurationTime(uint level)
    {
        switch (level)
        {
            case 0: return 2.0f;
            case 1: return 2.5f;
            case 2: return 3.0f;
            case 3: return 3.5f;
            case 4: return 4.0f;
            case 5: return 4.5f;
            case 6: return 5.0f;
            case 7: return 5.5f;
        }

        return 0.0f;
    }

    // 돼지처치시 회복량증가의 강화의 레벨에 따른 능력치를 리턴
    public float RKillPigHeal(uint level)
    {
        switch (level)
        {
            case 0: return 1.5f;
            case 1: return 1.6f;
            case 2: return 1.7f;
            case 3: return 1.8f;
            case 4: return 1.9f;
            case 5: return 2.0f;
            case 6: return 2.1f;
            case 7: return 2.2f;
            case 8: return 2.3f;
            case 9: return 2.4f;
            case 10: return 2.5f;
        }

        return 0.0f;
    }

    // 히오스 크기 강화의 레벨에 따른 능력치를 리턴
    public float RHosSize(uint level)
    {
        switch (level)
        {
            case 0: return 2.0f;
            case 1: return 2.2f;
            case 2: return 2.4f;
            case 3: return 2.6f;
            case 4: return 2.8f;
        }

        return 0.0f;
    }

    // 총알 크기 강화의 레벨에 따른 능력치를 리턴
    public float RBulletSize(uint level)
    {
        switch (level)
        {
            case 0: return 1.0f;
            case 1: return 1.1f;
            case 2: return 1.2f;
            case 3: return 1.3f;
            case 4: return 1.4f;
            case 5: return 1.5f;
            case 6: return 1.6f;
            case 7: return 1.7f;
        }

        return 0.0f;
    }

    // 볼데미지 강화에 따른 볼데미지 리턴
    public int RBallDamage(uint level)
    {
        switch (level)
        {
            case 0: return 1;
            case 1: return 2;
            case 2: return 3;
        }

        return 0;
    }

    // 핵범위 강화에 따른 범위 리턴
    public float RHackSize(uint level)
    {
        switch (level)
        {
            case 0: return 1.0f;
            case 1: return 1.5f;
            case 2: return 2.0f;
        }

        return 0.0f;
    }

    // 핵 최대충전량 강화에 따른 최대충전량 리턴
    public int RHackMaxCount(uint level)
    {
        switch (level)
        {
            case 0: return 1;
            case 1: return 2;
            case 2: return 3;
            case 3: return 4;
        }

        return 0;
    }

    // 추가 돈 획득량 강화에 따른 돈 획득 배수 리턴
    public float RAddMoney(uint level)
    {
        switch (level)
        {
            case 0: return 1.0f;
            case 1: return 1.05f;
            case 2: return 1.07f;
            case 3: return 1.1f;
            case 4: return 1.12f;
            case 5: return 1.15f;
            case 6: return 1.17f;
            case 7: return 1.2f;
            case 8: return 1.22f;
            case 9: return 1.24f;
            case 10: return 1.26f;
            case 11: return 1.28f;
            case 12: return 1.30f;
            case 13: return 1.32f;
            case 14: return 1.34f;
            case 15: return 1.36f;
            case 16: return 1.38f;
            case 17: return 1.40f;
            case 18: return 1.42f;
            case 19: return 1.44f;
            case 20: return 1.46f;
            case 21: return 1.48f;
            case 22: return 1.50f;
            case 23: return 1.52f;
            case 24: return 1.54f;
            case 25: return 1.56f;
            case 26: return 1.58f;
            case 27: return 1.60f;
            case 28: return 1.62f;
            case 29: return 1.64f;
            case 30: return 1.66f;
        }

        return 0.0f;
    }

    // 핵 데미지 강화에 따른 핵 데미지 리턴
    public uint RHackDamage(uint level)
    {
        switch (level)
        {
            case 0: return 2;
            case 1: return 3;
            case 2: return 4;
            case 3: return 5;
            case 4: return 6;
            case 5: return 7;
            case 6: return 8;
        }

        return 0;
    }

    // 스테이지 간격 감소
    public int RBossInterval(uint level)
    {
        switch (level)
        {
            case 0: return 10;
            case 1: return 9;
            case 2: return 8;
            case 3: return 7;
        }

        return 0;
    }

    // 추가 입장권 강화에 따른 추가 입장권
    public int RAddVigor(uint level)
    {
        switch (level)
        {
            case 0: return 0;
            case 1: return 1;
            case 2: return 2;
            case 3: return 3;
            case 4: return 4;
        }

        return 0;
    }

    #endregion

}
