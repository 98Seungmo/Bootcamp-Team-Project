using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /**
     * @brief 미니맵용 카메라, 카메라가 target(player)이 스폰될 때 target의 위치 추적
     */
    public class MinimapCamera : MonoBehaviour
    {

        [SerializeField]
        /// x축 제어.
        private bool x; ///< x축 제어.
        private bool y; ///< y축 제어.
        private bool z; ///< z축 제어.

        [Header("Target Transform")]
        [SerializeField] private Transform target; ///< 추적할 대상.

        /**
         * @brief Player Tag 로 target의 위치 찾음
         */
        private void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
        }
        /**
         * @brief target 이 있으면 target 위치, 없으면 현재 위치
         */
        void Update()
        {
            if (!target) return;
            transform.position = new Vector3(
                (x ? target.position.x : transform.position.x),
                (y ? target.position.y : transform.position.y),
                (z ? target.position.z : transform.position.z));
        }
    }
}
