using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IInteractable
{
    public void Interact();
}


public class Interact : MonoBehaviour
{
    // Start is called before the first frame update
  

    // Update is called once per frame
    void Update()
    {
     if (Input.GetKeyDown(KeyCode.E))
        {   
            float InteractRange = 1.5f;
            Vector3 direction2 = Vector3.forward;
            Ray forwardRay = new Ray(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.TransformDirection(direction2 * InteractRange));
            if (Physics.Raycast(forwardRay, out RaycastHit hitInfo, InteractRange))
               {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }
                
               }

        }
    }
}
