using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_instance;

    public delegate void LevelUpHandler(); //끈 촛불의 개수가 늘어났을 때 발생할 이벤트 등록
    public event LevelUpHandler SpeedUp;

    [SerializeField] PlayableDirector Door_Director;
    [SerializeField] CinemachineVirtualCamera DoorCamera;
    [SerializeField] Light Door_Light;
    [SerializeField] Light LastLight;

    private GameObject LastCandleObj;
    public GameObject LastOffCandle;

    private readonly string[] FristCandleTalk = {
        "'왜 이런 곳에 양초가 놓아져 있는거지?'",
        "'양초를 건드리니 발자국 소리가 더욱 빨라졌군.. 조심해야겠어..'",
        "'다른 양초가 있는지 찾아봐야겠어.'"
    };
    private readonly string[] FristDemonTalk = {
        "'주변에서 무슨 소리가 들리는군..'",
        "'괴물의 외피가 정말 단단해 보이는군, 총으로 상처를 내긴 어렵겠어.'",
        "'양초를 끄면 괴물이 자극을 받는군 양초를 끌 때 더욱 조심해야겠어'"
    };
    private readonly string[] BasementTalk = {
        "'문이 열리는 소리? 지하실 방향인것 같군.'",
        "'지하실에는 이 지긋지긋한 곳을 나갈 방법이 있을까...'"
    };

    private int candleCounter;

    public bool AllStop = false;
    public int CandleCounter
    {
        get { return candleCounter; }
        set
        {
            candleCounter += value;
            DifficultyLevelUp();
        }
    }
    public bool isGameover = false;
    void Awake()
    {
        if (G_instance == null)
            G_instance = this;
        else if (G_instance != this)
            Destroy(G_instance);

        Door_Director = GameObject.Find("Door_V1 (5)").transform.GetChild(0).GetComponent<PlayableDirector>();
        DoorCamera = GameObject.Find("Door_Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        Door_Light = GameObject.Find("Door_V1 (5)").transform.GetChild(2).GetComponent<Light>();
        LastLight = GameObject.Find("HallDownstair").transform.GetChild(2).GetChild(0).GetComponent<Light>();
        LastCandleObj = GameObject.Find("MachineRoom").transform.GetChild(5).gameObject;

        LastLight.gameObject.SetActive(false);
        LastCandleObj.gameObject.SetActive(false);

        DoorCamera.enabled = false;
        Door_Light.enabled = false;
    }


    public void CanndleCounter(int counter)
    {
        CandleCounter = counter;
        InGameUIManager.instance.OnMisson($"양초를 모두 끄십시오.\n\n{CandleCounter}/6");
    }

    private void DifficultyLevelUp()
    {
        switch(candleCounter) //촛불이 꺼진 개수에 따라 이벤트 발생
        {
            case 1:
                SpeedUp.Invoke(); //북헤드 스피드업 이벤트
                InGameUIManager.instance.AutoSetTalk(FristCandleTalk);


                break;

            case 2:
                SpawnManager.instance.SetActiveDemonTrue(LastOffCandle);
                InGameUIManager.instance.AutoSetTalk(FristDemonTalk);

                break;

            case 3:
                SpeedUp.Invoke(); //북헤드 스피드업 이벤트

                break;


            case 4:
                SpawnManager.instance.SetActiveDemonTrue(LastOffCandle);

                break;


            case 5:
                SpeedUp.Invoke(); //북헤드 스피드업 이벤트

                break;

            case 6:
                SpawnManager.instance.SetActiveDemonTrue(LastOffCandle);
                SpawnManager.instance.SetActiveBookHead_Final();
                StartCoroutine(SeeTheDoor());
                
                break;

        }

        IEnumerator SeeTheDoor()
        {
            AllStop = true;
            InGameUIManager.instance.OpenDoorOffUi();
            Door_Director.Play();
            DoorCamera.enabled = true;
            Door_Light.enabled = true;
            yield return new WaitForSeconds(4f);
            InGameUIManager.instance.OpenDoorONUi();
            DoorCamera.enabled = false;
            Door_Light.gameObject.SetActive(false);
            AllStop = false;
            LastCandleObj.gameObject.SetActive(true);
            InGameUIManager.instance.OnMisson("지하실로 들어가십시오.");
            InGameUIManager.instance.AutoSetTalk(BasementTalk);           
        }
    }

    public void CtrlLastLight(bool state)
    {
        LastLight.gameObject.SetActive(state);
    }
}
