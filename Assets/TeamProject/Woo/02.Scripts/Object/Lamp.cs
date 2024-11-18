using UnityEngine;

public class Lamp : MonoBehaviour
{
    [SerializeField] Light[] AllLight;
    [SerializeField] AudioClip start_sound;
    void Start()
    {
        start_sound = Resources.Load<AudioClip>("Sound/Object/Lamp");

        AllLight = gameObject.GetComponentsInChildren<Light>();

        InGameSoundManager.instance.ActiveSound(gameObject, start_sound, 8f, true, true, true, 1);

        ToggleLights();
    }

    void ToggleLights()
    {
        foreach (var light in AllLight)
        {
            light.enabled = !light.enabled; // 라이트 상태 반전
        }

        Invoke("ToggleLights", 0.5f);
    }

    public void BreakLamp()
    {
        InGameSoundManager.instance.EditSoundBox(start_sound.name, false);
        InGameSoundManager.instance.Data.Remove(start_sound.name);
    }
}
