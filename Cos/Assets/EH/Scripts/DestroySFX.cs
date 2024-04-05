using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySFX : MonoBehaviour
{
    public AudioSource _audioSource;

    private void Start()
    {

    }
    //SFX Manager와 연계된 스크립트 
    //음악을 재생하면 플레이중이 아닐때 해당 게임오브젝트를 지운다
    private void LateUpdate()
    {
        if (!_audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
