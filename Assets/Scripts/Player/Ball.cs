using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ball : MonoBehaviour
{
    private Rigidbody2D _Rigid = null;
    private HackGauge _HackGauge;
    private float _HackPlus;
    public bool isHack = false;
    private HackEffect _HackEffect;
    [SerializeField] private int _BallDamage;

    // 이펙트
    [SerializeField] private GameObject _AttackEffect;
    private List<GameObject> _AttackEffectList = new List<GameObject>();

    // 오디오
    [SerializeField] private AudioSource _Audio;

    // 핵
    [SerializeField] private AudioSource _HackAudio;
    
    private void Awake()
    {
        JsonData.ItemEnchant item = GameManager.getJsonDataManager.itemEnchant;

        // 공 데미지
        switch (item.BallDamageLV)
        {
            case 0: _BallDamage = 1; break;
            case 1: _BallDamage = 2; break;
            case 2: _BallDamage = 3; break;
        }

        // 핵 게이지 충전속도?
        switch (item.HackPlusLV) {
            case 0: _HackPlus = 0.1f; break;
        }

        // 핵 게이지 참조
        _HackGauge = GameObject.Find("Canvas").transform.Find("ScreenPanel").transform.Find("HackGauge").GetComponent<HackGauge>();

        // 핵 이펙트 참조
        _HackEffect = GameObject.Find("HackEffect").GetComponent<HackEffect>();

        // 핵 이펙트 비활성화
        _HackEffect.gameObject.SetActive(false);

        // 리지드바디 참조
        _Rigid = GetComponent<Rigidbody2D>();

        // 
        GameManager.getCharacterManager.ball = this;
    }

    private void Update()
    {
        // 케릭터가 비활성화 되었고 공이 활성화 중이라면
        if (!GameManager.getCharacterManager.player.gameObject.activeSelf && gameObject.activeSelf)
            gameObject.SetActive(false);
    }

    public void Initialize()
    {
        // 최대 속도
        float limitSpeed = 20.0f;

        IEnumerator velocityLimit()
        {
            while (true)
            {
                yield return new WaitUntil(() => (
                Mathf.Abs(_Rigid.velocity.x) >= limitSpeed || Mathf.Abs(_Rigid.velocity.y) >= limitSpeed));

                Vector2 limitVelocity = _Rigid.velocity;
                limitVelocity.x = Mathf.Clamp(limitVelocity.x, -limitSpeed, limitSpeed);
                limitVelocity.y = Mathf.Clamp(limitVelocity.y, -limitSpeed, limitSpeed);
                _Rigid.velocity = limitVelocity;
            }
        }

        StartCoroutine(velocityLimit());
    }

    //public void BallDamageUp() { _BallDamage++; }
    //public void BallDamageDown() { _BallDamage--; }
    public int BallDamage { get { return _BallDamage; } }

	// 핵을 추가해줌
    public void AttackEnemy() {

        _HackGauge.AddHackGauge(_HackPlus);

        // 비활성화중인 이펙트가 있다면 담음
        int index = FindDisableAttackEffect();

        // 비활성화중인 이펙트가 없다면 생성
        if (index == -1) CreateAttackEffect();

        // 있다면 활성화
        else EnableAttackEffect(index);

		// 플레이어가 핵이 준비됬고 몬스터와 부디친다면
        if (isHack) {
            _HackEffect.gameObject.SetActive(true);
            PlayHackSound();
            isHack = false;
        }
    }

    public void BallAttackSound()
    { 
        // 볼륨 설정
        _Audio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 소리 재생
        _Audio.Play();
    }
    
    private void PlayHackSound()
    {
        _HackAudio.volume = GameManager.getJsonDataManager.playerData.EffectValue;

        // 소리 재생
        _HackAudio.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            AttackEnemy();

            // 공 공격소리 재생
            BallAttackSound();
        }

        // 옥수수 밭에 닿으면 체력감소
        if (collision.transform.tag == "CornArea" && GameManager.getJsonDataManager.playerData.Hardmode) GameManager.getCharacterManager.player.HPDown(5.0f);

    }
    
    // 공격 이펙트 생성
    private void CreateAttackEffect()
    {
        // 생성
        GameObject effect = Instantiate(_AttackEffect);

        // 좌표 설정
        effect.transform.position = transform.position;

        // 리스트에 추가
        _AttackEffectList.Add(effect);
    }

    // 비활성화 중인 이펙트를 찾음
    private int FindDisableAttackEffect()
    {
        for (int i = 0; i < _AttackEffectList.Count; i++)
            if (!_AttackEffectList[i].gameObject.activeSelf) return i;

        return -1;
    }

    // 공격 이펙트 활성화
    private void EnableAttackEffect(int index)
    {
        // 활성화
        _AttackEffectList[index].gameObject.SetActive(true);

        // 좌표설정
        _AttackEffectList[index].transform.position = transform.position;
    }

}

