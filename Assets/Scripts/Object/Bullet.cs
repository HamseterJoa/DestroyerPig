using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType { Player, Enemy, Magic, Arong, PigHouse, Strom, Esther, Induction}
    public BulletType _BulletType = BulletType.Player;
    public float _Damage;

    // 투사체 속도
    public float _Speed = 7.0f;

	// 아롱이 공
	public float _XSpeed = 7.0f;

	// 돼지집
	public Vector2 _FireForce = Vector2.zero;

    // 공격을 했는지 검사(공격했다면 위치가 업데이트 되지않음)
    public bool lockOn = false;

    // 플레이어의 위치로 던질것 인지
    public bool aim = false;

    // 회오리 움직임
    private bool _StromMove = false;

    // 회오리가 날아갈 방향
    public Vector2 stormtarget;

    //  플레이어의 위치
    private Vector2 _TargetDiraction;

	// 에스더가 던질공의 리지드바디참조
	[SerializeField] private Rigidbody2D _EstherBallrigid;
    
	// 에스더의 공이 튕길 때마다 증가할 카운트
	private int _EstherBallCount;

	private void OnEnable()
    { 
        // 플레이어의 위치로 던지는 탄이라면 다시 위치 받아오도록 설정
        if (aim) lockOn = false;

		// 에스더의 오물이라면
		if (_BulletType == BulletType.Esther) {
			
			// 방향설정 (플레이어방향으로)
			Vector2 targetPos = GameManager.getCharacterManager.player.transform.position - transform.position;

			// 날려주기
			_EstherBallrigid.AddForce(targetPos * _Speed * 10.0f);
		}

        // 활성화 되었을 때 아롱이 공이라면
        if (_BulletType == BulletType.Arong)
        {
            // 아롱이 참조
            ArongE arongE = GameObject.Find("ArongE(Clone)").GetComponent<ArongE>();

            if (arongE != null) _XSpeed = 7.0f;
        }
    }
	
	private void Update() {

		// 플레이어의 위치로 던질것이라면
		if (aim){

			// 공격하지 않았다면 방향을 설정
			if (!lockOn) _TargetDiraction = GameManager.getCharacterManager.player.transform.position - transform.position;

		}

        // 총알 날아감
        BulletMove();
    }

    // 총알 날아감
    private void BulletMove() {

        switch (_BulletType) {

            // 적의 투사체
            case BulletType.Enemy:

				// 만약 플레이어의 방향으로 던질것이라면
                if (aim)
                {
                    lockOn = true;

                    _TargetDiraction.Normalize();
                    
                    transform.Translate(_TargetDiraction * _Speed * Time.deltaTime);
                }

				// 통상
                else transform.Translate(Vector2.down * _Speed * Time.deltaTime);

                break;

            // 플레이어 투사체
            case BulletType.Player:
                transform.Translate(Vector2.up * _Speed * Time.deltaTime);
                break;

            // 적의 마법 투사체
            case BulletType.Magic:

				// 만약 플레이어의 방향으로 던질것이라면
				if (aim) {
                    lockOn = true;

                    _TargetDiraction.Normalize();

                    transform.Translate(_TargetDiraction * _Speed * Time.deltaTime);
                }

				// 통상
                else transform.Translate(Vector2.down * _Speed * Time.deltaTime);

                break;

            // 아롱이 공(x자로)
			case BulletType.Arong:

				transform.Translate(Vector2.down * 3.5f * Time.deltaTime);

				transform.Translate(Vector2.right * _XSpeed *Time.deltaTime);

                // 위치에 따라 방향 바꿔줌
				if (transform.position.x <= -2.5f || transform.position.x >= 2.5f) _XSpeed *= -1.0f;

				break;

            // 돼지집(방향설정 후 밀어주기)
			case BulletType.PigHouse:

				transform.Translate(_FireForce * _Speed * Time.deltaTime);

				break;

            // 회오리
            case BulletType.Strom:

                if (!_StromMove)
                {
					transform.position = Vector2.MoveTowards(transform.position, stormtarget, _Speed * Time.deltaTime);

                    if (transform.position.x == stormtarget.x && transform.position.y == stormtarget.y) _StromMove = true;
                }

                // 아래로
                else transform.Translate(Vector2.down * _Speed * Time.deltaTime);

                break;

            // 유도
            case BulletType.Induction:

                transform.position = Vector2.MoveTowards(transform.position, GameManager.getCharacterManager.player.transform.position, 1.0f * Time.deltaTime);

                break;
                
        }

        // 화면 밖으로 나가면 비활성화
        if (transform.position.y >= 6.0f || transform.position.y <= -5.5f || transform.position.x < -3.3f || transform.position.x > 3.3f)
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        // 회오리라면
        if (_BulletType == BulletType.Strom) _StromMove = false;

		// 에스더 공이라면
		if (_BulletType == BulletType.Esther) {
			
			// 카운트 0으로
			_EstherBallCount = 0;
            
		}
    }

    // 맞았을 때 처리(트리거)
    private void OnTriggerEnter2D(Collider2D collision) {

        switch (_BulletType) {

            // 적의 총알이
            case BulletType.Enemy:

                // 플레이어에게
                if (collision.transform.tag == "Player") {

                    // 플레이어가 데미지를 받을수 있는 지 검사
                    if (GameManager.getCharacterManager.player.damageable)
                        GameManager.getCharacterManager.player.HPDown(_Damage);
                    
                    // 비활성화
                    gameObject.SetActive(false);
                }

                // 보호막에
                if (collision.transform.tag == "HOS") gameObject.SetActive(false);
                
				break;

            // 나의 총알
            case BulletType.Player:

                // 적에게
                if (collision.transform.tag == "Enemy") {
                    gameObject.SetActive(false);
                }
                break;

            case BulletType.Magic:

                // 플레이어에게
                if (collision.transform.tag == "Player") {

                    // 플레이어가 데미지를 받을수 있는 지 검사
                    if (GameManager.getCharacterManager.player.damageable) {

                        // 데미지를 주고
                        GameManager.getCharacterManager.player.HPDown(_Damage);

                        // 정지시킴
                        GameManager.getCharacterManager.player.StopPlayer();

                    }

                    // 보호막에
                    if (collision.transform.tag == "HOS") gameObject.SetActive(false);

                    // 비활성화
                    gameObject.SetActive(false);
				}

                break;

			case BulletType.Arong: 

				if (collision.transform.tag == "Player"){

					// 플레이어가 데미지를 받을수 있는 지 검사
					if (GameManager.getCharacterManager.player.damageable)
						GameManager.getCharacterManager.player.HPDown(_Damage);

					// 비활성화
					gameObject.SetActive(false);

				}

				// 보호막에
				if (collision.transform.tag == "HOS") gameObject.SetActive(false);

				break;

			case BulletType.PigHouse:

				if (collision.transform.tag == "Player") {

					// 플레이어가 데미지를 받을수 있는 지 검사
					if (GameManager.getCharacterManager.player.damageable)
						GameManager.getCharacterManager.player.HPDown(_Damage);

					// 비활성화
					gameObject.SetActive(false);

				}

				// 보호막에
				if (collision.transform.tag == "HOS") gameObject.SetActive(false);

				break;

            // 회오리
            case BulletType.Strom:

                if (collision.transform.tag == "Player")
                {
                    // 플레이어가 데미지를 받을수 있는 지 검사
                    if (GameManager.getCharacterManager.player.damageable)
                        GameManager.getCharacterManager.player.HPDown(_Damage);

                    // 비활성화
                    gameObject.SetActive(false);

                }

                // 보호막에
                if (collision.transform.tag == "HOS") gameObject.SetActive(false);

                break;

			// 에스더 공
			case BulletType.Esther:
				if (collision.transform.tag == "HOS") gameObject.SetActive(false); break;

            // 유도탄
            case BulletType.Induction:

                if (collision.transform.tag == "Player")
                {
                    // 플레이어가 데미지를 받을수 있는 지 검사
                    if (GameManager.getCharacterManager.player.damageable)
                        GameManager.getCharacterManager.player.HPDown(_Damage);

                    // 비활성화
                    gameObject.SetActive(false);

                }

                // 보호막에 닿으면 사라짐
                if (collision.transform.tag == "HOS") gameObject.SetActive(false);

                break;
                
        }

    }

	// 맞았을 때 처리(콜라이더)
	private void OnCollisionEnter2D(Collision2D collision) {
		
		// 에스더 공
		if (_BulletType == BulletType.Esther) {

			// 플레이어와 충돌한다면
			if (collision.transform.tag == "Player") {

				// 플레이어가 데미지를 받을수 있는 지 검사
				if (GameManager.getCharacterManager.player.damageable)
					GameManager.getCharacterManager.player.HPDown(_Damage);

				// 비활성화
				gameObject.SetActive(false);
			}

			else {

				// 카운트 증가
				_EstherBallCount++;

				// 3번 이상 팅긴다면 비활성화
				if (_EstherBallCount >= 3) gameObject.SetActive(false);

			}
		}

	}

}
