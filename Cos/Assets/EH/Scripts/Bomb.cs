using GSpawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //폭 데미지 5 선언
    public int bombdamage = 5;
    //딜레이 3선언
    public float delay = 3f;
    public Vector3 offset_pos = Vector3.zero;   
    //게임오브젝트 폭발이펙트 선언
    public GameObject explosionEffect;

    //콜리더에 닿으면
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //폭탄에 달려있는 이펙트의 위치를 조정할 수있게 offset_pos로 값을 불러옴
            GameObject obj = Instantiate<GameObject>(explosionEffect, transform.position + offset_pos, transform.rotation);
            //꺼져있는 상태의 이펙트를 복사해 true로 켜준다.
            obj.SetActive(true);
            //오브젝트 삭제를 불러와 1초후 실행한다.
            Invoke("DestroyObject", 1f);

            //플레이어에게 데미지 주는 코드 필요
        }
    }


    private void DestroyObject()
    {
        Destroy(this.gameObject);
    }

}
