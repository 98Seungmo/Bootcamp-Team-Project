using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("Options")]
    [Range(0, 10)]
    // ���� ���� ����
    [SerializeField] private int _horizontalSlotCount = 5;
    // �ִ� ���� ���� ����
    [SerializeField] private int _maxSlotCount = 30;
    [Range(0, 10)]
    // ���� ���� ����
    [SerializeField] private int _verticalSlotCount = 6;
    // ���� �����¿� ����
    [SerializeField] private float _slotMargin = 8f;
    // �κ��丮 ���� ���� ����
    [SerializeField] private float _contentAreaPadding = 20f;
    [Range(32, 64)]
    // �� ������ ũ��
    [SerializeField] private float _slotSize = 64f;

    [Header("Connected Objects")]
    // ���Ե��� ��ġ�� ����
    [SerializeField] private RectTransform _contentAreaRT;
    // ���� ������
    [SerializeField] private GameObject _slotUiPrefab;

    /// <summary>
    /// ������ ������ŭ ���� ���� ���� ���Ե� ���� ����.
    /// </summary>
    private void InitSlots()
    {
        // _slotUiPrefab �� ����ؼ� ���� �������� RectTransform �� ItemSlotUI ������Ʈ�� ������.
        _slotUiPrefab.TryGetComponent(out RectTransform slotRect);
        // _slotSize ������ ����ؼ� ������ ũ�� ����, �ش� ũ��� ���� �������� sizeDelta �� ������.
        slotRect.sizeDelta = new Vector2(_slotSize, _slotSize);

        // TryGetComponent �� ����Ͽ� _slotUiPrefab ���� UI�� ã������ �õ��ϰ� ���� ��� ItemSlotUI ������Ʈ�� �߰���. 
        _slotUiPrefab.TryGetComponent(out ItemSlotUI itemSlot);
        if (itemSlot == null)
            _slotUiPrefab.AddComponent<ItemSlotUI>();

        // ���� ������ ��Ȱ��ȭ
        _slotUiPrefab.SetActive(false);

        // ������ġ ����
        Vector2 beginPos = new Vector2(_contentAreaPadding, _contentAreaPadding-_contentAreaPadding);
        Vector2 curPos = beginPos;

        _slotUIList = new List<ItemSlotUI>(_verticalSlotCount * _horizontalSlotCount);

    }
}
