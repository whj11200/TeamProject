using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LobbySoundManager : MonoBehaviour
{
    //�κ� ����(BG)
    //�κ� ��� �к� Ÿ�� �Ҹ�(SFX)

    public static LobbySoundManager instance;
    public Dictionary<string, SoundData> Data = new Dictionary<string, SoundData>();

    private AudioMixer audioMixer;
    private AudioClip Lobby_BG;
    private AudioClip Lobby_Candle;

    private Slider BG_Slider;
    private Slider SFX_Slider;

    private GameObject SoundBox;
    private GameObject MainCamera;
    private GameObject Candle;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        SoundBox = Resources.Load<GameObject>("SoundBox");
        audioMixer = Resources.Load<AudioMixer>("Sound/AudioMixer");
        Lobby_BG = Resources.Load<AudioClip>("Sound/BG/LobbyBG");
        Lobby_Candle = Resources.Load<AudioClip>("Sound/Item/CandleBuzz");

        BG_Slider = GameObject.Find("Ui").transform.GetChild(2).GetChild(3).GetChild(2).GetComponent<Slider>();
        SFX_Slider = GameObject.Find("Ui").transform.GetChild(2).GetChild(3).GetChild(3).GetComponent<Slider>();
        MainCamera = GameObject.Find("MainCamera");
        Candle = GameObject.Find("Candle");

        BG_Slider.onValueChanged.AddListener(value => SoundManager.instance.BG_Sound = value); // �����̴��� ���� ����Ǹ� �ڵ����� ������Ƽ�� ����
        SFX_Slider.onValueChanged.AddListener(value => SoundManager.instance.SFX_Sound = value); // �����̴��� ���� ����Ǹ� �ڵ����� ������Ƽ�� ����

        ActiveSound(MainCamera, Lobby_BG, 5.0f, true, true, true, 0);
        ActiveSound(Candle, Lobby_Candle, 5.0f, true, true, true, 1);

    }
    private void Start()
    {
        SoundSetting(0, SoundManager.instance.BG_Sound); //�� ���۽� �������� �ɼ��� �״�� �����´�.
        SoundSetting(1, SoundManager.instance.SFX_Sound); //�� ���۽� �������� �ɼ��� �״�� �����´�.

        BG_Slider.value = (SoundManager.instance.BG_Sound + 40.0f) / 40.0f;
        SFX_Slider.value = (SoundManager.instance.SFX_Sound + 40.0f) / 40.0f;
    }

    public void SoundSetting(int option, float value) //�κ���� ���� ����
    {
        switch (option)
        {
            case 0: //BG ���� ����
                audioMixer.SetFloat("BG_Volume", value);

                break;

            case 1: //SFX ���� ����
                audioMixer.SetFloat("SFX_Volume", value);

                break;
        }
    }

    public void ActiveSound(GameObject target, AudioClip clip, float MaxDist, bool awakePlay, bool loop, bool reside, int option, float? clipLength = null)
    {//�Ҹ��� ���� ������Ʈ, �Ҹ�, �Ҹ��� �鸱 �ִ� �Ÿ�, �ٷ� �������,�ݺ� ����, ���� �ڽ��� ���� ��ų��,�Ҹ� Ÿ��, (���Է½�:Ŭ�� ���̸�ŭ ���/�Է½�:�� ��ŭ Ŭ�� ���)
        switch (option)
        {
            case 0: //BG
                GameObject BackGroundObj = Instantiate(SoundBox);
                AudioSource BG_audioSource = BackGroundObj.GetComponent<AudioSource>();
                BackGroundObj.transform.SetParent(target.transform); //�Ҹ��� ���� ������Ʈ�� �ڽ����� �̵�.
                BackGroundObj.transform.localPosition = Vector3.zero; //�θ��� ��ġ�� �̵�.

                BG_audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BackGround")[0]; //����� �ͼ����� ã�� BackGound �׷��� 1��° �׷��� �Ҵ�.
                BG_audioSource.playOnAwake = awakePlay;
                BG_audioSource.clip = clip;
                BG_audioSource.loop = loop;
                BG_audioSource.rolloffMode = AudioRolloffMode.Custom;
                BG_audioSource.maxDistance = MaxDist;
                BG_audioSource.volume = 1.0f;

                BackGroundObj.SetActive(true);

                if (loop == false && reside == false && clipLength == null) //Ŭ���� ���̸�ŭ �ѹ� ���, ���� �ڽ� ��Ȱ��ȭ
                {
                    BG_audioSource.Play();
                    Destroy(BackGroundObj, clip.length);
                }
                else if (loop == false && reside == false && clipLength != null) //������ ��ŭ �ѹ� ���, ���� �ڽ� ��Ȱ��ȭ
                {
                    BG_audioSource.Play();
                    Destroy(BackGroundObj, clipLength.Value);
                }
                else if (loop == false && reside == true && clipLength == null) //���� �ڽ��� ���ֽ�Ű�� ���� ���� Ŭ���� ���� ��ŭ ����� ���� ������ ����
                {
                    Data.Add(clip.name, new SoundData(BackGroundObj, BG_audioSource, clip.length));
                }
                else if (loop == false && reside == true && clipLength != null) //���� �ڽ��� ���ֽ�Ű�� ���� ���� ������ �� ��ŭ ����� ���� ������ ����
                {
                    Data.Add(clip.name, new SoundData(BackGroundObj, BG_audioSource, clipLength));
                }
                else if (loop == true && awakePlay == true) //���� �ڽ��� ���ֽ�Ű�� �ݺ� ���.
                {
                    BG_audioSource.Play();
                    Data.Add(clip.name, new SoundData(BackGroundObj, BG_audioSource, clip.length));
                }
                else if (loop == true && awakePlay == false) //���� �ڽ��� ���ֽ�Ű�� ����ڰ� �ʿ��� ���� �ݺ� ���.
                {
                    Data.Add(clip.name, new SoundData(BackGroundObj, BG_audioSource, clip.length));
                }

                break;

            case 1: //SFX
                GameObject SFXObj = Instantiate(SoundBox);
                AudioSource SFX_audioSource = SFXObj.GetComponent<AudioSource>();
                SFXObj.transform.SetParent(target.transform); //�Ҹ��� ���� ������Ʈ�� �ڽ����� �̵�.
                SFXObj.transform.localPosition = Vector3.zero; //�θ��� ��ġ�� �̵�.

                SFX_audioSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0]; //����� �ͼ����� ã�� SFX�׷��� 1��° �׷��� �Ҵ�.
                SFX_audioSource.playOnAwake = awakePlay;
                SFX_audioSource.clip = clip;
                SFX_audioSource.loop = loop;
                SFX_audioSource.rolloffMode = AudioRolloffMode.Custom;
                SFX_audioSource.maxDistance = MaxDist;
                SFX_audioSource.volume = 1.0f;
                SFX_audioSource.spatialBlend = 1.0f; //���� ���� ����, �Ÿ��� ���� �Ҹ����谡 �����.

                SFXObj.SetActive(true);

                if (loop == false && reside == false && clipLength == null) //Ŭ���� ���̸�ŭ �ѹ� ���, ���� �ڽ� ��Ȱ��ȭ
                {
                    SFX_audioSource.Play();
                    Destroy(SFXObj, clip.length);
                }
                else if (loop == false && reside == false && clipLength != null) //������ ��ŭ �ѹ� ���, ���� �ڽ� ��Ȱ��ȭ
                {
                    SFX_audioSource.Play();
                    Destroy(SFXObj, clipLength.Value);
                }
                else if (loop == false && reside == true && clipLength == null) //���� �ڽ��� ���ֽ�Ű�� ���� ���� Ŭ���� ���� ��ŭ ����� ���� ������ ����
                {
                    Data.Add(clip.name, new SoundData(SFXObj, SFX_audioSource, clip.length));
                }
                else if (loop == false && reside == true && clipLength != null) //���� �ڽ��� ���ֽ�Ű�� ���� ���� ������ �� ��ŭ ����� ���� ������ ����
                {
                    Data.Add(clip.name, new SoundData(SFXObj, SFX_audioSource, clipLength));
                }
                else if (loop == true && awakePlay == true) //���� �ڽ��� ���ֽ�Ű�� �ݺ� ���.
                {
                    SFX_audioSource.Play();
                    Data.Add(clip.name, new SoundData(SFXObj, SFX_audioSource, clip.length));
                }
                else if (loop == true && awakePlay == false) //���� �ڽ��� ���ֽ�Ű�� ����ڰ� �ʿ��� ���� �ݺ� ���.
                {
                    Data.Add(clip.name, new SoundData(SFXObj, SFX_audioSource, clip.length));
                }

                break;
        }
    }

    public void EditSoundBox(string key, bool setActive, AudioClip clip = null) //���������� ���� �ڽ��� �����ϴ� �Լ� (������ ����ڽ� Ű, Ȱ��ȭ ����, ��ü�� ���� Ŭ��)
    {
        if (setActive) // ���� �ڽ��� ��� Ȱ��ȭ �Ѵٸ� ���� Ŭ�� ��ü
            Data[key].SoundBox_AudioSource.clip = clip;
        else // ���� �ڽ��� ��Ȱ��ȭ �Ѵٸ� 0.1���� ���� �ڽ� ��Ȱ��ȭ
            Destroy(Data[key].SoundBox);
    }
}
