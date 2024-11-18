using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gizimos : MonoBehaviour
{
    public Color _color;
    public float _radius;
    void Start()
    {
        
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
