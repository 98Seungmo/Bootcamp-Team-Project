using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageUIManager : MonoBehaviour
{
    /* �޴� UI */
    public GameObject menuUI;
    /* �˾�â UI */
    public GameObject weaponUI;
    public GameObject armorUI;
    public GameObject accUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuUI.SetActive(true);

            Cursor.visible = true;
        }
    }
    /* UIâ �ݱ� */
    public void CloseMenuUI()
    {
        menuUI.SetActive(false);

        Cursor.visible = false;
    }
    #region
    /* �˾�â �ݱ� */
    // ���� UI â
    public void OpenWeaponUI()
    {
        weaponUI.SetActive(true);
    }

    public void CloseWeaponUI()
    {
        weaponUI.SetActive(false);
    }
    // �� UI â
    public void OpenArmorUI()
    {
        armorUI.SetActive(true);
    }

    public void CloseArmorUI()
    {
        armorUI.SetActive(false);
    }
    // �Ǽ����� UI â
    public void OpenAccUI()
    {
        accUI.SetActive(true);
    }

    public void CloseAccUI()
    {
        accUI.SetActive(false);
    }
    #endregion
}
