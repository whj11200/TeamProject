using UnityEngine;
using UnityEngine.UI;

public class TalkText : MonoBehaviour
{
    private int currentDialogueIndex = 0;
    public bool talkout = false;
    private string[] dialogues = {
        "요즘 폐병원에서 수상한 소리와, 무언가가 돌아다닌다는 신고를 받게 되었다.",
        "하지만 폐병원에 들어온 후 모종의 힘에 의해 입구가 막혀 다시 나갈 수 없게 되었다.",
        "이곳을 탈출하기 위해서는 이 장소를 수색할 수 밖에 없다.",
        "예전에 사용하던 손전등이 입구 바로 왼쪽 방에 있다.",
        "불빛이 깜빡이는 방에 들어가 손전등을 가져가자."
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
