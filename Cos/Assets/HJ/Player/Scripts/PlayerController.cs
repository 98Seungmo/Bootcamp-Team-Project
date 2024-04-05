using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

using CharacterController = HJ.CharacterController;
 
namespace HJ
{
    /// <summary>
    /// 플레이어 캐릭터의 CharacterController를 제어하고 플레이어의 고유한 기능을 부여한다.
    /// </summary>
    public class PlayerController : CharacterController
    {
        protected override void Start()
        {
            base.Start();
            StaminaStart();
            PotionStart();
        }
        protected override void Update()
        {
            base.Update();
            StaminaUpdate();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        // Stamina =============================================================================================================================================================================

        // 기력의 입출력을 제어한다.
        public float stamina
        {
            get => _stamina;
            set { _stamina = Mathf.Clamp(value, 0, _spMax); }
        }
        private float _stamina;

        // 최대 기력 = (기본값(100) + 장신구) * ( 1 + 스킬강화 + 음식강화)
        public float spMax { get => ( _spMax + spMaxItem ) * ( 1 + spMaxSkill + spMaxFood ); }
        private float _spMax = 100;
        public float spMaxItem;
        public float spMaxSkill;
        public float spMaxFood;

        // 최대 기력 회복속도(초당) = (기본값(35) + 장신구) * ( 1 + 스킬강화 + 음식강화)
        public float spRecovery { get => (_spRecovery + spRecoveryItem) * (1 + spRecoverySkill + spRecoveryFood); }
        private float _spRecovery = 35;
        public float spRecoveryItem;
        public float spRecoverySkill;
        public float spRecoveryFood;

        // 기력 회복 여부
        public bool isSpRecover { get => _isSpRecover; set => _isSpRecover = value; }
        private bool _isSpRecover;

        // 캐릭터의 동작의 기력 소모량
        public float staminaRequired { set => _staminaRequired = value; }
        private float _staminaRequired;

        /// <summary>
        /// 기력 관련 초기 설정. Start시 실행.
        /// </summary>
        private void StaminaStart()
        {
            stamina = spMax;
        }

        /// <summary>
        /// _isSpRecover == true라면 기력을 회복한다. Update시 실행.
        /// </summary>
        private void StaminaUpdate()
        {
            if (_isSpRecover)
            {
                if (stamina < spMax)
                {
                    stamina += spRecovery * Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// _isSpRecover == true로 대입. 기력 회복을 중단시킨다.
        /// </summary>
        public void StaminaRecoverStart()
        {
            _isSpRecover = true;
        }
        /// <summary>
        /// _isSpRecover = false로 대입. 기력 회복을 시작한다.
        /// </summary>
        public void StaminaRecoverStop()
        {
            _isSpRecover = false;
        }

        /// <summary>
        /// 기력을 소모하고 기력 회복을 중단시킨다. 기력이 부족하다면 동작을 취소시킨다.
        /// </summary>
        /// <returns></returns>
        public bool StaminaUse()
        {
            if (stamina > _staminaRequired)
            {
                stamina -= _staminaRequired;
                StaminaRecoverStop();
                return true;
            }
            else
            {
                StateCancle();
                return false;
            }
        }

        // [Potion] ====================================================================================================================================================================
        // 최대 회복물약 소지량 = 기본값(5) + 추가 소지량.
        [SerializeField] int maxPotion { get => _maxPotion + maxPotionItem;  }
        private int _maxPotion = 5;
        public int maxPotionItem;

        // 현재 회복물약 소지량.
        public int potionNumber { get => _potionNumber; set => _potionNumber = Mathf.Clamp(value, 0, maxPotion); }
        [SerializeField] int _potionNumber;
        public float potionHp { get => _potionNumber + potionHpItem; }
        // 회복물약 회복량 = 기본값(35) + 추가 회복량;
        [SerializeField] float _potionHp = 35;
        public float potionHpItem;

        /// <summary>
        /// 회복물약 관련 초기 설정. Start시 실행된다.
        /// </summary>
        private void PotionStart()
        {
            _potionNumber = maxPotion;
        }
        /// <summary>
        /// 회복물약 사용시 포션 갯수를 소모하고 생명력을 회복한다.
        /// </summary>
        protected void Potion()
        {
            potionNumber--;
            RecoverHp(potionHp);
        }
        /// <summary>
        /// 회복물약 소지량을 최대로 한다.
        /// </summary>
        public void PotionFull()
        {
            potionNumber = _maxPotion;
        }

        #region InputSystem ===============================================
        /// <summary>
        /// 이동 키(WASD)를 입력하면 (x,y)벡터를 가져오고, 카메라 시점에 맞게 45도 회전한다.
        /// </summary>
        /// <param name="context"></param>
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();
            horizontal = inputVector.x * 0.707f + inputVector.y * 0.707f;
            vertical = inputVector.x * -0.707f + inputVector.y * 0.707f;
        }

        /// <summary>
        /// 걷기 키(Cntrol)를 누르면 이동속도를 0.5배로 늦춘다. 현재는 사용되지 않는다.
        /// </summary>
        /// <param name="context"></param>
        public void OnWalk(InputAction.CallbackContext context)
        {
            //if (context.performed)
            //    _velocity = 0.5f;
            //else
            //    _velocity = 1.0f;
        }

        /// <summary>
        /// 회피 키(space)를 누르면 Dodge를 호출한다.
        /// </summary>
        /// <param name="context"></param>
        public void OnDodge(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Dodge();
            }
        }

        /// <summary>
        /// 공격A 키(J)를 누르면 AttackA를 호출한다.
        /// </summary>
        /// <param name="context"></param>
        public void OnAttackA(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                AttackA();
            }
        }

        /// <summary>
        /// 공격B 키(K)를 누르면 AttackA를 호출하고 떼면 AttackBRelease를 호출한다.
        /// </summary>
        /// <param name="context"></param>
        public void OnAttackB(InputAction.CallbackContext context)
        {
            if (context.interaction is HoldInteraction)
            {
                AttackB();
            }
            else if (context.interaction is PressInteraction)
            {
                AttackBRelease();
            }
        }

        /// <summary>
        /// 상호작용 키(F)를 누르면 Interact를 호출한다.
        /// </summary>
        /// <param name="context"></param>
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Interact();
            }
        }

        /// <summary>
        /// 회복물약 사용 키(U)를 누르면 포션 소지량이 남아있고 체력이 최대치가 아니라면 UseItem을 호출한다.
        /// </summary>
        /// <param name="context"></param>
        public void OnUseItem(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (_potionNumber > 0 && hp <= hpMax -1)
                {
                    UseItem();
                }
            }
        }
        #endregion ========================================================
        /// <summary>
        /// 피격시 호출. 방어중이며 공격 각도가 방어 각도 이내라면 생명력 대신 기력을 소모하고, Animator의 "state"를 11로 한다.
        /// 기력이 부족하다면 기력을 모두 소모하고 나머지 피해량만큼 생명력을 소모하고 Animator의 "state"를 3으로 한다.
        /// 방어중이 아니라면 기존과 같다.
        /// </summary>
        /// <param name="damage">피해량</param>
        /// <param name="powerAttack"></param>
        /// <param name="hitRotation"></param>
        public override void Hit(float damage, bool powerAttack, Quaternion hitRotation)
        {
            if (defending == true && 180 - Quaternion.Angle(transform.rotation, hitRotation) < defendingAngle)
            {
                transform.rotation = hitRotation;
                transform.Rotate(0, 180, 0);

                if (stamina > damage * (10 / armor))
                {
                    stamina -= damage * (10 / armor);
                    animator.SetInteger("state", 11);
                }
                else
                {
                    hp -= ((damage * (10 / armor)) - stamina);
                    stamina = 0;
                    animator.SetInteger("state", 3);
                }

                return;
            }

            base.Hit(damage, powerAttack, hitRotation);
        }

    }
}
