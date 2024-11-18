using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxOnOff : MonoBehaviour
{
    private BoxCollider boxcol;

    private void Start()
    {
        boxcol = gameObject.transform.GetChild(6).GetComponent<BoxCollider>();
        boxcol.enabled = false;
    }

    public void OnEneld()
    {
        boxcol.enabled = true;
    }
    public void OffAttack()
    {
        boxcol.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            boxcol.enabled = false;
        }
    }
}
