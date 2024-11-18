using UnityEngine;

public class ItemData //퀵슬롯에 들어가는 아이템의 정보만 저장한다.
{
    public GameObject Item;
    public GameObject Item_Icon;

    public int ItemId;
    public string ItemUIGroup;


    public ItemData(GameObject item, GameObject item_icon, int id, string Groupname) //각 아이템 정보 저장
    {
        Item = item;
        Item_Icon = item_icon;
        ItemId = id;
        ItemUIGroup = Groupname;
    }
}
