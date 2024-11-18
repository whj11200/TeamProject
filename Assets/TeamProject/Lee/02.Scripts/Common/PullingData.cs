using System.Collections.Generic;
using UnityEngine;

public class PullingData
{//풀링 매니저에서 데이터 값을 받아와서 저장
    public List<GameObject> Pool_List;
    public GameObject Prefab;
    public string GroupName;
    public string ObjName;
    public int MaxPull;

    public PullingData(List<GameObject> pool, GameObject prefab, string Groupname, string Objname, int maxpull)
    {
        Pool_List = pool;
        Prefab = prefab;
        GroupName = Groupname;
        ObjName = Objname;
        MaxPull = maxpull;
    }
}
