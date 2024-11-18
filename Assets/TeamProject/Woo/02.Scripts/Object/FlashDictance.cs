using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDistance : MonoBehaviour
{
    [SerializeField] Light flash_light;
    [SerializeField] float angle = 93f; // �ּ� Range ��
    [SerializeField] float maxAngle = 140f; // �ִ� Range ��
    [SerializeField] LayerMask Wall; // �� ���̾�
   

    void Start()
    {
        flash_light = transform.GetChild(0).GetComponent<Light>();
        flash_light.spotAngle = angle; // �ʱ� Range ���� �ּҰ����� ����
        Wall = LayerMask.GetMask("Wall"); // "Wall" ���̾� �̸��� ����Ͽ� ����
    }

    void Update()
    {
        // ���� ������Ʈ���� �� ������Ʈ������ �Ÿ� ���
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f, Wall);

        if (hitColliders.Length > 0)
        {
            // ���� ����� ���
            flash_light.spotAngle = maxAngle;
            
        }
        else
        {
            // ���� �־����� ���� ������ ����
            flash_light.spotAngle = angle;
            
        }
    }
}
