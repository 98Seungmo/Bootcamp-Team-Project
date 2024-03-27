using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;


public class FirebaseAuthManager : MonoBehaviour
{
    [Header("Firebase")]
    // Firebase �� �����ϰ� �ʱ�ȭ �ϰ� ���� Ȯ���� ����.
    public DependencyStatus dependencyStatus;
    // �α���, ȸ�����Կ� ���.
    private FirebaseAuth _auth;
    // ������ �Ϸ�� ���� ����.
    private FirebaseUser _user;

    [Header("LogIn")]
    // email �Է��� ����.
    public TMP_InputField email;
    // password �Է��� ����.
    public TMP_InputField password;
    // �����޽���
    public TMP_Text warningLoginText;
    // ������ ��Ÿ���� �޽���
    public TMP_Text confirmLoginText;

    [Header("Register")]
    // username ���� �Է� ����.
    public TMP_InputField usernameRegister;
    // email ���� �Է� ����.
    public TMP_InputField emailRegister;
    // password ���� �Է��� ����.
    public TMP_InputField passwordRegister;
    // password check �Է��� ����.
    public TMP_InputField passwordCheck;
    // ���� �޽���
    public TMP_Text warningRegisterText;
    // ������ ��Ÿ���� �޽���
    public TMP_Text ConfrimRegisterText;

    // �̱��� ����
    private static FirebaseAuthManager _instance;
    public static FirebaseAuthManager Instance
    {
        get
        {
            if (_instance == null)
            {

            }
            return _instance;
        }
    }

    void Awake()
    {
        // �̱��� ����
        if (_instance == null)
        {
            _instance = this;
            // �� ��ȯ�ÿ��� �ı����� �ʰ� ��
            DontDestroyOnLoad(gameObject);
        }
        else if(_instance != this)
        {
            // �ߺ� �ν��Ͻ� �ı�
            Destroy(gameObject);
        }
        // Firebase DependencyStatus Ȯ����
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            // dependencyStatus �� ���� ����
            dependencyStatus = task.Result;
            // dependencyStatus ��� �������� Ȯ��.
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase �ʱ�ȭ
                InitializeFirebase();
            }
            else
            {
                // ���� �޽��� ���.
                Debug.LogError("Firebase �ʱ�ȭ ���� : " + dependencyStatus);
            }
        });
    }

    public void InitializeFirebase()
    {
        // �ʱ�ȭ
        _auth = FirebaseAuth.DefaultInstance;
    }
    

    public void LoginButton(Action<bool> onLoginCompleted)
    {
        // �̸��ϰ� ��й�ȣ�� �����ϴ� �α��� ȣ��.
        StartCoroutine(Login(email.text, password.text, onLoginCompleted));
    }

    public void RegisterButton()
    {
        // ���� ������ �� �̸��ϰ� ��й�ȣ�� �����ϴ� Register ȣ��.
        StartCoroutine(Register(emailRegister.text, passwordRegister.text, usernameRegister.text));
    }

    IEnumerator Login(string _email, string _password, Action<bool> onLoginCompleted)
    {
        // email �� password �� �����Ͽ� firebase ���� �Լ��� ȣ��.
        var LoginTask = _auth.SignInWithEmailAndPasswordAsync(_email, _password);
        // LoginTask.IsCompleted �� ���� �� �� ���� ��ٸ�.
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);
        _user = LoginTask.Result.User;
        if (LoginTask.Exception != null)
        {
            // ���� ���ܰ� �߻��Ͽ� ������ ��Ÿ����
            Debug.LogWarning(message: $"�α��� �ϴµ� ���ܰ� �߻��Ͽ� ���� {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "�α��� ����";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "�̸����� ��������.";
                    break;

                case AuthError.MissingPassword:
                    message = "��й�ȣ�� ��������.";
                    break;

                case AuthError.WrongPassword:
                    message = "�߸��� ��й�ȣ.";
                    break;

                case AuthError.InvalidEmail:
                    message = "�߸��� �̸���";
                    break;

                case AuthError.UserNotFound:
                    message = "������ �������� �ʽ��ϴ�.";
                    break;
            }
            warningLoginText.text = message;
            onLoginCompleted?.Invoke(false);
            
        }
        // ����� �۵� �Ѵٸ�.
        else
        {
            Debug.LogFormat("�α��� ���� : {0} {1}", _user.Email, _user.DisplayName);
            warningLoginText.text = "";
            confirmLoginText.text = "�α��� ����!!";
            onLoginCompleted?.Invoke(true);
            
        }
    }

    IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            warningRegisterText.text = "�г����� �����ּ���.";
        }
        else if (passwordRegister.text != passwordCheck.text)
        {
            warningRegisterText.text = " ��й�ȣ�� ��ġ���� �ʽ��ϴ�. �ٽ� �õ��ϼ���.";
        }
        else
        {
            var RegisterTask = _auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            // ������ ���� �� ������ �߻��ϸ�
            if (RegisterTask.Exception != null)
            {
                Debug.LogWarning(message: $"����ϴµ� ���ܰ� �߻��Ͽ� ���� {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "ȸ������ ����!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "�̸����� ��������.";
                        break;

                    case AuthError.MissingPassword:
                        message = "��й�ȣ�� ��������.";
                        break;

                    case AuthError.WeakPassword:
                        message = "��й�ȣ�� ������ ����մϴ�.";
                        break;

                    case AuthError.EmailAlreadyInUse:
                        message = "�ߺ��� �̸����Դϴ�.";
                        break;
                }
                warningRegisterText.text = message;
            }
            // ȸ������ ����.
            else
            {
                _user = RegisterTask.Result.User;

                if (_user != null)
                {
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    var ProfileTask = _user.UpdateUserProfileAsync(profile);

                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        // ����� ���� (username) �� �ҷ����µ� ���ܰ� �߻��ϸ�
                        Debug.LogWarning(message: $"����� ������ ������ ������Ʈ �ϴµ� ���ܰ� �߻��߽��ϴ�. {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "���� ������ �ҷ����µ� �����߽��ϴ�!";
                    }
                    // �����Ѵٸ�
                    else
                    {
                        Debug.Log("ȸ�������� ���������� �̷�������ϴ�." + _user.DisplayName);
                        ConfrimRegisterText.text = "ȸ������ ����!!";
                        warningRegisterText.text = "";
                    }
                }
            }
        }
    }
}
