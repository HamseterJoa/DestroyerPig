using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 이펙트
    [SerializeField] private GameObject _Effect;
    private List<GameObject> _EffectList = new List<GameObject>();

    // 등장시 폭발
    [SerializeField]private bool _EnableBoom = false;

    private void OnEnable()
    {
        if (_EnableBoom) Effect(transform.position);
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            // 이펙트 재생
            Effect(transform.position);

            // 플레이어가 데미지를 받을수 있는 지 검사
            if (GameManager.getCharacterManager.player.damageable)
                GameManager.getCharacterManager.player.HPDown(50.0f);
        }
            
    }
}
