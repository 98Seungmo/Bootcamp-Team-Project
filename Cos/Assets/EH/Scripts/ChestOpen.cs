using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    public Animator animator;
    //콜리더와 충돌한 채로 
    private void OnCollisionStay(Collision collision)
    { //로그 호출
        Debug.Log(collision.gameObject.name);
       
        //F를 누르면 해당 실행 파라미터가 실행됨
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (animator.GetBool("isOpenChest") == false)
            {
                animator.SetBool("isOpenChest", true);
            }
        }

    }
}
