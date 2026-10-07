using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDoor : MonoBehaviour
{
    public bool inProgress = false;
    bool Switching = false;
    public Vector3 targetA;
    public Vector3 targetB;
    public Vector3 initialPosition;
    private float speed = 1f;
    private float targetHeight = 5;



    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        targetA = initialPosition + Vector3.up * targetHeight;
        targetB = initialPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (inProgress)
        {
            if(!Switching)
            {
                MoveUp();
            }

            if (Switching)
            {
                MoveBack();
                
            }

            
        }
        


    }
    void MoveBack()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetB, speed * Time.deltaTime);
        if (transform.position == targetB)
        {
            Switching = false;
            inProgress = false;
        }
    }

    void MoveUp()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetA, speed * Time.deltaTime);

        if (transform.position == targetA)
        {
            StartCoroutine(delay());
        }
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(2f);
        Switching = true;
    }
}



