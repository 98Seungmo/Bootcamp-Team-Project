using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    // 버튼을 누르면 로비(마을)로 전환
    public void InLoby()
    {
        SceneManager.LoadScene("Village(Loby)");
    }
    // 버튼을 누르면 던전(인게임)으로 전환
    public void InDungeon()
    {
        SceneManager.LoadScene("Dungeon(Ingame)");
    }
    // 게임종료
    public void OutGame()
    {
        SceneManager.LoadScene("Quit");
    }


}
