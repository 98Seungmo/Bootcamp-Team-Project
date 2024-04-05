using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 로딩 씬 에서 이미지 회전 
     */
    public class LoadingSpinnerAnimation : MonoBehaviour
    {
        public int rotSpeed = -1; ///< 회전 속도

        /**
         * @brief 일정 주기로 회전
         */
        void Update()
        {
            transform.Rotate(0, 0, rotSpeed + Time.deltaTime);
        }
    }
}
