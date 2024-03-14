using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInManager : MonoBehaviour
{
    #region 설정창 열고 닫기
    [SerializeField] private GameObject _settingUI;

    public void OpenSettingUI()
    {
        _settingUI.SetActive(true);
    }

    public void CloseSettingUI()
    {
        _settingUI.SetActive(false);
    }
    #endregion
    #region 게임 시작시 로그인 창 열기, X 누르면 닫기
    [SerializeField] private GameObject _logInUI;

    public void OpenLogIn()
    {
        _logInUI.SetActive(true);
    }

    public void CloseLogIn()
    {
        _logInUI.SetActive(false); 
    }
    #endregion
    #region 게임 종료 (현재는 테스트씬으로 이동)
    public void Quit()
    {
        SceneManager.LoadScene("TestGameQuit");
    }
    #endregion
    #region 로그인 하면 로딩창으로 이동 (테스트)
    public void OpenLoading()
    {
        SceneManager.LoadSceneAsync("LoadingScene");
    }
    #endregion
}
