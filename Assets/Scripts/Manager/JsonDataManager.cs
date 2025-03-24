using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JsonData;
using System.IO;

public enum MiniGamePlayerState { Pig, Potato, Black, Angel, Soil, Tied, Rolling, Coward, Three, Pearl, Magic, Sludge, Bouncing, Hamster, Destroyer, Iron, Guv,
    Hundred, Arong, House, Crow, King, Golden, Esther, Doll, Duck, Baby1, Baby2, Baby3, Baby4, Servant, GoldendCoin, P, CrocoMaster, Bird, Croco}

public class JsonDataManager : MonoBehaviour
{
    public ItemEnchant itemEnchant { get; set; }
    public PlayerData playerData { get; set; }

    private void Awake()
    {
        // 폴더 생성
        if (!playerData.Prolog) Directory.CreateDirectory(Application.persistentDataPath + "/Data");

        LoadItemEnchantData();
        LoadPlayerData();
    }

    // 경로
    public string GetJsonFilePath(string fileName)
    {
        return Application.persistentDataPath + "/Data" + "/" + fileName + ".json";
    }

    //  아이템 데이터 로드
    private void LoadItemEnchantData()
    {
        string fileName = "ItemEnchant";

        itemEnchant = JsonUtility.FromJson<ItemEnchant>(File.ReadAllText(
            GetJsonFilePath(fileName)));
    }

    //  > 설정된 itemEnchant 값을 파일에 저장합니다.
    public void SaveItemEnchantData()
    {
        //  > 저장할 데이터를 문자열로 변환합니다.
        string save = JsonUtility.ToJson(itemEnchant, true);
        File.WriteAllText(GetJsonFilePath("ItemEnchant"), save);
    }

    // 플레이어 데이터 로드
    private void LoadPlayerData()
    {
        string fileName = "Player";

        playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(GetJsonFilePath(fileName)));
    }

    // //  > 설정된 playerData 값을 파일에 저장합니다.
    public void SavePlayerData()
    {
        string save = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(GetJsonFilePath("Player"), save);
    }

    private void OnDestroy()
    {
        //  > 아이템값 저장
        SaveItemEnchantData();

        //  > 플레이어값 저장
        SavePlayerData();
    }

}

namespace JsonData
{
    [System.Serializable]
    public struct ItemEnchant
    {
        // 플레이어 최대체력
        public uint PlayerMaxHPLV;

        // 총알 최대 발사수
        public uint MaximumShootingCountLV;

        // 미니볼 최대 생성수
        public uint MiniBallMaximumCountLV;

        // 빠 최대 크기 증가횟수
        public uint BarMaxPlusCountLV;

        // 업그레이드 최대 크기 증가회수
        public uint UpgradeMaxCountLV;

        // 공 데미지
        public uint BallDamageLV;

        // 과부하 지속시간
        public uint OverloadLV;

        // 핵 게이지 충전량 증가
        public uint HackPlusLV;

        // 핵 최대 충전량 증가
        public uint HackMaxCountLV;

        // 핵 데미지 증가
        public uint HackDamageLV;

        // 힐 증가
        public uint HealLV;

        // 돼지 처치시 얻는 돈 증가
        public uint AddMoneyLV;

        // 체력감소 딜레이 증가
        public uint AddTimeLV;

        // 히오스 지속시간
        public uint HosDurationTimeLV;

        // 돼지 처치시 회복할 체력 증가
        public uint KillPigHealLV;

        // 핵 범위 증가
        public uint HackSizeLV;

        // 스테이지 시작 시간
        //public uint StartStageLV;

        // 보스가 나오는 스테이지 간격
        public uint BossIntervalLV;

        // 히오스 크기 증가
        public uint HosSizeLV;

        // 총알 크기 증가
        public uint BulletSizeLV;

        // 추가 행동력
        public uint AddVigorLV;
    }

    [System.Serializable]
    public struct PlayerData
    {
        // 재화
        public uint Coin;

        // 기록
        public uint Record;

        // 캐쉬
        public uint Cash;

        // 행동력
        public uint Vigor;

		// 년
		public string StartTimeYear;

		// 월
		public string StartTimeMonth;

		// 일
		public string StartTimeDay;

		// 시
		public string StartTimeHour;

		// 분
		public string StartTimeMinute;

		// 초
		public string StartTimeSecond;

        // BGM 볼륨
        public float BGMValue;

        // 효과음 볼륨
        public float EffectValue;

        // 목소리 볼륨
        public float VoiceValue;

        // 프롤로그를 봤는지
        public bool Prolog;

        // 미니 게임 기록
        public float MiniGameRecord;
        
        // 미니 게임에서의 플레이어 상태
        public MiniGamePlayerState PlayerState;

        // 뚱딴지를 뽑았는지 검사
        public bool Potato;

        // 흑돼지를 뽑았는지 검사
        public bool Black;

        // 날개돼지를 뽑았는지 검사
        public bool Angel;

        // 오물 돼지를 뽑았는지 검사
        public bool Soil;

        // 묶인 돼지를 뽑았는지 검사
        public bool Tied;

        // 구르는 돼지를 뽑았는지 검사
        public bool Rolling;

        // 겁쟁이 돼지를 뽑았는지 검사
        public bool Coward;

        // 삼형제를 뽑았는지 검사
        public bool Three;

        // 진주를 뽑았는지 검사
        public bool Pearl;

        // 마법요정 돼지를 뽑았는지 검사
        public bool Magic;

        // 진 오물돼지를 뽑았는지 검사
        public bool Sludge;

        // 튀는 애를 뽑았는지 검사
        public bool Bouncing;

        // 햄스터를 뽑았는지 검사
        public bool Hamster;

        // 파괴자를 뽑았는지 검사
        public bool Destroyer;

        // 철갑을 뽑았는지 검사
        public bool Iron;

        // 우두머리를 뽑았는지 검사
        public bool Guv;

        // 100인분을 뽑았는지 검사
        public bool Hundred;

        // 아롱이를 뽑았는지 검사
        public bool Arong;

        // 돼지집을 뽑았는지 검사
        public bool House;

        // 까마귀를 뽑았는지 검사
        public bool Crow;

        // 돼지왕을 뽑았는지 검사
        public bool King;

        // 복돼지를 뽑았는지 검사
        public bool Golden;

        // 에스더를 뽑았는지 검사
        public bool Esther;

        // 돼지인형옷을 뽑았는지 검사
        public bool Doll;

        // 오리를 뽑았는지 검사
        public bool Duck;

        // 복실이를 뽑았는지 검사
        public bool Baby1;

        // 삼쨩을 뽑았는지 검사
        public bool Baby2;

        // 마롱이를 뽑았는지 검사
        public bool Baby3;

        // 이치쨩을 뽑았는지 검사
        public bool Baby4;

        // 하수인을 뽑았는지 검사
        public bool Servant;

        // 황금 동전을 뽑았는지 검사
        public bool GoldenCoin;

        // P를 뽑았는지 검사
        public bool P;

        // 악어주인을 뽑았는지 검사
        public bool CrocoMaster;

        // 악어새를 뽑았는지 검사
        public bool Bird;

        // 악어를 뽑았는지 검사
        public bool Croco;

        // 엑스트라 어려움인지 검사
        public bool Hardmode;
        
    }

}
