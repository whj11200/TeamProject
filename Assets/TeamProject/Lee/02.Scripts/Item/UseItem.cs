using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    [SerializeField] private List<GameObject> ItemSlots = new List<GameObject>();
    private Transform Camera_Tr;

    [SerializeField] private int inventory_Idx;
    public int Inventory_Idx
    {
        get { return inventory_Idx; }
        set { inventory_Idx = value; }
    }

    [SerializeField] private bool isUse; //���콺 ���� ��ư�� ������ �� ������Ʈ
    public bool IsUse
    {
        get { return isUse; }
        set { isUse = value; }
    }
    void Awake()
    {
        Camera_Tr = transform.GetChild(0).transform;

        for (int i = 0; i < Camera_Tr.childCount; i++)
        {
            ItemSlots.Add(Camera_Tr.GetChild(i).gameObject);
        }
        ItemSlots.RemoveAt(0);
    }
    void Update()
    {
        if (GameManager.G_instance.isGameover) return;

        UsingItem();
    }
    private void UsingItem() //��� ������ ��� �Լ��� �����.
    {
        if (IsUse && ItemSlots[Inventory_Idx].transform.childCount != 0) //�� �Լ� �ϳ��� ��� ������ �۵� ����.
            ItemSlots[Inventory_Idx].transform.GetChild(0).SendMessage("Use", Inventory_Idx);
    }
}
