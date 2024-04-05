using System;
using UnityEngine;

namespace HJ
{
    /// <summary>
    /// 생명력을 관리하는 인터페이스
    /// </summary>
    internal interface IHp
    {
        // 생명력의 입출력을 제어하는 property.
        float hp { get; set; }
        // 최대 생명력
        float hpMax { get; }
        // 생명력의 변화를 알리는 delegate.
        event Action<float> onHpChanged;
        event Action<float> onHpDepleted;
        event Action<float> onHpRecovered;
        event Action onHpMin;
        event Action onHpMax;
        // 생명력을 감소시키는 함수.
        void DepleteHp(float amount);
        // 생명력을 회복시키는 함수.
        void RecoverHp(float amount);

        /// <summary>
        /// 피격시 호출.
        /// </summary>
        /// <param name="damage">피해량</param>
        /// <param name="powerAttack">강한 충격 공격 여부</param>
        /// <param name="hitRotation">공격 방향</param>
        void Hit(float damage, bool powerAttack, Quaternion hitRotation);
        /// <summary>
        /// 피격시 호출
        /// </summary>
        /// <param name="damage"></param>
        void Hit(float damage);
    }
}