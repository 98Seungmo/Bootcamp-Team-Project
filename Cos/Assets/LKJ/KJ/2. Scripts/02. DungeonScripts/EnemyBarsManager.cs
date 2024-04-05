using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 보스 및 적들 체력바 설정
     */
    public class EnemyBarsManager : MonoBehaviour
    {
        [Header("Boss")]
        public Slider bossHpSlider; ///< 보스 체력바
        [Header("Enemy")]
        public Slider enemyHpSlider; ///< 적 체력바
        [Header("Cur / Max")]
        public TMP_Text bossHpCur; ///< 보스 현재 체력
        public TMP_Text bossHpMax; ///< 보스 최대 체력
        public TMP_Text enemyHpCur; ///< 적 현재 체력
        public TMP_Text enemyHpMax; ///< 적 최대 체력
        [Header("MaxHp")]
        public float maxBossHp = 500f; ///< 보스 최대 체력 = 500f
        public float maxEnemyHp = 80f; ///< 적 최대 체력 = 80f

        private float _currentBossHp; ///< 현재 보스 체력
        private float _currentEnemyHp; ///< 현재 적 체력

        /**
         * @brief 보스 및 적들 초기 체력 설정
         */
        void Start()
        {
            /* 초기 체력 설정. */
            _currentBossHp = maxBossHp;
            _currentEnemyHp = maxEnemyHp;
            /* 슬라이더 최대값 최대 체력으로 설정. */
            bossHpSlider.maxValue = maxBossHp;
            enemyHpSlider.maxValue = maxEnemyHp;
            UpdateTargetUI();
        }

        /**
         * @brief 보스 및 적들 체력 UI 업데이트
         */
        void Update()
        {
            UpdateTargetUI();
        }

        /**
         * @brief 체력바 UI 업데이트
         */
        void UpdateTargetUI()
        {
            /* Slider, 텍스트 업데이트 */
            bossHpSlider.value = _currentBossHp;
            bossHpCur.text = $"{_currentBossHp}";
            bossHpMax.text = $"{maxBossHp}";

            enemyHpSlider.value = _currentEnemyHp;
            enemyHpCur.text = $"{_currentEnemyHp}";
            enemyHpMax.text = $"{maxEnemyHp}";
        }
    }
}
