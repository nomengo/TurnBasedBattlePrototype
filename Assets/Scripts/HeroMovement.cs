using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{

     float moveSpeed = 10f;
    void Start()
    {
        
    }

    
    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0f, moveZ);
        GetComponent<Rigidbody>().velocity = movement * moveSpeed;
    }
}
