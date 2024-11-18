using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashDistance : MonoBehaviour
{
    [SerializeField] Light flash_light;
    [SerializeField] float angle = 93f; // 최소 Range 값
    [SerializeField] float maxAngle = 140f; // 최대 Range 값
    [SerializeField] LayerMask Wall; // 벽 레이어
   

    void Start()
    {
        flash_light = transform.GetChild(0).GetComponent<Light>();
        flash_light.spotAngle = angle; // 초기 Range 값을 최소값으로 설정
        Wall = LayerMask.GetMask("Wall"); // "Wall" 레이어 이름을 사용하여 설정
    }

    void Update()
    {
        // 현재 오브젝트에서 벽 오브젝트까지의 거리 계산
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f, Wall);

        if (hitColliders.Length > 0)
        {
            // 벽이 가까운 경우
            flash_light.spotAngle = maxAngle;
            
        }
        else
        {
            // 벽이 멀어지면 원래 값으로 복원
            flash_light.spotAngle = angle;
            
        }
    }
}
