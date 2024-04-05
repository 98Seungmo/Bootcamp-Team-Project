using System.Collections;
using UnityEngine;

namespace HJ
{
    public class AniOnEvent_Mage : MonoBehaviour
    {
        [Header("Dash")] //==============================================================================
        [SerializeField] GameObject _dashEffect;
        [SerializeField] int _dashSoundNum;
        [SerializeField] float _dashDelay;
        public void Dash()
        {
            StartCoroutine(Effect(_dashEffect, _dashSoundNum, _dashDelay));
        }

        [Header("Potion")] //============================================================================
        [SerializeField] GameObject _potionEffect;
        [SerializeField] int _potionSoundNum;
        [SerializeField] float _potionDelay;
        public void Potion()
        {
            StartCoroutine(Effect(_potionEffect, _potionSoundNum, _potionDelay));
        }

        [Header("HitEffect")] //=========================================================================
        [SerializeField] GameObject _hitEffect;
        [SerializeField] int _hitSoundNum;
        [SerializeField] float _hitDelay;
        public void HitEffect()
        {
            StartCoroutine(Effect(_hitEffect, _hitSoundNum, _hitDelay));
        }

        [Header("DieEffect")] //=========================================================================
        [SerializeField] GameObject _dieEffect;
        [SerializeField] int _dieSoundNum;
        [SerializeField] float _dieDelay;
        public void DieEffect()
        {
            StartCoroutine(Effect(_dieEffect, _dieSoundNum, _dieDelay));
        }

        [Header("Spell 1")] //============================================================================
        public GameObject _mageSpell1effect;
        [SerializeField] int _mageSpell1SoundNum;
        [SerializeField] float _mageSpell1Delay;
        public void Spell1()
        {
            StartCoroutine(Effect(_mageSpell1effect, _mageSpell1SoundNum, _mageSpell1Delay));
        }

        [Header("Spell 2")] //============================================================================
        public GameObject _mageSpell2effect;
        [SerializeField] int _mageSpell2SoundNum;
        [SerializeField] float _mageSpell2Delay;
        public void Spell2()
        {
            StartCoroutine(Effect(_mageSpell2effect, _mageSpell2SoundNum, _mageSpell2Delay));
        }

        [Header("Spell 3")] //============================================================================
        public GameObject _mageSpell3effect;
        [SerializeField] int _mageSpell3SoundNum;
        [SerializeField] float _mageSpell3Delay;
        public void Spell3()
        {
            StartCoroutine(Effect(_mageSpell3effect, _mageSpell3SoundNum, _mageSpell3Delay));
        }

        /// <summary>
        /// 비주얼 이펙트를 생성하고 delay 초 이후 파괴한다. 사운드 이펙트를 재생하도록 호출한다.
        /// </summary>
        /// <param name="effect">생성할 비주얼 이펙트</param>
        /// <param name="soundNum">재생할 사운드 이펙트</param>
        /// <param name="delay">비주얼 이펙트 파괴 시점</param>
        /// <returns></returns>
        IEnumerator Effect(GameObject effect, int soundNum, float delay)
        {
            GameObject effectInstanse = Instantiate(effect, transform.position, transform.rotation);
            SFX_Manager.Instance.VFX(soundNum);

            yield return new WaitForSeconds(delay);
            Destroy(effectInstanse);
        }
    }
}