using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManger : MonoBehaviour
{
    [SerializeField] bool OptionCilck;
    [SerializeField] Camera L_Camera;
    [SerializeField] Image Optionimage;

    [SerializeField] RectTransform VideoOption;
    [SerializeField] RectTransform AudioOption;
    [SerializeField] AudioClip Ui_Button_Clip;
    void Awake()
    {
        L_Camera = Camera.main;
        Optionimage = GameObject.Find("Ui").transform.GetChild(2).GetComponent<Image>();
        VideoOption = GameObject.Find("Ui").transform.GetChild(2).GetChild(2).GetComponent<RectTransform>();
        AudioOption = GameObject.Find("Ui").transform.GetChild(2).GetChild(3).GetComponent<RectTransform>();
        Ui_Button_Clip = Resources.Load<AudioClip>("Sound/Button/UiButton");

        Optionimage.gameObject.SetActive(false);
        VideoOption.gameObject.SetActive(false);
        AudioOption.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManger.S_instance.NextGameScene();
        LobbySoundManager.instance.ActiveSound(L_Camera.gameObject, Ui_Button_Clip, 7.0f, true, false, false, 1);
    }
    public void Quit()
    {
        LobbySoundManager.instance.ActiveSound(L_Camera.gameObject, Ui_Button_Clip, 7, true, false, false, 1);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#else
       Application.Quit();
#endif
    }
    public void Optionopen()
    {
        LobbySoundManager.instance.ActiveSound(L_Camera.gameObject, Ui_Button_Clip, 7, true, false, false, 1);
        Optionimage.gameObject.SetActive(true);
    }
    public void OptionClose()
    {
        LobbySoundManager.instance.ActiveSound(L_Camera.gameObject, Ui_Button_Clip, 7, true, false, false, 1);
        Optionimage.gameObject.SetActive(false);
    }
    public void SoundMeauOpen()
    {
        LobbySoundManager.instance.ActiveSound(L_Camera.gameObject, Ui_Button_Clip, 7, true, false, false, 1);
        AudioOption.gameObject.SetActive(true);
        VideoOption.gameObject.SetActive(false);
    }
    public void VideoMeauOpen()
    {
        LobbySoundManager.instance.ActiveSound(L_Camera.gameObject, Ui_Button_Clip, 7, true, false, false, 1);
        AudioOption.gameObject.SetActive(false);
        VideoOption.gameObject.SetActive(true);
    }
}
