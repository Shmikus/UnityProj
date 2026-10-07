using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{

    public float speed = 12;

    public CharacterController controller;

    public float gravity = -9.81f;

    public float ray1range = 1.1f;

    public float ray2range = 1;

    public float ray3range = 1.2f;

    public float jumpHeight = 5f;

    public float rayCheckCooldown = 1f;

    public float rayCheckTimer = 0f;

    bool CanCast = true;

    bool isgrounded = false;

    bool CanWallJump = true;

    public float WallJumpTimer = 0f;

    public float WallJumpCooldown = 1.5f;

    


    

    Vector3 velocity;
  
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        Vector3 direction = Vector3.down;

        Vector3 direction2 = Vector3.forward;

        Ray downRay = new Ray(transform.position, transform.TransformDirection(direction * ray1range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * ray1range));

        Ray slopeDownRay = new Ray(transform.position, transform.TransformDirection(direction * ray3range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * ray3range));

        Ray forwardRay = new Ray(transform.position  , transform.TransformDirection(direction2 * ray2range));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction2 * ray2range));

         if (Physics.Raycast(downRay, out RaycastHit hit, ray1range ,1 ,  QueryTriggerInteraction.Ignore) && CanCast)
        {
         
            {
                velocity.y = 0f;
                isgrounded = true;
            } 
        }
        if (Physics.Raycast(slopeDownRay, out hit, ray3range) && CanCast)
        {
            if (hit.transform.CompareTag("2")) 
            { 
                isgrounded = true;
                velocity.y = 0;
            }

        }
        if (!CanCast)
        {
            rayCheckTimer -= Time.deltaTime;
            if (rayCheckTimer <= 0)
            { CanCast = true;
                rayCheckTimer = rayCheckCooldown;
            }
        }

        if (!CanWallJump)
        {
            WallJumpTimer -= Time.deltaTime;
            if (WallJumpTimer <= 0)
            { CanWallJump = true;
                WallJumpTimer = WallJumpCooldown;
            }
        }
        if (isgrounded == true && velocity.y <= 0f)
        {
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
                CanCast = false;
                isgrounded = false;
            }
        }
        if (Physics.Raycast(forwardRay, out RaycastHit hits, ray2range) && hits.transform.CompareTag("1"))
        {
            if (Input.GetButtonDown("Jump") && CanWallJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
                CanWallJump = false;
                
            }
        }
        
        velocity.y += gravity * Time.deltaTime;

        

        controller.Move(velocity * Time.deltaTime);
        
        

       
        
    }

}
