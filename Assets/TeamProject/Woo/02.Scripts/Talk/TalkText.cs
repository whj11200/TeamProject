using UnityEngine;
using UnityEngine.UI;

public class TalkText : MonoBehaviour
{
    private int currentDialogueIndex = 0;
    public bool talkout = false;
    private string[] dialogues = {
        "���� �󺴿����� ������ �Ҹ���, ���𰡰� ���ƴٴѴٴ� �Ű� �ް� �Ǿ���.",
        "������ �󺴿��� ���� �� ������ ���� ���� �Ա��� ���� �ٽ� ���� �� ���� �Ǿ���.",
        "�̰��� Ż���ϱ� ���ؼ��� �� ��Ҹ� ������ �� �ۿ� ����.",
        "������ ����ϴ� �������� �Ա� �ٷ� ���� �濡 �ִ�.",
        "�Һ��� �����̴� �濡 �� �������� ��������."
    };

    private void Start()
    {
        InGameUIManager.instance.SetTalk(dialogues[currentDialogueIndex]);
        InGameUIManager.instance.OnTalk(true);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShowNextDialogue();
        }
    }

    private void ShowNextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < dialogues.Length)
            InGameUIManager.instance.SetTalk(dialogues[currentDialogueIndex]);
        else
        {
            InGameUIManager.instance.OnTalk(false);
            talkout = true;
            Destroy(gameObject);
        }   
    }
}
