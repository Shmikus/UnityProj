using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public ProgressBar Pb;
    public GameObject cam1;
    public GameObject cam2;
    bool Tiny = false;
    bool Deployed = false;
    public GameObject SpaceCheck;

    public Quaternion ogRotation;
    MiniMovement minimovement;
    [SerializeField] GameObject player;
    public GameObject mini;
    Collider spaceCollider;
    int SpawnDistance = 1;
    float DeployTime = 15;
    float DeployCooldown = 15;
    float FifteenConversion = 6.6f;






    void Start()
    {
        spaceCollider = SpaceCheck.GetComponent<Collider>();
        Pb.BarValue = DeployTime * FifteenConversion;
    }

    // Start is called before the first frame update


    IEnumerator Teleport()
    {
        mini.GetComponent<MiniMovement>().enabled = false;
        Vector3 SpawnPos = player.transform.position + player.transform.forward * SpawnDistance;
        
        mini.transform.position = SpawnPos;
        yield return new WaitForSeconds(0.02f);
        mini.GetComponent<MiniMovement>().enabled = true;
        

    }

    void DeployTimer()
    {
        if (Tiny)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                DeployTime -= Time.deltaTime;
                Pb.BarValue = DeployTime * FifteenConversion;
                if (DeployTime <= 0)
                {
                    switchBack();
                    Deployed = false;
                    DeployTime = DeployCooldown;
                   
                }
            }
        }
    }
    void switchBack()
    {
        
        {
            cam1.SetActive(true);
            player.GetComponent<playermovement>().enabled = true;
            mini.GetComponent<MiniMovement>().enabled = false;
            cam2.SetActive(false);
            mini.GetComponent<Rigidbody>().isKinematic = false;
            mini.GetComponent<Rigidbody>().useGravity = true;
            mini.GetComponent<Rigidbody>().detectCollisions = true;
            
            SpaceCheck.SetActive(false);
            Tiny = false;
            
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("M") && !IsColliding())
        {
                
                player.GetComponent<playermovement>().enabled = false;
                SpaceCheck.SetActive(true);
                cam1.SetActive(false);
                mini.SetActive(true);
                
                mini.GetComponent<Rigidbody>().useGravity = false;
                mini.GetComponent<Rigidbody>().detectCollisions = false;
                mini.GetComponent<Rigidbody>().isKinematic = true;
                cam2.SetActive(true);
                mini.GetComponent<MiniMovement>().enabled = true;
                if (!Deployed)
                {
                    ogRotation = mini.transform.rotation;
                    StartCoroutine(Teleport());
                }
            mini.transform.rotation = ogRotation;
            SpaceCheck.SetActive(false);

            Tiny = true;
            Deployed = true;




        }
        if (Input.GetButtonDown("K"))
        {
            switchBack();
        }

        DeployTimer();
        
    }
    private bool IsColliding()
    {
        
        Collider[] colliders = Physics.OverlapBox(spaceCollider.bounds.center, spaceCollider.bounds.extents, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider != spaceCollider)
            {
                return true;
            }
        }
        return false;
    }
}


    

