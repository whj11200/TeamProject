using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGround : MonoBehaviour
{
    private PlayerMove playerMove;
    private CharacterController characterController;

    private void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (GameManager.G_instance.isGameover) return;

        Check_Ground();
    }
    private void Check_Ground()
    {
        if (characterController.isGrounded) playerMove.IsGround = true;
        else
        {
            RaycastHit hit;
            playerMove.IsGround = Physics.Raycast(transform.position, Vector3.down, out hit, 0.15f);
        }
    }
}
