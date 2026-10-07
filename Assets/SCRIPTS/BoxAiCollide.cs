using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BoxAiCollide : MonoBehaviour
{
    public Rigidbody CurrentObjRigid;
    public GameObject enemy;
    public NavMeshAgent enemyNavMeshAgent;
    public Rigidbody currentEnemyRigidbody;
    public bool moving = false;

    public float hitForce = 3f;
    private GameObject CurrentObj;

    private bool delayedStopStarted = false;
    private float delayedStopTime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        CurrentObj = gameObject;
        CurrentObjRigid = CurrentObj.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentObjRigid != null && CurrentObjRigid.velocity.magnitude > 0.1f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (moving && other.CompareTag("Enemy"))
        {
            enemy = other.gameObject;
            enemyNavMeshAgent = enemy.GetComponent<NavMeshAgent>();
            currentEnemyRigidbody = enemy.GetComponent<Rigidbody>();
            currentEnemyRigidbody.isKinematic = false; // Allow the enemy's Rigidbody to be affected by forces
            enemyNavMeshAgent.isStopped = true; // Stop the enemy's NavMeshAgent movement
            enemyNavMeshAgent.enabled = false;

            if (!delayedStopStarted)
            {
                delayedStopStarted = true;
                StartCoroutine(DelayedStop());
            }
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if(moving && currentEnemyRigidbody != null)
        {
            enemy.GetComponent<AiMovement>().pushed = true;
            enemy.GetComponent<AiMovement>().pushing = true;
        }
    }
    private void OnCollisionExit(Collision other)
    {
        if (moving && currentEnemyRigidbody != null)
        {
            enemy.GetComponent<AiMovement>().pushed = false;
            enemy.GetComponent<AiMovement>().pushing = false;
        }
    }

    IEnumerator DelayedStop()
    {
        yield return new WaitForSeconds(delayedStopTime);

        if (currentEnemyRigidbody.velocity.magnitude < 0.1f)
        {
            currentEnemyRigidbody.isKinematic = true;
            enemyNavMeshAgent.enabled = true;
            enemy.GetComponent<AiMovement>().pushed = true;
            enemy.GetComponent<AiMovement>().pushing = false;
            enemyNavMeshAgent.isStopped = false;

            delayedStopStarted = false; // Reset the flag
        }
    }
}

    
  

    



