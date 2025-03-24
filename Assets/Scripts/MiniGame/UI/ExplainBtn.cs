using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainBtn : MonoBehaviour
{
    // 설명 이미지
    [SerializeField] private Image _ExplainImage;

    public void On()
    {
        _ExplainImage.gameObject.SetActive(true);
    }

    public void Off()
    {
        _ExplainImage.gameObject.SetActive(false);
    }
}
