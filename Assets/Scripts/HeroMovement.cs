using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{

     float moveSpeed = 10f;

    Vector3 curPos, lastPos;
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        GetComponent<Rigidbody>().velocity = movement * moveSpeed;//*Time.deltaTime;

        curPos = transform.position;
        if (curPos == lastPos)
        {
            GameManager.instance.isWalking = false;
        }
        else
        {
            GameManager.instance.isWalking = true;
        }
        lastPos = curPos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "EnterTown")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }
        if(other.tag == "LeaveTown")
        {
            CollisionHandler col = other.gameObject.GetComponent<CollisionHandler>();
            GameManager.instance.nextHeroPosition = col.spawnPoint.transform.position;
            GameManager.instance.sceneToLoad = col.sceneToLoad;
            GameManager.instance.LoadNextScene();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "region1" || other.tag == "region2"|| other.tag == "region3")
        {
            GameManager.instance.canGetEncounter = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "region1" || other.tag == "region2" || other.tag == "region3")
        {
            GameManager.instance.canGetEncounter = false;
        }
    }
}
