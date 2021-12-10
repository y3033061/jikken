using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Vector3 movement;
    private CharacterController controller;

    public GameObject cameraC;
    private Vector3 moveDir = Vector3.zero;
    private float gravity = 9.8f;
    private float moveH;
    private float moveV;
    private float jump;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // PCã‚Ì“®ìŠm”F—p
        if (Application.isEditor)
        {
            moveH = Input.GetAxis("Horizontal");
            moveV = Input.GetAxis("Vertical");
            jump = Input.GetAxis("Jump");
        }
        // Oculus Quest2‚Å“®‚©‚·
        else
        {
            moveH = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
            moveV = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;
        }

        movement = new Vector3(moveH, jump, moveV);

        Vector3 desiredMove = cameraC.transform.forward * movement.z + cameraC.transform.up * movement.y + cameraC.transform.right * movement.x;
        moveDir.x = desiredMove.x * 3f;
        moveDir.z = desiredMove.z * 3f;
        if (jump > 0)
        {
            moveDir.y = desiredMove.y * 3f;
        }
        else
        {
            moveDir.y -= gravity * Time.deltaTime;
        }
        

        controller.Move(moveDir * Time.deltaTime * speed);
    }
}
