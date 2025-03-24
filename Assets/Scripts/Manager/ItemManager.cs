using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] private List<Item> _ItemList;

    [SerializeField] private List<Item> _CreateItemList = new List<Item>();

    private AudioSource _Audio;

    private void Awake()
    {
        _Audio = GetComponent<AudioSource>();
    }

    public void ClearItemList()
    {
        _CreateItemList = new List<Item>();
    }

    public void PlayItemUseSound()
    {
        // 볼륨 설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 재생
        _Audio.Play();
    }

    #region 아이템
    // 아이템 번호를 받아와 비활성화 중인 아이템을 찾습니다.
    public int FindDisableItemNumber(int itemNumber)
    {
        for (int i = 0; i < _CreateItemList.Count; i++)
        {
            if (_CreateItemList[i].transform.name == ItemNumberToString(itemNumber) && !_CreateItemList[i].transform.gameObject.activeSelf)
            {
                return i;
            }
        }

        return -1;
    }

    // 아이템 생성
    public void CreateItem(int itemNumber, Vector2 spawnPos)
    {
        Vector2 initPos = spawnPos;

        Quaternion initRot = Quaternion.identity;

        Item randomItem = Instantiate(_ItemList[itemNumber], initPos, initRot);

        _CreateItemList.Add(randomItem);

    }

    // 아이템 활성화
    public void EnableItem(int itemNumber,Vector2 spawnPos)
    {
        _CreateItemList[itemNumber].transform.gameObject.SetActive(true);

        _CreateItemList[itemNumber].transform.position = spawnPos;
    }

    // 아이템번호 뽑기
    public int RandomItemNumber()
    {
        // 확률 뽑기
        int randomNumber = Random.Range(0, 100);
        
        // 아이템번호를 담을 변수
        int ItemNumber = 0;

        if (randomNumber < 14) ItemNumber = 6; // 과부하
        else if (randomNumber < 33) ItemNumber = 5; // 히오스
        else if (randomNumber < 52) ItemNumber = 1; // 슛
        else if (randomNumber < 63) ItemNumber = 4; // 미니볼
        else if (randomNumber < 73) ItemNumber = 0; // 힐
        else if (randomNumber < 87) ItemNumber = 3; // 빅바
        else ItemNumber = 2; // 업그레이드

        return ItemNumber;
    }

    // 아이템번호를 이름으로
    public string ItemNumberToString(int itemNumber)
    {
        switch (itemNumber)
        {
            case 0: return "HealItem(Clone)";
            case 1: return "ShootItem(Clone)";
            case 2: return "UpgradeItem(Clone)";
            case 3: return "BigBarItem(Clone)";
            case 4: return "MiniBallItem(Clone)";
            case 5: return "HOSItem(Clone)";
            case 6: return "OverloadItem(Clone)";
        }

        return null;
    }

    #endregion

}
