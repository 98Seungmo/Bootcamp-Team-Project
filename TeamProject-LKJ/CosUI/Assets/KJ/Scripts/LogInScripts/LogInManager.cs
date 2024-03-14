using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogInManager : MonoBehaviour
{
    #region ����â ���� �ݱ�
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
    #region ���� ���۽� �α��� â ����, X ������ �ݱ�
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
    #region ���� ���� (����� �׽�Ʈ������ �̵�)
    public void Quit()
    {
        SceneManager.LoadScene("TestGameQuit");
    }
    #endregion
    #region �α��� �ϸ� �ε�â���� �̵� (�׽�Ʈ)
    public void OpenLoading()
    {
        SceneManager.LoadSceneAsync("LoadingScene");
    }
    #endregion
}
