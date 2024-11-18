using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPack : MonoBehaviour, IItem
{
    private ItemData HealPackData;

    private GameObject Player;
    private Inventory inventory;
    private CamerRay camerRay;
    private PlayerHealth playerHealth;
    private AudioClip Heal;

    private float prevTime;

    private readonly float CatchDelay = 0.05f;
    private readonly float UseDelay = 0.2f;
    private readonly float Heal_Amount = 3.0f;

    private void Awake()
    {
        HealPackData = new ItemData(gameObject, null, 3, "HealPackImg_Group");
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();
        camerRay = Player.transform.GetChild(0).GetComponent<CamerRay>();
        playerHealth = Player.GetComponent<PlayerHealth>();

        Heal = Resources.Load<AudioClip>("Sound/Item/Heal");
    }
    private void OnEnable()
    { 
        prevTime = Time.time;
    }

    public void CatchItem()
    {
        if (Time.time - prevTime > CatchDelay && inventory.CanGetItem) //������ ȣ�� �Ǵ� �� ����.
        {
            prevTime = Time.time;
            inventory.GetItem(HealPackData);           
        }
    }

    public void Use()
    {
        if(playerHealth.health < 10 && Time.time - prevTime > UseDelay)
        {
            prevTime = Time.time;
            InGameSoundManager.instance.ActiveSound(Player, Heal, 2.0f, true, false, false, 1);
            playerHealth.AddHealth(Heal_Amount);
            InGameUIManager.instance.OffItemIcon(HealPackData.Item_Icon,  HealPackData.ItemUIGroup);
            Destroy(this.gameObject);
            inventory.CanGetItem = true;            
        }
        else if(playerHealth.health >= 10 && !camerRay.UseFalse)
        {
            camerRay.UseFalse = true;
            InGameUIManager.instance.SetPlayerUI_Text("ü���� ���� �ʾҽ��ϴ�.");
        }
    }

    public void ItemUIOn()
    {
        if(inventory.CanGetItem)
            InGameUIManager.instance.SetPlayerUI_Text("���޻��� [E]");            
        else
            InGameUIManager.instance.SetPlayerUI_Text("�κ��丮�� �� á���ϴ�.");
    }

    private void DropItem()
    {
        InGameUIManager.instance.OffItemIcon(HealPackData.Item_Icon, HealPackData.ItemUIGroup);
    }
}
