using System;
using UnityEngine;

namespace HJ
{
    /// <summary>
    /// 플레이어 발사체를 제어한다.
    /// </summary>
    public class Missile : MonoBehaviour
    {
        public void Start()
        {
            MissileMoveStart();
        }

        public void FixedUpdate()
        {
            MissileMoveFixedUpdate();
        }

        [Header ("Missile Info")] //==================================================================
        // 발사체 이동속도
        [SerializeField] private float _missileSpeed;
        // 발사체 지속시간
        [SerializeField] private float _missileTimer;

        /// <summary>
        /// 발사체 지속시간 이후 파괴하도록 설정. Start시 실행.
        /// </summary>
        private void MissileMoveStart()
        {
            Invoke("TimeOut", _missileTimer);
        }

        /// <summary>
        /// 발사체의 forward 방향으로 이동한다. FixedUpdate시 실행.
        /// </summary>
        private void MissileMoveFixedUpdate()
        {
            transform.position += _missileSpeed * transform.forward * Time.fixedDeltaTime;
        }

        /// <summary>
        /// 발사체를 파괴하고 _isExplosive라면 Explosion을 호출한다.
        /// </summary>
        private void TimeOut()
        {
            Destroy(gameObject);
            if (_isExplosive)
                Explosion();
        }

        [Header("Missile Attack")] //==================================================================
        // 관통 여부
        [SerializeField] bool _isPiercing;
        // 폭발 여부
        [SerializeField] bool _isExplosive;
        // 폭발 시각 효과
        [SerializeField] GameObject _explosionEffect;
        // 폭발 시각 효과 제거 시간
        [SerializeField] float _explosionDestroyDelay;

        // 공격력. 플레이어로부터 전달받는다.
        public float attack { set => _attack = value; }
        [SerializeField] float _attack;
        // 발사체 공격력 배율. 총 공격력 = 공격력 * 발사체 공격력 배율.
        [SerializeField] float _attackDamageRate;

        // 발사체 폭발 범위
        [SerializeField] float _attackRange;
        // 발사체 폭발 각도. 현재는 사용되지 않는다.
        [SerializeField] float _attackAngle;
        // 발사체 폭발 각도의 내적. 현재는 사용되지 않는다.
        [SerializeField] float _attackAngleInnerProduct;

        // 발사체 공격 LayerMask
        [SerializeField] LayerMask _attackLayerMask;
        // 벽 LayerMask
        [SerializeField] LayerMask _layerMaskWall;

        // 강한 충격 공격 여부
        [SerializeField] bool _isPowerAttack;

        /// <summary>
        /// 다른 콜라이더와 충돌시 해당 Collider.gameObject.layer가 Enemy라면,
        /// _isPiercing == false라면 자신을 파괴한다.
        /// _isExplosive == false라면 Hit(Collider 충돌 대상)을 호출하고,
        /// _isExplosive == true라면 Explosion을 호출한다.
        /// </summary>
        /// <param name="colliderHit"></param>
        private void OnTriggerEnter(Collider colliderHit)
        {
            if (colliderHit.gameObject.layer == 12)
            {
                if (_isPiercing == false)
                {
                    Destroy(gameObject);
                }


                if (_isExplosive == false)
                {
                    Hit(colliderHit);
                }
                else // (_isExplosive == true)
                {
                    Explosion();
                }
            }

            if (colliderHit.gameObject.layer == _layerMaskWall) // 벽에 박으면 터짐
            {
                Destroy(gameObject);

                if (_isExplosive)
                    Explosion();
            }
            
        }

        /// <summary>
        /// 폭발 범위 내 SphereCast를 실행하고 모든 대상에게 Hit(hit.collider)를 실행한다.
        /// 폭발 비쥬얼 이펙트를 생성하고 _explosionDestroyDelay 초 이후에 파괴한다.
        /// </summary>
        private void Explosion()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _attackRange, transform.up, 0, _attackLayerMask);

            foreach (RaycastHit hit in hits)
            {
                Hit(hit.collider);
            }

            GameObject effectInstanse = Instantiate(_explosionEffect, transform.position, transform.rotation);
            Destroy(effectInstanse, _explosionDestroyDelay);
        }

        /// <summary>
        /// 충돌한 대상에게 IHp.Hit()를 실행한다.
        /// </summary>
        /// <param name="coliderHit"></param>
        private void Hit(Collider coliderHit)
        {
            if (coliderHit.gameObject.TryGetComponent(out IHp iHp))
            {
                float _random = UnityEngine.Random.Range(0.75f, 1.25f);
                iHp.Hit( _attack * _attackDamageRate * _random, _isPowerAttack, transform.rotation);
            }
        }
    }
}
