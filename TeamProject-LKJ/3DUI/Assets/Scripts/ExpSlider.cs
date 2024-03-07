using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpSlider : MonoBehaviour
{
    // ���� ǥ���� �����̴� ����
    public Slider SoundSlider;
    public Slider BrightnessSlider;
    public Slider ZoomInOutSlider;
    // ���� ��Ÿ�� �� �����̴��� �ؽ�Ʈ
    public TextMeshProUGUI SSliderText;
    public TextMeshProUGUI BSliderText;
    public TextMeshProUGUI ZSliderText;

    public void ChangeSlider()
    {
        // �� �����̴��� value ���� �޾Ƽ� text�� ����.
        SSliderText.text = SoundSlider.value.ToString();
        BSliderText.text = BrightnessSlider.value.ToString();
        ZSliderText.text = ZoomInOutSlider.value.ToString();
    }
    
}
