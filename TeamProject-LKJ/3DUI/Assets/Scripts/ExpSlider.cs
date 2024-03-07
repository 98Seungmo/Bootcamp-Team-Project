using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpSlider : MonoBehaviour
{
    // 숫자 표시할 슬라이더 종류
    public Slider SoundSlider;
    public Slider BrightnessSlider;
    public Slider ZoomInOutSlider;
    // 숫자 나타낼 각 슬라이더의 텍스트
    public TextMeshProUGUI SSliderText;
    public TextMeshProUGUI BSliderText;
    public TextMeshProUGUI ZSliderText;

    public void ChangeSlider()
    {
        // 각 슬라이더의 value 값을 받아서 text에 저장.
        SSliderText.text = SoundSlider.value.ToString();
        BSliderText.text = BrightnessSlider.value.ToString();
        ZSliderText.text = ZoomInOutSlider.value.ToString();
    }
    
}
