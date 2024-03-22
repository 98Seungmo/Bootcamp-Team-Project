using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    #region ����â ���� �ݱ�
    [Header("SettingUI On/Off")]
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
    [Header("Login On/Off")]
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
    #region �α��� �ϸ� �ε�â���� �̵�
    
    public void AttemptLogin()
    {
        FirebaseAuthManager.Instance.LoginButton(Result =>
        {
            if (Result)
            {
                SceneManager.LoadScene("LoadingScene");
            }
            else
            {
                Debug.LogError("�α��� ����");
            }
        });
              
    }
    #endregion
    #region ȸ������â ���� �ݱ�
    [Header("RegisterUI On/Off")]
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
    /* Login */
    public TMP_InputField emailLogin;
    public TMP_InputField passwordLogin;
    /* Register */
    public TMP_InputField usernameRegister;
    public TMP_InputField emailRegister;
    public TMP_InputField passwordRegister;
    public TMP_InputField passwordCheckRegister;

    [Header("Image")]
    /* Login */
    public GameObject emailLoginImage;
    public GameObject passwordLoginImage;
    /* Register */
    public GameObject usernameRegisterImage;
    public GameObject emailRegisterImage;
    public GameObject passwordRegisterImage;
    public GameObject passwordCheckImage;
    #endregion
    #region Tab Ű�� InputField ��Ŀ�� �̵�
    // ���� Ȱ��ȭ �� �г� �ĺ��ϴ� enum.    
    public enum ActivePanel {Login, Register}
    [Header("InputField Focus")]
    public ActivePanel activePanel;

    
    // �α��� InputFields
    public List<TMP_InputField> inputFieldsLogin;
    // ȸ������ InputFields
    public List<TMP_InputField> inputFieldsRegister;
    #endregion
    #region �̸��� ����
    [Header("SaveEmail")]
    // �̸��� InputField.
    public TMP_InputField emailInputField;
    // �̸��� ���� ���
    public Toggle saveEmailToggle;

    // PlayerPrefs ���� �̸��� �����Ҷ� ����� Ű
    private const string EmailKey = "UserEmail";


    #endregion

    void Start()
    {
        #region InputField �̹��� �����
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
        #endregion
        #region �̸��� ����
        // �� ���۽� ����� �̸��� �ҷ�����.
        LoadEmail();

        // ��� ��ư�� ���� �̸��� ���� ���� ����.
        saveEmailToggle.onValueChanged.AddListener(OnToggleChanged);

        #endregion
    }

    void Update()
    {
        #region Tab Ű�� InputField ��Ŀ�� �̵�
        // tab Ű�� ������ NavigateThroughInputField �޼��� ����.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // �Ǻ��� Ȱ��ȭ
            switch (activePanel)
            {
                case ActivePanel.Login:
                    NavigateThroughInputField(inputFieldsLogin);
                    break;
                case ActivePanel.Register:
                    NavigateThroughInputField(inputFieldsRegister);
                    break;
            }
            
        }
        #endregion
    }

    #region Tab Ű�� InputField ��Ŀ�� �̵�

    /* InputField ��Ŀ�� ���� */
    public void NavigateThroughInputField(List<TMP_InputField> inputFields)
    {
        for (int i = 0; i < inputFields.Count; i++)
        {
            if (inputFields[i].isFocused)
            {
                // ���� InputField ���.
                int nextIndex = (i + 1) % inputFields.Count;
                // ���� InputField �� ��Ŀ�� ����.
                inputFields[nextIndex].Select();
                // ���� InputField Ȱ��ȭ.
                inputFields[nextIndex].ActivateInputField();
                break;
            }
        }
    }

    // InputField ��Ŀ�� Ȱ��ȭ �г�
    public void SetActivePanel(ActivePanel panel)
    {
        activePanel = panel;
    }

    // InputField Login â���� ��Ŀ�� Ȱ��ȭ.
    public void SetActivePanelToLogin()
    {
        SetActivePanel(LoginManager.ActivePanel.Login);
    }
    // InputField Register â���� ��Ŀ�� Ȱ��ȭ.
    public void SetActivePanelToRegister()
    {
        SetActivePanel(LoginManager.ActivePanel.Register);
    }
    #endregion

    #region InputField �̹��� �����
    // �̹��� Ȱ��ȭ / ��Ȱ��ȭ �ϴ� �޼���
    void ToggleImage(TMP_InputField inputField, string inputValue)
    {
        // �ش� InputField �� ����� �̹��� Ȱ��ȭ / ��Ȱ��ȭ.
        if (_inputFieldImageMap.TryGetValue(inputField, out GameObject Image))
        {
            Image.SetActive(string .IsNullOrEmpty(inputValue));
        }
    }
    #endregion

    #region �̸��� ����
    public void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            // ����� On �϶� ���� InputField �� �̸��� �ּ� ����.
            SaveEmail();
        }
        else
        {
            // ����� Off �϶� ����� �̸��� �ּ� ����.
            PlayerPrefs.DeleteKey(EmailKey);
        }
    }

    public void SaveEmail()
    {
        PlayerPrefs.SetString(EmailKey, emailInputField.text);
        PlayerPrefs.Save();
    }

    public void LoadEmail()
    {
        if (PlayerPrefs.HasKey(EmailKey))
        {
            string saveEmail = PlayerPrefs.GetString(EmailKey);
            emailInputField.text = saveEmail;
            // ����� �̸����� ������ ����� true �� ���·� ����.
            saveEmailToggle.isOn = true;
        }
        else
        {
            // ����� �̸����� ������ ����� false �� ���·� ����.
            saveEmailToggle.isOn = false;
        }
        
    }
    #endregion
}
