using System.Collections;
using UnityEngine;

public class CamerRay : MonoBehaviour
{
    private Transform Camera_Tr;
    private InventoryUpdate inventoryUpdate;

    private readonly float raysize = 3.5f;

    private int CandleLayer;
    private int ItemLayer;

    private bool isAction;
    public bool IsAction
    {
        get { return isAction; }
        set { isAction = value; }
    }
    private bool isCatch;
    public bool IsCatch
    {
        get { return isCatch; }
        set { isCatch = value; }
    }
    private bool useFalse = false;
    public bool UseFalse
    {
        get { return useFalse; }
        set
        {
            useFalse = value;
            StartCoroutine(UseFalseUpdate());
        }
    }

    void Awake()
    {
        Camera_Tr = transform;
        inventoryUpdate = Camera_Tr.parent.GetComponent<InventoryUpdate>();   

        ItemLayer = 1 << 8;
        CandleLayer = (1 << 10) | (1 << 12) | (1 << 13);
    }

    void Update()
    {
        SearchItem();
    }

    private void SearchItem()
    {
        Ray ray = new Ray(Camera_Tr.position, Camera_Tr.forward);
        RaycastHit hit;

        if (Physics.Raycast(Camera_Tr.position, Camera_Tr.forward, out hit, raysize, ItemLayer)) //아이템 레이어만 감지 
        {
            hit.collider.gameObject.SendMessage("ItemUIOn"); //레이 맞을때 UI
            UseFalse = false;

            if (IsCatch)
            {
                hit.collider.gameObject.SendMessage("CatchItem");
                inventoryUpdate.InventorySetup();
            }
        }
        else if (Physics.Raycast(Camera_Tr.position, Camera_Tr.forward, out hit, raysize, CandleLayer))
        {
            hit.collider.gameObject.SendMessage("ItemUIOn"); //레이 맞을때 UI
            UseFalse = false;

            if (IsAction)
            {
                hit.collider.gameObject.SendMessage("CatchItem");
                inventoryUpdate.InventorySetup();
            }
        }
        else if(UseFalse)
        {
            //아이템 사용 실패시 UI를 끄지 않기 위한 조건문 (끄는 걸 방지하려고 선언만 해둠.)
        }
        else
        {
            InGameUIManager.instance.ActivePlayerUI_Text(false);
        }
    }

    private IEnumerator UseFalseUpdate()
    {
        yield return new WaitForSeconds(1.0f);
        UseFalse = false;
    }
}
