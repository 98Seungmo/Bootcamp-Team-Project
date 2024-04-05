using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KJ
{
    /**
     * @brief 로딩시 나오는 텍스트
     */
    public class LoadingTextAnimation : MonoBehaviour
    {
        [Header("LoadingText")]
        public TextMeshProUGUI loadingText; ///< 로딩 text
        public float dotSpeed = 0.5f; ///< '.' 텍스트 속도
        public string baseText = "로딩 중"; ///< 기본 문장
        public int dotCount = 0; ///< 현재 '.' 텍스트 개수

        /**
         * @brief AnimateDots 실행
         */
        void Start()
        {
            StartCoroutine(AnimateDots());
        }

        /**
         * @brief AnimateDots, 로딩에서 텍스트 뒤에 ... 개수 설정 및 반복
         */
        IEnumerator AnimateDots()
        {
            while (true)
            {
                /* '.' 의 개수를 0 ~ 4까지 반복 */
                dotCount = (dotCount + 1) % 5;
                /* 기본 텍스트에 '.' 추가 */
                loadingText.text = baseText + new string('.', dotCount);
                /* 지정된 시간 동안 대기 */
                yield return new WaitForSeconds(dotSpeed);
            }
        }
    }
}
