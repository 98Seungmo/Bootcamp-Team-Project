using System.Linq;
using HJ;
using KJ;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 버프 온오프 활성화
     */
    public class BuffOnOff : MonoBehaviour
    {
        [Header("Buff Images")]
        public Image powerBuffImage; ///< 공격력 버프 이미지
        public Image healthBuffImage; ///< 방어력 버프 이미지
        public Image specialBuffImage; ///< 스테미나 회복 버프 이미지

        [Header("Bool Buffs")]
        [SerializeField] public bool _powerBuff = false; ///< 버프 활성화 = 오프
        [SerializeField] public bool _healthBuff = false; ///< 버프 활성화 = 오프
        [SerializeField] public bool _specialBuff = false; ///< 버프 활성화 = 오프

        PlayerController playerController; ///< 플레이어 컨트롤러

        /**
         * @brief 초기 버프 이미지의 컬러 값 회색으로 설정
         */
        void Start()
        {
            powerBuffImage.color = Color.gray;
            healthBuffImage.color = Color.gray;
            specialBuffImage.color = Color.gray;
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }

        /**
         * @brief 공격력 버프 활성화시 해당 색으로 바뀜, 플레이어의 공격력 = 0.2f 향상
         */
        public void PowerBuff()
        {

            if (_powerBuff == true)
            {
                powerBuffImage.color = new Color(214 / 255f, 150 / 255f, 150 / 255f, 1f);
                playerController.attackFood = 0.2f;
            }
            else
            {
                powerBuffImage.color = Color.gray;
                playerController.attackFood = 0;
            }
        }

        /**
         * @brief 방어력 버프 활성화시 해당 색으로 바뀜, 플레이어의 방어력 = 0.2f 
         */
        public void HealthBuff()
        {
            if (_healthBuff == true)
            {
                healthBuffImage.color = new Color(1f, 0f, 0f, 1f);
                playerController.armorFood = 0.2f;
            }
            else
            {
                healthBuffImage.color = Color.gray;
                playerController.armorFood = 0;
            }
        }

        /**
         * @brief 스테미나 회복량 활성화, 해당 버프 이미지의 색이 바뀜, 버프 = 0.4f
         */
        public void SpecialBuff()
        {
            if (_specialBuff == true)
            {
                specialBuffImage.color = new Color(1f, 1f, 0f, 1f);
                playerController.spRecoveryFood = 0.4f;
            }
            else
            {
                specialBuffImage.color = Color.gray;
                playerController.spRecoveryFood = 0;
            }
        }

        #region 버프 
        
        /**
         * @brief 공격력 버프 활성화
         */
        public void PowerOn()
        {
            _powerBuff = true;
            PowerBuff();
        }

        /**
         * @brief 방어력 버프 활성화 
         */
        public void HealthOn()
        {
            _healthBuff = true;
            HealthBuff();
        }

        /**
         * @brief 스테미나 회복량 버프 활성화
         */
        public void SpecialOn()
        {
            _specialBuff = true;
            SpecialBuff();
        }
        #endregion
    }
}
