using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 로그인 시 필요한 UI 및 기능 구현
     */
    public class LoginManager : MonoBehaviour
    {
        #region 설정창 열고 닫기
        [Header("SettingUI On/Off")]
        [SerializeField] private GameObject _settingUI; ///< 옵션창 

        /**
         * @brief 옵션창 활성화
         */
        public void OpenSettingUI()
        {
            _settingUI.SetActive(true);
        }

        /**
         * @brief 옵션창 비활성화
         */
        public void CloseSettingUI()
        {
            _settingUI.SetActive(false);
        }
        #endregion
        #region 게임 시작시 로그인 창 열기, X 누르면 닫기
        [Header("Login On/Off")]
        [SerializeField] private GameObject _logInUI; ///< 로그인창

        /**
         * @brief 로그인창 활성화
         */
        public void OpenLogIn()
        {
            _logInUI.SetActive(true);
        }

        /**
         * @brief 로그인창 비활성화
         */
        public void CloseLogIn()
        {
            _logInUI.SetActive(false);
        }
        #endregion
        #region 게임 종료 (현재는 테스트씬으로 이동)
        /**
         * @brief 게임 종료
         */
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
        #endregion
        #region 로그인 하면 로딩창으로 이동

        /**
         * @brief 로그인 시도 후 성공하면 로딩창으로 이동, 실패하면 로그인 실패
         */
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
                    Debug.LogError("로그인 실패");
                }
            });

        }
        #endregion
        #region 회원가입창 열고 닫기
        [Header("RegisterUI On/Off")]
        [SerializeField] private GameObject _registerUI; ///< 회원가입창

        /**
         * @brief 회원가입창 활성화
         */
        public void OpenRegister()
        {
            _registerUI.SetActive(true);
        }

        /**
         * @brief 회원가입창 비활성화
         */
        public void CloseRegister()
        {
            _registerUI.SetActive(false);
        }
        #endregion
        #region InputField 이미지 숨기기
        /// InputField 와 Image 를 1대1 매칭하기 위한 Dictionary
        private Dictionary<TMP_InputField, GameObject> _inputFieldImageMap = new Dictionary<TMP_InputField, GameObject>();

        [Header("ImageToHide")]
        /* Login */
        public TMP_InputField emailLogin; ///< 이메일 로그인
        public TMP_InputField passwordLogin; ///< 비밀번호 로그인
        /* Register */
        public TMP_InputField usernameRegister; ///< 유저 이름
        public TMP_InputField emailRegister; ///< 이메일
        public TMP_InputField passwordRegister; ///< 비밀번호
        public TMP_InputField passwordCheckRegister; ///< 비밀번호 확인

        [Header("Image")]
        /* Login */
        public GameObject emailLoginImage; ///< 이메일 로그인 이미지
        public GameObject passwordLoginImage; ///< 비밀번호 로그인 이미지
        /* Register */
        public GameObject usernameRegisterImage; ///< 유저 이름 이미지
        public GameObject emailRegisterImage; ///< 이메일 이미지
        public GameObject passwordRegisterImage; ///< 비밀번호 이미지
        public GameObject passwordCheckImage; ///< 비밀번호 확인 이미지
        #endregion
        #region Tab 키로 InputField 포커스 이동
        // 현재 활성화 된 패널 식별하는 enum */    
        public enum ActivePanel { Login, Register } ///< 활성화 된 패널 식별
        [Header("InputField Focus")]
        public ActivePanel activePanel; ///< 활성화 패널


        // 로그인 InputFields
        public List<TMP_InputField> inputFieldsLogin; ///< 로그인 인풋 필드
        // 회원가입 InputFields
        public List<TMP_InputField> inputFieldsRegister; ///< 회원가입 인풋 필드
        #endregion
        #region 이메일 저장
        [Header("SaveEmail")]
        // 이메일 InputField.
        public TMP_InputField emailInputField; ///< 이메일 인풋필드
        // 이메일 저장 토글
        public Toggle saveEmailToggle; ///< 이메일 저장 토글

        // PlayerPrefs 에서 이메일 저장할때 사용할 키
        private const string EmailKey = "UserEmail"; ///< PlayerPrefs 에서 이메일 저장할 때 사용할 키


        #endregion

        /**
         * @brief 인풋 필드 이미지 숨기기 및 이메일 저장
         */
        void Start()
        {
            #region InputField 이미지 숨기기
            // 각 InputField 와 Image 매칭
            /* -----------------------------Login------------------------------- */
            _inputFieldImageMap.Add(emailLogin, emailLoginImage);
            _inputFieldImageMap.Add(passwordLogin, passwordLoginImage);
            /* ---------------------------Register------------------------------ */
            _inputFieldImageMap.Add(usernameRegister, usernameRegisterImage);
            _inputFieldImageMap.Add(emailRegister, emailRegisterImage);
            _inputFieldImageMap.Add(passwordRegister, passwordRegisterImage);
            _inputFieldImageMap.Add(passwordCheckRegister, passwordCheckImage);

            // 모든 InputField 에 Listener 추가 */
            foreach (var pair in _inputFieldImageMap)
            {
                pair.Key.onValueChanged.AddListener((value) => ToggleImage(pair.Key, value));
            }
            #endregion
            #region 이메일 저장
            /* 앱 시작시 저장된 이메일 불러오기 */
            LoadEmail();

            /* 토글 버튼에 따라 이메일 저장 여부 결정 */
            saveEmailToggle.onValueChanged.AddListener(OnToggleChanged);

            #endregion
        }

        /**
         * @brief Tab 키 누를 때 인풋필드끼리 이동
         */
        void Update()
        {
            #region Tab 키로 InputField 포커스 이동
            /* tab 키를 누르면 NavigateThroughInputField 메서드 실행 */
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                /* 탭별로 활성화 */
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

        #region Tab 키로 InputField 포커스 이동

        /**
         * @biref 인풋필드끼리 이동할 수 있게 방향 제시
         * @param[in] inputFields 인풋 필드
         */
        public void NavigateThroughInputField(List<TMP_InputField> inputFields)
        {
            for (int i = 0; i < inputFields.Count; i++)
            {
                if (inputFields[i].isFocused)
                {
                    /* 다음 InputField 계산 */
                    int nextIndex = (i + 1) % inputFields.Count;
                    /* 다음 InputField 에 포커스 설정 */
                    inputFields[nextIndex].Select();
                    /* 다음 InputField 활성화 */
                    inputFields[nextIndex].ActivateInputField();
                    break;
                }
            }
        }

        /**
         * @brief 인풋 필드 이동 활성화 패널
         * @param[in] panel 활성화 패널
         */
        public void SetActivePanel(ActivePanel panel)
        {
            activePanel = panel;
        }

        /**
         * @brief 인풋 필드 로그인창에서 포커스 활성화
         */
        public void SetActivePanelToLogin()
        {
            SetActivePanel(LoginManager.ActivePanel.Login);
        }
        /**
         * @brief 인풋 필드 회원가입창에서 포커스 활성화
         */
        public void SetActivePanelToRegister()
        {
            SetActivePanel(LoginManager.ActivePanel.Register);
        }
        #endregion

        #region InputField 이미지 숨기기
        /**
         * @brief 인풋 필드에 입력시 이미지 숨기기
         * @param[in] inputField 인풋 필드
         * @param[in] inputValue 인풋 값
         */
        void ToggleImage(TMP_InputField inputField, string inputValue)
        {
            /* 해당 InputField 에 연결된 이미지 활성화 / 비활성화 */
            if (_inputFieldImageMap.TryGetValue(inputField, out GameObject Image))
            {
                Image.SetActive(string.IsNullOrEmpty(inputValue));
            }
        }
        #endregion

        #region 이메일 저장
        /**
         * @brief 이메일 저장
         * @param[in] isOn bool 형식 들어올 경우 이메일 저장
         */
        public void OnToggleChanged(bool isOn)
        {
            if (isOn)
            {
                /* 토글이 On 일때 현재 InputField 의 이메일 주소 저장 */
                SaveEmail();
            }
            else
            {
                /* 토글이 Off 일때 저장된 이메일 주소 삭제 */
                PlayerPrefs.DeleteKey(EmailKey);
            }
        }

        /**
         * @brief 이메일 저장
         */
        public void SaveEmail()
        {
            PlayerPrefs.SetString(EmailKey, emailInputField.text);
            PlayerPrefs.Save();
        }

        /**
         * @brief 저장된 이메일 불러오기
         */
        public void LoadEmail()
        {
            if (PlayerPrefs.HasKey(EmailKey))
            {
                string saveEmail = PlayerPrefs.GetString(EmailKey);
                emailInputField.text = saveEmail;
                /* 저장된 이메일이 있으면 토글이 true 인 상태로 설정 */
                saveEmailToggle.isOn = true;
            }
            else
            {
                /* 저장된 이메일이 없으면 토글이 false 인 상태로 설정 */
                saveEmailToggle.isOn = false;
            }

        }
        #endregion

        #region 플레이어 정보
#if UNITY_EDITOR
        /**
         * @brief 직렬화된 플레이어 정보 
         */
        private void OnGUI()
        {
            if (GUI.Button(new Rect(0, 0, 100, 50), "Create file"))
            {


                ///* 플레이어 기본 정보 */
                GameData gameData = new GameData();

                Player players = new Player();
                players.uid = string.Empty;
                players.shortUID = string.Empty;
                players.userName = string.Empty;
                players.inventory = new Inventory();
                players.gold = 0;
                players.combatPower = 0;
                players.buffs = new List<string>();

                gameData.players.Add(players.shortUID, players);

                /*직업 기본 정보*/
                Class knight = new Class();
                knight.name = "Knight";
                knight.classType = ClassType.knight;
                knight.baseHp = 100;
                knight.baseSp = 100;
                knight.inventory = new Inventory();
                knight.inventory.items.Add(new Item() { id = "12", quantity = 1, imagePath = "Images/Items/SwordCommon" });
                knight.inventory.items.Add(new Item() { id = "24", quantity = 1, imagePath = "Images/Items/ArmorCommon" });
                knight.skills.Add(new Skill() { id = "33", name = "기사의 검술", description = "검을 휘둘러 벤다. 연속해서 베어낸 후 강하게 찌르며 밀쳐낸다." });
                knight.skills.Add(new Skill() { id = "34", name = "방어 태세", description = "방패를 들어 체력 대신 스태미나를 소비하여 적의 공격을 받아낸다. 방어에 성공할 경우 반격을 가해 밀쳐낼 수 있다." });
                knight.skills.Add(new Skill() { id = "30", name = "체력 단련", description = "훈련을 통해 더욱 격렬한 전투에도 견딜 수 있게 한다." });
                knight.skills.Add(new Skill() { id = "31", name = "지구력 단련", description = "훈련으로 더욱 격렬한 움직임을 가능하게 한다." });
                knight.gold = 100;

                gameData.classes.Add(ClassType.knight, knight);

                Class barbarian = new Class();
                barbarian.name = "Barbarian";
                barbarian.baseHp = 100;
                barbarian.baseSp = 100;
                barbarian.classType = ClassType.barbarian;
                barbarian.inventory = new Inventory();
                barbarian.inventory.items.Add(new Item() { id = "15", quantity = 1 });
                barbarian.inventory.items.Add(new Item() { id = "24", quantity = 1 });
                barbarian.skills.Add(new Skill() { id = "35", name = "광전사의 도끼", description = "적에게 거대한 도끼를 휘둘러 강력한 피해를 준다." });
                barbarian.skills.Add(new Skill() { id = "36", name = "피바람", description = "도끼를 휘두르며 주위의 모든 것을 부수며 이동한다." });
                barbarian.skills.Add(new Skill() { id = "30", name = "체력 단련", description = "훈련을 통해 더욱 격렬한 전투에도 견딜 수 있게 한다." });
                barbarian.skills.Add(new Skill() { id = "32", name = "공격력 단련", description = "훈련을 통해 더욱 치명적인 공격하는 법을 익힌다." });
                barbarian.gold = 100;

                gameData.classes.Add(ClassType.barbarian, barbarian);

                Class rogue = new Class();
                rogue.name = "Rogue";
                rogue.baseHp = 100;
                rogue.baseSp = 100;
                rogue.classType = ClassType.rogue;
                rogue.inventory = new Inventory();
                rogue.inventory.items.Add(new Item() { id = "18", quantity = 1 });
                rogue.inventory.items.Add(new Item() { id = "24", quantity = 1 });
                rogue.skills.Add(new Skill() { id = "33", name = "불한당의 단검", description = "치명적인 두 쌍의 단검으로 적을 갈기갈기 찢는다." });
                rogue.skills.Add(new Skill() { id = "34", name = "눈먼 화살", description = "쇠뇌를 장전해 빠르게 발사한다." });
                rogue.skills.Add(new Skill() { id = "30", name = "지구력 단련", description = "훈련으로 더욱 격렬한 움직임을 가능하게 한다." });
                rogue.skills.Add(new Skill() { id = "31", name = "공격력 단련", description = "훈련을 통해 더욱 치명적인 공격하는 법을 익힌다." });
                rogue.gold = 100;

                gameData.classes.Add(ClassType.rogue, rogue);

                Class mage = new Class();
                mage.name = "Mage";
                mage.baseHp = 100;
                mage.baseSp = 100;
                mage.classType = ClassType.mage;
                mage.inventory = new Inventory();
                mage.inventory.items.Add(new Item() { id = "21", quantity = 1 });
                mage.inventory.items.Add(new Item() { id = "24", quantity = 1 });
                mage.skills.Add(new Skill() { id = "39", name = "마력 구체", description = "순수한 마력을 담은 구체를 발사한다." });
                mage.skills.Add(new Skill() { id = "40", name = "마력 폭풍", description = "주변의 모든 마력을 흡수하는 소용돌이를 만들어 마력 폭발을 일으킨다." });
                mage.skills.Add(new Skill() { id = "31", name = "지구력 단련", description = "훈련으로 더욱 격렬한 움직임을 가능하게 한다." });
                mage.skills.Add(new Skill() { id = "32", name = "공격력 단련", description = "훈련을 통해 더욱 치명적인 공격하는 법을 익힌다." });
                mage.gold = 100;

                gameData.classes.Add(ClassType.mage, mage);

                string jsondata = Newtonsoft.Json.JsonConvert.SerializeObject(gameData);

                string path = "C:/Users/LKJ/Documents/GitHub/Bootcamp-Team-Project/Cos/Assets/LKJ/KJ/Resources/ClassDB.txt";
                StreamWriter w = new StreamWriter(path);
                w.Write(jsondata);

                w.Close();
            }

            if (GUI.Button(new Rect(0, 100, 100, 50), "firebase"))
            {
                NetData.Instance.SavePlayerDB(null);
            }

            if (GUI.Button(new Rect(0, 200, 100, 50), "firebase"))
            {
                NetData.Instance.ReadDataItem("Items");
                NetData.Instance.ReadDataPlayer("players");
            }

        }
#endif
#endregion
    }
}
