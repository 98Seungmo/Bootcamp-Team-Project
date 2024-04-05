using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    public class ItemCollector : MonoBehaviour
    {
        /* 알림 메시지 */
        [Header("Notification")]
        public GameObject notification; ///< 알림 메시지 UI
        public TMP_Text notificationText; ///< 알림 메시지 텍스트

        /**
         * @brief 처음에 알림 메시지 숨김
         */
        void Start()
        {
            /* 처음에만 알림 메시지 숨김 */
            notification.SetActive(false);
        }

        /**
         * @brief 아이템 얻을 시 알림 메시지 표시
         */
        void CollectItem()
        {
            // 아이템을 인벤토리에 추가하는 로직 넣기.

            /* 알림메시지 표시 */
            notification.SetActive(true);
            notificationText.text = "아이템을 얻음."; // $"{getItem}을 얻었습니다."

            StartCoroutine(HideNotification(3));
        }

        /**
         * @brief 알림 메시지 자동 숨김
         */
        IEnumerator HideNotification(float delay)
        {
            yield return new WaitForSeconds(delay);
            notification.SetActive(false);
        }
    }
}
