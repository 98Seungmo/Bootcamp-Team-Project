using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    // ��ư�� ������ �κ�(����)�� ��ȯ
    public void InLoby()
    {
        SceneManager.LoadScene("Village(Loby)");
    }
    // ��ư�� ������ ����(�ΰ���)���� ��ȯ
    public void InDungeon()
    {
        SceneManager.LoadScene("Dungeon(Ingame)");
    }
    // ��������
    public void OutGame()
    {
        SceneManager.LoadScene("Quit");
    }


}
