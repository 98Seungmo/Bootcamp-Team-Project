using UnityEngine;
using CharacterController = HJ.CharacterController;

namespace HJ
{
    /// <summary>
    /// State들의 공통적인 기능을 부여하기 위해 만든 스크립트.
    /// </summary>
    public class UniversialStateScript : StateMachineBehaviour
    {
        /// <summary>
        /// 해당 State로 Transition시 실행되는 함수들.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            GetComponents(animator, stateInfo);
            ResetEnter();
            StaminaEnter(stateInfo);
            AdvanceEnter();
            AttackEnter();
            InvincibleEnter();
            ItemEnter();
        }

        /// <summary>
        /// State 실행중일시 실행되는 함수들. FixedUpdate에 실행.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            MoveUpdate();
            AdvanceUpdate();
        }

        /// <summary>
        /// 다른 State로 Transition시 실행되는 함수들. 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        /// <param name="layerIndex"></param>
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            ResetExit();
            AttackExit();
            StaminaExit();
            InvincibleExit();
        }

        [Header("Get Components")] //======================================================================================================================================================
        protected CharacterController _characterController;
        protected PlayerController _playerController;
        protected Transform _transform;
        protected float _stateLength;

        /// <summary>
        /// 필요한 컴포넌트들을 불러온다. OnStateEnter시 실행.
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="stateInfo"></param>
        private void GetComponents(Animator animator, AnimatorStateInfo stateInfo)
        {
            _characterController = animator.GetComponent<CharacterController>();
            _transform = _characterController.transform;
            if (_characterController.isPlayer)
                _playerController = _characterController.GetComponent<PlayerController>();
            _stateLength = stateInfo.length;
        }

        [Header("Stamina")] //========================================================================================================================
        // 동작의 기력 사용 여부
        [SerializeField] bool _useStamina;
        // 기력이 충분한지 여부
        private bool _staminaEnough;
        // 기력 요구량
        [SerializeField] float _staminaRequired;
        // 기력 회복 시작 시점이 따로 설정되어 있는지 여부
        [SerializeField] bool _isStaminaDelayTime;
        // 그렇다면 기력 회복 시작 시점 설정
        [Range(0, 2f)]
        [SerializeField] float _StaminaDelayTime;

        /// <summary>
        /// OnStateEnter시 실행.
        /// 이 동작이 기력을 소모할 경우, 기력이 충분하다면 기력을 소모하고 기력 회복을 중지시킨다.
        /// 지속 공격이 아닐 경우 기력 회복 시점이 따로 설정되어 있다면 해당 시점에 기력 회복을 시작한다.
        /// 지속 공격일 경우 매 간격마다 기력을 소모한다.
        /// </summary>
        /// <param name="stateInfo"></param>
        private void StaminaEnter(AnimatorStateInfo stateInfo)
        {
            if (_useStamina)
            {
                _playerController.staminaRequired = _staminaRequired;
                _staminaEnough = _playerController.StaminaUse();
                if (_staminaEnough)
                {
                    _playerController.StaminaRecoverStop();

                    if (_isRepeatingAttack == false)
                    {
                        if (_isStaminaDelayTime)
                        {
                            _playerController.Invoke("StaminaRecoverStart", _StaminaDelayTime * _stateLength);
                        }
                    }
                    else // (_isRepeatingAttack == true)
                    {
                        _playerController.InvokeRepeating("StaminaUse", 0, _attackRepeatingTime * _stateLength);
                    }
                }
            }
            else
            {
                _staminaEnough = true;
            }
        }

        /// <summary>
        /// OnStateExit시 실행. 기력 회복을 재개하고 지속 공격이라면 기력 소모를 중단시킨다.
        /// </summary>
        private void StaminaExit()
        {
            _playerController.StaminaRecoverStart();

            if (_isRepeatingAttack)
            {
                _playerController.CancelInvoke("StaminaUse");
            }
        }

        [Header("Reset Timing")] //========================================================================================================================================================
        // "state"를 기본값으로 재설정하는 시점
        [SerializeField] bool _resetStart;
        [SerializeField] bool _resetEnd;
        [SerializeField] bool _resetDelayed;
        [SerializeField] float _stateResetTime;

        /// <summary>
        /// OnStateEnter시 실행.
        /// 설정에 따라 State 진입시 재설정하거나, 특정 시점에 재설정하도록 한다.
        /// </summary>
        private void ResetEnter()
        {
            if (_resetStart)
                _characterController.StateReset();

            if (_resetDelayed)
                _characterController.Invoke("StateReset", _stateResetTime * _stateLength);
        }

        /// <summary>
        /// OnStateExit시 실행. 설정에 따라 "state"값을 재설정한다.
        /// </summary>
        private void ResetExit()
        {
            if (_resetEnd)
                _characterController.StateReset();
        }

        [Header("Move")] //================================================================================================================================================================
        // 이동 가능 여부
        [SerializeField] bool _canMove;
        // 회전 가능 여부
        [SerializeField] bool _canRotate;
        // 이동 속도
        [SerializeField] float _moveSpeed;

        /// <summary>
        /// OnStateUpdate시 실행. 이동할 수 있다면 이동하고, 회전할 수 있다면 회전한다.
        /// </summary>
        private void MoveUpdate()
        {
            if (_canMove)
            {
                _transform.position += _characterController.moveDirection * _moveSpeed * _characterController.speed * Time.fixedDeltaTime;
            }
            if (_canRotate && _characterController.moveDirection != Vector3.zero)
            {
                _transform.rotation = Quaternion.LookRotation(_characterController.moveDirection);
            }
        }

        [Header("Advance")] //=============================================================================================================================================================
        // 전진 및 후진 여부
        [SerializeField] bool _isAdvance;
        // 회전 가능 여부
        [SerializeField] bool _canTurn;
        // 속도
        [SerializeField] float _advanceSpeed;
        // 감속
        [SerializeField] float _advanceSpeedReduce;
        private float _advanceSpeedLeft;

        /// <summary>
        /// 
        /// </summary>
        private void AdvanceEnter()
        {
            if (_isAdvance)
            {
                if (_canTurn && _characterController.moveDirection != Vector3.zero)
                {
                    _transform.rotation = Quaternion.LookRotation(_characterController.moveDirection);
                }

                _advanceSpeedLeft = _advanceSpeed;
            }
        }
        private void AdvanceUpdate()
        {
            if(_isAdvance)
            {
                _characterController.transform.position += _advanceSpeedLeft * _characterController.transform.forward * Time.fixedDeltaTime;
                _advanceSpeedLeft -= _advanceSpeedReduce * Time.fixedDeltaTime;
            }
        }

        [Header("Attack")] //==============================================================================================================================================================
        [SerializeField] float _attackDamageRate; // 데미지 배율
        [SerializeField] float _attackRange;
        [Range(0, 180f)]
        [SerializeField] float _attackAngle;
        [SerializeField] LayerMask _attackLayerMask;

        [Space(10f)]
        [SerializeField] bool _isAttack; // 1타 여부
        [SerializeField] bool _isPowerAttack; // 넉백 여부

        [Space(10f)]
        [SerializeField] bool _isRangedAttack; // 사격 여부
        [SerializeField] Missile _missile; // 미사일
        //[SerializeField] float _missileSpeed;
        //[SerializeField] float _missileTimer;
        //[SerializeField] bool _isPiercing;
        //[SerializeField] bool _isExplosive;

        [Range(0, 1f)]
        [SerializeField] float _attackDelayTime; // 1타 타이밍
        [SerializeField] bool _isDoubleAttack; // 2타 여부
        [Range(0, 1f)]
        [SerializeField] float _doubleAttackDelayTime; // 2타 타이밍
        [SerializeField] bool _isRepeatingAttack; // 연속공격 여부
        [Range(0, 1f)]
        [SerializeField] float _attackRepeatingTime; // 연속공격 간격

        private void AttackEnter()
        {
            _characterController.attackDamageRate = _attackDamageRate;
            _characterController.attackRange = _attackRange;
            _characterController.attackAngle = _attackAngle;
            _characterController.attackLayerMask = _attackLayerMask;
            _characterController.isPowerAttack = _isPowerAttack;

            if (_isAttack)
            {
                if (_isRepeatingAttack == false)
                {
                    _characterController.Invoke("Attack", _attackDelayTime * _stateLength);

                    if (_isDoubleAttack)
                    {
                        _characterController.Invoke("Attack", _doubleAttackDelayTime * _stateLength);
                    }
                }
                else // (_isRepeatingAttack == true)
                {
                    _characterController.InvokeRepeating("Attack", _attackDelayTime * _stateLength, _attackRepeatingTime * _stateLength);
                }
            }

            if (_isRangedAttack)
            {
                _characterController.missile = _missile;

                //_characterController.missileSpeed = _missileSpeed;
                //_characterController.missileTimer = _missileTimer;
                //_characterController.isPiercing = _isPiercing;
                //_characterController.isExplosive = _isExplosive;

                _characterController.Invoke("Shoot", _attackDelayTime * _stateLength);
            }
        }
        private void AttackExit()
        {
            if (_isRepeatingAttack)
            {
                _characterController.CancelInvoke("Attack");
            }
        }

        [Header("Invincible")] //==============================================================================================================================================================
        [SerializeField] bool _isInvincible; // 무적 여부
        [SerializeField] float _invincibleTime; // 무적 시간
        private void InvincibleEnter()
        {
            _characterController.InvincibleStart();
            _characterController.Invoke("InvincibleEnd", _invincibleTime);
        }
        private void InvincibleExit()
        {
            if (_isInvincible)
            {
                _characterController.InvincibleEnd();
            }
        }

        [Header("Item")] //==============================================================================================================================================================
        [SerializeField] bool _weapon1;
        [SerializeField] bool _weapon2;
        [SerializeField] bool _weapon3;
        [SerializeField] bool _potion;
        private void ItemEnter()
        {
            if (_weapon1)
                _characterController.weapon1.SetActive(true);
            else
                _characterController.weapon1.SetActive(false);
            
            if (_weapon2)
                _characterController.weapon2.SetActive(true);
            else
                _characterController.weapon2.SetActive(false);
            
            if (_weapon3)
                _characterController.weapon3.SetActive(true);
            else
                _characterController.weapon3.SetActive(false);

            if (_potion)
                _characterController.potion.SetActive(true);
            else
                _characterController.potion.SetActive(false);
        }
    }
}