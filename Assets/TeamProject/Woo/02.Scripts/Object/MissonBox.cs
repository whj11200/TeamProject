using UnityEngine;
using UnityEngine.UI;

public class MissonBox : MonoBehaviour
{
    private readonly string[] StartTalk = {
        "'이 끔찍한 소리는 뭐지?'",
        "'수색하면서 주위를 조심해야겠어...'",
        "'이전에 내가 이곳에 총을 가져온것 같은데 어디에 뒀는지 기억이 나질 않는군.'",
        "'총이 없다면 몸을 조심해야겠어.'"
    };

    readonly string PlayerTag = "Player";

    [SerializeField] Text Misson_Text;
    [SerializeField] RectTransform Inventroy_object;
    [SerializeField] GameObject MainCamera;
    [SerializeField] AudioClip Shout;
    void Start()
    {
        Misson_Text = GameObject.Find("Misson").transform.GetChild(0).GetComponent<Text>();
        Inventroy_object = GameObject.Find("PlayerUi").transform.GetChild(3).GetComponent<RectTransform>();
        MainCamera = GameObject.Find("MainCamera").gameObject;
        Shout = Resources.Load<AudioClip>("Sound/Object/StartShout");      

        Inventroy_object.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
        {
            Inventroy_object.gameObject.SetActive(true);
            Misson_Text.gameObject.SetActive(true);
            InGameUIManager.instance.OnMisson("수상한 것을 찾으시오.");

            SpawnManager.instance.SetActiveTrueCandel();
            SpawnManager.instance.SetActiveBookHead();
            SpawnManager.instance.SetActiveTrueItem();

            InGameSoundManager.instance.ActiveSound(MainCamera, Shout, 2.0f, true, false, false, 1);
            InGameUIManager.instance.AutoSetTalk(StartTalk);
            Destroy(gameObject);
        }
    }

}
