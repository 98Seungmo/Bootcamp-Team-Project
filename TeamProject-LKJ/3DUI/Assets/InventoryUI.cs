using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Options")]
    [Range(0, 10)]
    // 슬롯 가로 개수
    [SerializeField] private int _horizontalSlotCount = 5;
    // 최대 슬롯 개수 설정
    [SerializeField] private int _maxSlotCount = 30;
    [Range(0, 10)]
    // 슬롯 세로 개수
    [SerializeField] private int _verticalSlotCount = 6;
    // 슬롯 상하좌우 여백
    [SerializeField] private float _slotMargin = 8f;
    // 인벤토리 영역 내부 여백
    [SerializeField] private float _contentAreaPadding = 20f;
    [Range(32, 64)]
    // 각 슬롯의 크기
    [SerializeField] private float _slotSize = 64f;

    [Header("Connected Objects")]
    // 슬롯들이 위치할 영역
    [SerializeField] private RectTransform _contentAreaRT;
    // 슬롯 프리팹
    [SerializeField] private GameObject _slotUiPrefab;

    /// <summary>
    /// 지정된 개수만큼 슬롯 영역 내에 슬롯들 동적 생성.
    /// </summary>
    private void InitSlots()
    {
        // _slotUiPrefab 을 사용해서 슬롯 프리팹의 RectTransform 과 ItemSlotUI 컴포넌트를 설정함.
        _slotUiPrefab.TryGetComponent(out RectTransform slotRect);
        // _slotSize 변수를 사용해서 슬롯의 크기 결정, 해당 크기로 슬롯 프리팹의 sizeDelta 를 설정함.
        slotRect.sizeDelta = new Vector2(_slotSize, _slotSize);

        // TryGetComponent 를 사용하여 _slotUiPrefab 에서 UI를 찾으려고 시도하고 없을 경우 ItemSlotUI 컴포넌트를 추가함. 
        _slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
            _slotUiPrefab.AddComponent<ItemSlotUI>();

        // 슬롯 프리팹 비활성화
        _slotUiPrefab.SetActive(false);

        // 시작위치 설정
        Vector2 beginPos = new Vector2(_contentAreaPadding, _contentAreaPadding-_contentAreaPadding);
        Vector2 curPos = beginPos;

        _slotUIList = new List<ItemSlotUI>(_verticalSlotCount * _horizontalSlotCount);

    }
}
