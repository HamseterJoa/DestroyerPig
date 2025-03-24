using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    //  > 현재 어느 페이지를 표시중인지 검사할 변수입니다.
    public enum GameSceneState { Playing, Pause };
    [SerializeField] private GameSceneState _GameSceneState = GameSceneState.Playing;

    //  > 페이지를 넘기고있는지 확인할 때 사용될 변수
    public bool isStateSwitching { get; private set; }

    //  > 넘겨질 때 Panel 의 목표 X 위치
    private Vector2 _TargetPosition = Vector2.zero;

    //  > Option, Main Panel 을 가지고있는 Panel 의 RectTransform
    [SerializeField] private RectTransform _Panel = null;
    
    // 지정한 페이지로 넘깁니다.
    public void SwitchingPage(GameSceneState nextPage)
    {
        //  > 만약 요정된 페이지가 현재 페이지와 다르다면
        if (_GameSceneState != nextPage)
        {
            //  > 다음 페이지를 설정합니다.
            _GameSceneState = nextPage;

            //  > 메뉴 페이지를 넘깁니다.
            isStateSwitching = true;

            //  > 요청된 페이지에 따라 목표 좌표를 설정합니다.
            switch (nextPage)
            {
                case GameSceneState.Playing:
                    _TargetPosition.Set(0.0f, _Panel.anchoredPosition.y);
                    break;
                case GameSceneState.Pause:
                    _TargetPosition.Set(_Panel.sizeDelta.x * 0.5f, _Panel.anchoredPosition.y);
                    break;
            }

            // 상태에 따라 시간 정지, 흐름
            switch (_GameSceneState)
            {
                case GameSceneState.Playing: Time.timeScale = 1.0f; break;
                case GameSceneState.Pause: Time.timeScale = 0.0f; break;
            }

            //  > 페이지를 넘깁니다.
            StartCoroutine(MoveNextPage());
        }
    }

    //  > 다음 페이지로 Panel을 이동시킵니다.
    private IEnumerator MoveNextPage()
    {
        if (!isStateSwitching) yield break;

        while (isStateSwitching)
        {
            //  > 목표 위치로 이동시킵니다.
            _Panel.anchoredPosition = Vector2.Lerp(
                _Panel.anchoredPosition, _TargetPosition, 10.0f * Time.fixedDeltaTime);

            //  > 목표 위치와 근접해 있다면
            if (Vector2.Distance(_Panel.anchoredPosition, _TargetPosition) < 0.05f)
            {
                //  > 목표 위치로 설정합니다.
                _Panel.anchoredPosition = _TargetPosition;
                isStateSwitching = false;
            }
            
            // 다음 Update 까지 대기합니다.
            yield return null;
        }
    }
    
}
