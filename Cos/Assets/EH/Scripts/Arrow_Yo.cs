using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Yo : MonoBehaviour
{
   
   
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //만들다 중간에 세은이에게 넘겨서 미완
        // 포워드 방향으로 5초마다 위치를 옮김
        transform.position += transform.forward * Time.deltaTime * 5;
         
    }
}
