using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe_Swing : MonoBehaviour
{
    //5데미지 변수 선언
    public int damage = 5;

    //콜리더가 닿으면 태그가 플레이어가 아니면 바로 리턴
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.tag.Equals("Player"))
        {
            return;

            // 태그가 플레이어면 의사코드를 실행
        }
        else if (collision.gameObject.tag.Equals("Player"))
        {
            //플레이어에게 데미지를 주며 넉백시킨다.
        }
    }

}
