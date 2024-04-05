using System;
using UnityEngine;

namespace HJ
{
    /// <summary>
    /// 플레이어의 앞에서 상호작용 가능 Object가 상호작용 가능한 범위 내에 들어왔는지 확인한다.
    /// </summary>
    public class Interactor : MonoBehaviour
    {
        /// <summary>
        /// 대상 Object의 IInteractable.InteractableOn을 호출한다.
        /// </summary>
        /// <param name="other">감지한 대상 Collider</param>
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                interactable.InteractableOn();
            }
        }

        /// <summary>
        /// 대상 Object의 IInteractable.InteractableOff를 호출한다.
        /// </summary>
        /// <param name="other">감지한 대상 Collider</param>
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IInteractable interactable))
            {
                interactable.InteractableOff();
            }
        }
    }
}