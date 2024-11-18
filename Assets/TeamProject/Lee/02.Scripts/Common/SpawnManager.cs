using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private List<Transform> BookHead_SpawnPoint;
    [SerializeField] private List<Transform> CandlePos;
    [SerializeField] private List<Transform> ItemPos;
    [SerializeField] private List<Transform> Gunpos;
    [SerializeField] private List<int> CandleRandomIdx;
    [SerializeField] private List<int> ItemRandomIdx;

    private Transform Player_Tr;
    private Transform LastCandle;

    private AudioClip Candle_Buzz;
    private AudioClip B_Idle;
    private AudioClip SpawnDemon_SFX;
    private AudioClip Demon_Sound_SFX;

    private Vector3 Fin_BookHeadSpawnPos = new Vector3(22.207f, -4.24f, -22.942f);

    private int Demon_IndexCounter = 0;
    private int BookHead_IndexCoundter = 0;
    private readonly string RespawnObj = "Respawn";
    private readonly string CandlePosObj = "CandlePosition";
    private readonly string ItemPosObj = "ItemCreatePos";
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);

        Player_Tr = GameObject.FindWithTag("Player").transform;
        LastCandle = GameObject.Find("Basement").transform.GetChild(6).GetComponent<Transform>();

        Candle_Buzz = Resources.Load<AudioClip>("Sound/Item/CandleBuzz");
        B_Idle = Resources.Load<AudioClip>("Sound/BookHead/B_Idle");
        SpawnDemon_SFX = Resources.Load<AudioClip>("Sound/Demon/Demon_SpwanSound");
        Demon_Sound_SFX = Resources.Load<AudioClip>("Sound/Demon/DemonBgSound");


        LastCandle.gameObject.SetActive(false);


        ListSetting();
    }

    private void ListSetting()
    {
        GetPoint(BookHead_SpawnPoint, RespawnObj);
        GetPoint(CandlePos, CandlePosObj);
        GetPoint(ItemPos, ItemPosObj);

        for (int i = 0; i < ItemPos.Count; i++)
            ItemRandomIdx.Add(i);
        for (int i = 0; i < CandlePos.Count; i++)
            CandleRandomIdx.Add(i);
    }
    private void GetPoint(List<Transform> PosList, string GetPosName) //Pos리스트 가저오기.
    {
        var spawn = GameObject.Find(GetPosName).transform;
        if (spawn != null)
        {
            foreach (Transform pos in spawn)
                PosList.Add(pos);
        }
    }

    public void FarRespawnSetup(List<Transform> SpawnPoint, GameObject RespawnObj, Transform Standard) //먼 리스폰 위치 찾는 로직
    {
        float Respawn_Dist = (SpawnPoint[0].position - Standard.position).magnitude;
        Vector3 Respawn_Pos = SpawnPoint[0].position;
        foreach (Transform point in SpawnPoint)
        {
            float Dist = (point.position - Standard.position).magnitude;
            if (Dist > Respawn_Dist)
                Respawn_Pos = point.position;
        }

        RespawnObj.transform.position = Respawn_Pos;
    }

    public void SetActiveDemonTrue(GameObject candlepos) //촛불이 꺼지면 데몬 함수 호출
    {
        if (candlepos != null)
        {
            GameObject demon = Pulling_Manger.instance.GetObject(0);
            Transform pos = candlepos.transform.parent.GetChild(0).transform;
            demon.transform.position = pos.position;

            EnemyFlashDamage enemyFlashDamage = demon.GetComponent<EnemyFlashDamage>();
            enemyFlashDamage.Demon_Counter = Demon_IndexCounter;

            Enemy enemy = demon.GetComponent<Enemy>();
            enemy.Demon_Counter = Demon_IndexCounter;

            Demon_Sound_SFX.name = $"DemonBgSound_{Demon_IndexCounter}";
            Demon_IndexCounter++;

            demon.SetActive(true);

            InGameSoundManager.instance.ActiveSound(demon, SpawnDemon_SFX, 25, true, false, false, 1);
            InGameSoundManager.instance.ActiveSound(demon, Demon_Sound_SFX, 10, true, true, true, 1);
        }
    }

    public void SetActiveBookHead_Final() //촛불 6개 꺼지면 호출
    {
        GameObject bookhead = Pulling_Manger.instance.GetObject(1);
        bookhead.transform.position = Fin_BookHeadSpawnPos;

        BookHeadHealth health = bookhead.GetComponent<BookHeadHealth>();
        BookHeadAttack attack = bookhead.GetComponent<BookHeadAttack>();

        B_Idle.name = $"B_Idle{BookHead_IndexCoundter}";

        health.BookHeadClip_Idx = BookHead_IndexCoundter;
        attack.BookHeadClip_Idx = BookHead_IndexCoundter;

        bookhead.SetActive(true);
        LastCandle.gameObject.SetActive(true);

        InGameSoundManager.instance.ActiveSound(bookhead, B_Idle, 8.0f, true, true, true, 1);
        BookHead_IndexCoundter++;
    }

    public void SetActiveBookHead() //미션시작에 호출
    {
        GameObject bookhead = Pulling_Manger.instance.GetObject(1);
        if (bookhead != null)
            StartCoroutine(RespawnWait(10f, BookHead_SpawnPoint, bookhead, Player_Tr));
    }

    public void BookHeadRespawn(GameObject RespawnObj) //북헤드가 플레이어에게 죽었을 때 호출
    {
        StartCoroutine(RespawnWait(10f, BookHead_SpawnPoint, RespawnObj, Player_Tr));
    }

    IEnumerator RespawnWait(float Delay, List<Transform> SpawnPoint, GameObject RespawnObj, Transform Standard)
    {
        yield return new WaitForSeconds(Delay);
        FarRespawnSetup(SpawnPoint, RespawnObj, Standard);

        BookHeadHealth health = RespawnObj.GetComponent<BookHeadHealth>();
        BookHeadAttack attack = RespawnObj.GetComponent<BookHeadAttack>();

        B_Idle.name = $"B_Idle{BookHead_IndexCoundter}";

        health.BookHeadClip_Idx = BookHead_IndexCoundter;
        attack.BookHeadClip_Idx = BookHead_IndexCoundter;

        RespawnObj.SetActive(true);

        InGameSoundManager.instance.ActiveSound(RespawnObj, B_Idle, 8.0f, true, true, true, 1);
        BookHead_IndexCoundter++;
    }


    public void SetActiveTrueCandel() //미션시작시 호출
    {

        foreach (GameObject candle in Pulling_Manger.instance.Data[2].Pool_List)
        {
            int idx = GetRandomIdx(CandleRandomIdx);
            candle.transform.parent = CandlePos[idx];
            candle.transform.position = CandlePos[idx].position;
            Candle_Buzz.name = $"CandleBuzz_{idx}";
            candle.SetActive(true);
            InGameSoundManager.instance.ActiveSound(candle, Candle_Buzz, 2.5f, true, true, false, 1);
        }

        GameObject candleGroup = GameObject.Find(Pulling_Manger.instance.Data[2].GroupName);
        Destroy(candleGroup);
    }
    public void SetActiveTrueItem()
    {
        foreach (GameObject item in Pulling_Manger.instance.Data[3].Pool_List) //아이템 리스트는 3 ~ 7번 전부 동일 
        {
            int idx = GetRandomIdx(ItemRandomIdx);
            item.transform.parent = ItemPos[idx];
            item.transform.position = ItemPos[idx].position;
            item.SetActive(true);
        }
        for (int i = 3; i < 7; i++)
        {
            GameObject itemGroup = GameObject.Find(Pulling_Manger.instance.Data[i].GroupName);
            Destroy(itemGroup);
        }
    }
    private int GetRandomIdx(List<int> RandomIdx) // 리스트에서 랜덤값 뽑기.
    {
        int i = Random.Range(0, RandomIdx.Count);
        int idx = RandomIdx[i];
        RandomIdx.RemoveAt(i);
        return idx;
    }
}
