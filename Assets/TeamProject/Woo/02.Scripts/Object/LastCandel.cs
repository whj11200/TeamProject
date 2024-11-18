using System.Collections;
using UnityEngine;

public class LastCandel : MonoBehaviour, IItem
{
    SpriteRenderer[] CandleFire;
    [SerializeField] ParticleSystem FireParticle;
    [SerializeField] Light[] Firelights;
    [SerializeField] ExitDoor exitDoor;
    Collider Candle_Collider;

    [SerializeField] AudioClip LastCandelDead;
    [SerializeField] AudioClip LastCandelDemon_Dead;
    [SerializeField] GameObject Player;

    [SerializeField] private float Timer;

    private readonly string[] LastCandleTalk = {
        "'누가 이런 장소에서 악마를 소환하고 있던거지..?'",
        "'괴물들의 비명소리가 들리는군.. 조금 있으면 이곳의 모든 괴물이 사라지겠어..'",
        "'괴물들이 마지막 발악을 하는 건가.. 발자국 소리가 더욱 빨라졌어. 조심해야겠군.'"
    };
    void Start()
    {
        LastCandelDead = Resources.Load<AudioClip>("Sound/Object/LastCandel");
        LastCandelDemon_Dead = Resources.Load<AudioClip>("Sound/Object/LastCandel_AllDead");

        exitDoor = GameObject.Find("MainDoor").GetComponent<ExitDoor>();
        FireParticle = transform.GetChild(3).GetComponent<ParticleSystem>();
        Firelights = GetComponentsInChildren<Light>();
        CandleFire = GetComponentsInChildren<SpriteRenderer>();
        Candle_Collider = GetComponent<Collider>();
        Player = GameObject.Find("Player").gameObject;

        Timer = 60;
    }
    IEnumerator StartTimer()
    {
        while (Timer > 0) // Timer가 0보다 큰 동안 반복
        {
            yield return new WaitForSeconds(1.0f);
            Timer -= 1.0f;
            InGameUIManager.instance.SetTimer(Timer);
            
            if(Timer <= 0)
            {
                yield return new WaitForSeconds(0.5f);
                GameManager.G_instance.CtrlLastLight(true);
                exitDoor.OpenCololider();
            }
            Debug.Log(Timer);
        }
    }

    public void ItemUIOn()
    {
        InGameUIManager.instance.SetPlayerUI_Text("양초끄기 [G]");
    }

    public void CatchItem()
    {
        foreach (var candle in CandleFire)
            candle.enabled = false;

        Candle_Collider.enabled = false;
        FireParticle.Stop();
        foreach (var candle in Firelights)
        {
            candle.enabled = false;
        }
        InGameUIManager.instance.OnMisson("제한 시간까지 살아남으세요.");
        InGameUIManager.instance.OnTimer(true);
        InGameUIManager.instance.AutoSetTalk(LastCandleTalk);

        InGameSoundManager.instance.ActiveSound(Player, LastCandelDemon_Dead, 6, true, false, false, 1);
        InGameSoundManager.instance.ActiveSound(Player, LastCandelDead, 8, true, false, false, 1);

        StartCoroutine(StartTimer());
    }

    public void Use()
    {
       //인벤토리에서 사용하는 오브젝트 아님 구현 x
    }
}
