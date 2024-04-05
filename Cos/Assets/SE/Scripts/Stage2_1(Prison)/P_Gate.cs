using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스테이지2에서 1번째 맵에서 쇠창살 내려가는 던전 기능
/// </summary>
public class P_Gate : MonoBehaviour  
{
    /**전역변수_애니메이터 변수*/
    Animator animator;

    /// <summary>
    /// 시작 함수
    /// </summary>
    void Start()
    {
        /**애니메이터 가져오기*/
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// 애니메이션 실행시키는 함수
    /// </summary>
    public void PlayAnimation()
    {
        /**"P_Gate"라는 애니메이션 재생*/
        animator.Play("P_Gate");
    }
}
