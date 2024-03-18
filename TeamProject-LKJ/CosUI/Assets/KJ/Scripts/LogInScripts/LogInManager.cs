using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    #region ȸ������â ���� �ݱ�
    [SerializeField] private GameObject _registerUI;

    public void OpenRegister()
    {
        _registerUI.SetActive(true);
    }

    public void CloseRegister()
    {
        _registerUI.SetActive(false);
    }
    #endregion
    #region InputField �̹��� �����
    // InputField �� Image �� 1��1 ��Ī�ϱ� ���� Dictionary.
    private Dictionary<TMP_InputField, GameObject> _inputFieldImageMap = new Dictionary<TMP_InputField, GameObject>();

    [Header("ImageToHide")]
    public TMP_InputField emailLogin;
    public TMP_InputField passwordLogin;

    public TMP_InputField usernameRegister;
    public TMP_InputField emailRegister;
    public TMP_InputField passwordRegister;
    public TMP_InputField passwordCheckRegister;

    [Header("Image")]
    public GameObject emailLoginImage;
    public GameObject passwordLoginImage;

    public GameObject usernameRegisterImage;
    public GameObject emailRegisterImage;
    public GameObject passwordRegisterImage;
    public GameObject passwordCheckImage;



    #endregion

    void Start()
    {
        // �� InputField �� Image ��Ī
        /* -----------------------------Login------------------------------- */
        _inputFieldImageMap.Add(emailLogin, emailLoginImage);
        _inputFieldImageMap.Add(passwordLogin, passwordLoginImage);
        /* ---------------------------Register------------------------------ */
        _inputFieldImageMap.Add(usernameRegister, usernameRegisterImage);
        _inputFieldImageMap.Add(emailRegister, emailRegisterImage);
        _inputFieldImageMap.Add(passwordRegister, passwordRegisterImage);
        _inputFieldImageMap.Add(passwordCheckRegister, passwordCheckImage);

        // ��� InputField �� Listener �߰�.
        foreach (var pair in _inputFieldImageMap)
        {
            pair.Key.onValueChanged.AddListener((value) => ToggleImage(pair.Key, value));
        }
    }
        
    void ToggleImage(TMP_InputField inputField, string inputValue)
    {
        // �ش� InputField �� ����� �̹��� Ȱ��ȭ / ��Ȱ��ȭ.
        if (_inputFieldImageMap.TryGetValue(inputField, out GameObject Image))
        {
            Image.SetActive(string .IsNullOrEmpty(inputValue));
        }
    }

}
