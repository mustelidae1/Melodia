using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollider : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle" && PitchDetection.s.m_noteIndex == other.GetComponent<NoteData>().getNoteIndex())
        {
            Spawn.s.correctNote(); 
            //other.GetComponent<Animator>().SetTrigger("SphereFade");
            other.GetComponent<Renderer>().enabled = false;
            other.GetComponent<SphereCollider>().enabled = false; 
            other.GetComponent<MoveTowardPlayer>().go = false;
            other.GetComponent<AudioSource>().enabled = false; 
            other.GetComponent<ParticleSystem>().Play();
            other.GetComponent<ParticleSystem>().transform.SetParent(null); // detach the particle system so it plays 
            Destroy(other); // destroy the orb 
            SounndFX.s.playSuccess();
            
            // Initial menu logic 
            if (!GameManager.s.gameStarted())
            {
                GameManager.s.startGame(); 
            }
            else if (!GameManager.s.setMode())
            {
                switch (other.GetComponent<NoteData>().getNoteIndex())
                {
                    case 0:
                        GameManager.s.setGameMode(0);
                        break;
                    case 4:
                        GameManager.s.setGameMode(1);
                        break;
                    default:
                        break; 
                }
                    
            }
        }
    }
}
