using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PressurePlate : MonoBehaviour
{

    public GameObject InteractedObj;


    // Start is called before the first frame update
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            InteractedObj.GetComponent<WallDoor>().inProgress = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            
        }
    }
   

}
