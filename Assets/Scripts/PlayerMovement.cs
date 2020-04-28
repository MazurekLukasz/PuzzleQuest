using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController Controller;
    [SerializeField] float MovementSpeed = 6f;

    float VelocityY;
    [SerializeField] float Gravity = -9.81f;
    [SerializeField] Transform GroundCheck;
    float GroundCheckRadius = 0.4f;
    public LayerMask GroundMask;
    bool IsGrounded;

    // Start is called before the first frame update
    void Start()
    {
        Controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = Physics.CheckSphere(GroundCheck.position,GroundCheckRadius,GroundMask);
        if (IsGrounded && VelocityY < 0)
        {
            VelocityY = -1f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        VelocityY += Gravity * Time.deltaTime;
        Vector3 move = (transform.right * x + transform.forward * z) * MovementSpeed * Time.deltaTime;
        move = new Vector3(move.x, VelocityY * Time.deltaTime, move.z);
        Controller.Move(move);
    }
}
