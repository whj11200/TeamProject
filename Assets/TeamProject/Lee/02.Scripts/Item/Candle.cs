using UnityEngine;

public class Candle : MonoBehaviour,IItem
{
    SpriteRenderer[] CandleFire;
    Collider Candle_Collider;

    GameObject MainCamera;
    AudioClip BlowCandle;

    private float prevTime;

    private readonly float CatchDelay = 0.05f;

    private void Awake()
    {
        CandleFire = GetComponentsInChildren<SpriteRenderer>();
        Candle_Collider = GetComponent<Collider>();

        MainCamera = GameObject.Find("MainCamera");

        BlowCandle = Resources.Load<AudioClip>("Sound/Item/BlowCandle");
    }

    void OnEnable()
    {
        prevTime = Time.time;
    }

    public void CatchItem()
    {
        if (Time.time - prevTime > CatchDelay) //������ ȣ�� �Ǵ� �� ����.
        {
            prevTime = Time.time;
            InGameSoundManager.instance.ActiveSound(MainCamera, BlowCandle, 2.0f, true, false, false, 1, 0.5f);

            foreach (var candle in CandleFire)
            candle.enabled = false;

            Candle_Collider.enabled = false;

            InGameSoundManager.instance.EditSoundBox($"CandleBuzz_{FindSoundBoxKey()}", false); //�к��� ������ ���� �ڽ� ��Ȱ��ȭ
            GameManager.G_instance.LastOffCandle = gameObject;
            GameManager.G_instance.CanndleCounter(1);
        }
    }

    public void Use() { /* Candle�� �κ��丮�� ���� �����Ƿ� ���� �ص�. */ }
    public void ItemUIOn()
    {
        InGameUIManager.instance.SetPlayerUI_Text("���ʲ��� [G]");
    }

    private int FindSoundBoxKey()
    {
        GameObject CandleSpawnpoint = this.gameObject.transform.parent.gameObject;
        int SoundBoxKeyidx = CandleSpawnpoint.transform.GetSiblingIndex();
        return SoundBoxKeyidx;
    }
}
