using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LodingSceneCanvas : MonoBehaviour
{
    // 로딩시 변경될 배경
    [SerializeField] private Image _LoadingImage;

    // 로딩시 출력될 랜덤 이미지들의 리스트
    [SerializeField] private List<Sprite> _LoadingImages;

    private void Start()
    {
        int lodingImageIndex = Random.Range(0, _LoadingImages.Count);

        // 랜덤으로 스프라이트 변경
        _LoadingImage.sprite = _LoadingImages[lodingImageIndex];
    }
}
