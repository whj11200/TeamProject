using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager G_instance;

    public delegate void LevelUpHandler(); //�� �к��� ������ �þ�� �� �߻��� �̺�Ʈ ���
    public event LevelUpHandler SpeedUp;

    [SerializeField] PlayableDirector Door_Director;
    [SerializeField] CinemachineVirtualCamera DoorCamera;
    [SerializeField] Light Door_Light;
    [SerializeField] Light LastLight;

    private GameObject LastCandleObj;
    public GameObject LastOffCandle;

    private readonly string[] FristCandleTalk = {
        "'�� �̷� ���� ���ʰ� ������ �ִ°���?'",
        "'���ʸ� �ǵ帮�� ���ڱ� �Ҹ��� ���� ��������.. �����ؾ߰ھ�..'",
        "'�ٸ� ���ʰ� �ִ��� ã�ƺ��߰ھ�.'"
    };
    private readonly string[] FristDemonTalk = {
        "'�ֺ����� ���� �Ҹ��� �鸮�±�..'",
        "'������ ���ǰ� ���� �ܴ��� ���̴±�, ������ ��ó�� ���� ��ưھ�.'",
        "'���ʸ� ���� ������ �ڱ��� �޴±� ���ʸ� �� �� ���� �����ؾ߰ھ�'"
    };
    private readonly string[] BasementTalk = {
        "'���� ������ �Ҹ�? ���Ͻ� �����ΰ� ����.'",
        "'���Ͻǿ��� �� ���������� ���� ���� ����� ������...'"
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
        InGameUIManager.instance.OnMisson($"���ʸ� ��� ���ʽÿ�.\n\n{CandleCounter}/6");
    }

    private void DifficultyLevelUp()
    {
        switch(candleCounter) //�к��� ���� ������ ���� �̺�Ʈ �߻�
        {
            case 1:
                SpeedUp.Invoke(); //����� ���ǵ�� �̺�Ʈ
                InGameUIManager.instance.AutoSetTalk(FristCandleTalk);


                break;

            case 2:
                SpawnManager.instance.SetActiveDemonTrue(LastOffCandle);
                InGameUIManager.instance.AutoSetTalk(FristDemonTalk);

                break;

            case 3:
                SpeedUp.Invoke(); //����� ���ǵ�� �̺�Ʈ

                break;


            case 4:
                SpawnManager.instance.SetActiveDemonTrue(LastOffCandle);

                break;


            case 5:
                SpeedUp.Invoke(); //����� ���ǵ�� �̺�Ʈ

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
            InGameUIManager.instance.OnMisson("���ϽǷ� ���ʽÿ�.");
            InGameUIManager.instance.AutoSetTalk(BasementTalk);           
        }
    }

    public void CtrlLastLight(bool state)
    {
        LastLight.gameObject.SetActive(state);
    }
}
