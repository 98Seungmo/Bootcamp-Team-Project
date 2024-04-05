using KJ;
using Ricimi;
using Scene_Teleportation_Kit.Scripts.player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief MenuUI 에 관한 관리
     */
    public class MenuUIManager : MonoBehaviour
    {
        PlayerDBManager playerDBManager = PlayerDBManager.Instance; ///< 플레이어 DB 불러옴
        public static MenuUIManager Instance = null;

        /* 메뉴 UI */
        [Header("Menu UI")]
        public GameObject menuUI; ///< 메뉴 UI
        /* 장비 팝업창 UI */
        [Header("Equip UI")]
        public GameObject weaponUI; ///< 무기장비 UI
        public GameObject armorUI; ///< 방어구 장비 UI
        public GameObject accUI; ///< 장신구 장비 UI
        /* 인벤토리 UI */
        [Header("Inventory UI")]
        public GameObject inventoryUI; ///< 인벤토리 UI
        /* 확인창 UI */
        [Header("Check UI")]
        public GameObject checkQuit; ///< 종료 확인창 
        [Header("Class")]
        public TMP_Text classType; ///< 클래스 타입 텍스트

        [Header("ClassImage")]
        public Image knghtImage; ///< 기사 로고 이미지
        public Image babarianImage; ///< 바바리안 로고 이미지
        public Image rogueImage; ///< 로그 로고 이미지
        public Image mageImage; ///< 메이지 로고 이미지


        public Image _img_weapon; ///< 무기 이미지
        public Image _img_armor; ///< 방어구 이미지
        public Image _img_accessory; ///< 장신구 이미지


        public void Awake()
        {
            Instance = this;


        }

        /**
         * @brief esc 눌렀을 때 메뉴 호출 해당 클래스에 맞게 이미지 로고 호출
         */
        void Update()
        {
            /* esc 누르면 메뉴 호출 */
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("ESC!!");
                menuUI.SetActive(true);
                classType.text = playerDBManager.LoadGameData(playerDBManager.CurrentShortUID).classType;

                switch (playerDBManager.LoadGameData(playerDBManager.CurrentShortUID).classType)
                {
                    case "Knight":
                        knghtImage.gameObject.SetActive(true);
                        break;
                    case "Babarian":
                        babarianImage.gameObject.SetActive(true);
                        break;
                    case "Rogue":
                        rogueImage.gameObject.SetActive(true);
                        break;
                    case "Mage":
                        mageImage.gameObject.SetActive(true);
                        break;
                }
                Cursor.visible = true;
                /* 메뉴 호출시 인게임 멈춤 */
                PauseGame();
            }
        }
        #region 메뉴 팝업
        /**
         * brief 메뉴창 닫기
         */
        public void CloseMenuUI()
        {
            menuUI.SetActive(false);

            //Cursor.visible = false;

            ResumeGame();
        }
        #endregion
        #region 메뉴 팝업시 일시정지

        /**
         * brief 게임 일시정지
         */
        public void PauseGame()
        {
            Debug.Log("멈춤");
            Time.timeScale = 0;
        }
        /**
         * @brief 게임 재개
         */
        public void ResumeGame()
        {
            Debug.Log("재생");
            Time.timeScale = 1;
        }
        #endregion
        #region 인벤토리 팝업
        /**
         * @brief 인벤토리 활성화
         */
        public void OpenInventoryUI()
        {
            inventoryUI.SetActive(true);
        }

        /**
         * @brief 인벤토리 비활성화
         */
        public void CloseInventoryUI()
        {
            inventoryUI.SetActive(false);
        }
        #endregion
        #region 확인창 팝업

        /* 게임 종료 하는 확인창 */
        /**
         * @brief 확인창 활성화
         */
        public void OpenCheckQuit()
        {
            checkQuit.SetActive(true);
        }
        /**
         * @brief 확인창 비활성화
         */
        public void CloseCheckQuit()
        {
            checkQuit.SetActive(false);
        }
        #endregion
        #region Scene 이동
        /* 게임 종료 */
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
    }
}
