using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지1에서 4번째 맵에서 아이템 상자 열리는 던전 기능
/// </summary>
public class C_ItemBox : MonoBehaviour
{
    /**게임오브젝트 ItemBox 변수*/
    public GameObject ItemBox;
    /**게임오브젝트 Door 변수*/
    public GameObject Door;

    /**ItemBox 애니메이터 변수*/
    private Animator animator;
    /**ItemBox 상태 변수*/
    public bool isBox;

    /// <summary>
    /// 시작 함수
    /// </summary>
    private void Start()
    {
        /**애니메이터 가져오기*/
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Collider가 트리거 영역 안에 있을 때 호출하는 함수
    /// </summary>
    /// <param name="other">트리거 영역과 충돌한 다른 Collider</param>
    private void OnTriggerStay(Collider other)
    {
        /**충돌한 콜라이더가 Player일때*/
        if (other.CompareTag("Player"))
        {
            /**F키를 누르고 아이템상자 닫혔있을 때*/
            if (Input.GetKeyDown(KeyCode.F) && !isBox)
            {
                /**아이템상자 열림*/
                isBox = true;
                /**아이템상자 열리는 애니메이션 실행*/
                animator.SetBool("isBox", true);
                /**F키 누르면*/
                if (Input.GetKeyDown(KeyCode.F))
                {
                    /**DisableItemBox 코루틴 실행*/
                    StartCoroutine(DisableItemBox());
                }
            }
        }
    }

    /// <summary>
    /// 아이템 상자를 비활성화하고 Door 애니메이션을 재생하는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator DisableItemBox()
    {
        /**2초 동안 기다린 후 다음 코드 실행*/
        yield return new WaitForSeconds(2f);
        /**게임 오브젝트 아이템상자를 비활성화*/
        ItemBox.SetActive(false);

        /**게임오브젝트 Door가 null이 아니면*/
        if (Door != null)
        {
            /**Door 게임 오브젝트의 Animator 컴포넌트를 찾아 "Door"라는 애니메이션을 재생*/
            Door.GetComponent<Animator>().Play("Door");
        }
    }
}
