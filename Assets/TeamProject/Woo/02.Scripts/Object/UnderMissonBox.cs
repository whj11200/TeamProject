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
            InGameUIManager.instance.OnMisson("������ �к��� ���� �Ͽ� ��ȯ�� �����Ͻÿ�.");
            Destroy(gameObject);
        }
    }
}
