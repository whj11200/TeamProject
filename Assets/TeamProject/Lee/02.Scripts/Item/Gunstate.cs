using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunstate : MonoBehaviour, IItem
{
    private Inventory inventory;
    private ItemData GunData;
    private GameObject Player;
    private Transform Camera_Tr;

    private AudioSource ShotSource;
    private AudioClip Shot;
    private AudioClip EmptyShot;

    private float prevTime;

    private readonly float FireDist = 100.0f;
    private readonly float CatchDelay = 0.05f;
    private readonly float UseDelay = 0.5f;
    private readonly float Damage = 10.0f;
    private readonly float fireOffset = 0.5f;

    [SerializeField]private int initBullet = 1;
    public int InitBullet
    {
        get { return initBullet; }
        set
        {
            initBullet -= value;
            UpdateState();
        }
    }
    void Awake()
    {        
        Player = GameObject.Find("Player");
        inventory = Player.GetComponent<Inventory>();
        Camera_Tr = Player.transform.GetChild(0).GetComponent<Transform>();

        Shot = Resources.Load<AudioClip>("Sound/Item/Shot");
        EmptyShot = Resources.Load<AudioClip>("Sound/Item/EmptyShot");

        GunData = new ItemData(gameObject, null, 2, "GunImg_Group");
    }
    void OnEnable()
    {
        UpdateState();
        prevTime = Time.time;
    }

    public void CatchItem()
    {
        if (Time.time - prevTime > CatchDelay && inventory.CanGetItem) //여러번 호출 되는 것 방지.
        {
            prevTime = Time.time;
            inventory.GetItem(GunData);
            InGameSoundManager.instance.ActiveSound(gameObject, Shot, 2.0f, false, false, true, 1);
            ShotSource = InGameSoundManager.instance.Data["Shot"].SoundBox_AudioSource;
        }
    }

    public void Use()
    {
        if (InitBullet > 0 && Time.time - prevTime > UseDelay)
        {
            Ray ray = new Ray(Camera_Tr.position + (Camera_Tr.forward * fireOffset), Camera_Tr.forward);
            RaycastHit hit;
            StartCoroutine(PlaySound(0.3f));
            if (Physics.Raycast(ray, out hit, FireDist, 1 << 7))
            {
                object[] param = new object[2];
                param[0] = hit.point;
                param[1] = Damage;
                if (hit.collider.CompareTag("BookHead"))
                {
                    hit.collider.transform.SendMessage("OnDamage", param, SendMessageOptions.DontRequireReceiver);
                }
            }
            prevTime = Time.time;
        }
        else if (InitBullet <= 0 && Time.time - prevTime > UseDelay)
        {
            ShotSource.Play();
            prevTime = Time.time;
        }
    }

    public void ItemUIOn()
    {
        if(inventory.CanGetItem)
            InGameUIManager.instance.SetPlayerUI_Text("공기총 [E]");
        else
            InGameUIManager.instance.SetPlayerUI_Text("인벤토리가 꽉 찼습니다.");
    }

    private void UpdateState()
    {
        if (InitBullet <= 0)
        {
            inventory.CanReload = true;
            if(inventory.GetGun)
                ShotSource.clip = EmptyShot;
        }
        else if (InitBullet == 1)
        {
            inventory.CanReload = true;
            if (inventory.GetGun)
                ShotSource.clip = Shot;
        }
        else if (InitBullet > 1)
        {
            inventory.CanReload = false;
            if(inventory.GetGun)
                ShotSource.clip = Shot;
        }
    }
    public void AddBullet(int value)
    {
        InitBullet = -value;
    }

    private void DropItem()
    {
        InGameSoundManager.instance.EditSoundBox("Shot", false);
        InGameSoundManager.instance.Data.Remove("Shot");
        InGameUIManager.instance.OffItemIcon(GunData.Item_Icon, GunData.ItemUIGroup);
    }

    IEnumerator PlaySound(float wait)
    {
        ShotSource.Play();
        yield return new WaitForSeconds(wait);
        AddBullet(-1);
    }

    private void OnDisable() //슬롯이 꺼지면 업데이트
    {
        inventory.CanReload = false;
    }
}
