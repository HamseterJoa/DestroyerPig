using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private void Awake()
    {
        // 카메라
        Camera cam = GetComponent<Camera>();

        // 카메라 렉트
        Rect rect = cam.rect;

        // 높이(세로)
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 16);

        // 넓이(가로)
        float scaleWidth = 1.0f / scaleHeight;

        // 더 길다면
        if (scaleHeight < 1.0f)
        {
            rect.height = scaleHeight;
            rect.y = (1.0f - scaleHeight) / 2.0f;
        }

        else
        {
            rect.width = scaleWidth;
            rect.x = (1.0f - scaleWidth) / 2.0f;
        }

        // 렉트 설정
        cam.rect = rect;
        
    }
}
