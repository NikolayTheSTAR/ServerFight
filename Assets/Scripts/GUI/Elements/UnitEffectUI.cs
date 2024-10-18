using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitEffectUI : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private TextMeshProUGUI valueText;

    public void Set(Sprite iconSprite, int value)
    {
        iconImg.sprite = iconSprite;
        valueText.text = value.ToString();
    }
}