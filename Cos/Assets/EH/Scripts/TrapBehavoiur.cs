using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBehaviour : MonoBehaviour
{
    // 생성될 오브젝트에 대한 참조
    public GameObject Smog; // 연기 효과
    public GameObject Shake; // 흔들릴 오브젝트

    private Rigidbody rb; // Rigidbody 컴포넌트

    void Start()
    {
        // 이 게임 오브젝트에 부착된 Rigidbody 컴포넌트에 대한 참조 가져오기
        rb = GetComponent<Rigidbody>();
    }

    
    void Smogtrigger()
    {
        // 이 게임 오브젝트의 위치에 연기 효과를 생성
        Instantiate(Smog, transform.position, Quaternion.identity);
    }

    //트리거에 닿으면
    void OnTriggerEnter(Collider collision)
    {
        // 충돌한 콜라이더가 플레이어 오브젝트인지 확인
        if (collision.gameObject.tag.Equals("Player"))
        {
            // 2초 후에 흔들 오브젝트를 파괴
            Destroy(Shake, 2f);
            // 2초 후에 Smogtrigger 함수 호출
            Invoke("Smogtrigger", 2f);
        }
    }

    // 플레이어와 충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag.Equals("Player"))
        {
            // 2초 후에 이 게임 오브젝트의 Rigidbody 컴포넌트를 파괴
            Destroy(rb.gameObject, 2f);
        }
    }
}