using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.UI; 

public class PitchDetection : MonoBehaviour
{
    public static PitchDetection s;
    public bool manualOverride = false; 

    [DllImport("AudioPluginDemo")]
    private static extern float PitchDetectorGetFreq(int index);

    [DllImport("AudioPluginDemo")]
    private static extern int PitchDetectorDebug(float[] data);

    //public Material mat;
    public string frequency = "detected frequency";
    public float frequencyFloat = 0; 
    public string note = "detected note";
   
    public int prevNoteIndex = 0;

    [Range(0, 11)]
    public int m_noteIndex = 0;
    public AudioMixer mixer;

    public Text text;

    public bool generateMusic;

    public delegate void NoteChanged(int note, int octave);
    public delegate void NoteChangedPureMidi(int note); 
    public static event NoteChanged OnNoteChanged;
    public static event NoteChangedPureMidi OnNoteChangedPureMidi; 
    
    void Start()
    {
        s = this;
        OnNoteChangedPureMidi += stop; 
    }
    
    void Update()
    {
        float freq = PitchDetectorGetFreq(0), deviation = 0.0f;
        frequencyFloat = freq;
        frequency = freq.ToString() + " Hz";

        if (freq > 0.0f && MicrophoneFeed.MicLoudness > MicrophoneFeed.MicSensitivity)
        {
            float noteval = 57.0f + 12.0f * Mathf.Log10(freq / 440.0f) / Mathf.Log10(2.0f);
            float f = Mathf.Floor(noteval + 0.5f);            
            deviation = Mathf.Floor((noteval - f) * 100.0f);
            int noteIndex = (int)f % 12; // mod by 12 to get the note in the octave 
            noteIndex = PitchUtils.all[noteIndex]; 
            if (!manualOverride)
            {
                m_noteIndex = noteIndex;
                
            }
            if (m_noteIndex != prevNoteIndex)  // Note has changed 
            {
                prevNoteIndex = m_noteIndex; 
            }
            int octave = (int)Mathf.Floor((noteval + 0.5f) / 12.0f);
            note = PitchUtils.noteNames[noteIndex] + " " + octave;

            if (generateMusic)
            {
                Spawn.s.spawn = false;
                Spawn.s.spawnSixteenths = true; 
                Spawn.s.changeChord((int)f % 12);
            }

            //OnNoteChanged(noteIndex, octave);
            OnNoteChangedPureMidi((int)f); 

            
        }
        else
        {
            note = "unknown";
        }

        if (text != null)
            text.text = "Detected frequency: " + frequency + "\nDetected note: " + note + " (deviation: " + deviation + " cents)";
    }

    public void stop(int note)
    {

    }
}
