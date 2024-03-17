using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public Item(string _type, string _name, string _description, string _quantity, string _imagePath, bool _enhanceable, string _enhanceLevel) 
    {Type = _type; Name = _name; Description = _description; Quantity = _quantity; ImagePath = _imagePath; Enhanceable = _enhanceable; EnhanceLevel = _enhanceLevel;}

    public string Type, Name, Description, Quantity, ImagePath, EnhanceLevel;
    public bool Enhanceable;
}

public class ItemDBManager : MonoBehaviour
{
    public TextAsset itemDatabase;
    // itemDatabase �� Item Ŭ������ �°� ����Ʈȭ ��Ŵ.
    public List<Item> allItemList;

    void Start()
    {
        // ��ü ������ ����Ʈ �ҷ�����
        string[] line = itemDatabase.text.Substring(0, itemDatabase.text.Length).Split('\n');
        for (int i = 1; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');

            allItemList.Add(new Item(row[0], row[1], row[2], row[3], row[4], row[5] == "True", row[6]));
        }
    }
}
