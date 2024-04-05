using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KJ
{
    public class TalkingHeadSystem : MonoBehaviour
    {
        [Header("talking head")]
        public GameObject talkingHead; ///< Talking Head UI
        public TMP_Text talkingHeadText; ///< Talking Head 텍스트
        [Header("sectionid")]
        public int sectionID; ///< Talking Head 나타날 구역 ID

        /**
         * @brief 처음 토킹헤드 비활성화
         */
        void Start()
        {
            talkingHead.SetActive(false);
        }

        /**
         * @brief switch 문으로 구역 진입시 토킹헤드 나타나기 위한 구역들 나열
         * @param[in] other 구역 충돌 지점
         */
        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                Debug.Log("충돌?");
                switch (sectionID)
                {
                    case 1:
                        talkingHeadText.text = "첫 번째 방";
                        break;
                    case 2:
                        talkingHeadText.text = "두 번째 방";
                        break;
                }
                Debug.Log("코루틴 불러옴");
                ShowTalkingHeadUI();
            }
        }

        /**
         * @brief UI 활성화
         */
        public void ShowTalkingHeadUI()
        {
            Debug.Log("UI 시작");
            talkingHead.SetActive(true);
            StartCoroutine(TalkingHeadTimer(3));
        }

        /**
         * @brief x 초뒤에 UI 사라짐
         */
        IEnumerator TalkingHeadTimer(float delay)
        {
            yield return new WaitForSeconds(delay);
            HideTalkingHeadUI();
        }

        /**
         * @brief UI 비활성화
         */
        public void HideTalkingHeadUI()
        {
            talkingHead.SetActive(false);
        }
    }
}
