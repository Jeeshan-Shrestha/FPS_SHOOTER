using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    public float speed = 5.0f;
    public float sprintSpeed = 9.0f;
    public float gravityScale = -9.8f;
    public float jumpForce = 5.0f;

    private Boolean isGrounded;
    private PlayerHealthScripts playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealthScripts>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
    }

    public void ProcessMovement(Vector2 input)
    {
        float currentSpeed = (playerHealth.isSprinting) ? sprintSpeed : speed;

        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * currentSpeed * Time.deltaTime);

        playerVelocity.y += gravityScale * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
            playerVelocity.y = -2f;

        controller.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (isGrounded)
            playerVelocity.y = Mathf.Sqrt(jumpForce * -3.0f * gravityScale);
    }
}