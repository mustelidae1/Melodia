using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteData : MonoBehaviour {
    public int noteIndex = 0;
    private Renderer rend;
    private AudioSource audio;
    private ParticleSystem particles;
    public List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    private void Awake()
    {
        rend = this.GetComponent<MeshRenderer>();
        audio = this.GetComponent<AudioSource>();
        particles = this.GetComponent<ParticleSystem>(); 
    }

    public int getNoteIndex()
    {
        return noteIndex;
    }

    public void setNoteIndex(int index)
    {
        if (!PitchUtils.isInRange(index, 0, 11))
        {
            Debug.Log("Invalid Note Index");
            return; 
        }
        noteIndex = index;
        setColor(index);
        setTone(index); 
    }

    private void setColor(int index)
    {
        Color color = PitchUtils.mapNoteToRBG(index);
        rend.material.color = color;
        particles.startColor = color; 
    }

    void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particles.GetCollisionEvents(other, collisionEvents);
        
        int i = 0;

        while (i < numCollisionEvents)
        {
            if (Random.Range(0, 4) == 0)
            {
                GenerateLandscape.s.createFlower(collisionEvents[i].intersection.x, collisionEvents[i].intersection.z, noteIndex);
            }            
            i++;
        }
    }

    private void setTone(int index)
    {
        float freq = PitchUtils.noteFreq[index];

        float curPitch = 440f; // 261.6f;

        float factor = freq / curPitch;
        audio.pitch = factor; 
    }

}
