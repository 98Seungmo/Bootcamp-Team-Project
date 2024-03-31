using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetItemManager : MonoBehaviour
{
    public TMP_Text itemText; // UI 텍스트(Text) 요소를 연결합니다.
    public float displayTime = 3f; // 텍스트가 표시되는 시간을 정의합니다.

    private float timer; // 텍스트가 표시된 후의 경과 시간을 추적합니다.

    public void GetItem(string itemName)
    {
        if (itemText != null)
        {
            // 현재 UI 텍스트에 표시된 내용을 가져옵니다.
            string currentText = itemText.text;

            // 새 아이템 정보를 추가합니다.
            string newText = currentText + "\n" + itemName + " +1";

            // UI 텍스트를 업데이트하여 새로운 아이템 정보를 표시합니다.
            itemText.text = newText;

            // 텍스트가 표시된 후의 경과 시간을 초기화합니다.
            timer = 0f;
        }
        switch (itemName)
        {
            case "뼈":
                //id = 7
                break;
            case "고기":
                //id = 9
                break;
            case "야채 바구니":
                //id = 11
                break;
            case "호박":
                //id = 10
                break;
            case "향신료":
                //id = 8
                break;
            case "하급 강화석":
                //id = 5
                break;
            case "상급 강화석":
                //id = 6
                break;
            case "반지":
                //id = 27
                break;
            case "목걸이":
                //id = 28
                break;
            case "귀걸이":
                //id = 29
                break;
        }
    }

    void Update()
    {
        // 텍스트가 표시된 후의 경과 시간을 업데이트합니다.
        timer += Time.deltaTime;

        // 일정 시간이 지난 후에 UI 텍스트를 비웁니다.
        if (timer >= displayTime)
        {
            ClearItemText();
        }
    }

    void ClearItemText()
    {
        // UI 텍스트를 비웁니다.
        if (itemText != null)
        {
            itemText.text = "";
        }
    }
}
