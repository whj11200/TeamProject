using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Pulling_Manger : MonoBehaviour
{//풀링 + 딕셔너리 (풀링 리스트, 프리팹, 그룹이름, 오브젝트이름, 풀링개수 데이터 저장), 생성한 오브젝트들은 false, 스폰 매니저에서 초반 지역 통과하면 재배치, 리스트는 싱글턴으로 data에서 가져가기.
    public static Pulling_Manger instance; // Singleton 패턴을 위해 static으로 변경
    public Dictionary<int, PullingData> Data = new Dictionary<int, PullingData>();
    //0 : 데몬 / 1 : 북헤드 / 2 : 촛불 / 3 : 배터리 / 4 : 힐팩 / 5 : 총 / 6 : 탄약 / 7 : 손전등 퀵슬롯 이미지 / 8 : 총 퀵슬롯 이미지 / 9 : 힐팩 퀵슬롯 이미지 

    [SerializeField] List<GameObject> demonPool;
    [SerializeField] List<GameObject> bookHeadPool;
    [SerializeField] List<GameObject> candlePool;
    [SerializeField] List<GameObject> itemPool;
    [SerializeField] List<GameObject> FlashLight_ImgPool;
    [SerializeField] List<GameObject> Gun_ImgPool;
    [SerializeField] List<GameObject> HealPack_ImgPool;
    [SerializeField] List<GameObject> SoundBox_Pool;

    [SerializeField] GameObject DemonPrefab;
    [SerializeField] GameObject BookHead_Prefab;
    [SerializeField] GameObject Candle_Prefab;
    [SerializeField] GameObject Battery_Prefab;
    [SerializeField] GameObject HealPack_Prefab;
    [SerializeField] GameObject Gun_Prefab;
    [SerializeField] GameObject Bullet_Prefab;
    [SerializeField] GameObject FlashLight_Img;
    [SerializeField] GameObject Gun_Img;
    [SerializeField] GameObject HealPack_Img;
    [SerializeField] GameObject SoundBox_Prefab;

    private readonly int Demon_Max = 3;
    private readonly int BookHead_Max = 2;
    private readonly int Candle_Max = 6;
    private readonly int Battery_Max = 6;
    private readonly int HealPack_Max = 4;
    private readonly int Gun_Max = 1;
    private readonly int Bullet_Max = 4;
    private readonly int FlashLightImg_Max = 1;
    private readonly int GunImg_Max = 1;
    private readonly int HealPackImg_Max = 4;
    private readonly int SoundBox_Max = 20;

    private readonly string Demon_Group = "DemonGroup";
    private readonly string Demon_Obj = "Demon_M";
    private readonly string BookHead_Group = "BookHeadGroup";
    private readonly string BookHead_Obj = "BookHead";
    private readonly string Candle_Group = "CandleGroup";
    private readonly string Candle_Obj = "Candle";
    private readonly string Battery_Group = "BatteryGroup";
    private readonly string Battery_Obj = "Battery";
    private readonly string HealPack_Group = "HealpackGroup";
    private readonly string HealPack_Obj = "Healpack";
    private readonly string Gun_Group = "GunGroup";
    private readonly string Gun_Obj = "Gun";
    private readonly string Bullet_Group = "BulletGroup";
    private readonly string Bullet_Obj = "Bullet";
    private readonly string FlashLightImg_Group = "FlashLightImg_Group";
    private readonly string FlashLightImg_Obj = "FlashLight_Img";
    private readonly string GunImg_Group = "GunImg_Group";
    private readonly string GunImg_Obj = "Gun_Img";
    private readonly string HealPackImg_Group = "HealPackImg_Group";
    private readonly string HealPackImg_Obj = "HealPack_Img";
    private readonly string SoundBox_Group = "SoundBox_Group";
    private readonly string SoundBox_Obj = "SoundBox";
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DemonPrefab = Resources.Load<GameObject>(Demon_Obj);
        BookHead_Prefab = Resources.Load<GameObject>(BookHead_Obj);
        Candle_Prefab = Resources.Load<GameObject>(Candle_Obj);
        Battery_Prefab = Resources.Load<GameObject>(Battery_Obj);
        HealPack_Prefab = Resources.Load<GameObject>(HealPack_Obj);
        Gun_Prefab = Resources.Load<GameObject>(Gun_Obj);
        Bullet_Prefab = Resources.Load<GameObject>(Bullet_Obj);
        FlashLight_Img = Resources.Load<GameObject>(FlashLightImg_Obj);
        Gun_Img = Resources.Load<GameObject>(GunImg_Obj);
        HealPack_Img = Resources.Load<GameObject>(HealPackImg_Obj);
        SoundBox_Prefab = Resources.Load<GameObject>(SoundBox_Obj);

        Data.Add(0, new PullingData(demonPool, DemonPrefab, Demon_Group, Demon_Obj, Demon_Max));
        Data.Add(1, new PullingData(bookHeadPool, BookHead_Prefab, BookHead_Group, BookHead_Obj, BookHead_Max));
        Data.Add(2, new PullingData(candlePool, Candle_Prefab, Candle_Group, Candle_Obj, Candle_Max));
        Data.Add(3, new PullingData(itemPool, Battery_Prefab, Battery_Group, Battery_Obj, Battery_Max));
        Data.Add(4, new PullingData(itemPool, HealPack_Prefab, HealPack_Group, HealPack_Obj, HealPack_Max));
        Data.Add(5, new PullingData(itemPool, Gun_Prefab, Gun_Group, Gun_Obj, Gun_Max));
        Data.Add(6, new PullingData(itemPool, Bullet_Prefab, Bullet_Group, Bullet_Obj, Bullet_Max));
        Data.Add(7, new PullingData(FlashLight_ImgPool, FlashLight_Img, FlashLightImg_Group, FlashLightImg_Obj, FlashLightImg_Max));
        Data.Add(8, new PullingData(Gun_ImgPool, Gun_Img, GunImg_Group, GunImg_Obj, GunImg_Max));
        Data.Add(9, new PullingData(HealPack_ImgPool, HealPack_Img, HealPackImg_Group, HealPackImg_Obj, HealPackImg_Max));
        Data.Add(10, new PullingData(SoundBox_Pool, SoundBox_Prefab, SoundBox_Group, SoundBox_Obj, SoundBox_Max));

        for (int i = 0; i < Data.Count; i++)
            Pooling(i, Data);
    }

    private void Pooling(int key, Dictionary<int, PullingData> data) //딕셔너리에 저장한 데이터로 오브젝트 풀링
    {
        GameObject Group = new GameObject(data[key].GroupName);
        for (int i = 0; i < data[key].MaxPull; i++)
        {
            var obj = Instantiate(data[key].Prefab, Group.transform);
            obj.transform.position = new Vector3(0f, -30f, 0f);
            obj.transform.rotation = Quaternion.identity;
            obj.name = $"{data[key].ObjName}";
            data[key].Pool_List.Add(obj);
            obj.SetActive(false);
        }
    }

    public GameObject GetObject(int key)
    {
        foreach (var obj in Data[key].Pool_List)
        {
            if (!obj.activeSelf) // 비활성화된 오브젝트 찾기
                return obj; // 비활성화된 오브젝트 반환
        }
        return null; // 모든 오브젝트가 활성화 상태라면 null 반환
    }
}
