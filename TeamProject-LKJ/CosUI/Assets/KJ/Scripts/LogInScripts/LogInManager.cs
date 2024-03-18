using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    #region 회원가입창 열고 닫기
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
    #region InputField 이미지 숨기기
    // InputField 와 Image 를 1대1 매칭하기 위한 Dictionary.
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
        // 각 InputField 와 Image 매칭
        /* -----------------------------Login------------------------------- */
        _inputFieldImageMap.Add(emailLogin, emailLoginImage);
        _inputFieldImageMap.Add(passwordLogin, passwordLoginImage);
        /* ---------------------------Register------------------------------ */
        _inputFieldImageMap.Add(usernameRegister, usernameRegisterImage);
        _inputFieldImageMap.Add(emailRegister, emailRegisterImage);
        _inputFieldImageMap.Add(passwordRegister, passwordRegisterImage);
        _inputFieldImageMap.Add(passwordCheckRegister, passwordCheckImage);

        // 모든 InputField 에 Listener 추가.
        foreach (var pair in _inputFieldImageMap)
        {
            pair.Key.onValueChanged.AddListener((value) => ToggleImage(pair.Key, value));
        }
    }
        
    void ToggleImage(TMP_InputField inputField, string inputValue)
    {
        // 해당 InputField 에 연결된 이미지 활성화 / 비활성화.
        if (_inputFieldImageMap.TryGetValue(inputField, out GameObject Image))
        {
            Image.SetActive(string .IsNullOrEmpty(inputValue));
        }
    }

}
