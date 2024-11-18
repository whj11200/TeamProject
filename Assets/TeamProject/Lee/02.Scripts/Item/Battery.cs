using UnityEngine;

public class Battery : MonoBehaviour, IItem
{
    private GameObject Player;
    private Inventory inventory;
    private AudioClip Charge;

    private float prevTime;

    private readonly float CatchDelay = 0.05f;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();

        Charge = Resources.Load<AudioClip>("Sound/Item/Charge");
    }

    void OnEnable()
    {
        prevTime = Time.time;
    }

    public void CatchItem()
    {
        if (inventory.CanGetBattery && inventory.GetFlash && Time.time - prevTime > CatchDelay)
        {
            prevTime = Time.time;
            InGameSoundManager.instance.ActiveSound(Player, Charge, 2.0f, true, false, false, 1);
            inventory.AddBattery();           
            Destroy(gameObject);         
        }
    }

    public void ItemUIOn()
    {
        if(inventory.CanGetBattery && inventory.GetFlash)
            InGameUIManager.instance.SetPlayerUI_Text("배터리 교체 [E]");
        else
        {
            InGameUIManager.instance.SetPlayerUI_Text("배터리 교체가 불가능합니다.");
        }
    }

    public void Use()
    {
        //인벤토리에서 사용하는 아이템 아님 구현 x.
    }
}
