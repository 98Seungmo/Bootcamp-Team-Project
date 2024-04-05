using GSpawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicMissleHitImpact : MonoBehaviour
{
    //트리거(플레이어)에 닿으면 생성함
    public GameObject Hitimpact;
    private void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("Player"))
        {
            Instantiate(Hitimpact, transform.position, Quaternion.identity);
        }

    }
}
