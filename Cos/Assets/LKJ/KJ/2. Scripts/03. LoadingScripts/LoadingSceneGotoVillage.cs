using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 마을로 가는 로딩 씬
     */
    public class LoadingSceneGotoVillage : MonoBehaviour
    {
        Transform characterSelectManager = GameObject.FindWithTag("Player").transform; ///< 캐릭터 씬 이동시 플레이어 태그를 찾음

        /**
         * 던전에서 마을로 포탈 탔을 때 나오는 로딩씬
         */
        void Start()
        {
            #region 던전 -> 마을 로딩 씬 
            /* LoadAsyncSceneCoroutine() 을 코루틴으로 시작 */
            StartCoroutine(LoadAsyncSceneCoroutine());
            #endregion
        }

        /**
         * @brief 로딩동안 슬라이더바 채워짐
         */
        void Update()
        {
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * 10);
            /* 슬라이더 값 업데이트 */
            slider.value = currentProgress;
        }

        #region 던전 -> 마을 로딩 씬 
        [Header("Slider")]
        public Slider slider; ///< 슬라이더
        public string sceneName; ///< 씬 이름

        float currentProgress = 0f; ///< 현재 진행 상태
        float targetProgress = 0f; ///< 목표 진행 상태

        /**
         * @brief 아이템 데이터 로드 후 해당 씬으로 이동
         */
        IEnumerator LoadAsyncSceneCoroutine()
        {
            /* 아이템 데이터 로드 */
            yield return ItemDBManager.Instance.LoadItemDB();
            targetProgress += 1f;

            targetProgress = 1f;

            // sceneName 으로 비동기 형식으로 넘어가게 하는 operation 생성.
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            // operation 이 완료되어도 다음 씬으로 넘어가는걸 막음.
            operation.allowSceneActivation = false;

            // operation.isDone 이 false 일 동안 반복.
            while (!operation.isDone)
            {
                if (targetProgress >= 1f)
                {
                    operation.allowSceneActivation = true;
                }

                //다음 프레임까지 대기.
                yield return null;
            }

            

            SceneManager.LoadSceneAsync(sceneName);
        }
        #endregion
    }

}
