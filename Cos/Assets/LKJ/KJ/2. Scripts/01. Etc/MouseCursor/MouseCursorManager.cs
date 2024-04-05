using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KJ
{
    /**
     * @brief 마우스 커서의 이미지를 바꿈
     */
    public class MouseCursorManager : MonoBehaviour
    {
        [Header("MosueCursor")]
        [SerializeField] Texture2D cursorImage; ///< 메인카메라의 마우스 커서 이미지
        [SerializeField] Texture2D cursorClickImage; ///< 메인카메라의 마우스 클릭 이미지

        /**
         * @brief 처음 마우스 이미지 설정
         */
        void Start()
        {
            Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
        }

        /**
         * @brief 마우스 클릭에 따라 이미지 다르게 출력
         */
        void Update()
        {
            /* 마우스를 눌렀을 때 */
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.SetCursor(cursorClickImage, Vector2.zero, CursorMode.Auto);
            }
            /* 마우스를 누르지 않았을 때 */
            else if (!Input.GetMouseButton(0))
            {
                Cursor.SetCursor(cursorImage, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}

