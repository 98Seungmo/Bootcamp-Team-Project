using KJ;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Item = KJ.Item;

namespace KJ
{
    /**
     * @brief 인벤토리 UI
     */
    public class InventoryUI : MonoBehaviour
    {
        [Header("Slot")]
        public GameObject slotPrefab; ///< slot 프리팹
        public Transform slotPanel; ///< 슬롯 담을 공간
        [Header("Item Infomation")]
        public TMP_Text itemName; ///< 아이템 이름
        public TMP_Text itemDescription; ///< 아이템 설명
        public TMP_Text itemQuantity; ///< 아이템 수량

        public string uid = PlayerDBManager.Instance.CurrentShortUID; ///< 현재 UID 가져옴

        PlayerDBManager playerDBManager = PlayerDBManager.Instance; ///< PlayerDB매니저 불러옴
        private GameData _gameData = NetData.Instance.gameData; ///< 게임 데이터 불러옴

        /**
         * @brief 플레이어의 클래스 선택하면 해당 클래스의 정보 가져옴
         */
        private Class _class
        {
            get
            {
                Debug.Log("type: " + playerDBManager.LoadGameData(playerDBManager.CurrentShortUID).classType);

                switch (playerDBManager.LoadGameData(playerDBManager.CurrentShortUID).classType)
                {
                    case "Knight":
                        Debug.Log("기사");
                        return _gameData.classes[ClassType.knight];

                    case "Barbarian":
                        Debug.Log("바바리안");
                        return _gameData.classes[ClassType.barbarian];

                    case "Rogue":
                        Debug.Log("로그");
                        return _gameData.classes[ClassType.rogue];

                    case "Mage":
                        Debug.Log("메이지");
                        return _gameData.classes[ClassType.mage];

                }
                Debug.Log("안불러와짐");
                return null;
            }
        }
        
        /**
         * @brief 클래스 인벤토리 불러옴
         */
        public Inventory _inventory
        {
            get
            {
                return _class.inventory;
            }
        }

        /**
         * @brief 아이템 데이터 불러옴
         */
        private ItemData _itemData
        {
            get
            {
                return ItemDBManager.Instance._itemData;
            }
        }

        /**
         * @brief 아이템 가져옴
         * @param[in] id 아이템 ID
         */
        public Item GetItem(string id)
        {
            return ItemDBManager.Instance.GetItem(id);
        }

        /**
         * 인벤토리 초기화 후 해당 클래스의 기본 장비 인벤토리에 추가
         */
        void Start()
        {
            _inventory.items.Clear();

            foreach (var c in _gameData.classes.Values)
            {
                Debug.Log("start " + c.classType.ToString());
            }
            if (_class.classType == ClassType.knight)
            {
                Debug.Log("기사2");
                AddItem("12");
                AddItem("24");
            }
            else if (_class.classType == ClassType.barbarian)
            {
                Debug.Log("바바리안2");
                AddItem("15");
                AddItem("24");
            }
            else if (_class.classType == ClassType.rogue)
            {
                Debug.Log("로그2");
                AddItem("18");
                AddItem("24");
            }
            else
            {
                Debug.Log("메이지2");
                AddItem("21");
                AddItem("24");
            }
        }

        /**
         * @brief 현재 인벤토리에서 해당 아이템 탐색 후 아이템 추가
         */
        public void UpdateInventoryUI()
        {
            foreach (var item in _inventory.items)
            {
                AddItem(item.id);
            }
        }

        /* 인벤토리에 아이템 추가 */
        /**
         * @brief 인벤토리에 아이템 추가
         * @param[in] itemId 아이템 ID
         */
        public void AddItem(string itemId)
        {
            Debug.Log($"{itemId} 추가");
            /*  근데 해당 아이템이 뭔지 알아야함 
                플레이어 인벤토리에 해당 아이템이 있는지 체크해야 함 
                이미 있는 아이템이면 수량만 +1, 없으면 플레이어 인벤토리에 아이템이 추가됨. 
                새로운 아이템(즉 없는 아이템을 얻을 경우 슬롯도 같이 생성) */

            Item itemToAdd = GetItItemById(itemId);

            if (itemToAdd != null)
            {
                Item item = null;
                foreach (var i in _inventory.items)
                {
                    if (i.id == itemId)
                    {
                        item = i;
                        break;
                    }
                }

                if (item != null && item.id == itemId)
                {
                    Debug.Log($"제발 :{item.id}");
                    Debug.Log($"제발2 :{item.imagePath}");

                    item.quantity++;
                    itemQuantity.text = item.quantity.ToString();
                    CreateSlot(item);
                }
                else
                {

                    _inventory.items.Add(itemToAdd);

                    Debug.Log($"이미지 : {itemToAdd.id}");
                    CreateSlot(itemToAdd);
                }

            }
        }

        /* 인벤토리에 아이템 제거 */
        /**
         * @brief 인벤토리에 아이템 제거
         * @param[in] itemId 아이템 ID
         */
        public void RemoveItem(string itemId)
        {
            Item itemToRemove = GetItItemById(itemId);

            if (itemToRemove != null)
            {
                Item item = _inventory.items.Find(item => item.id == itemId);

                if (item != null)
                {
                    item.quantity--;
                    itemQuantity.text = item.quantity.ToString();

                    if (itemToRemove.quantity <= 0)
                    {
                        _inventory.items.Remove(itemToRemove);
                        RemoveSlot(itemToRemove);
                    }
                }
                else
                {
                    Debug.Log("제거할 아이템 찾을 수 없음." + itemId);
                }
            }
        }

        /* 슬롯 생성 */
        /**
         * @brief 아이템 생성시 슬롯 생성
         * @param[in] item 해당 아이템
         */
        private void CreateSlot(Item item)
        {
            GameObject slot = Instantiate(slotPrefab, slotPanel);
            Debug.Log("슬롯추가");
            slot.name = item.id;
            ItemSlot_Inven sSlot = slot.GetComponent<ItemSlot_Inven>();
            sSlot._itemidx = item.id;


            Image[] itemImageComponent = slot.GetComponentsInChildren<Image>();

            Sprite itemImage = ItemDBManager.Instance.LoadItemSprite(item.imagePath);

            if (itemImage == null) Debug.Log("itemImage == null : " + item.imagePath);
            if (itemImageComponent != null)
            {
                Debug.Log("이미지 추가");
                itemImageComponent[1].sprite = itemImage;
                itemImageComponent[1].enabled = true;
            }

        }

        /* 슬롯 삭제 */
        /**
         * @brief 아이템 제거시 슬롯 삭제
         * @param[in] item 해당 아이템
         */
        private void RemoveSlot(Item item)
        {
            /* 제거할 아이템 id 찾기 
               아이템이 없는거 확인하면 해당 슬롯도 같이 삭제*/
            foreach (Transform slotTransform in slotPanel)
            {
                if (slotTransform.name == item.id)
                {
                    Destroy(slotTransform.gameObject);
                    break;
                }
            }
        }

        /* 아이템DB 에서 주어진 ID 를 가진 아이템 찾음. */
        /**
         * @brief 아이템 DB 에서 주어진 ID 를 가진 아이템 찾음
         * @param[in] id 해당 아이템 ID
         */
        public Item GetItItemById(string id)
        {
            Debug.Log($"ID = {id}");
            return _itemData.items.Find(item => item.id == id);
        }

        /* 슬롯 클릭 */
        /**
         * @brief 슬롯 클릭시 해당 정보 표시
         * @param[in] item 해당 아이템
         */
        public void ClickDescription(Item item)
        {
            itemName.text = item.id;
            itemDescription.text = item.description;
            //bool _type = item.type

            switch (item.type)
            {
                case "weapon":

                    break;
                case "armor": break;
                case "accessory": break;

            }
            /* 특정 타입(Weapon, Armor, Acc)을 클릭할 때 장비창에 해당 이미지 전달.*/
        }

        public void ClickSlot()
        {
            //ClickDescription();
        }
    }
}
