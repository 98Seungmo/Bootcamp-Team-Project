using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HJ
{
    public class VillageCamera : MonoBehaviour
    {
        private Transform _transform;
        public Transform player;
        [SerializeField] Vector3 _cameraOffset;
        float delay = 0;

        void Start()
        {
            _transform = GetComponent<Transform>();
            if (player == null)
            {
                player = GameObject.FindWithTag("Player").transform;
            }
        }

        void Update()
        {
            if (player.position.y <= 0)
            {
                _transform.position = player.position + _cameraOffset;
            }
        }
    }
}
