using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNotice : MonoBehaviour
{
    private bool _Notice;
    private Vector2 _NoticeTargetPos = new Vector2(1.3f, 3.0f);
    private Vector2 _NoticeStandardPos = new Vector2(4.5f, 3.0f);

    private SpriteRenderer _SR;

    // 도망감
    [SerializeField] private Sprite _BossRun;

    // 등장
    [SerializeField] private Sprite _BossAppear;

    // 죽음
    [SerializeField] private Sprite _BossDie;

    // 알림 종류
    public enum NoticeSort { Appear, Run, Die}
    public static NoticeSort _NoticeState = NoticeSort.Appear;

    private void Awake()
    {
        _SR = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // 이미지 변경
        switch (_NoticeState)
        {
            // 등장
            case NoticeSort.Appear: _SR.sprite = _BossAppear; break;

            // 도망
            case NoticeSort.Run: _SR.sprite = _BossRun; break;

            // 죽음
            case NoticeSort.Die: _SR.sprite = _BossDie; break;
        }

        _Notice = true;

        // 이동
        StartCoroutine(Notice());
    }

    private IEnumerator Notice()
    {
        while (true)
        {
            if (_Notice)
            {
                transform.position = Vector2.MoveTowards(transform.position, _NoticeTargetPos, 1.5f * Time.deltaTime);

                if (transform.position.x == _NoticeTargetPos.x)
                {
                    yield return new WaitForSeconds(0.5f);
                    _Notice = false;
                }

            }

            else
            {
                transform.position = Vector2.MoveTowards(transform.position, _NoticeStandardPos, 1.5f * Time.deltaTime);

                if (transform.position.x == _NoticeStandardPos.x) transform.gameObject.SetActive(false);
            }

            yield return null;

        }

    }
}
