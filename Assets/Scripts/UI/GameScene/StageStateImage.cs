using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageStateImage : MonoBehaviour
{
    [SerializeField] private Image _StageImage;
    [SerializeField] private Text _StageText;
    [SerializeField] private WaveManager wave;
    private float _StageStateSpeed = 400.0f;
    private bool _StateMoveable = true;
    private float _StartX;

    // 스테이지 이미지 시작시점
    private float stageStateImageStartX;

    private void Awake() {
        stageStateImageStartX = _StageImage.rectTransform.anchoredPosition.x;
    }

    private void OnEnable() {

        // 현재 스테이지로 텍스트 변경
        _StageText.text = "WAVE" + wave.ReturnStage();

        // 스테이지 상태 이미지 활성화
        _StageImage.gameObject.SetActive(true);

        // 스테이지 상태 이미지 움직이기 가능
        _StateMoveable = true;

        // 스테이지 텍스트
        StartCoroutine(moveStateText());
    }

    // 스테이지 텍스트 옆으로 이동
    private IEnumerator moveStateText() {

        // 시작 지점
        _StartX = stageStateImageStartX;

        // 현재 X 값
        float currentX = _StartX;

        // 끝 지점
        float endX = 0.0f;

        while (_StateMoveable) {

            // 오른쪽으로 이동
            currentX = Mathf.MoveTowards(currentX, endX, _StageStateSpeed * Time.deltaTime);

            _StageImage.rectTransform.anchoredPosition = new Vector2(
                currentX
                , _StageImage.rectTransform.anchoredPosition.y);

            // 목표 지점에 도달했다면
            if (Mathf.Approximately(endX, currentX)) {

                if (Mathf.Approximately(endX, 0.0f)) {
                    yield return new WaitForSecondsRealtime(1.0f);
                    endX = _StartX * -1.0f;
                }
                else {
                    _StateMoveable = false;
                }
            }

            yield return null;
        }

        // 비활성화 구문
        _StageImage.gameObject.SetActive(false);
    }

}
