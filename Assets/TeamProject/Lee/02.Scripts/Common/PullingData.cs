using System.Collections.Generic;
using UnityEngine;

public class PullingData
{//Ǯ�� �Ŵ������� ������ ���� �޾ƿͼ� ����
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
