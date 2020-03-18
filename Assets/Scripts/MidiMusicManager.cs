using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;

public class MidiMusicManager : MonoBehaviour
{

    public MidiPlayer midiChords;
    public MidiPlayer midiMelody;
    public MidiPlayer midiArpeggio;  
    public static MidiMusicManager s;

    public bool playMusic; 

    int prevChord = 0;
    int curChord = 0;
    PitchUtils.ChordType curChordType = PitchUtils.ChordType.Major; 
    int prevMelody = 0;

    int arpeggioPattern = 0;
    int arpeggioStep = 0;

    private void Start()
    {
        s = this;
        
        // choose random instruments
        midiChords.changeTone(PitchUtils.chordTones[UnityEngine.Random.Range(0, PitchUtils.chordTones.Length)]);
        midiMelody.changeTone(PitchUtils.melodyTones[UnityEngine.Random.Range(0, PitchUtils.melodyTones.Length)]);
        midiArpeggio.changeTone(PitchUtils.arpeggioTones[UnityEngine.Random.Range(0, PitchUtils.arpeggioTones.Length)]);
    }

    public void changeNote(int melody, int chordBass, PitchUtils.ChordType chordType)
    {      
        if (!playMusic) return;
        curChord = chordBass;
        curChordType = chordType; 
        //Debug.Log("Chord: " + chordBass); // Note is between 0 and 6 
        //Debug.Log("Melody: " + melody); // Note is also between 0 and 6

        if (!(melody == prevMelody)) // don't play repeating melody notes 
        {           
            midiMelody.CurrentNote = getModeNote(melody) + 48; 

            midiMelody.PlayOneNote();
            prevMelody = melody;
        }

        if (chordBass == prevChord) return; // don't play the same chord again if it hasn't changed 
         
        prevChord = chordBass;
        int MidiNote = -1; // We need to map it to the correct Midi Note between 0 and 12 
        MidiNote = getModeNote(chordBass); 

        int inversion = -1;
        if (melody == chordBass)
        {
            inversion = 1;
        }
        else if ((melody == (chordBass + 3)) || (melody == (chordBass + 4)))
        {
            inversion = 2;
        }

        midiChords.CurrentNote = MidiNote + 48; // put it in a reasonable octave         
        if (chordType == PitchUtils.ChordType.Major)
        {
            midiChords.PlayMajorChord(inversion);
        }
        else if (chordType == PitchUtils.ChordType.Minor)
        {
            midiChords.PlayMinorChord(inversion);
        }
        else if (chordType == PitchUtils.ChordType.Dim)
        {
            midiChords.PlayDiminishedChord(inversion);
        }
    }

    // use commented code for a more predictable arpeggio pattern 
    public void arpeggiate()
    {
        //int curNote = PitchUtils.arpeggioPatterns[arpeggioPattern][arpeggioStep];
        int arpeggioPatternRand = UnityEngine.Random.Range(0, 10);
        if (arpeggioPatternRand > 8)
        {
            arpeggioPattern = 1; // rare arpeggio notes
        } else
        {
            arpeggioPattern = 0; // common arpeggio notes
        }
        int curNote = PitchUtils.arpeggioPatterns[arpeggioPattern][UnityEngine.Random.Range(0, PitchUtils.arpeggioPatterns[arpeggioPattern].Length)];       
        //arpeggioStep++;
        if (curNote == -1) return;
        midiArpeggio.CurrentNote = getModeNote(curChord + curNote) + 48;        
         midiArpeggio.PlayOneNote(); 
         
         //if (arpeggioStep >= PitchUtils.arpeggioPatterns[arpeggioPattern].Length)
         //{
         //    arpeggioStep = 0; 
         //} 
    }

    // expects an input between 0 and 6 to represent scale tones 
    // Transforms scale tone to actual midi note based on current key and mode 
    private int getModeNote(int note)
    {
        if (note > 6)
        {
            return ((PitchUtils.ModeNotes[(int)Spawn.s.mode][(note % 6)-1] + PitchUtils.tonic) % 12) + 12;
            // please don't ask me why subtracting 1 works. I have no idea. 
        }
        return ((PitchUtils.ModeNotes[(int)Spawn.s.mode][note] + PitchUtils.tonic) % 12); 
    }
}