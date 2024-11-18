using UnityEngine;
using UnityEngine.UI;

public class MissonBox : MonoBehaviour
{
    private readonly string[] StartTalk = {
        "'�� ������ �Ҹ��� ����?'",
        "'�����ϸ鼭 ������ �����ؾ߰ھ�...'",
        "'������ ���� �̰��� ���� �����°� ������ ��� �״��� ����� ���� �ʴ±�.'",
        "'���� ���ٸ� ���� �����ؾ߰ھ�.'"
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
            InGameUIManager.instance.OnMisson("������ ���� ã���ÿ�.");

            SpawnManager.instance.SetActiveTrueCandel();
            SpawnManager.instance.SetActiveBookHead();
            SpawnManager.instance.SetActiveTrueItem();

            InGameSoundManager.instance.ActiveSound(MainCamera, Shout, 2.0f, true, false, false, 1);
            InGameUIManager.instance.AutoSetTalk(StartTalk);
            Destroy(gameObject);
        }
    }

}
