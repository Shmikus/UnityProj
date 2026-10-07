using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 targetA;
    public Vector3 targetB;
    public Vector3 initialPosition;
    private float speed = 1f;
    private bool switching = false;
    private float targetHeight = 5;

    private void Start()
    {
        initialPosition = transform.position;
        targetA = initialPosition + Vector3.up * targetHeight;
        targetB = initialPosition;
    }

    private void FixedUpdate()
    {
        if (!switching)
        {
            MoveToTarget(targetA);
        }
        else if (switching)
        {
            MoveToTarget(targetB);
        }
    }

    private void MoveToTarget(Vector3 targetPosition)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            switching = !switching;
            StartCoroutine(DelayBeforeMoving());
        }
    }

    private IEnumerator DelayBeforeMoving()
    {
        yield return new WaitForSeconds(3f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }
}


