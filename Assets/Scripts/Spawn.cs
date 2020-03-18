using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour {
    public GameObject spherePrefab;
    private GameObject[] spawnPoints;

    [Header("Song Settings")]
    public int tempoBPM; // use this to make sure notes happen on beat
    public PitchUtils.Mode mode; 
    [Range(1, 100)]
    public int activityLevel; // percent liklihood a note will be played on any given beat

    [Range(0, 10)]
    public int melodyChaos; // chance of large melody leaps 

    private float beatsPerSecond;
    private float sixteenthsPerSecond; 
    float nextBeatCounter = 0;
    float nextSixteenthCounter = 0; 

    private int activityLevelIncreaseCount = 0; 
    //private int activityLevelIncreaseThreshhold = 20;
    private int maxActivityLevel = 70;

    ChordProgression chords;

    public static Spawn s;

    public bool spawn;
    public bool spawnSixteenths = false; 

    private int correctInRow = 0;
    private int incorrectInRow = 0;

    int prevNote = 0;

    bool firstBeat = true;

    private int frequencyOfArpeggios; 

    // and keep track of repeating themes to come back to

    // phrases - maybe pick a length for each upcoming phrase? 
    // dynamics - needs to know if it's in a loud/active part of the song or not

    // throw in random seeds somewhere to make it more unpredictable? 

    // make close intervals more likely? Or have a setting that determines how likely they are? 

    // there needs to be a musical heuristic - sense of direction 

    // state machine? Different states based on dynamics, activity, phrase length, etc. 
    // maybe one state machine for melody and one for accompaniment 
    // quiet state -> building state (transition) -> big state  

    // we should introduce syncopation as well 

	void Start () {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        beatsPerSecond = 60f / tempoBPM;
        sixteenthsPerSecond = 15f / tempoBPM;

        nextBeatCounter = beatsPerSecond; // so we get a note on the first beat and arpeggios line up

        chords = new ChordProgression(0, mode);

        if (s != null)
        {
            Destroy(s); 
        }
        s = this;
        frequencyOfArpeggios = Random.Range(0, 10);
    }
	
	void FixedUpdate () {

        if (!spawnSixteenths) return;
        // we have reached a sixteenth 
        if (nextSixteenthCounter >= sixteenthsPerSecond)
        {
            //Debug.Log("Sixteenth");
            nextSixteenthCounter = 0;
            int rand3 = Random.Range(0, 10);
            if (rand3 < frequencyOfArpeggios)
            {
                MidiMusicManager.s.arpeggiate();
            }
        }
        nextSixteenthCounter += Time.deltaTime;

        if (!spawn) return; 

        beatsPerSecond = 60f / tempoBPM; // calculated here so we can change the tempo in the middle of the song
        sixteenthsPerSecond = 15f / tempoBPM;
        int rand = Random.Range(1, 100); 
        
        // we have reached a beat
        if (nextBeatCounter >= beatsPerSecond)
        {
            //Debug.Log("Beat!"); 
            if (rand < activityLevel || firstBeat)
            {
                firstBeat = false; 
                //spawn
                int rand2 = Random.Range(0, melodyChaos + ((prevNote == 5) ? 2 : 0)); // 5 is more likely to lead to the tonic
                                                                                      // (adding a larger number makes a tonic transition more likely)
                int note; 
                if (rand2 < melodyChaos)
                {
                    note = PitchUtils.getRandomNoteIndex(mode);
                } else if (rand2 > 10)
                {
                    note = 0; // going back to the tonic is nice 
                } else 
                {
                    note = PitchUtils.getCloseNoteIndex(mode, prevNote); 
                }
                
                spawnNote(note);
                //Debug.Log("Note: " + note);

                // note is in 0-12 range 
                changeChord(note);  

                prevNote = note;                
            }

            // reset beat counter
            nextBeatCounter = 0;
        }
        
        // based on frame rate (even in fixed update?) - is this bad? seems pretty constant  
        nextBeatCounter += Time.deltaTime; // set to Time.time for a joyful sound       
      
	}

    public void changeChord(int note)
    {
        //Debug.Log("Changing chord to: " + note); 
        int scaleDegree = note; 

        for (int k = 0; k < PitchUtils.ModeNotes[(int)mode].Length; k++)
        {
            if (PitchUtils.ModeNotes[(int)mode][k] == note)
            {
                scaleDegree = k; 
            }
        }
        // at this point scaleDegree will be between 0 and 6 
        Chord newChord = chords.nextChord(scaleDegree);
        MidiMusicManager.s.changeNote(scaleDegree, newChord.getNote(), newChord.getChordType());
    }

    public GameObject spawnNote(int note)
    {
        return spawnNote(note, Random.Range(0, spawnPoints.Length)); 
    }

    public GameObject spawnNote(int note, int spawnLocation)
    {
        GameObject newSphere = Instantiate(spherePrefab) as GameObject;
        newSphere.GetComponent<NoteData>().setNoteIndex(note);
        newSphere.transform.position = spawnPoints[spawnLocation].transform.position;
        return newSphere;
    }

    public void correctNote()
    {
        correctInRow++;
        incorrectInRow = 0; 

        if (correctInRow == 5 && activityLevel < maxActivityLevel)
        {
            activityLevel++; 
        }
    }

    public void incorrectNote()
    {
        incorrectInRow++;
        correctInRow = 0; 

        if (incorrectInRow == 5 && activityLevel > 10)
        {
            activityLevel--; 
        }
    }
}
