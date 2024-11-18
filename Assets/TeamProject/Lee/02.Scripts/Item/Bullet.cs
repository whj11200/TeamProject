using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IItem
{
    private GameObject Player;
    private Inventory inventory;
    private AudioClip AddBullet;

    private readonly int AddBullet_Num = 1; //아이템을 먹었을 때 장전되는 탄약 개수

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

    public void CatchItem() //플레이어가 들고있는 gun에 총알수를 1개 증가 시킨다.
    {
        if (inventory.GetGun && inventory.CanReload && Time.time-prevTime > CatchDelay) //여러번 호출 방지
        {
            prevTime = Time.time;
            InGameSoundManager.instance.ActiveSound(Player, AddBullet, 2.0f, true, false, false, 1, 1.0f);
            inventory.AddBullet(AddBullet_Num);
            Destroy(this.gameObject);         
        }
    }

    public void Use() 
    {
        //인벤토리에서 사용하는 아이템 아님 구현 x.
    }

    public void ItemUIOn() //해당 아이템의 UI를 띄운다.
    {
        if(inventory.GetGun && inventory.CanReload)
            InGameUIManager.instance.SetPlayerUI_Text("총알 장전 [E]");
        else
            InGameUIManager.instance.SetPlayerUI_Text("장전할 공간이 없습니다.");
    }
}
