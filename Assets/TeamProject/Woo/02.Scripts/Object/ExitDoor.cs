using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitDoor : MonoBehaviour, IItem
{
    MeshCollider DoorCol;
    AudioClip DoorOpen;
    AudioClip EndingSound;
    void Start()
    {
        DoorOpen = Resources.Load<AudioClip>("Sound/Object/EndDoorOpen");
        EndingSound = Resources.Load<AudioClip>("Sound/BG/InGameBG");
        DoorCol = GetComponent<MeshCollider>();
        DoorCol.enabled = false;
    }

    public void OpenCololider()
    {
        DoorCol.enabled = true;
    }
    public void CatchItem()
    {
        StartCoroutine(EndingimgStart());
        InGameSoundManager.instance.ActiveSound(gameObject, DoorOpen, 10, true, false, false, 1);
        InGameSoundManager.instance.EditSoundBox("InGameBG",false);
        InGameSoundManager.instance.Data.Remove("InGameBG");

    }
    IEnumerator EndingimgStart()
    {
        DoorCol.enabled = false;
        InGameUIManager.instance.EndingUI();
        yield return null;
    }
    public void Use()
    {
        //인벤토리에서 사용하는 아이템아님 구현 x
    }

    public void ItemUIOn()
    {
        InGameUIManager.instance.SetPlayerUI_Text("탈출하기 [G]");
    }
}
