using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /**
     * @brief 해상도 설정
     */
    public class ResolutionManager : MonoBehaviour
    {
        /**
         * @brief FHD 해상도
         * @param[in] width 넓이
         * @param[in] height 높이
         * @param[in] isFullScreen 전체화면 가능 여부
         */
        public void SetGameFHD(int width, int height, bool isFullScreen)
        {
            Debug.Log("FHD 해상도로 변경!");
            /* 화면을 1920 x 1080, 전체화면 가능하게 만듬 */
            Screen.SetResolution(1920, 1080, true);
        }

        /**
         * @brief QHD 해상도
         */
        public void SetGameQHD(int width, int height, bool isFullScreen)
        {
            Debug.Log("QHD 해상도로 변경!");
            /* 화면을 2560 x 1440, 전체화면 가능하게 만듬 */
            Screen.SetResolution(2560, 1440, true);
        }
    }
}

