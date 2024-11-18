using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampDestoryBox : MonoBehaviour
{
    [SerializeField] Transform Lamp;
    [SerializeField] AudioClip BrakingLamp_Sound;
    BoxCollider box;
    Lamp lamp;

    private void Start()
    {
        BrakingLamp_Sound = Resources.Load<AudioClip>("Sound/Object/LampBraking");

        Lamp = GameObject.Find("Lamp").GetComponent<Transform>();      
        lamp = GameObject.Find("Lamp").GetComponent<Lamp>();
        box = gameObject.GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lamp.BreakLamp();
            box.enabled = false;
            InGameSoundManager.instance.ActiveSound(gameObject, BrakingLamp_Sound, 5, true, false, false, 1);
            Lamp.gameObject.SetActive(false);
            Destroy(gameObject, 3.0f);
        }
    }
    
}
