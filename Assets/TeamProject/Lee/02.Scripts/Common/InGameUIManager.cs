using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager instance;

    [SerializeField] private List<Image> ItemImages; //아이템 UI 이미지 목록 (0:손전등, 1:손전등 배경)
    [SerializeField] private List<RectTransform> ItemSlots_UI = new List<RectTransform>(); //아이템 슬롯 UI 리스트.

    [SerializeField] private GameObject Player;
    [SerializeField] private BoxCollider Flash_BoxCol;
    [SerializeField] private Transform PlayerUI_TextObj;
    [SerializeField] private Transform PlayerUI_Image;
    [SerializeField] private Transform PlayCanvaus;
    [SerializeField] private Transform ItemSlot;
    [SerializeField] private Transform Playerui;

    [SerializeField] private RectTransform Inventroy_object;
    [SerializeField] private RectTransform SlotsUI_Tr;
    [SerializeField] private RectTransform TalkBox;
    [SerializeField] private RectTransform S_Option_Bg;
    [SerializeField] private RectTransform ItemImage;
    [SerializeField] private RectTransform Aim;

    [SerializeField] private Text PlayerUI_Text;
    [SerializeField] private Text Misson_Text;
    [SerializeField] private Text Timer_Text;
    [SerializeField] private Text Talk_Text;
    [SerializeField] private Text endingText;

    [SerializeField] private Image Damage_Image;
    [SerializeField] private Image endingimg;

    [SerializeField] private AudioClip KeyboredSound;
    [SerializeField] private AudioClip Ui_Button_Sound;

    private readonly float TalkPading = 20.0f;
    private readonly string PlayerUI_Obj = "PlayerUi";
    public bool OnOptionsPlayerstop = false;
    string fulltext = "문을 열고 탈출에 성공했고\n 뒤돌아 봤을땐 건물은 사라져 있었다...";
    string playertag = "Player";
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(instance);

        KeyboredSound = Resources.Load<AudioClip>("Sound/Object/KeybordSound");
        Ui_Button_Sound = Resources.Load<AudioClip>("Sound/Button/UiButton");

        PlayCanvaus = GameObject.Find("PlayCanvas").transform;
        Playerui = GameObject.Find(PlayerUI_Obj).transform;
        Player = GameObject.Find("Player").gameObject;

        PlayerUI_TextObj = Playerui.transform.GetChild(1).GetChild(0);       
        PlayerUI_Image = Playerui.transform.GetChild(2).transform;
        SlotsUI_Tr = Playerui.transform.GetChild(3).GetChild(0).GetComponent<RectTransform>();
        S_Option_Bg = PlayCanvaus.transform.GetChild(5).GetComponent<RectTransform>();
        TalkBox = GameObject.Find("Talk").transform.GetComponent<RectTransform>();
        Inventroy_object = Playerui.transform.GetChild(3).GetComponent<RectTransform>();

        PlayerUI_Text = PlayerUI_TextObj.GetComponent<Text>();
        Misson_Text = GameObject.Find("Misson").transform.GetChild(0).GetComponent<Text>();
        Timer_Text = GameObject.Find("Timer").transform.GetChild(0).GetComponent<Text>();
        Talk_Text = TalkBox.transform.GetChild(0).GetComponent<Text>();
        endingText = PlayCanvaus.transform.GetChild(6).GetChild(0).GetComponent<Text>();

        ItemSlot = Inventroy_object.parent.parent.GetChild(0).transform;

        Damage_Image = PlayCanvaus.transform.GetChild(0).GetComponent<Image>();
        endingimg = PlayCanvaus.transform.GetChild(6).GetComponent<Image>();

        Flash_BoxCol = GameObject.Find("flashlight").transform.GetChild(0).GetComponent<BoxCollider>();
        ItemImage = Playerui.transform.GetChild(2).GetComponent<RectTransform>();
        Aim = Playerui.transform.GetChild(0).GetComponent<RectTransform>(); 

        Misson_Text.gameObject.SetActive(false);
        Timer_Text.gameObject.SetActive(false);
        S_Option_Bg.gameObject.SetActive(false);

        endingimg.enabled = false;
        endingText.enabled = false;

        for (int i = 0; i < PlayerUI_Image.childCount; i++)
        {
            ItemImages.Add(PlayerUI_Image.GetChild(i).GetComponent<Image>());
            ItemImages[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < SlotsUI_Tr.childCount; i++)
        {
            ItemSlots_UI.Add(SlotsUI_Tr.GetChild(i).GetComponent<RectTransform>());
        }
    }

    public void SetPlayerUI_Text(string txt)
    {
        PlayerUI_Text.text = txt;
        PlayerUI_TextObj.gameObject.SetActive(true);
    }

    public void EndingUI()
    {
       
        GameManager.G_instance.isGameover = true;

        for (int i = 0; i < 5; i++)
        {
            PlayCanvaus.transform.GetChild(i).gameObject.SetActive(false);
        }
        ActivePlayerUI_Text(false);
        OnTimer(false);
        StartCoroutine(EndingStartImage());

    }
    public void OpenDoorONUi()
    {
       
        Playerui.gameObject.SetActive(true);
        PlayCanvaus.gameObject.SetActive(true);
    }
    public void OpenDoorOffUi()
    {
        Playerui.gameObject.SetActive(false);
        PlayCanvaus.gameObject.SetActive(false);
    }
    IEnumerator EndingStartImage()
    {
        Playerui.gameObject.SetActive(false);
        endingimg.enabled = true;
        float duration = 5f;
        float elapsedTime = 0f;
        Color startColor = endingimg.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            endingimg.color = Color.Lerp(startColor, targetColor, elapsedTime / duration);
            yield return null;
        }
        endingimg.color = targetColor;


        endingText.enabled = true;

        List<char> characters = new List<char>(fulltext.ToCharArray());

        foreach (char character in characters)
        {
            endingText.text += character;
            InGameSoundManager.instance.ActiveSound(gameObject, KeyboredSound, 20, true, false, false, 1);
            yield return new WaitForSeconds(0.25f);
        }

        yield return new WaitForSeconds(5f);
        SceneManger.S_instance.NextLobbyScene();
    }


    public void ActivePlayerUI_Text(bool state)
    {
        PlayerUI_TextObj.gameObject.SetActive(state);
    }

    public void OnPlayerUI_Img(int idx)
    {
        ItemImages[idx].gameObject.SetActive(true);
    }

    public void OffPlayerUI_Img()
    {
        for (int i = 0; i < ItemImages.Count; i++)
        {
            ItemImages[i].gameObject.SetActive(false);
        }
    }

    public void OnOption_S_Toggle()
    {
        if (!GameManager.G_instance.isGameover)
        {
            OnOptionsPlayerstop = !OnOptionsPlayerstop;
            if (OnOptionsPlayerstop)
            {
                Playerui.gameObject.SetActive(false);
                S_Option_Bg.gameObject.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                OnOptionsPlayerstop = false;
                Playerui.gameObject.SetActive(true);
                S_Option_Bg.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
       
        
    }
    public void OnOption_S_OFF()
    {
        if (!GameManager.G_instance.isGameover)
        {
            OnOptionsPlayerstop = false;
            Playerui.gameObject.SetActive(true);
            S_Option_Bg.gameObject.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
       
    }


    public void QuitGame()
    { 
        SceneManger.S_instance.NextLobbyScene();
    }
    public void Uioff_PlayerDead()
    {
        Aim.gameObject.SetActive(false);
        ItemSlot.gameObject.SetActive(false);
        ItemImage.gameObject.SetActive(false);  
        Playerui.gameObject.SetActive(false);
        Flash_BoxCol.enabled = false;
        Inventroy_object.gameObject.SetActive(false);
        for (int i = 3; i <= 5; i++)
        {
            Transform child = PlayCanvaus.transform.GetChild(i);
            child.gameObject.SetActive(false);
        }
    }
    

    public void OnItemIcon(GameObject obj, int idx) //인벤토리에 들어오는 아이템을 먹었을 때 호출
    {
        RectTransform Tr = obj.GetComponent<RectTransform>();
        obj.transform.SetParent(ItemSlots_UI[idx].transform);
        obj.GetComponent<RectTransform>().localScale = new Vector3 (1f, 1f, 1f);
        Tr.anchoredPosition = Vector2.zero;
        obj.SetActive(true);
    }

    public void OffItemIcon(GameObject obj, string GroupName) //인벤토리에서 사용하거나 아이템을 바닥에 버릴 때 호출
    {
        RectTransform Tr = obj.GetComponent<RectTransform>();
        GameObject UIgroup = GameObject.Find(GroupName);
        obj.transform.SetParent(UIgroup.transform);
        Tr.anchoredPosition = new Vector2(0f, -30f);
        obj.SetActive(false);
    }

    public void OnMisson(string txt)
    {
        Misson_Text.text = txt;
    }

    public void SetTimer(float time)
    {
        Timer_Text.text = $"{time}";
    }

    public void OnTimer(bool state)
    {
        Timer_Text.gameObject.SetActive(state);
    }

    public void SetTalk(string txt)
    {
        Talk_Text.text= txt;

        float txtWidth = Talk_Text.preferredWidth;
        TalkBox.sizeDelta = new Vector2(txtWidth + TalkPading, TalkBox.sizeDelta.y);
    }

    public void AutoSetTalk(string[] texts)
    {
        StartCoroutine(AutoTalk(texts));
    }

    IEnumerator AutoTalk(string[] texts)
    {
        int idx = 0;
        while(texts.Length > idx && !GameManager.G_instance.isGameover)
        {
            OnTalk(true);
            Talk_Text.text = texts[idx];

            float txtWidth = Talk_Text.preferredWidth;
            TalkBox.sizeDelta = new Vector2(txtWidth + TalkPading, TalkBox.sizeDelta.y);

            idx++;
            yield return new WaitForSeconds(3.0f);
        }
        OnTalk(false);
    }

    public void OnTalk(bool state)
    {
        TalkBox.gameObject.SetActive(state);
    }
}
