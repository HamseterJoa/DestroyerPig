using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected float m_HP;
    protected float m_MaxHP;
    protected float m_MoveSpeed;
    protected uint m_Money;
    protected bool m_Moveable = true;
    protected float m_Damage;
    protected float m_AddMoney;
    protected bool m_IsBoss = false;
    protected Animator m_Anim;

    protected uint _HackDamage;

    [SerializeField] protected List<AudioClip> m_PigDieAudioList = new List<AudioClip>();
    [SerializeField] protected AudioSource m_Audio;

    // 웨이브 매니저
    protected WaveManager m_Wave;

    protected void Awake()
    {
        JsonData.ItemEnchant itemEnchant = GameManager.getJsonDataManager.itemEnchant;

        // 오디오 참조
        m_Audio = GetComponent<AudioSource>();

        // 웨이브매니저 참조
        m_Wave = GameObject.Find("WaveManager").GetComponent<WaveManager>();

        // 애니메이터 참조
        m_Anim = GetComponent<Animator>();

        // 추가로 얻는 돈
        switch (itemEnchant.AddMoneyLV)
        {
            case 0: m_AddMoney = 1.0f; break;
            case 1: m_AddMoney = 1.05f; break;
            case 2: m_AddMoney = 1.07f; break;
            case 3: m_AddMoney = 1.1f; break;
            case 4: m_AddMoney = 1.12f; break;
            case 5: m_AddMoney = 1.15f; break;
            case 6: m_AddMoney = 1.17f; break;
            case 7: m_AddMoney = 1.2f; break;
            case 8: m_AddMoney = 1.22f; break;
            case 9: m_AddMoney = 1.24f; break;
            case 10: m_AddMoney = 1.26f; break;
            case 11: m_AddMoney = 1.28f; break;
            case 12: m_AddMoney = 1.30f; break;
            case 13: m_AddMoney = 1.32f; break;
            case 14: m_AddMoney = 1.34f; break;
            case 15: m_AddMoney = 1.36f; break;
            case 16: m_AddMoney = 1.38f; break;
            case 17: m_AddMoney = 1.40f; break;
            case 18: m_AddMoney = 1.42f; break;
            case 19: m_AddMoney = 1.44f; break;
            case 20: m_AddMoney = 1.46f; break;
            case 21: m_AddMoney = 1.48f; break;
            case 22: m_AddMoney = 1.50f; break;
            case 23: m_AddMoney = 1.52f; break;
            case 24: m_AddMoney = 1.54f; break;
            case 25: m_AddMoney = 1.56f; break;
            case 26: m_AddMoney = 1.58f; break;
            case 27: m_AddMoney = 1.60f; break;
            case 28: m_AddMoney = 1.62f; break;
            case 29: m_AddMoney = 1.64f; break;
            case 30: m_AddMoney = 1.66f; break;
        }

        // 하드 모드라면 1.5배
        if (GameManager.getJsonDataManager.playerData.Hardmode)
            m_AddMoney *= 1.5f;

        // 핵 데미지
        switch (itemEnchant.HackDamageLV)
        {
            case 0: _HackDamage = 2; break;
            case 1: _HackDamage = 3; break;
            case 2: _HackDamage = 4; break;
            case 3: _HackDamage = 5; break;
            case 4: _HackDamage = 6; break;
            case 5: _HackDamage = 7; break;
            case 6: _HackDamage = 8; break;
        }

    }

    protected void PlayDieSound()
    {
        if (m_Audio != null)
        {
            // 볼륨 설정
            m_Audio.volume = GameManager.getJsonDataManager.playerData.VoiceValue;

            // 재생할 목소리 랜덤 설정
            int randomNumber = Random.Range(0, m_PigDieAudioList.Count);

            // 재생할 목소리 설정
            m_Audio.clip = m_PigDieAudioList[randomNumber];
            // 재생
            m_Audio.Play();
        }
    }

    // 체력이 0이되면 호출
    protected void Disable()
    {
		// 체력이 0이하로 내려갓다면
        if (m_HP <= 0) {

            // 코인 추가
            GameManager.getCharacterManager.coinCountgetset += (uint)Mathf.Round(m_Money * m_AddMoney);

            // 코인상태 업데이트
            m_Wave.CoinStateUpdate();

            // 플레이어 회복
            GameManager.getCharacterManager.player.EnemyDieToHeal();

            // 아이템 생성
            int Rand = Random.Range(0, 10);

            if (Rand < 2) {

				// 비활성화 중인 아이템 담기
                int itemNumber = GameManager.getItemManager.RandomItemNumber();

				// 생성
                if (GameManager.getItemManager.FindDisableItemNumber(itemNumber) == -1)
                    GameManager.getItemManager.CreateItem(itemNumber, transform.position);
				
				// 활성화
                else GameManager.getItemManager.EnableItem(GameManager.getItemManager.FindDisableItemNumber(itemNumber), transform.position);
            }

            // 보스라면 보스킬카운트 증가
            if (m_IsBoss) GameManager.getCharacterManager.KillBossgetset++;

            // 돼지 처치 추가
            GameManager.getCharacterManager.killPiggetset++;
        }

		// 비활성화
        gameObject.SetActive(false);
        
		// 움직이기 가능
		m_Moveable = true;
		
		// 체력 최대로
		m_HP = m_MaxHP;
    }

    protected abstract void Move();

    // 체력이 감소할 때 호출
    protected void HPDown(string damageType) {

        // 데미지 받기
        switch (damageType)
        {
            case "Ball": m_HP -= GameManager.getCharacterManager.ball.BallDamage; break;
            case "MiniBall": m_HP--; break;
            case "Bullet": m_HP--; break;
            case "Hack": m_HP -= (int)_HackDamage; break;
            case "HOS": m_HP -= 0.1f; break;
        }

        // 체력 검사
        if (m_HP <= 0.0f)
        {
            m_Moveable = false;
            m_Anim.SetBool("Die", true);
        }

    }

    protected void OnDisable()
    {
        // 보스이고 비활성화될 때 체력이 0보다 작다면 죽음 호출
        if (m_IsBoss) if (m_HP < 0.0f) m_Wave.BossDieNotice();
    }

    #region 맞음처리
    protected void OnCollisionEnter2D(Collision2D collision) {

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
        if (collision.transform.tag== "CornArea") {

			// 보스가 아니라면 대미지를 주고 비활성화
			if (!m_IsBoss) {
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
        if (collision.transform.tag == "Ball")
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
