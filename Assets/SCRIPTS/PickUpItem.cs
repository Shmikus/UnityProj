using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public GameObject whipPrefab;
    public Transform parentTransform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            GameObject whip = Instantiate(whipPrefab, parentTransform);
            Destroy(gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
}
