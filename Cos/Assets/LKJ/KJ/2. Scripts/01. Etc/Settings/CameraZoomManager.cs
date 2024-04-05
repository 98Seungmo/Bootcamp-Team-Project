using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 게임 옵션 중 카메라 줌 조절 기능
     */
    public class CameraZoomManager : MonoBehaviour
    {
        [Header("Slider")]
        public Slider zoomSlider; ///< 카메라 줌 슬라이더
        [Header("Camera")]
        public Camera orthographicCamera; ///< Orthographic 카메라

        /**
         * @brief 슬라이더 조절에 따라 카메라 줌 인 아웃 설정
         */
        void Start()
        {
            /* 슬라이더의 초기값을 현재 Orthographic Size 값으로 설정 */
            zoomSlider.value = orthographicCamera.orthographicSize;
            /* 줌 인 최소값 최대값 */
            zoomSlider.minValue = 5;
            zoomSlider.maxValue = 10;
            /* 슬라이더 값이 변경될때 OrthographicSize 값도 변하는 AdjustZoom 메서드 호출 */
            zoomSlider.onValueChanged.AddListener(AdjustZoom);
        }

        /**
         * @brief Orthographic Size 값 조절
         * @param[in] newSize 슬라이더 값
         */
        public void AdjustZoom(float newSize)
        {
            orthographicCamera.orthographicSize = newSize;
        }
    }
}
