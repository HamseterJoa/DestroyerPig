using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackEffect : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(AutoDisable());
    }

    // 핵 범위
    private Vector2 _HackSize;

    // 이펙트
    [SerializeField] private GameObject _Effect;
    private List<GameObject> _EffectList = new List<GameObject>();
    
    private void Effect(Vector2 pos)
    {
        int index = FindDisableEffect();

        if (index == -1) CreateEffect(pos);

        else EnableEffect(index, pos);
    }

    // 생성
    private void CreateEffect(Vector2 pos)
    {
        // 생성 
        GameObject effect = Instantiate(_Effect);

        // 좌표설정
        effect.transform.position = pos;

        // 리스트에 추가
        _EffectList.Add(effect);
    }

    // 비활찾기
    private int FindDisableEffect()
    {
        for (int i = 0; i < _EffectList.Count; i++)
            if (!_EffectList[i].activeSelf) return i;

        return -1;
    }

    // 활성화
    private void EnableEffect(int index, Vector2 pos)
    {
        // 활성화
        _EffectList[index].gameObject.SetActive(true);

        // 좌표설정
        _EffectList[index].transform.position = pos;
    }

    private void Awake()
    {
        JsonData.ItemEnchant item = GameManager.getJsonDataManager.itemEnchant;

        // 핵 범위
        switch (item.HackSizeLV)
        {
            case 0: _HackSize = new Vector2(0.3f, 0.3f); break;
            case 1: _HackSize = new Vector2(0.45f, 0.45f); break;
            case 2: _HackSize = new Vector2(0.6f, 0.6f); break;
        }

        // 핵 범위 설정
        transform.localScale = _HackSize;
    }

    private void Update()
    {
		Mov();
	}

	private void Mov(){
        if (GameManager.getCharacterManager.ball != null)
            transform.position = GameManager.getCharacterManager.ball.transform.position;
	}
    
    private IEnumerator AutoDisable()
    {

        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.15f);

            transform.gameObject.SetActive(false);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy") Effect(transform.position);
    }
}
