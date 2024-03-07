using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // �ɼ�â ���� �ݱ����� ��������.
    [SerializeField] private GameObject _optionUI;

    // Setting ��ư�� ������ optionâ �˾�
    public void Open()
    {
        _optionUI.SetActive(true);
    }

    // X��ư ������ optionâ �ݱ�
    public void Close()
    {
        _optionUI.SetActive(false);
    }
}
