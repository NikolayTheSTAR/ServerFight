using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image fillImg;
    [SerializeField] private TextMeshProUGUI valueLabel;

    public void Set(int value, float progress)
    {
        valueLabel.text = value.ToString();
        fillImg.fillAmount = progress;
    }
}