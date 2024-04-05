using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /**
     * @brief 던전내 상호작용 UI
     */
    public class DungeonInteraction : MonoBehaviour
    {
        [Header("InteractionUI")]
        private Transform player; ///< 플레이어 위치에 고정
        public GameObject interactUI; ///< 상호작용 UI
        public float moveUI = 5.0f; ///< UI 플레이어로부터 얼마만큼 올릴건지
        public float interactRange = 3.0f; ///< 상호작용 범위
        public List<GameObject> interactionObjects; ///< 상호작용 리스트

        /**
         * @brief UI 활성화 및 플레이어에 위치
         */
        void Start()
        {
            Debug.Log("비활성화");
            interactUI.SetActive(false);
            player = GameObject.FindWithTag("Player").transform;
        }

        /**
         * @brief 상호작용 가능한 오브젝트 찾고 오브젝트 근처에 있으면 UI 활성화
         */
        void Update()
        {
            /* 가장 가까운 상호작용 가능 오브젝트 찾기 */
            /* 가장 가까운 오브젝트와의 거리를 저장할 변수를 최대값으로 초기화 */
            float closestDistance = float.MaxValue;
            /* 상호작용 가능 오브젝트 순회 */
            foreach (var interactionObject in interactionObjects)
            {
                /* 플레이어와 오브젝트 사이의 거리를 변수로 저장 */
                float distance = Vector3.Distance(player.position, interactionObject.transform.position);
                /* 현재 오브젝트가 지금까지 찾은 가장 가까운 오브젝트보다 가깝다면 */
                if (distance < closestDistance)
                {
                    Debug.Log("업데이트");
                    /* 가장 가까운 거리를 현재 거리로 업데이트 */
                    closestDistance = distance;
                }
            }
            /* 플레이어와 가장 가까운 오브젝트가 상호작용 범위에 있으면 */
            if (closestDistance <= interactRange)
            {
                Debug.Log("UI 활성화");
                /* 플레이어가 collider 에 진입하면 활성화. */
                ShowInteractionUI();
            }
            else   // 없으면
            {
                Debug.Log("UI 비활성화");
                /* 플레이어가 collider 에 벗어나면 비활성화. */
                HideInteractionUI();
            }
        }

        /**
         * @brief 상호작용 UI 활성화
         */
        public void ShowInteractionUI()
        {
            /* UI 활성화 */
            interactUI.SetActive(true);
            /* UI 위치 */
            interactUI.transform.position = player.position + Vector3.up * moveUI;
            /* UI 를 카메라로부터 항상 정면으로 보이게 설정. */
            Vector3 direction = Camera.main.transform.position - interactUI.transform.position;
            direction.y = 0;
            Quaternion rotation = Quaternion.LookRotation(direction);
            interactUI.transform.rotation = Quaternion.Slerp(interactUI.transform.rotation, rotation, Time.deltaTime * 10);
        }

        /**
         * @brief 상호작용 UI 비활성화
         */
        public void HideInteractionUI()
        {
            interactUI.SetActive(false);
        }
    }
}
