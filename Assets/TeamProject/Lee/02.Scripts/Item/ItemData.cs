using UnityEngine;

public class ItemData //�����Կ� ���� �������� ������ �����Ѵ�.
{
    public GameObject Item;
    public GameObject Item_Icon;

    public int ItemId;
    public string ItemUIGroup;


    public ItemData(GameObject item, GameObject item_icon, int id, string Groupname) //�� ������ ���� ����
    {
        Item = item;
        Item_Icon = item_icon;
        ItemId = id;
        ItemUIGroup = Groupname;
    }
}
