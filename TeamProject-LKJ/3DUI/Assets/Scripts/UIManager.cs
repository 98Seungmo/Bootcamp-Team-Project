using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // 옵션창 열고 닫기위해 변수생성.
    [SerializeField] private GameObject _optionUI;

    // Setting 버튼을 누르면 option창 팝업
    public void Open()
    {
        _optionUI.SetActive(true);
    }

    // X버튼 누르면 option창 닫기
    public void Close()
    {
        _optionUI.SetActive(false);
    }
}
