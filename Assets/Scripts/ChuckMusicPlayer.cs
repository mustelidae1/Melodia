using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuckMusicPlayer : MonoBehaviour {

    public bool musicOn; 
    public static ChuckMusicPlayer s; 

    public void changeNote(int note, PitchUtils.ChordType chordType)
    {
        Debug.Log("Changing note to " + note); 
        //int noteToPlay = (int)PitchUtils.noteFreq[note];
        //Debug.Log(noteToPlay); 
        GetComponent<ChuckSubInstance>().SetInt("note", note);
        GetComponent<ChuckSubInstance>().SetInt("chordType", (int)chordType); // TODO: get this to actually work 
        GetComponent<ChuckSubInstance>().BroadcastEvent("playChordEvent");
    }

    // Note to self: Calling this on update is a very very bad idea. Don't do it. 
    private void doTheChuck()
    {
        GetComponent<ChuckSubInstance>().RunCode(@"
            global int note;
            global int chordType; 
            global Event playChordEvent; 

            Moog chord[3] => dac;

            Gain master => dac;

            for( 0 => int i; i < chord.cap(); i++)
            {
                chord[i] => master;
            }

            .3 => master.gain;

            fun void playChord( int midiNote, string quality, float length) // add inversion 
            {
                5 => int octave; 
                midiNote + (12*octave) => int root; 

                Std.mtof(root) => chord[0].freq;
    
                //
                if( quality == ""major"")
                {
                        Std.mtof(root + 4) => chord[1].freq;
                }
                else if (quality == ""minor"")
                {
                    Std.mtof(root + 3) => chord[1].freq;
                }
                else
                {
                <<< ""Please indicate major or minor"" >>>;
                }


                // fifth
                Std.mtof(root + 7) => chord[2].freq;

                for (1=>int j; j < 3; j++)
                {
                    //0.9 => chord[j].bowPressure;
                    //0.9 => chord[j].bowPosition; 
                    1 => chord[j].noteOn;
                }

                length::ms=> now;

                }

                //[0,0,2,3,3,5,5,7,7,9,9,10] @=> int chordMap[];
                //[""major"",0,2,3,3,5,5,7,7,9,9,10] @=> string chordMajorMap[];

                while( true )
                {
                    playChordEvent => now;
                    if (chordType == 0) { spork ~ playChord( note, ""major"", 600 ); }
                    if (chordType == 1) { spork ~ playChord( note, ""minor"", 600 ); } 
                    else { spork ~ playChord( note, ""major"", 600 ); } 
                 }

                //playChord( 60, ""major"", 600);
                //playChord( 57, ""minor"", 600);
                //playChord( 67, ""major"", 600); 
            ");
    }
    
	void Start () {
        s = this;
        if (musicOn)
        {
            doTheChuck();
        }
    }
}
