using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolBox.Tags;

public class MiniMovement : MonoBehaviour
{
    public int speed = 12;

    public CharacterController controller;

    public float gravity = -9.81f;

    public float climbSpeed = .05f;

    public float ForwardLength = .5f; // ray length
    public float DownLength = .3f;

    public bool grounded = false;



    Vector3 velocity;

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);




    }

    void Climbing()
    {
        Vector3 Direction = Vector3.forward;
        Ray ClimbingRay = new Ray(transform.position, transform.TransformDirection(Direction));
        Debug.DrawRay(transform.position, transform.TransformDirection(Direction * ForwardLength) );

        if (Physics.Raycast(ClimbingRay, out RaycastHit hit, ForwardLength, 1, QueryTriggerInteraction.Ignore) && hit.transform.CompareTag("4"))
        {
            velocity.y = 0;
            Vector3 Climb = transform.up * climbSpeed;
            controller.Move(Climb * Time.deltaTime);
        }
        Vector3 Down = Vector3.down;
        Ray DownRay = new Ray(transform.position, transform.TransformDirection(Down));
        Debug.DrawRay(transform.position, transform.TransformDirection(Down * DownLength));

        if (Physics.Raycast(DownRay, out RaycastHit hits, DownLength, 1, QueryTriggerInteraction.Ignore))
        {
            velocity.y = 0;
            grounded = true;

        }
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        Movement();
        Climbing();

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        print(velocity.y);

    }
}