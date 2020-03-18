using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class headCollider : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle" && !GameManager.s.tutorial)
        {
            Spawn.s.incorrectNote(); 
            Destroy(other.gameObject);
        }
    }
}
