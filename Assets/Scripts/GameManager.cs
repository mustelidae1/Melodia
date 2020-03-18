using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour {

    public static GameManager s; 

    public Spawn spawn;
    public Text tutorialText;
    public GameObject colorBar; 
    public bool tutorial;
    public Animator textAnim;
    public RawImage tutorialImage; 

    private int oldBpm;
    private bool startedGame = false;
    private bool modeSet = false; 

    private bool gotFirstNote = false;
    int firstNote = -1;

    // menu spheres 
    private GameObject sphere1;
    private GameObject sphere2;
    
	void Start () {
        s = this; 
		if (tutorial)
        {             
            oldBpm = spawn.tempoBPM;
            spawn.tempoBPM = 0;

            StartCoroutine(Tutorial());
        } else
        {
            startedGame = true; 
        }
        PitchDetection.OnNoteChangedPureMidi += getFirstNote; 
    }

    IEnumerator Tutorial()
    {
        MicrophoneFeed.MicSensitivity = 0.01f; 
        tutorialImage.gameObject.SetActive(true);

        setText("Welcome to Melodia!");
        //textAnim.SetTrigger("fadeIn");         

        yield return new WaitForSeconds(5);

        //textAnim.SetTrigger("transition");
        //yield return new WaitForSeconds(0.5f); 
        setText("When you sing, your hands change color. Try it!");
        
        while (!gotFirstNote) yield return new WaitForSeconds(0.001f); // wait to get first note
        setText("Nice job!");
        //Debug.Log("First note: " + firstNote);

        PitchUtils.initalizeNoteFrequencies(firstNote, Spawn.s.mode); 

        yield return new WaitForSeconds(1);
         
        // Make orb appear and stop near them 
        GameObject sphere = spawn.spawnNote(0);
        MoveTowardPlayer m = sphere.GetComponent<MoveTowardPlayer>(); 

        yield return new WaitForSeconds(2f);

        m.go = false;
        //textAnim.SetTrigger("transition");
        //yield return new WaitForSeconds(0.5f);
        setText("It's an orb! Listen and match the color, then touch it with your hand.");

        // Once they hit it, start the game (called in onCollisionEnter of orb) 
        
    }

    public void getFirstNote(int note)
    {
        if (!gotFirstNote)
        {
            firstNote = note;
            StartCoroutine(checkDuration(note));
        }       
    }

    IEnumerator checkDuration(int note)
    {
        yield return new WaitForSeconds(1f); 
        if (firstNote == note && MicrophoneFeed.MicLoudness > MicrophoneFeed.MicSensitivity)
        {
            gotFirstNote = true; 
            PitchDetection.OnNoteChangedPureMidi -= getFirstNote;
            MicrophoneFeed.MicSensitivity = 0.0001f;
        }
    }

    public void startGame()
    {
        //textAnim.SetTrigger("transition");
        startedGame = true;
        StartCoroutine(ChooseMode()); 
       
    }

    IEnumerator ChooseMode()
    {       
        yield return new WaitForSeconds(1); 
        setText("Now do you want to play the game (red) or free mode (green)?");
        sphere1 = spawn.spawnNote(0, 0);
        sphere2 = spawn.spawnNote(4, 1);
        MoveTowardPlayer m1 = sphere1.GetComponent<MoveTowardPlayer>();
        MoveTowardPlayer m2 = sphere2.GetComponent<MoveTowardPlayer>();

        yield return new WaitForSeconds(2f);

        m1.go = false;
        m2.go = false;

    }

    public void setGameMode(int mode)
    {
        setText("Good choice!");
        tutorial = false; 
        Destroy(sphere1);
        Destroy(sphere2);

        // For now only Ionian and Aeolian are supported, so we choose a random one between the two 
        int musicMode = Random.Range(0, 2);
        switch (musicMode)
        {
            case 0:
                Spawn.s.mode = PitchUtils.Mode.Ionian;
                break;
            case 1:
                Spawn.s.mode = PitchUtils.Mode.Aeolian;
                break;
            default:
                break; 
        }
        modeSet = true;
        if (mode == 0)
        {
            Invoke("goTime", 5f);
        } 
        if (mode == 1)
        {
            Invoke("freeMode", 3f);
        }
    }

    private void freeMode()
    {
        PitchUtils.initalizeNoteFrequencies(60, Spawn.s.mode); //TODO: check if it works without this in vr 
        setText("Sing and the music will follow you!");
        PitchDetection.s.generateMusic = true;
        Spawn.s.spawn = false;
        Invoke("goAwayText", 3); 
    }

    private void goTime()
    {
        goAwayText(); 
        spawn.tempoBPM = oldBpm;       
        PitchDetection.s.generateMusic = false;
        Spawn.s.spawn = true; 
        colorBar.GetComponent<RawImage>().enabled = false; 
    }

    private void goAwayText()
    {
        textAnim.SetTrigger("fadeOut");
        setText("");
    }

    public bool gameStarted()
    {
        return startedGame; 
    }

    public bool setMode()
    {
        return modeSet; 
    }

    void setText(string text)
    {
        tutorialText.text = text; 
    }
}
