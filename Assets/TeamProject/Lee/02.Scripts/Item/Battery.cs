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
            InGameUIManager.instance.SetPlayerUI_Text("���͸� ��ü [E]");
        else
        {
            InGameUIManager.instance.SetPlayerUI_Text("���͸� ��ü�� �Ұ����մϴ�.");
        }
    }

    public void Use()
    {
        //�κ��丮���� ����ϴ� ������ �ƴ� ���� x.
    }
}
