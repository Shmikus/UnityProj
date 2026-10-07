using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ToolBox.Tags;

public class Whip : MonoBehaviour
{
    public Camera PlayerCam;
    public int hitForce = 10;
    public int interactRange = 10;
    public bool pushing = false;
    public bool holdingWhip = true;
    public bool drawing = false;
    public GameObject linePrefab;
    public LineRenderer linerenderer;
    public GameObject whippp;
    public int vertexCount = 40;
    public Vector3 Enemypos;
    public bool destroyLine;
    public GameObject lineObject;
    bool canWhip = true;
    public float movementSpeed = 5f;
    public bool Pulling = false;


    private Rigidbody currentEnemyRigidbody; // Reference to the current enemy's Rigidbody
    private NavMeshAgent enemyNavMeshAgent;
    private GameObject Enemy;// Reference to the current enemy's NavMeshAgent


    void Start()
    {
        PlayerCam = FindObjectOfType<Camera>();
    }



    void Update()
    {
        Vector3 direction = Vector3.forward;
        
        if (canWhip)
        {
            
            Ray forwardRay = new Ray(PlayerCam.transform.position, PlayerCam.transform.forward);
            Debug.DrawRay(PlayerCam.transform.position, transform.TransformDirection(direction * interactRange));

            // Check if the raycast hits an enemy tagged object within the interactRange
            if (Physics.Raycast(forwardRay, out RaycastHit hit, interactRange,1, QueryTriggerInteraction.Ignore) && hit.transform.CompareTag("Enemy") && Input.GetKeyDown(KeyCode.Mouse0))
            {
                hitForce = 10;
                lineObject = Instantiate(linePrefab);
                GameObject enemy = hit.transform.gameObject;
                Enemy = enemy;
                Enemypos = Enemy.transform.position + PlayerCam.transform.forward * 2f;
                drawing = true;
                Enemy.GetComponent<AiMovement>().pushing = true;
                linerenderer = lineObject.GetComponent<LineRenderer>();
                canWhip = false;
                StartCoroutine(canwhip());
                StartCoroutine(DrawBezier());
                PushEnemy(enemy);
            }

            if (Physics.Raycast(forwardRay, out RaycastHit hits, interactRange) && hits.transform.CompareTag("MoveableObj") && Input.GetKeyDown(KeyCode.Mouse0))
            {
                hitForce = 15;
                StartCoroutine(canwhip());
                GameObject box = hits.transform.gameObject;
                Enemypos = box.transform.position;
                lineObject = Instantiate(linePrefab);
                linerenderer = lineObject.GetComponent<LineRenderer>();
                StartCoroutine(DrawBezier());
                drawing = true;
                Rigidbody BoxRigidBody = box.GetComponent<Rigidbody>();
                BoxRigidBody.AddForce(PlayerCam.transform.forward * hitForce, ForceMode.Impulse);
                StartCoroutine(deleteLine());

                drawing = true;
                canWhip = false;
            }

            if (Physics.Raycast(forwardRay, out RaycastHit hitt, interactRange, 1, QueryTriggerInteraction.Ignore) && Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (hitt.transform.CompareTag("Pullable") || hitt.transform.CompareTag("MoveableObj"))
                {
                    StartCoroutine(canwhip());
                    Enemy = hitt.transform.gameObject;
                    Enemypos = Enemy.transform.position;
                    lineObject = Instantiate(linePrefab);
                    linerenderer = lineObject.GetComponent<LineRenderer>();
                    Pulling = true;
                    StartCoroutine(DrawBezier());
                    StartCoroutine(deleteLine());
                    drawing = true;
                    canWhip = false;
                }
            }
            

        }

       

        if (pushing && currentEnemyRigidbody != null)
        {
            enemyNavMeshAgent.enabled = false;
            // Optionally, you can add ongoing actions while pushing an enemy here
            
            // Check if the enemy has stopped moving, and if so, set it back to the "stopped" state
            if (currentEnemyRigidbody.velocity.magnitude < 0.3f)
            {
                StartCoroutine(StopDelay());
            }
        }
        if (drawing)
            {
            linerenderer.SetPosition(0, whippp.transform.position);
            }
        if (destroyLine)
        {
            drawing = false;
            Destroy(linerenderer);
            Destroy(lineObject);
            destroyLine = false;
        }
        
    }

    public void PullObj(List<Vector3> PointList)
    {
        StartCoroutine(MoveAlongWaypoints(PointList));
    }

    public IEnumerator RemovingLine(List<Vector3> waypoints)
    {
        yield return new WaitForSeconds(.04f);
        for (int i = waypoints.Count - 1; i >= 1; i--)
        {
            waypoints.RemoveAt(i);
            UpdateLineRenderer(waypoints);
            yield return new WaitForSeconds(.01f);
        }
    }

    private IEnumerator MoveAlongWaypoints(List<Vector3> waypoints)
    {
        Vector3 initialPos = Enemy.transform.position;

        for (int i = waypoints.Count -1; i >= 1; i--)
        {
            Vector3 targetPosition = waypoints[i];
            float elapsedTime = 0;

            while (elapsedTime < 1)
            {
                Enemy.transform.position = Vector3.Lerp(initialPos, targetPosition, elapsedTime);
                elapsedTime += Time.deltaTime * movementSpeed;
                yield return null;
            }

            Enemy.transform.position = targetPosition; // Ensure the object reaches exactly the target
            initialPos = targetPosition; // Update initialPos for the next iteration
            waypoints.RemoveAt(i);
            UpdateLineRenderer(waypoints);
        }
    }

    private void UpdateLineRenderer(List<Vector3> updatedWaypoints)
    {
        linerenderer.positionCount = updatedWaypoints.Count;
        linerenderer.SetPositions(updatedWaypoints.ToArray());
    }

    private void PushEnemy(GameObject enemy)
    {
        enemyNavMeshAgent = enemy.GetComponent<NavMeshAgent>();
        currentEnemyRigidbody = enemy.GetComponent<Rigidbody>();
        

        if (enemyNavMeshAgent != null && currentEnemyRigidbody != null)
        {
            enemyNavMeshAgent.isStopped = true; // Stop the enemy's NavMeshAgent movement
            currentEnemyRigidbody.isKinematic = false; // Allow the enemy's Rigidbody to be affected by forces

            StartCoroutine(PushDelay(currentEnemyRigidbody, enemyNavMeshAgent));
        }
    }
    Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }

    IEnumerator PushDelay(Rigidbody enemyRigidbody, NavMeshAgent enemyNavMeshAgent)
    {
        
        enemyRigidbody.AddForce(PlayerCam.transform.forward * hitForce, ForceMode.Impulse);
        yield return new WaitForSeconds(0.1f);
        pushing = true;
        // Optionally, you can add a delay to set the enemy back to a "stopped" state
    }

    IEnumerator StopDelay()
    {
        
        yield return new WaitForSeconds(.1f); 
        currentEnemyRigidbody.isKinematic = true; 
        
        enemyNavMeshAgent.enabled = true;
        Enemy.GetComponent<AiMovement>().pushed = true;
        Enemy.GetComponent<AiMovement>().pushing = false;
        enemyNavMeshAgent.isStopped = false;
        destroyLine = true;
        pushing = false;
     
    }

    IEnumerator DrawBezier()
    {
        Vector3 EnemyPos = Enemypos;
        Vector3 initialPos = whippp.transform.position;
        float randoNumber = Random.Range(1,2);
        float randoNumber2 = Random.Range(-1,-2);
        Vector3 InterpolatedPosition = Vector3.Lerp(initialPos, EnemyPos, .5f);
        Vector3 InterpolatedPositionHalf = Vector3.Lerp(initialPos, EnemyPos, .25f) + Vector3.up * randoNumber;
        Vector3 InterpolatedPosition2Qt = Vector3.Lerp(initialPos, EnemyPos, .75f) + Vector3.up * randoNumber2;



        List<Vector3> pointList = new List<Vector3>();
        for (int i = 0; i < vertexCount/2; i++)
        {
            float t = i / (float)(vertexCount/2 - 1);
            Vector3 point = CalculateQuadraticBezierPoint(t, initialPos, InterpolatedPositionHalf, InterpolatedPosition);
            pointList.Add(point);
            linerenderer.positionCount = i + 1;
            linerenderer.SetPosition(i, point);
            yield return new WaitForSeconds(.005f);
        }

        for (int e = vertexCount/2; e < vertexCount; e++)
        {
            float t = (e - vertexCount / 2) / (float)(vertexCount / 2 - 1);
            Vector3 point2 = CalculateQuadraticBezierPoint(t, InterpolatedPosition, InterpolatedPosition2Qt, EnemyPos);
            pointList.Add(point2);
            linerenderer.positionCount = e + 1;
            linerenderer.SetPosition(e, point2);
            yield return new WaitForSeconds(.005f);
        }
        if (Pulling) {
            PullObj(pointList);
        }
        else
        {
            StartCoroutine (RemovingLine(pointList));
        }
        Pulling = false;
        
    }

    IEnumerator canwhip ()
    {
        yield return new WaitForSeconds (3);
        canWhip = true;
    }
    IEnumerator deleteLine()
    {
        yield return new WaitForSeconds(1.5f);
        destroyLine = true;
    }
}



