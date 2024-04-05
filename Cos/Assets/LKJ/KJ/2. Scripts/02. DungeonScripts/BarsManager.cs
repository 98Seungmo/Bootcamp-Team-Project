using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 플레이어 체력바 관리
     */
    public class BarsManager : MonoBehaviour
    {
        [Header("Slider")]
        public Slider healthSlider; ///< 체력
        public Slider staminaSlider; ///< 스테미나
        [Header("Cur / Max")]
        public TMP_Text healthCur; ///< 현재 체력 텍스트
        public TMP_Text healthMax; ///< 최대 체력 텍스트
        public TMP_Text staminaCur; ///< 현재 스테미나 텍스트
        public TMP_Text staminaMax; ///< 최대 스테미나 텍스트
        [Header("MaxHp")]
        public float maxHealth = 100f; ///< 최대 체력 = 100f
        [Header("MaxSp")]
        public float maxStamina = 100f; ///< 최대 스테미나 = 100f
        [Header("StaminaRecovery")]
        public float staminaRecoveryRate = 5f; ///< 스테미나 회복 값 = 5f

        private float _currentHealth; ///< 현재 체력
        private float _currentStamina; ///< 현재 스테미나

        /**
         * @brief 체력 및 스테미나 UI 설정
         */
        void Start()
        {
            /* 초기 체력 설정 */
            _currentHealth = maxHealth;
            /* 슬라이더의 최대값을 최대 체력으로 설정 */
            healthSlider.maxValue = maxHealth;
            UpdateHealthUI();
            /* 초기 스테미나 설정 */
            _currentStamina = maxStamina;
            /* 슬라이더의 최대값을 최대 스테미너로 설정 */
            staminaSlider.maxValue = maxStamina;
            /* 슬라이더의 값 초기화 */
            staminaSlider.value = _currentStamina;
        }

        /**
         * @brief UpdateStaminaUI 호출
         */
        void Update()
        {
            UpdateStaminaUI();
        }

        /**
         * @brief 체력바, 텍스트 UI 업데이트
         */
        void UpdateHealthUI()
        {
            // Debug.Log($"Current Health: {_currentHealth}, Max Health: {maxHealth}, Slider Value: {healthSlider.value}");
            /* Slider, 텍스트 업데이트 */
            healthSlider.value = _currentHealth;
            healthCur.text = $"{_currentHealth}";
            healthMax.text = $"{maxHealth}";
        }

        /**
         * @brief 스테미나 UI 업데이트
         */
        void UpdateStaminaUI()
        {
            /* 스테미나 업데이트 */
            if (_currentStamina < maxStamina)
            {
                Debug.Log($"Current SP : {_currentStamina}, Max SP : {maxStamina}, Slider Value : {staminaSlider.value}");
                /* 스테미나가 시간에 따라 자동으로 채워짐. */
                _currentStamina += staminaRecoveryRate * Time.deltaTime;
                /* 최대 스테미너 넘지 않음. */
                _currentStamina = Mathf.Min(_currentStamina, maxStamina);
                /* 슬라이더 업데이트 */
                staminaSlider.value = _currentStamina;
                /* 텍스트 업데이트 */
                /* 정수로 변환하여 소수점 안나오게 함. */
                staminaCur.text = Mathf.RoundToInt(_currentStamina).ToString();
                staminaMax.text = $"{maxStamina}";
            }
        }
    }
}
