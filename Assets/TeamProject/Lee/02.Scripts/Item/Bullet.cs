using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IItem
{
    private GameObject Player;
    private Inventory inventory;
    private AudioClip AddBullet;

    private readonly int AddBullet_Num = 1; //�������� �Ծ��� �� �����Ǵ� ź�� ����

    private float prevTime;

    private readonly float CatchDelay = 0.05f;
    private void Awake()
    {
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();
        AddBullet = Resources.Load<AudioClip>("Sound/Item/AddBullet");
    }

    void OnEnable()
    {
        prevTime = Time.time;
    }

    public void CatchItem() //�÷��̾ ����ִ� gun�� �Ѿ˼��� 1�� ���� ��Ų��.
    {
        if (inventory.GetGun && inventory.CanReload && Time.time-prevTime > CatchDelay) //������ ȣ�� ����
        {
            prevTime = Time.time;
            InGameSoundManager.instance.ActiveSound(Player, AddBullet, 2.0f, true, false, false, 1, 1.0f);
            inventory.AddBullet(AddBullet_Num);
            Destroy(this.gameObject);         
        }
    }

    public void Use() 
    {
        //�κ��丮���� ����ϴ� ������ �ƴ� ���� x.
    }

    public void ItemUIOn() //�ش� �������� UI�� ����.
    {
        if(inventory.GetGun && inventory.CanReload)
            InGameUIManager.instance.SetPlayerUI_Text("�Ѿ� ���� [E]");
        else
            InGameUIManager.instance.SetPlayerUI_Text("������ ������ �����ϴ�.");
    }
}
