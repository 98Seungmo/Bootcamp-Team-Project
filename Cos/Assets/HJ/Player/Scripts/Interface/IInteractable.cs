using HJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    /// <summary>
    /// 플레이어와 Object간의 상호작용을 가능하게 해주는 Interface.
    /// 또는 Object간의 상호작용을 수행한다.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// 플레이어의 상호작용 범위 내로 들어오면 상호작용 가능함을 알린다.
        /// </summary>
        void InteractableOn();
        /// <summary>
        /// 플레이어의 상호작용 범위 밖으로 벗어나면 원상태로 돌아온다.
        /// </summary>
        void InteractableOff();
        /// <summary>
        /// 상호작용시 호출된다.
        /// </summary>
        /// <param name="interactor">상호작용 대상에게 자신 GameObject를 전달한다.</param>
        void Interaction(GameObject interactor);
    }
}
