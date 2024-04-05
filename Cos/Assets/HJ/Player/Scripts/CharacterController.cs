using KJ;
using Scene_Teleportation_Kit.Scripts.player;
using System;
using UnityEngine;
using Attribute = KJ.Attribute;

namespace HJ
{
    /// <summary>
    /// Character를 제어한다.
    /// </summary>
    public abstract class CharacterController : MonoBehaviour, IHp
    {
        /// <summary>
        /// 오브젝트 활성화시 실행
        /// </summary>
        private void Awake()
        {
            GetComponentAwake();
        }

        /// <summary>
        /// 첫 Update 이전에 실행
        /// </summary>
        protected virtual void Start()
        {
            // 데이터베이스 관련==========================================================================
            ItemDBManager itemDBManager = ItemDBManager.Instance;
            GameData gameData = NetData.Instance.gameData;
            Class classKnight = GetClass(ClassType.knight);
            Class classbabarian = GetClass(ClassType.barbarian);
            Class GetClass(ClassType classType)
            {
                return gameData.classes[classType];
            }
            _hpMax = classKnight.baseHp;
            for (int i = 0; i < itemDBManager._itemData.items.Count; i++)
            {
                if (itemDBManager._itemData.items[i].type == "weapon")
                {
                    attackItem = 10;
                }
                if (itemDBManager._itemData.items[i].type == "armor")
                {
                    armorItem = 10;
                }
            }
            for (int j = 0; j < itemDBManager._itemData.items.Count; j++)
            {
                if (itemDBManager._itemData.items[j].name == "귀걸이")
                {
                    //attackSkill = 0.3f;
                }
                else if (itemDBManager._itemData.items[j].name == "목걸이")
                {
                    //attackSkill = 0.2f;
                }
                else if (itemDBManager._itemData.items[j].name == "반지")
                {
                    //attackSkill = 0.1f;
                }
            }

            // 캐릭터 초기 설정
            HealthStart();
            CharacterInfoStart();

            // 생명력 소진시 쓰러지도록 설정
            onHpMin += () => Death();
        }

        /// <summary>
        /// 매 업데이트마다 실행
        /// </summary>
        protected virtual void Update()
        {
            MoveUpdate();
        }

        /// <summary>
        /// 매 FixedUpdate마다 실행
        /// </summary>
        protected virtual void FixedUpdate()
        {

        }

        [Header("Get Component")] //======================================================================================================================================================
        protected Transform transform;
        protected Animator animator;

        // 컴포넌트 설정. Awake시 실행.
        private void GetComponentAwake()
        {
            /// Transform
            transform = GetComponent<Transform>();
            /// Animator
            animator = GetComponent<Animator>();
        }

        [Header("CharacterInfo")] //======================================================================================================================================================
        // 캐릭터 타입: (1: 기사/ 2: 전사/ 3: 도적/ 4: 법사)
        [SerializeField] int _type;
        // 각 Animation에서 활성화 및 비활성화 할 장비 object를 할당.
        public GameObject weapon1;
        public GameObject weapon2;
        public GameObject weapon3;
        public GameObject potion;

        // 발사체 공격시 사용할 발사체를 할당.
        public Missile missile;

        // Player 캐릭터인가?
        public bool isPlayer;

        // 이동속도 설정
        public float speed { get => _speed; }
        [SerializeField] float _speed = 5f;

        // 공격력: (무기 + 장신구) * ( 1 + 스킬강화 + 음식강화)
        public float attack { get => (attackWeapon + attackItem) * (1 + attackSkill + attackFood); }
        public float attackWeapon;
        public float attackItem;
        public float attackSkill;
        public float attackFood;

        // 방어력: (방어구 + 장신구) * ( 1 + 스킬강화 + 음식강화)
        public float armor { get => (armorArmor + armorItem) * (1 + armorSkill + armorFood); }
        public float armorArmor;
        public float armorItem;
        public float armorSkill;
        public float armorFood;

        /// <summary>
        /// 캐릭터의 설정. Start시 실행.
        /// </summary>
        private void CharacterInfoStart()
        {
            /// 캐릭터의 Type에 따라 Animator의 parameter를 설정한다.
            animator.SetInteger("type", _type);
        }

        // [("IHp")] //====================================================================================================================================================================
        public float hp
        {
            get
            {
                /// _hp 값을 반환.
                return _hp;
            }
            set
            {
                /// _hp를 value를 0~_hpMax 사잇값으로 변환해서 대입
                _hp = Mathf.Clamp(value, 0, hpMax);

                /// 변화된 생명력이 범위 내라면 return
                if (_hp == value)
                {
                    onHpChanged?.Invoke(_hp);
                    return;
                }

                /// 생명력이 1 미만이라면, 생명력이 최대치라면 각각 알리고, 체력이 변화했음을 알림.
                if (value < 1)
                {
                    onHpMin?.Invoke();
                }
                else if (value >= _hpMax)
                {
                    onHpMax?.Invoke();
                }
                onHpChanged?.Invoke(_hp);
            }
        }

        // 현재 생명력.
        private float _hp;

        // 최대 생명력: (기본값(100) + 장신구) * ( 1 + 스킬강화 + 음식강화)
        public float hpMax { get => (_hpMax + hpMaxItem) * (1 + hpSkill + hpFood); }
        private float _hpMax = 100f;
        public float hpMaxItem;
        public float hpSkill;
        public float hpFood;

        // 생명력의 변화에 따라 알리는 대리자.
        public event Action<float> onHpChanged;
        public event Action<float> onHpDepleted;
        public event Action<float> onHpRecovered;
        public event Action onHpMin;
        public event Action onHpMax;

        public void Hit(float damage)
        {
            DepleteHp(damage);
        }

        /// <summary>
        /// 피격시 호출
        /// </summary>
        /// <param name="damage">피해량</param>
        /// <param name="powerAttack"></param>
        /// <param name="hitRotation"></param>
        public virtual void Hit(float damage, bool powerAttack, Quaternion hitRotation)
        {
            // 무적 여부를 판단하고, 무적이 아니라면 공격 방향과 반대방향으로 캐릭터를 회전하고, 일반 공격인 경우 HitA를 호출, 강한 공격인 경우 HitB를 호출한다.
            // (적 공격력 * (10 / 방어력))만큼 생명력을 깎는다.
            if (_invincible == false)
            {
                transform.rotation = hitRotation;
                transform.Rotate(0, 180, 0);

                if (powerAttack == false)
                {
                    HitA();
                }
                else // (powerAttack ==  true)
                {
                    HitB();
                }

                DepleteHp(damage * (10 / armor));
            }
        }

        public void DepleteHp(float amount)
        {
            if (amount <= 0)
                return;

            hp -= amount;
            onHpDepleted?.Invoke(amount);
        }
        public void RecoverHp(float amount)
        {
            hp += amount;
            onHpRecovered?.Invoke(amount);
        }

        // [("Health")] ===================================================================================================================================================================
        /// <summary>
        /// 체력 관련 설정. Start시 실행
        /// </summary>
        private void HealthStart()
        {
            _hp = hpMax;
        }

        // [("Defending")] ================================================================================================================================================================
        // 방어중 여부
        public bool defending { get => _defending; set => _defending = value; }
        private bool _defending;
        // 방어 성공 여부
        public bool defend { get => _defend; set => _defend = value; }
        private bool _defend;
        // 방어 각도 (0~180f)
        public float defendingAngle { get => _defendingAngle; set => _defendingAngle = value; }
        private float _defendingAngle;

        // [("Attack")] ===================================================================================================================================================================
        // 공격 범위
        public float attackRange { set => _attackRange = value; }
        private float _attackRange;
        // 공격 각도 (0~180f)
        public float attackAngle { set => _attackAngle = value; }
        private float _attackAngle;
        // 공격 각도에 따른 내적
        private float _attackAngleInnerProduct;
        // 공격 대상 LayerMask
        public LayerMask attackLayerMask { set => _attackLayerMask = value; }
        private LayerMask _attackLayerMask;
        // 공격 기술에 따른 공격력 배율
        public float attackDamageRate { set => _attackDamageRate = value; }
        private float _attackDamageRate;

        // 강한 충격 공격 여부
        public bool isPowerAttack { get => _isPowerAttack; set => _isPowerAttack = value; }
        private bool _isPowerAttack;

        // 발사체 설정
        // 발사체 관통 여부
        public bool isPiercing { get => _isPiercing; set => _isPiercing = value; }
        private bool _isPiercing;
        // 발사체 폭발 여부
        public bool isExplosive { get => _isExplosive; set => _isExplosive = value; }
        private bool _isExplosive;
        // 발사체 속도
        public float missileSpeed { get => _missileSpeed; set => _missileSpeed = value; }
        private float _missileSpeed;
        // 발사체 유지시간
        public float missileTimer { get => _missileTimer; set => _missileTimer = value; }
        private float _missileTimer;

        /// <summary>
        /// 공격시 실행. 공격 범위 내 SphereCastAll을 실행 후,
        /// 공격 각도에 따른 내적값을 계산하고,
        /// 플레이어와 적 간 방향벡터를 구하고 플레이어의 forward 방향벡터와의 내적을 계산하여
        /// 공격 각도 이내라면 Hit를 호출하여 피해량, 공격 방향, 강한 충격 여부를 전달한다.
        /// </summary>
        public void Attack()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position + new Vector3(0, 1, 0),
                                                      _attackRange,
                                                      Vector3.up,
                                                      0,
                                                      _attackLayerMask);

            _attackAngleInnerProduct = Mathf.Cos(_attackAngle * Mathf.Deg2Rad);

            foreach (RaycastHit hit in hits)
            {
                if (Vector3.Dot((hit.transform.position - transform.position).normalized, transform.forward) > _attackAngleInnerProduct)
                {
                    if (hit.collider.TryGetComponent(out IHp iHp))
                    {
                        float _random = UnityEngine.Random.Range(0.75f, 1.25f);
                        iHp.Hit(attack * _attackDamageRate * _random, _isPowerAttack, transform.rotation);
                    }
                }
            }
        }

        /// <summary>
        /// 발사체 공격일 경우 호출. 발사체를 생성하고 플레이어의 공격력을 전달한다.
        /// </summary>
        public void Shoot()
        {
            Missile _missile = Instantiate(missile, transform.position + transform.forward, transform.rotation);
            //_missile.missileSpeed = _missileSpeed;
            //_missile.missileTimer = missileTimer;
            //_missile.isPiercing = _isPiercing;
            //_missile.isExplosive = _isExplosive;
            _missile.attack = attack;
            //_missile.attackDamageRate = _attackDamageRate;
            //_missile.attackRange = _attackRange;
            //_missile.attackAngle = _attackAngle;
            //_missile.attackLayerMask = _attackLayerMask;
            //_missile.isPowerAttack = _isPowerAttack;
        }

        // [("Invincible")] ===============================================================================================================================================================
        // 무적 여부
        public bool invincible { get => _invincible; set => _invincible = value; }
        private bool _invincible;

        /// <summary>
        /// 무적 여부를 true로 한다.
        /// </summary>
        public void InvincibleStart()
        {
            _invincible = true;
        }
        /// <summary>
        /// 무적 여부를 false로 한다.
        /// </summary>
        public void InvincibleEnd()
        {
            _invincible = false;
        }

        // [("States")] ===================================================================================================================================================================

        // states
        // 0 
        // 1 Move
        // 2 Dodge
        // 3 AttackA
        // 4 AttackB
        // 5 HitA
        // 6 HitB
        // 7 Death
        // 8 Raise?
        // 9 Interact
        // 10 UseItem
        // 11 Blocking
        // 12 필살기?

        // [("State Escape")] =============================================================================================================================================================
        /// <summary>
        /// Animator의 State를 강제로 중단시킨다.
        /// </summary>
        public void StateCancle()
        {
            animator.SetInteger("state", 0);
        }

        /// <summary>
        /// Animator의 다음 State를 Move로 예정한다.
        /// </summary>
        public void StateReset()
        {
            animator.SetInteger("state", 1);
        }

        // [("State 1 Move")] =============================================================================================================================================================
        // x축 이동입력
        public virtual float horizontal { get; set; }
        // y축 이동입력
        public virtual float vertical { get; set; }
        // 입력방향
        public Vector3 moveDirection { get => _moveDirection; set => _moveDirection = value; }
        private Vector3 _moveDirection;
        // 이동입력여부
        public float moveFloat { get => _moveFloat; }
        private float _moveFloat;

        /// <summary>
        /// StateMachineBehaviour Move에서 사용할 변수를 Update한다. Update시 실행.
        /// </summary>
        protected void MoveUpdate()
        {
            _moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
            _moveFloat = _moveDirection.magnitude;

            animator.SetFloat("moveFloat", _moveFloat);
        }

        // [("State 2 Dodge")] ============================================================================================================================================================
        /// <summary>
        /// Animator의 "state"에 2를 대입.
        /// </summary>
        protected void Dodge()
        {
            animator.SetInteger("state", 2);
        }

        // [("State 3 AttackA")] ==========================================================================================================================================================
        /// <summary>
        /// Animator의 "state"에 3을 대입.
        /// </summary>
        protected void AttackA()
        {
            animator.SetInteger("state", 3);
        }

        // [("State 4 AttackB")] ==========================================================================================================================================================
        /// <summary>
        /// Animator의 "state"에 4를 대입.
        /// </summary>
        protected void AttackB()
        {
            animator.SetInteger("state", 4);
        }

        /// <summary>
        /// Animator의 "state"에 1을 대입.
        /// </summary>
        protected void AttackBRelease()
        {
            animator.SetInteger("state", 1);
        }

        // [("State 5 HitA")] =============================================================================================================================================================
        /// <summary>
        /// Animator의 "state"에 5를 대입.
        /// </summary>
        public void HitA()
        {
            animator.SetInteger("state", 5);
        }

        // [("State 6 HitB")] =============================================================================================================================================================
        /// <summary>
        /// Animator의 "state"에 6을 대입.
        /// </summary>
        public void HitB()
        {
            animator.SetInteger("state", 6);
        }

        // [("State 7 Death")] ============================================================================================================================================================
        /// <summary>
        /// Animator의 "state"에 7을 대입.
        /// </summary>
        public void Death()
        {
            animator.SetInteger("state", 7);
            Destroy(this);
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<CapsuleCollider>());
        }

        // [("State 8")] ==================================================================================================================================================================

        // [("State 9 Interact")] =========================================================================================================================================================
        // LayerMask 상호작용할 대상의 Layer를 지정.
        [SerializeField] LayerMask _layerMaskInteractable;

        /// <summary>
        /// Animator의 "state"에 9를 대입한다. 플레이어의 전방으로 Raycast를 실행하고 IInteractable.Interaction을 호출해 플레이어 gameObject를 전달한다.
        /// </summary>
        public void Interact()
        {
            animator.SetInteger("state", 9);

            if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), transform.forward, out RaycastHit hit, 2.6f, _layerMaskInteractable))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Interaction(this.gameObject);
                }
            }
        }

        // [("State 10 UseItem)] ==========================================================================================================================================================
        /// <summary>
        /// Animator의 "state"에 10을 대입.
        /// </summary>
        public void UseItem()
        {
            animator.SetInteger("state", 10);
        }

    }
}
