using KJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KJ
{
    /**
     * @brief 인벤안에 있는 슬롯 클릭시 장비창에 해당 아이템 (무기, 방어구, 장신구) 표시 
     */
    public class ItemSlot_Inven : MonoBehaviour
    {
        public string _itemidx = string.Empty;

        /**
         * @brief 버튼 클릭시 해당 장비창에 아이템 이미지 표시
         */
        public void OnButtonClick()
        {
            MenuUIManager mag = MenuUIManager.Instance;
            InventoryUI invUI = mag.GetComponent<InventoryUI>();
            KJ.Item item = invUI._inventory.items.Find(item => item.id == _itemidx);

            Debug.Log("OnButtonClick: " + item.imagePath);
            Debug.Log("OnButtonClick: " + item.type);

            Sprite s = ItemDBManager.Instance.LoadItemSprite(item.imagePath);
            switch (item.type)
            {
                case "weapon":
                    mag._img_weapon.sprite = s;
                    break;
                case "armor":
                    mag._img_armor.sprite = s;
                    break;
                case "accessory":
                    mag._img_accessory.sprite = s;
                    break;
            }

        }

    }
}
