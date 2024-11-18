using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUpdate : MonoBehaviour
{
    [SerializeField]private List<GameObject> ItemSlots = new List<GameObject>();
    private Transform Camera_Tr;

    [SerializeField]private int inventory_Idx; // 활성화 할 인벤토리를 결정한다. 0~3 사이의 값으로 변함.
    public int Inventory_Idx // 0~3의 해당하는 자식 오브젝트를 제외하고 전부 비활성화 한다.
    {
        get { return inventory_Idx; }
        set 
        {
            inventory_Idx = (int)value;
            InventorySetup();
        }
    }

    private void Awake()
    {
        Camera_Tr = transform.GetChild(0).transform;

        for (int i = 0; i < Camera_Tr.childCount; i++)
        {
            ItemSlots.Add(Camera_Tr.GetChild(i).gameObject);
        }
        ItemSlots.RemoveAt(0);
    }

    public void InventorySetup()
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            if(i == Inventory_Idx && ItemSlots[i].transform.childCount != 0)
                ItemSlots[i].transform.GetChild(0).gameObject.SetActive(true);
            else if (i != Inventory_Idx && ItemSlots[i].transform.childCount != 0)
                ItemSlots[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
