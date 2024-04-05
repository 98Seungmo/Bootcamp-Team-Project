using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    /// <summary>
    /// 메인 카메라를 제어하기 위한 클래스.
    /// </summary>
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] Transform _playerTransform;
        [SerializeField] Vector3 _cameraOffset;

        // 카메라의 위치를 캐릭터의 위치 + _cameraOffset으로 설정한다.
        void Update()
        {
            transform.position = _playerTransform.position + _cameraOffset;
        }
    }
}
