using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<GameObject> ItemSlots = new List<GameObject>(); //아이템 슬롯 오브젝트. 아이템 데이터 Id는 다음과 같다. (0:손전등, 2:공기총, 3:구급상자)
    
    private InventoryUpdate Slot_Idx;
    private Transform Camera_Tr;

    private float prevTime;
    private float delay = 0.1f;

    [SerializeField]private int GunIdx;
    [SerializeField]private int FlashIdx;
    [SerializeField]private int HealPackIdx;

    public bool CanGetItem = true;

    [SerializeField] private bool getFlash = false;
    public bool GetFlash
    {
        get { return getFlash; }
        set { getFlash = value; }
    }

    [SerializeField] private bool canGetBattery = false;
    public bool CanGetBattery
    {
        get { return canGetBattery; }
        set { canGetBattery = value; }
    }


    [SerializeField]private bool getGun = false;
    public bool GetGun
    {
        get { return getGun; }
        private set { getGun = value; }
    }

    [SerializeField]private bool canReload;
    public bool CanReload
    {
        get { return canReload; }
        set { canReload = value; }
    }

    [SerializeField] private bool isDrop;
    public bool IsDrop
    {
        get { return isDrop; }
        set
        {
            isDrop = value;
            if (IsDrop && ItemSlots[Slot_Idx.Inventory_Idx].transform.childCount != 0)
                DropItem();
        }
    }

    private void Awake()
    {
        Slot_Idx = GetComponent<InventoryUpdate>();
        Camera_Tr = transform.GetChild(0).transform;

        for (int i = 0; i < Camera_Tr.childCount; i++)
        {
            ItemSlots.Add(Camera_Tr.GetChild(i).gameObject);
        }
        ItemSlots.RemoveAt(0);

        prevTime = Time.time;
    }

    public void GetItem(ItemData data) //먹은 아이템 플레이어 손으로 이동, 퀵슬롯 이미지 띄우는 코드도 여기서 제작
    {
        switch (data.ItemId)
        {

            case 0: //손전등
                Transform hitObject_F = data.Item.transform;
                for (int i = 0; i < ItemSlots.Count; i++)
                {
                    if (ItemSlots[i].transform.GetComponentsInChildren<Transform>(true).Length - 1 == 0)
                    {
                        CanGetItem = true;
                        FlashIdx = i;
                        break;
                    }
                    if (i == ItemSlots.Count - 1)
                        CanGetItem = false;
                }

                GameObject flashIcon = Pulling_Manger.instance.GetObject(7);
                if (CanGetItem && flashIcon != null)
                {
                    data.Item_Icon = flashIcon;
                    
                    hitObject_F.SetParent(ItemSlots[FlashIdx].transform);
                    InGameUIManager.instance.OnItemIcon(flashIcon, FlashIdx);
                    GetFlash = true;

                    hitObject_F.localPosition = new Vector3(0, 0, 0.338f);
                    hitObject_F.localRotation = Quaternion.identity;
                    
                    InGameUIManager.instance.OnPlayerUI_Img(0);
                    InGameUIManager.instance.OnPlayerUI_Img(1);
                }
                break;

            case 2: //총
                Transform hitObject_G = data.Item.transform;
                for (int i = 0; i < ItemSlots.Count; i++)
                {

                    if (ItemSlots[i].transform.GetComponentsInChildren<Transform>(true).Length - 1 == 0)
                    {
                        CanGetItem = true;
                        GunIdx = i;
                        break;
                    }
                    if (i == ItemSlots.Count - 1)
                        CanGetItem = false;
                }

                GameObject gunIcon = Pulling_Manger.instance.GetObject(8);
                if (CanGetItem && gunIcon != null)
                {
                    data.Item_Icon = gunIcon;
                    hitObject_G.SetParent(ItemSlots[GunIdx].transform);
                    InGameUIManager.instance.OnItemIcon(gunIcon, GunIdx);
                    GetGun = true;

                    hitObject_G.localPosition = new Vector3(0f, 0f, 0.5f);
                    hitObject_G.localRotation = Quaternion.identity;
                }
                break;

            case 3: //힐팩
                Transform hitObject_H = data.Item.transform; ;
                for (int i = 0; i < ItemSlots.Count; i++)
                {
                    if (ItemSlots[i].transform.GetComponentsInChildren<Transform>(true).Length - 1 == 0)
                    {
                        CanGetItem = true;
                        HealPackIdx = i;
                        break;
                    }
                    if (i == ItemSlots.Count - 1)
                        CanGetItem = false;
                }

                GameObject healpackIcon = Pulling_Manger.instance.GetObject(9);
                if (CanGetItem && healpackIcon != null)
                {                    
                    data.Item_Icon = healpackIcon;
                    hitObject_H.SetParent(ItemSlots[HealPackIdx].transform);
                    InGameUIManager.instance.OnItemIcon(healpackIcon, HealPackIdx);

                    hitObject_H.localPosition = new Vector3(0.1f, -0.5f, 0.7f);
                    hitObject_H.localRotation = Quaternion.identity;
                }
                break;
        }
    }

    public void DropItem() //아이템을 땅에버린다. 땅 위치값 필요.
    {
        if (Time.time - prevTime > delay)
        {
            GameObject item = ItemSlots[Slot_Idx.Inventory_Idx].transform.GetChild(0).gameObject;

            item.transform.position = transform.position; //플레이어 발 위치로 아이템 이동
            item.transform.parent = null; // 아이템을 월드 공간으로 떨어뜨린다.
            item.SendMessage("DropItem");

            CanGetItem = true;
            prevTime = Time.time;
        }
    }

    public void AddBullet(int amount)
    {
        Gunstate gun = ItemSlots[Slot_Idx.Inventory_Idx].transform.GetChild(0).GetComponent<Gunstate>();
        gun.AddBullet(amount);
    }

    public void AddBattery()
    {
        FlashLight flashLight = ItemSlots[Slot_Idx.Inventory_Idx].transform.GetChild(0).GetComponent<FlashLight>();
        flashLight.CollectBattery();
    }
}
