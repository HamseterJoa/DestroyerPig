using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingCredit : MonoBehaviour
{
    [SerializeField] private List<Image> _CreditList;

    private float _Speed = Screen.height / 2;

    private void Update()
    {
        MoveDown();

        if (Input.GetKey(KeyCode.Mouse0)) _Speed = 300.0f;
        else if (Input.GetKeyUp(KeyCode.Mouse0)) _Speed = 100.0f;
    }

    private void MoveDown()
    {
        _CreditList[0].rectTransform.Translate(Vector2.up * _Speed * Time.deltaTime);
        _CreditList[1].rectTransform.Translate(Vector2.up * _Speed * Time.deltaTime);
        _CreditList[2].rectTransform.Translate(Vector2.up * _Speed * Time.deltaTime);
        _CreditList[3].rectTransform.Translate(Vector2.up * _Speed * Time.deltaTime);
        _CreditList[4].rectTransform.Translate(Vector2.up * _Speed * Time.deltaTime);

        // 끝나면사라짐
        if (_CreditList[4].rectTransform.anchoredPosition.y >= 0.0f) gameObject.SetActive(false);
    }

}
