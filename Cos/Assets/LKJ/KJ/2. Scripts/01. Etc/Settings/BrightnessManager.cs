using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 게임의 밝기 조절
     */
    public class BrightnessManager : MonoBehaviour
    {
        [Header("Slider")]
        public Slider brightnessSlider; ///< 밝기 조절을 위한 슬라이더
        [Header("PostProcessing")]
        public Volume volume; ///< Volume 컴포넌트 할당
        private ColorAdjustments _colorAdjustments; ///< Volume 의 ColorAdjustments 할당

        /**
         * @brief 슬라이더 조절에 따라 밝기 조절
         */
        void Start()
        {
            /* 볼룸의 프로필에서 colorGrading 찾아옴 */
            if (volume.profile.TryGet<ColorAdjustments>(out _colorAdjustments))
            {
                /* 슬라이더 값에 따라 HandleSliderChanged 메서드를 호출 */
                brightnessSlider.onValueChanged.AddListener(HandleSliderChanged);
            }

        }

        /**
         * @brief Post Exposure 값 조절
         * @param[in] value 슬라이더 값
         */
        public void HandleSliderChanged(float value)
        {
            _colorAdjustments.postExposure.value = value;
            // Debug.Log(_colorAdjustments.postExposure.value);

        }
    }
}

