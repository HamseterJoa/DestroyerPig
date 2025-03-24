using JsonData;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ItemEnchantButton : MonoBehaviour
{
	// 스크립트를 넣어준 오브젝트의 종류
	private enum ObjectState { Icon, purchaseBtn}
	[SerializeField] private ObjectState _ObjectState = ObjectState.Icon;

	// 강화할 아이템의 종류
    public enum EnchantState { Overload, Shoot, MiniBall, BigBar, Upgrade, Heal, Hos, HosSize, BulletSize }
    public EnchantState _EnchantState = EnchantState.Overload;

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
	[SerializeField] private ItemEnchantButton _ItemBtn;

	// 플레이어 구매 버튼
	[SerializeField] private GameObject _PlayerBtn;

    // 캔버스
    [SerializeField] private EnchantCanvas _EnchantCanvas;
    #region 강화진행도
    // 과부하 강화진행도(게이지)
    [SerializeField] private Image _OverloadEnchantStateBar;

    // 과부하 강화진행도(숫자)
    [SerializeField] private Text _OverloadEnchantStateText;

    // 수박씨 강화진행도(게이지)
    [SerializeField] private Image _ShootEnchantStateBar;

    // 수박씨 강화진행도(숫자)
    [SerializeField] private Text _ShootEnchantStateText;
    
    // 미니볼 강화진행도(게이지)
    [SerializeField] private Image _MiniBallEnchantStateBar;

    // 미니볼 강화진행도(숫자)
    [SerializeField] private Text _MiniBallEnchantStateText;

    // 빅바 강화진행도(게이지)
    [SerializeField] private Image _BigBarEnchantStateBar;

    // 빅바 강화진행도(숫자)
    [SerializeField] private Text _BigBarEnchantStateText;

    // 업그레이드 강화진행도(게이지)
    [SerializeField] private Image _UpgradeEnchantStateBar;

    // 업그레이드 강화진행도(숫자)
    [SerializeField] private Text _UpgradeEnchantStateText;

    // 힐 강화진행도(게이지)
    [SerializeField] private Image _HealEnchantStateBar;

    // 힐 강화진행도(숫자)
    [SerializeField] private Text _HealEnchantStateText;

    // 히오스 강화진행도(게이지)
    [SerializeField] private Image _HosEnchantStateBar;

    // 히오스 강화진행도(숫자)
    [SerializeField] private Text _HosEnchantStateText;

    // 히오스 사이즈 강화진행도(게이지)
    [SerializeField] private Image _HosSizeEnchantStateBar;

    // 히오스 사이즈강화진행도(숫자)
    [SerializeField] private Text _HosSizeEnchantStateText;

    // 총알 사이즈 강화진행도(게이지)
    [SerializeField] private Image _BulletSizeEnchantStateBar;

    // 총알 사이즈 강화진행도(숫자)
    [SerializeField] private Text _BulletSizeEnchantStateText;
    #endregion
    // 가격
    private uint _Price;

    // 구매 조건
    private uint _Stage;

    // 최대레벨
    private uint _EnchantMaxLv;

	// 플레이어 데이터
	private static PlayerData playerData;

	// 아이템 데이터
	private static ItemEnchant itemEnchant;

    // 터치 소리
    [SerializeField] private AudioSource _Audio;

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

    //  > 버튼 클릭 사운드 재생
    private void PlayClickSound()
    {
        // 볼륨 설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _Audio.Play();
    }

    // 표면상 데이터 설정
    private void OnEnable() {
        
		// 최대 강화횟수 설정
		EnchantMaxUpdate();

		// 강화 단계별 가격, 구매조건
		EnchantPriceUpdate();
		EnchantStageUpdate();

        // 아이템, 플레이어 정보 설정
        playerData = GameManager.getJsonDataManager.playerData;
        itemEnchant = GameManager.getJsonDataManager.itemEnchant;

        if (_BigBarEnchantStateBar != null)
        {
            // 강화진행도 업데이트
            EnchantStateGaugeUpdate();
            EnchantStateTextUpdate();
        }
        
    }
    
	// 구매창 업데이트
	private void PurchaseImageUpdate() {

		// 능력치 설명을 담을 거
		StringBuilder stat;
		
		// 구매 조건을 담을 거
		StringBuilder stage;
		
		switch (_EnchantState) {

			// 과부하
			case EnchantState.Overload:

				// 능력치 설명을 담음
				stat = new StringBuilder();
				stat.Append("지속시간\n");
				stat.Append(ROverload(itemEnchant.OverloadLV).ToString());
				stat.Append("초 → ");
				stat.Append(ROverload(itemEnchant.OverloadLV + 1).ToString());
				stat.Append("초");

				// 구매 조건을 담음
				stage = new StringBuilder();
				stage.Append("구매가능 스테이지 : ");
				stage.Append(_Stage.ToString());

				// 능력치 설명
				_Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "[불타는 고추]의 지속시간 증가!!";

				// 구매 가능 스테이지 설정
				_PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.OverloadLV == _EnchantMaxLv) {

					// 능력치 설명을 담기
					stat = new StringBuilder();
					stat.Append("최고레벨 입니다.(");
					stat.Append(ROverload(itemEnchant.OverloadLV).ToString());
					stat.Append("초)");

					// 변경점 설정
					_Stat.text = stat.ToString();

					// 아이템 가격 설정
					_PurchaseablePrice.text = "최고레벨 입니다.";

					// 구매 가능 스테이지 설정
					_PurchaseableStage.text = "최고레벨 입니다.";
				}

				break;

			// 총
			case EnchantState.Shoot:

                // 변경점 담기
				stat = new StringBuilder();
                stat.Append("발사수\n");
                stat.Append(RShootingCount(itemEnchant.MaximumShootingCountLV).ToString());
                stat.Append("발 → ");
                stat.Append(RShootingCount(itemEnchant.MaximumShootingCountLV + 1).ToString());
                stat.Append("발");

                // 구매조건 담기
				stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "[수박]을 먹고 더욱 많은 수박씨를 뱉을 수 있어요!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.MaximumShootingCountLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RShootingCount(itemEnchant.MaximumShootingCountLV).ToString());
                    stat.Append("발)");

                    // 변경점 설정
                    _Stat.text = stat.ToString();

					// 아이템 가격 설정
					_PurchaseablePrice.text = "최고레벨 입니다.";

					// 구매 가능 스테이지 설정
					_PurchaseableStage.text = "최고레벨 입니다.";
				}

				break;

			// 힐
			case EnchantState.Heal:

                // 능력치 변경점 담기
				stat = new StringBuilder();
                stat.Append("회복량\n");
                stat.Append(RHeal(itemEnchant.HealLV).ToString());
                stat.Append(" → ");
                stat.Append(RHeal(itemEnchant.HealLV + 1).ToString());

                // 구매 조건 담기
				stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "토마토쥬스가 더욱 당신을 편하게 해줄거야요";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.HealLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RHeal(itemEnchant.HealLV).ToString());
                    stat.Append(")");

                    // 변경점 설정
                    _Stat.text = stat.ToString();

					// 아이템 가격 설정
					_PurchaseablePrice.text = "최고레벨 입니다.";

					// 구매 가능 스테이지 설정
					_PurchaseableStage.text = "최고레벨 입니다.";
				}

				break;

			// 미니볼
			case EnchantState.MiniBall:

                // 능력치 변경점 담기
				stat = new StringBuilder();
                stat.Append("감구마 최대 생성개수\n");
                stat.Append(RMiniballMaxCount(itemEnchant.MiniBallMaximumCountLV).ToString());
                stat.Append("개 → ");
                stat.Append(RMiniballMaxCount(itemEnchant.MiniBallMaximumCountLV + 1).ToString());
                stat.Append("개");

				stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "별 필요는 없겠지만,\n 작은 친구들을 더 많이 부를 수 있어요!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.MiniBallMaximumCountLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RMiniballMaxCount(itemEnchant.MiniBallMaximumCountLV).ToString());
                    stat.Append("개)");

                    // 변경점 설정
                    _Stat.text = stat.ToString();

					// 아이템 가격 설정
					_PurchaseablePrice.text = "최고레벨 입니다.";

					// 구매 가능 스테이지 설정
					_PurchaseableStage.text = "최고레벨 입니다.";
				}

				break;

			// 빅바
			case EnchantState.BigBar:

                // 능력치 변경점 담기
				stat = new StringBuilder();
                stat.Append("모자 챙이 커지는 최대 횟수\n");
                stat.Append(RBigBarMaxCount(itemEnchant.BarMaxPlusCountLV).ToString());
                stat.Append("번 → ");
                stat.Append(RBigBarMaxCount(itemEnchant.BarMaxPlusCountLV + 1).ToString());
                stat.Append("번");

                // 구매 조건 담기
				stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "이제[우엉]을 더 먹어면 모자는\n 그만큼 더 커질거에용";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.BarMaxPlusCountLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RBigBarMaxCount(itemEnchant.BarMaxPlusCountLV).ToString());
                    stat.Append("번)");

                    // 변경점 설정
                    _Stat.text = stat.ToString();

					// 아이템 가격 설정
					_PurchaseablePrice.text = "최고레벨 입니다.";

					// 구매 가능 스테이지 설정
					_PurchaseableStage.text = "최고레벨 입니다.";
				}

				break;

			// 업그레이드
			case EnchantState.Upgrade:

                // 변경점 담기
				stat = new StringBuilder();
                stat.Append("공이 커지는 최대 횟수\n");
                stat.Append(RUpgradeMaxCount(itemEnchant.UpgradeMaxCountLV).ToString());
                stat.Append("번 → ");
                stat.Append(RUpgradeMaxCount(itemEnchant.UpgradeMaxCountLV + 1).ToString());
                stat.Append("번");

                // 구매조건 담기
				stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "공이 커지는 단계가 더욱 많아져요!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.UpgradeMaxCountLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RUpgradeMaxCount(itemEnchant.UpgradeMaxCountLV).ToString());
                    stat.Append("번)");

                    // 변경점 설정
                    _Stat.text = stat.ToString();

					// 아이템 가격 설정
					_PurchaseablePrice.text = "최고레벨 입니다.";

					// 구매 가능 스테이지 설정
					_PurchaseableStage.text = "최고레벨 입니다.";
				}
				break;

			// 히오스
			case EnchantState.Hos:

                // 변경점 담기
				stat = new StringBuilder();
                stat.Append("지속시간\n");
                stat.Append(RHosDurationTime(itemEnchant.HosDurationTimeLV).ToString());
                stat.Append("초 → ");
                stat.Append(RHosDurationTime(itemEnchant.HosDurationTimeLV + 1).ToString());
                stat.Append("초");

                // 구매조건 담기
				stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

                // 아이템의 설명
                _Info.text = "[썩은옥수수]가 더욱 오랫동안\n 당신을 지켜줄거야";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.HosDurationTimeLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RHosDurationTime(itemEnchant.HosDurationTimeLV).ToString());
                    stat.Append("초)");

                    // 변경점 설정
                    _Stat.text = stat.ToString();

					// 아이템 가격 설정
					_PurchaseablePrice.text = "최고레벨 입니다.";

					// 구매 가능 스테이지 설정
					_PurchaseableStage.text = "최고레벨 입니다.";
				}

				break;

			// 히오스 크기
			case EnchantState.HosSize:

                // 변경점 설명 담기
				stat = new StringBuilder();
                stat.Append("보호막 크기\n");
                stat.Append(RHosSize(itemEnchant.HosSizeLV).ToString());
                stat.Append("M → ");
                stat.Append(RHosSize(itemEnchant.HosSizeLV + 1).ToString());
                stat.Append("M");

                // 구매조건 담기
				stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "[썩은옥수수]의 크기를 증가합니다!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.HosSizeLV == _EnchantMaxLv) {

                    // 스탯설명 담기
                    stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RHosSize(itemEnchant.HosSizeLV).ToString());
                    stat.Append("M)");

                    // 변경점 설정
                    _Stat.text = stat.ToString();

					// 아이템 가격 설정
					_PurchaseablePrice.text = "최고레벨 입니다.";

					// 구매 가능 스테이지 설정
					_PurchaseableStage.text = "최고레벨 입니다.";
				}
				break;

			// 총알 크기
			case EnchantState.BulletSize:

                // 능력치 변경점 설명 담기
				stat = new StringBuilder();
				stat.Append("발사체의 크기\n");
				stat.Append(RBulletSize(itemEnchant.BulletSizeLV).ToString());
				stat.Append("배 → ");
				stat.Append(RBulletSize(itemEnchant.BulletSizeLV + 1).ToString());
				stat.Append("배");

                // 구매조건 담기
				stage = new StringBuilder();
                stage.Append("구매가능 스테이지 : ");
                stage.Append(_Stage.ToString());

                // 능력치 설명
                _Stat.text = stat.ToString();

				// 아이템의 설명
				_Info.text = "수박씨의 크기가 더욱 커집니다!!";

                // 구매 가능 스테이지 설정
                _PurchaseableStage.text = stage.ToString();

				// 아이템 가격
				_PurchaseablePrice.text = _Price.ToString();

				// 아이템 레벨이 최고레벨일 경우
				if (itemEnchant.BulletSizeLV == _EnchantMaxLv) {

                    // 스탯설명 담기
					stat = new StringBuilder();
                    stat.Append("최고레벨 입니다.(");
                    stat.Append(RBulletSize(itemEnchant.BulletSizeLV).ToString());
                    stat.Append("배)");

                    // 변경점 설정
                    _Stat.text = stat.ToString();

					// 아이템 가격 설정
					_PurchaseablePrice.text = "최고레벨 입니다.";

					// 구매 가능 스테이지 설정
					_PurchaseableStage.text = "최고레벨 입니다.";
				}
				break;

		}
	
	}

	// 최대 강화 횟수설정
	private void EnchantMaxUpdate(){
		switch (_EnchantState) {
			case EnchantState.Overload: _EnchantMaxLv = 10; break;
			case EnchantState.Shoot: _EnchantMaxLv = 5; break;
			case EnchantState.MiniBall: _EnchantMaxLv = 6; break;
			case EnchantState.BigBar: _EnchantMaxLv = 5; break;
			case EnchantState.Upgrade: _EnchantMaxLv = 6; break;
			case EnchantState.Heal: _EnchantMaxLv = 10; break;
			case EnchantState.Hos: _EnchantMaxLv = 7; break;
			case EnchantState.HosSize: _EnchantMaxLv = 4; break;
			case EnchantState.BulletSize: _EnchantMaxLv = 7; break;
		}
	}

	// 단계별 가격
	private void EnchantPriceUpdate() {
		switch (_EnchantState) {

			// 과부하 지속시간
			case EnchantState.Overload:

				switch (GameManager.getJsonDataManager.itemEnchant.OverloadLV) {
					case 0: _Price = 7000; break;
					case 1: _Price = 10000; break;
					case 2: _Price = 16000; break;
					case 3: _Price = 19500; break;
					case 4: _Price = 23000; break;
					case 5: _Price = 30400; break;
					case 6: _Price = 34500; break;
					case 7: _Price = 40000; break;
					case 8: _Price = 44400; break;
					case 9: _Price = 50000; break;
				}

				break;

			// 총
			case EnchantState.Shoot:
				switch (GameManager.getJsonDataManager.itemEnchant.MaximumShootingCountLV) {
					case 0: _Price = 10000; break;
					case 1: _Price = 20000; break;
					case 2: _Price = 31500; break;
					case 3: _Price = 50000; break;
					case 4: _Price = 76400; break;

				}

				break;

			// 힐
			case EnchantState.Heal:
				switch (GameManager.getJsonDataManager.itemEnchant.HealLV) {
					case 0: _Price = 7000; break;
					case 1: _Price = 8000; break;
					case 2: _Price = 11000; break;
					case 3: _Price = 16000; break;
					case 4: _Price = 23000; break;
					case 5: _Price = 28500; break;
					case 6: _Price = 34000; break;
					case 7: _Price = 41200; break;
					case 8: _Price = 46000; break;
					case 9: _Price = 52000; break;
				}

				break;

			// 미니볼
			case EnchantState.MiniBall:
				switch (GameManager.getJsonDataManager.itemEnchant.MiniBallMaximumCountLV) {
					case 0: _Price = 3000; break;
					case 1: _Price = 7000; break;
					case 2: _Price = 10000; break;
					case 3: _Price = 14000; break;
					case 4: _Price = 19500; break;
					case 5: _Price = 23000; break;
				}

				break;

			// 빅바
			case EnchantState.BigBar:
				switch (GameManager.getJsonDataManager.itemEnchant.BarMaxPlusCountLV) {
					case 0: _Price = 6000; break;
					case 1: _Price = 15000; break;
					case 2: _Price = 23000; break;
					case 3: _Price = 26000; break;
					case 4: _Price = 30000; break;
				}

				break;

			// 업그레이드
			case EnchantState.Upgrade:
				switch (GameManager.getJsonDataManager.itemEnchant.UpgradeMaxCountLV) {
					case 0: _Price = 8000; break;
					case 1: _Price = 12000; break;
					case 2: _Price = 16000; break;
					case 3: _Price = 19000; break;
					case 4: _Price = 23000; break;
					case 5: _Price = 27500; break;
				}

				break;

			// 히오스
			case EnchantState.Hos:
				switch (GameManager.getJsonDataManager.itemEnchant.HosDurationTimeLV) {
					case 0: _Price = 20000;break;
					case 1: _Price = 26500; break;
					case 2: _Price = 30000; break;
					case 3: _Price = 32000; break;
					case 4: _Price = 34000; break;
					case 5: _Price = 40000; break;
					case 6: _Price = 50000; break;
				}

				break;

			// 히오스 크기
			case EnchantState.HosSize:
				switch (GameManager.getJsonDataManager.itemEnchant.HosSizeLV) {
					case 0: _Price = 30000; break;
					case 1: _Price = 40000; break;
					case 2: _Price = 50000; break;
					case 3: _Price = 60000; break;
				}

				break;

			// 총알 크기
			case EnchantState.BulletSize:
				switch (GameManager.getJsonDataManager.itemEnchant.BulletSizeLV) {
					case 0: _Price = 7000; break;
					case 1: _Price = 13000; break;
					case 2: _Price = 18000; break;
					case 3: _Price = 24500; break;
					case 4: _Price = 34000; break;
					case 5: _Price = 40000; break;
					case 6: _Price = 42300; break;
				}

				break;
		}
	}

	// 단계별 구매조건
	private void EnchantStageUpdate(){

		switch (_EnchantState) {

			// 과부하 지속시간
			case EnchantState.Overload:

				switch (GameManager.getJsonDataManager.itemEnchant.OverloadLV) {
					case 0: _Stage = 5; break;
					case 1: _Stage = 5; break;
					case 2: _Stage = 5; break;
					case 3: _Stage = 15; break;
					case 4: _Stage = 15; break;
					case 5: _Stage = 20; break;
					case 6: _Stage = 20; break;
					case 7: _Stage = 20; break;
					case 8: _Stage = 30; break;
					case 9: _Stage = 30; break;
				}

				break;

			// 총
			case EnchantState.Shoot:
				switch (GameManager.getJsonDataManager.itemEnchant.MaximumShootingCountLV) {
					case 0: _Stage = 10; break;
					case 1: _Stage = 17; break;
					case 2: _Stage = 25; break;
					case 3: _Stage = 32; break;
					case 4: _Stage = 40; break;

				}

				break;

			// 힐
			case EnchantState.Heal:
				switch (GameManager.getJsonDataManager.itemEnchant.HealLV) {
					case 0:  _Stage = 5; break;
					case 1:  _Stage = 5; break;
					case 2:  _Stage = 5; break;
					case 3:  _Stage = 10; break;
					case 4:  _Stage = 15; break;
					case 5:  _Stage = 15; break;
					case 6:  _Stage = 15; break;
					case 7:  _Stage = 20; break;
					case 8:  _Stage = 20; break;
					case 9:  _Stage = 20; break;
				}

				break;

			// 미니볼
			case EnchantState.MiniBall:
				switch (GameManager.getJsonDataManager.itemEnchant.MiniBallMaximumCountLV) {
					case 0: _Stage = 0; break;
					case 1: _Stage = 5; break;
					case 2: _Stage = 7; break;
					case 3: _Stage = 10; break;
					case 4: _Stage = 20; break;
					case 5: _Stage = 25; break;
				}

				break;

			// 빅바
			case EnchantState.BigBar:
				switch (GameManager.getJsonDataManager.itemEnchant.BarMaxPlusCountLV) {
					case 0: _Stage = 0; break;
					case 1: _Stage = 3; break;
					case 2: _Stage = 6; break;
					case 3: _Stage = 12; break;
					case 4: _Stage = 15; break;
				}

				break;

			// 업그레이드
			case EnchantState.Upgrade:
				switch (GameManager.getJsonDataManager.itemEnchant.UpgradeMaxCountLV) {
					case 0: _Stage = 0; break;
					case 1: _Stage = 5; break;
					case 2: _Stage = 10; break;
					case 3: _Stage = 10; break;
					case 4: _Stage = 13; break;
					case 5: _Stage = 13; break;
				}

				break;

			// 히오스
			case EnchantState.Hos:
				switch (GameManager.getJsonDataManager.itemEnchant.HosDurationTimeLV) {
					case 0: _Stage = 5; break;
					case 1: _Stage = 10; break;
					case 2: _Stage = 10; break;
					case 3: _Stage = 19; break;
					case 4: _Stage = 28; break;
					case 5: _Stage = 32; break;
					case 6: _Stage = 43; break;
				}

				break;

			// 히오스 크기
			case EnchantState.HosSize:
				switch (GameManager.getJsonDataManager.itemEnchant.HosSizeLV) {
					case 0: _Stage = 20; break;
					case 1: _Stage = 40; break;
					case 2: _Stage = 60; break;
					case 3: _Stage = 70; break;
				}

				break;

			// 총알 크기
			case EnchantState.BulletSize:
				switch (GameManager.getJsonDataManager.itemEnchant.BulletSizeLV) {
					case 0: _Stage = 3; break;
					case 1: _Stage = 7; break;
					case 2: _Stage = 7; break;
					case 3: _Stage = 9; break;
					case 4: _Stage = 16; break;
					case 5: _Stage = 16; break;
					case 6: _Stage = 16; break;
				}

				break;
		}
	}
    
	// 강화 진행도 게이지 업데이트
	private void EnchantStateGaugeUpdate() {

		switch (_EnchantState) {

			// 과부하 지속시간
			case EnchantState.Overload:

				// 강화 진행도 업데이트(게이지)
				_OverloadEnchantStateBar.fillAmount = Mathf.MoveTowards(_OverloadEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.OverloadLV / (float)_EnchantMaxLv, 10.0f);
				
				break;

			// 총
			case EnchantState.Shoot:

				// 강화 진행도 업데이트(게이지)
				_ShootEnchantStateBar.fillAmount = Mathf.MoveTowards(_ShootEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.MaximumShootingCountLV / (float)_EnchantMaxLv, 10.0f);
				
				break;

			// 힐
			case EnchantState.Heal:

				// 강화 진행도 업데이트(게이지)
				_HealEnchantStateBar.fillAmount = Mathf.MoveTowards(_HealEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.HealLV / (float)_EnchantMaxLv, 10.0f);
				
				break;

			// 미니볼
			case EnchantState.MiniBall:

				// 강화 진행도 업데이트(게이지)
				_MiniBallEnchantStateBar.fillAmount = Mathf.MoveTowards(_MiniBallEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.MiniBallMaximumCountLV / (float)_EnchantMaxLv, 10.0f);
				
				break;

			// 빅바
			case EnchantState.BigBar:

				// 강화 진행도 업데이트(게이지)
				_BigBarEnchantStateBar.fillAmount = Mathf.MoveTowards(_BigBarEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.BarMaxPlusCountLV / (float)_EnchantMaxLv, 10.0f);
				
				break;

			// 업그레이드
			case EnchantState.Upgrade:

				// 강화 진행도 업데이트(게이지)
				_UpgradeEnchantStateBar.fillAmount = Mathf.MoveTowards(_UpgradeEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.UpgradeMaxCountLV / (float)_EnchantMaxLv, 10.0f);
				
				break;

			// 히오스
			case EnchantState.Hos:

				// 강화 진행도 업데이트(게이지)
				_HosEnchantStateBar.fillAmount = Mathf.MoveTowards(_HosEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.HosDurationTimeLV / (float)_EnchantMaxLv, 10.0f);
				
				break;

			// 히오스 크기
			case EnchantState.HosSize:

				// 강화 진행도 업데이트(게이지)
				_HosSizeEnchantStateBar.fillAmount = Mathf.MoveTowards(_HosSizeEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.HosSizeLV / (float)_EnchantMaxLv, 10.0f);
				
				break;

			// 총알 크기
			case EnchantState.BulletSize:

				// 강화 진행도 업데이트(게이지)
				_BulletSizeEnchantStateBar.fillAmount = Mathf.MoveTowards(_BulletSizeEnchantStateBar.fillAmount, GameManager.getJsonDataManager.itemEnchant.BulletSizeLV / (float)_EnchantMaxLv, 10.0f);
				
				break;
		}

	}

	// 강화 진행도 텍스트 업데이트
	private void EnchantStateTextUpdate(){

		switch (_EnchantState) {

			// 과부하 지속시간
			case EnchantState.Overload:

				// 강화 진행도 업데이트(숫자)
				_OverloadEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.OverloadLV.ToString();

				// 만약 강화 진행도가 최대레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.OverloadLV == _EnchantMaxLv) _OverloadEnchantStateText.text = "최대레벨";

				break;

			// 총
			case EnchantState.Shoot:
			
				// 강화 진행도 업데이트(숫자)
				_ShootEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.MaximumShootingCountLV.ToString();

				// 만약 강화 진행도가 최대레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.MaximumShootingCountLV == _EnchantMaxLv) _ShootEnchantStateText.text = "최대레벨";
				break;

			// 힐
			case EnchantState.Heal:
			
				// 강화 진행도 업데이트(숫자)
				_HealEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.HealLV.ToString();

				// 만약 강화 진행도가 최대레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.HealLV == _EnchantMaxLv) _HealEnchantStateText.text = "최대레벨";
				break;

			// 미니볼
			case EnchantState.MiniBall:
			
				// 강화 진행도 업데이트(숫자)
				_MiniBallEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.MiniBallMaximumCountLV.ToString();

				// 만약 강화 진행도가 최대레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.MiniBallMaximumCountLV == _EnchantMaxLv) _MiniBallEnchantStateText.text = "최대레벨";
				break;

			// 빅바
			case EnchantState.BigBar:
			
				// 강화 진행도 업데이트(숫자)
				_BigBarEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.BarMaxPlusCountLV.ToString();

				// 만약 강화 진행도가 최대레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.BarMaxPlusCountLV == _EnchantMaxLv) _BigBarEnchantStateText.text = "최대레벨";
				break;

			// 업그레이드
			case EnchantState.Upgrade:
			
				// 강화 진행도 업데이트(숫자)
				_UpgradeEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.UpgradeMaxCountLV.ToString();

				// 만약 강화 진행도가 최대레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.UpgradeMaxCountLV == _EnchantMaxLv) _UpgradeEnchantStateText.text = "최대레벨";
				break;

			// 히오스
			case EnchantState.Hos:
			
				// 강화 진행도 업데이트(숫자)
				_HosEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.HosDurationTimeLV.ToString();

				// 만약 강화 진행도가 최대레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.HosDurationTimeLV == _EnchantMaxLv) _HosEnchantStateText.text = "최대레벨";
				break;

			// 히오스 크기
			case EnchantState.HosSize:
			
				// 강화 진행도 업데이트(숫자)
				_HosSizeEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.HosSizeLV.ToString();

				// 만약 강화 진행도가 최대레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.HosSizeLV == _EnchantMaxLv) _HosSizeEnchantStateText.text = "최대레벨";
				break;

			// 총알 크기
			case EnchantState.BulletSize:
			
				// 강화 진행도 업데이트(숫자)
				_BulletSizeEnchantStateText.text = GameManager.getJsonDataManager.itemEnchant.BulletSizeLV.ToString();

				// 만약 강화 진행도가 최대레벨이라면
				if (GameManager.getJsonDataManager.itemEnchant.BulletSizeLV == _EnchantMaxLv) _BulletSizeEnchantStateText.text = "최대레벨";
				break;
		}

	}

	// 아이콘을 눌렀을 때 구매 창을 띄웁니다.
	public void OnClickIcon() {

        // 터치음 재생
        PlayClickSound();

        // 아이템 종류 별로
        switch (_EnchantState) {

			// 과부하
			case EnchantState.Overload:

				// 구매버튼 상태 설정
				_ItemBtn._EnchantState = EnchantState.Overload;
				
				break;

			// 총
			case EnchantState.Shoot:

				// 구매버튼 상태 설정
				_ItemBtn._EnchantState = EnchantState.Shoot;
                
				break;

			// 힐
			case EnchantState.Heal:

				// 구매버튼 상태 설정
				_ItemBtn._EnchantState = EnchantState.Heal;
                
				break;

			// 미니볼
			case EnchantState.MiniBall:

				// 구매버튼 상태 설정
				_ItemBtn._EnchantState = EnchantState.MiniBall;
                
				break;

			// 빅바
			case EnchantState.BigBar:

				// 구매버튼 상태 설정
				_ItemBtn._EnchantState = EnchantState.BigBar;
                
				break;

			// 업그레이드
			case EnchantState.Upgrade:

				// 구매버튼 상태 설정
				_ItemBtn._EnchantState = EnchantState.Upgrade;
                
				break;

			// 히오스
			case EnchantState.Hos:

				// 구매버튼 상태 설정
				_ItemBtn._EnchantState = EnchantState.Hos;
                
				break;

			// 히오스 크기
			case EnchantState.HosSize:

				// 구매버튼 상태 설정
				_ItemBtn._EnchantState = EnchantState.HosSize;
                
				break;

			// 총알 크기
			case EnchantState.BulletSize:

				// 구매버튼 상태 설정
				_ItemBtn._EnchantState = EnchantState.BulletSize;
                
				break;
		}

		// 아이템의 현재 강화도 설정
		//ItemDataUpdate();

		// 강화 단계별 가격, 구매조건
		EnchantPriceUpdate();
		EnchantStageUpdate();

		// 구매창 업데이트 호출
		PurchaseImageUpdate();

		// 한번 비활성화
		_PurchaseImage.gameObject.SetActive(false);

		// 구매 창 활성화
		_PurchaseImage.gameObject.SetActive(true);

		// 아이템 구매 버튼 활성화
		_ItemBtn.gameObject.SetActive(true);

		// 플레이어 구매 버튼 비활성화
		_PlayerBtn.gameObject.SetActive(false);

        // 설명 버튼 활성화
        _ExplanationBtn.gameObject.SetActive(true);
    }

    // 구매를 눌렀을시 호출
    public void OnClickPurchase() {

        // 터치음 재생
        PlayClickSound();

        switch (_EnchantState) {
            
            // 과부하
            case EnchantState.Overload:

				// 레벨이 최대라면 리턴
                if (itemEnchant.OverloadLV == _EnchantMaxLv) return;

				// 구매조건과, 재화가 충분하다면
                if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

					// 재화 감소
                    playerData.Coin -= _Price;

					// 레벨증가
                    itemEnchant.OverloadLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 총
            case EnchantState.Shoot:

				// 레벨이 최대라면 리턴
                if (itemEnchant.MaximumShootingCountLV == _EnchantMaxLv) return;

				// 구매조건과, 재화가 충분하다면
				if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

					// 재화 감소
					playerData.Coin -= _Price;

					// 레벨증가
					itemEnchant.MaximumShootingCountLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 힐
            case EnchantState.Heal:

				// 레벨이 최대라면 리턴
				if (itemEnchant.HealLV == _EnchantMaxLv) return;

				// 구매조건과, 재화가 충분하다면
				if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

					// 재화 감소
					playerData.Coin -= _Price;

					// 레벨증가
					itemEnchant.HealLV++;
                }

                // 구매 실패 호출
                else PuchaseFailed(_Price, _Stage); break;

            // 미니볼
            case EnchantState.MiniBall:

				// 레벨이 최대라면 리턴
				if (itemEnchant.MiniBallMaximumCountLV == _EnchantMaxLv) return;

				// 구매조건과, 재화가 충분하다면
				if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

					// 재화 감소
					playerData.Coin -= _Price;

					// 레벨증가
					itemEnchant.MiniBallMaximumCountLV++;
                }

				// 구매 실패 호출
				else PuchaseFailed(_Price, _Stage); break;

            // 빅바
            case EnchantState.BigBar:

				// 레벨이 최대라면 리턴
				if (itemEnchant.BarMaxPlusCountLV == _EnchantMaxLv) return;

				// 구매조건과, 재화가 충분하다면
				if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

					// 재화 감소
					playerData.Coin -= _Price;

					// 레벨증가
					itemEnchant.BarMaxPlusCountLV++;
                }

				// 구매 실패 호출
				else PuchaseFailed(_Price, _Stage); break;

            // 업그레이드
            case EnchantState.Upgrade:

				// 레벨이 최대라면 리턴
				if (itemEnchant.UpgradeMaxCountLV == _EnchantMaxLv) return;

				// 구매조건과, 재화가 충분하다면
				if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

					// 재화 감소
					playerData.Coin -= _Price;

					// 레벨증가
					itemEnchant.UpgradeMaxCountLV++;
                }

				// 구매 실패 호출
				else PuchaseFailed(_Price, _Stage); break;

            // 히오스
            case EnchantState.Hos:

				// 레벨이 최대라면 리턴
				if (itemEnchant.HosDurationTimeLV == _EnchantMaxLv) return;

				// 구매조건과, 재화가 충분하다면
				if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

					// 재화 감소
					playerData.Coin -= _Price;

					// 레벨증가
					itemEnchant.HosDurationTimeLV++;
                }

				// 구매 실패 호출
				else PuchaseFailed(_Price, _Stage); break;

            // 히오스 크기
            case EnchantState.HosSize:

				// 레벨이 최대라면 리턴
				if (itemEnchant.HosSizeLV == _EnchantMaxLv) return;

				// 구매조건과, 재화가 충분하다면
				if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

					// 재화 감소
					playerData.Coin -= _Price;

					// 레벨증가
					itemEnchant.HosSizeLV++;
                }

				// 구매 실패 호출
				else PuchaseFailed(_Price, _Stage); break;

            // 총알 크기
            case EnchantState.BulletSize:

				// 레벨이 최대라면 리턴
				if (itemEnchant.BulletSizeLV == _EnchantMaxLv) return;

				// 구매조건과, 재화가 충분하다면
				if (_Price <= playerData.Coin && playerData.Record >= _Stage) {

					// 재화 감소
					playerData.Coin -= _Price;

					// 레벨증가
					itemEnchant.BulletSizeLV++;
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

        // 강화진행도 업데이트
        EnchantStateGaugeUpdate();
        EnchantStateTextUpdate();

        // 코인 업데이트
        _EnchantCanvas.CoinsUpdate();
    }

    // 구매 실패시 호출
    private void PuchaseFailed(uint price, uint stage)
    {
		// 구매 불가창 활성화
        _EnoughImage.gameObject.SetActive(true);
        
        // 담기
        StringBuilder text = new StringBuilder();

		// 돈이 없다면
        if (GameManager.getJsonDataManager.playerData.Coin < 999) text.Append(" 그지는 구매할 수 없습니다.\n");

		// 돈이 부족하다면
		else if(price > GameManager.getJsonDataManager.playerData.Coin)
        {
            text.Append("코인이 부족합니다.\n필요한 최소 코인 : ");
            text.Append(price.ToString());

            // 코인 부족 목소리 실행
            PlayEnoughtCoin();
        }

		// 구매조건이 충족되지 않았다면
        else if (GameManager.getJsonDataManager.playerData.Record < stage)
        {
            text.Append("스테이지가 너무 낮습니다.\n구매가능 최소 스테이지 : ");
            text.Append(stage.ToString());

            // 기록 부족 목소리 실행
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
            case 0: return 0.0f;
            case 1: return 0.05f;
            case 2: return 0.1f;
            case 3: return 0.15f;
            case 4: return 0.2f;
            case 5: return 0.25f;
            case 6: return 0.3f;
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
    public int RBallDamage(int level)
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
    public float RHackSize(int level)
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
    public int RHackMaxCount(int level)
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
    public float RAddMoney(int level)
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
    public uint RHackDamage(int level)
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

    // 스테이지 건너뛰기 강화에 따른 시작 시간
    public int RStartStage(int level)
    {
        switch (level)
        {
            case 0: return 0;
            case 1: return 5;
            case 2: return 10;
            case 3: return 15;
            case 4: return 20;
            case 5: return 25;
            case 6: return 30;
            case 7: return 35;
            case 8: return 40;
            case 9: return 45;
            case 10: return 50;
        }

        return 0;
    }

    // 추가 입장권 강화에 따른 추가 입장권
    public int RAddVigor(int level)
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
