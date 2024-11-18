using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderMissonBox : MonoBehaviour
{
    readonly string PlayerTag = "Player";
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PlayerTag))
        {
            InGameUIManager.instance.OnMisson("마지막 촛불을 제거 하여 소환을 저지하시오.");
            Destroy(gameObject);
        }
    }
}
