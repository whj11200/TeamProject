using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropBox : MonoBehaviour
{
    FullScreenMode screenMode;
    public Dropdown resolutionDropdown;
    Toggle fullscreenbtn;
    List<Resolution> resolutions = new List<Resolution>();
    int resolutionNum = 0;

    private void Awake()
    {
        fullscreenbtn = GameObject.Find("Ui").transform.GetChild(2).GetChild(2).GetChild(1).GetComponent<Toggle>();
        resolutionDropdown = GameObject.Find("Ui").transform.GetChild(2).GetChild(2).GetChild(0).GetComponent<Dropdown>();
        Init();
        // PlayerPrefs에서 저장된 모드 불러오기
        int windowMode = PlayerPrefs.GetInt("WindowMode", 0); // 기본값은 0 (전체 화면)
        FullScreenBtn(windowMode == 0);
    }

    void Init()
    {
        resolutions.Clear();
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            resolutions.Add(Screen.resolutions[i]);
        }

        resolutionDropdown.options.Clear();
        int optionNum = 0;
        foreach (Resolution item in resolutions)
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "X" + item.height;
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
            {
                resolutionDropdown.value = optionNum;
            }
            optionNum++;
        }
        resolutionDropdown.RefreshShownValue();
    }

    public void DropOptionChange(int x)
    {
        resolutionNum = x;
    }

    public void FullScreenBtn(bool isFull)
    {
        if (isFull)
        {
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, FullScreenMode.FullScreenWindow);
            screenMode = FullScreenMode.FullScreenWindow;
            PlayerPrefs.SetInt("WindowMode", 0); // 전체 화면 저장
        }
        else
        {
            Screen.SetResolution(resolutions[resolutionNum].width, resolutions[resolutionNum].height, FullScreenMode.Windowed);
            screenMode = FullScreenMode.Windowed;
            PlayerPrefs.SetInt("WindowMode", 1); // 창 모드 저장
        }
    }

    public void OkBtnClick()
    {
        Resolution selectedResolution = resolutions[resolutionNum];
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, fullscreenbtn.isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);

        // 토글 상태 업데이트
        fullscreenbtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.MaximizedWindow);
        PlayerPrefs.SetInt("WindowMode", fullscreenbtn.isOn ? 0 : 1); // 상태 저장
    }
}
