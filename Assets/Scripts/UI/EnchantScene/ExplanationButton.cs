using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplanationButton : MonoBehaviour
{
    // 띄울 이미지
    [SerializeField] private Image _ExplanationImage;

    // 구매 버튼
    [SerializeField] private ItemEnchantButton _PurchaseBtn;

    // 이미지 리스트를 담음
    [SerializeField] private List<Sprite> _ItemExplanationList;

    private void OnEnable()
    {
        // 이미지 설정
        if (_PurchaseBtn != null)
            switch (_PurchaseBtn._EnchantState)
            {
                // 불꼬추
                case ItemEnchantButton.EnchantState.Overload: _ExplanationImage.sprite = _ItemExplanationList[0]; break;

                // 수박씨발사
                case ItemEnchantButton.EnchantState.Shoot: _ExplanationImage.sprite = _ItemExplanationList[1]; break;

                // 얼룩버섯
                case ItemEnchantButton.EnchantState.Upgrade: _ExplanationImage.sprite = _ItemExplanationList[2]; break;

                // 감구마
                case ItemEnchantButton.EnchantState.MiniBall: _ExplanationImage.sprite = _ItemExplanationList[3]; break;

                // 썩은 옥수수 크기
                case ItemEnchantButton.EnchantState.HosSize: _ExplanationImage.sprite = _ItemExplanationList[4]; break;

                // 옥수수
                case ItemEnchantButton.EnchantState.Hos: _ExplanationImage.sprite = _ItemExplanationList[4]; break;

                // 토마토 주스
                case ItemEnchantButton.EnchantState.Heal: _ExplanationImage.sprite = _ItemExplanationList[5]; break;

                // 수박씨 크기
                case ItemEnchantButton.EnchantState.BulletSize: _ExplanationImage.sprite = _ItemExplanationList[1]; break;

                // 우엉
                case ItemEnchantButton.EnchantState.BigBar: _ExplanationImage.sprite = _ItemExplanationList[6]; break;
            }

    }

    // 설명 켜기 버튼
    public void OpenExplanationImageBtn()
    {
        _ExplanationImage.gameObject.SetActive(true);
    }

    // 설명 끄기 버튼
    public void ExitExplanationImageBtn()
    {
        _ExplanationImage.gameObject.SetActive(false);
    }

    // 꺼질때
    private void OnDisable()
    {
        _ExplanationImage.gameObject.SetActive(false);
    }

}
