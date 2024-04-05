using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    //애니메이터 컴포넌트 생성
    public Animator animator;

    //컬리더에 닿고있으면 잘돌아가는지 로그 생성과 애니메이션의 상태 파라미터 변경
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);

        if( Input.GetKeyDown(KeyCode.F) )
        {
            if (animator.GetBool("isOpen") == false)
            {
                animator.SetBool("isOpen", true);
            }
        }
        
    }
}
