using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardPlayer : MonoBehaviour {
    float speed = 2f;
    Transform playerPos;
    public bool go = true; 

    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").transform;

            transform.LookAt(playerPos);
        }        
    }

    void Update () {
        if (playerPos != null && go)
        {
            transform.position = Vector3.Lerp(this.transform.position, playerPos.position, speed * Time.deltaTime);
        }
    }
}
