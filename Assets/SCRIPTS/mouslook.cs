using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Interactable
{
    public void Interact();
}

public class mouslook : MonoBehaviour
{

 
    public float mousesens = 100;
    public Transform playerbody;
    float xRotation = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    // Update is called once per frame
    void Update()
    {
        //looking up and down and left and right
        float mouseX = Input.GetAxis("Mouse X") * mousesens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mousesens * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        playerbody.Rotate(Vector3.up * mouseX);

        

    }

   


}



