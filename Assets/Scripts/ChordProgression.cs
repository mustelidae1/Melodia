using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChordProgression
{

    int key;
    PitchUtils.Mode mode;

    Chord curChord;
    int curNote;

    List<Chord> chords;

    // Have some kind of memory? 
    // Have it driven by the melody at first and then switch control to chord progression? 


        // When it resolves it should hold for longer - end of a phrase 
        // Encourage melody notes close to each other 

    public ChordProgression(int key, PitchUtils.Mode mode)
    {
        this.key = key;
        curNote = key;
        //this.curChord = new Chord(curNote, PitchUtils.ChordType.Major);
        this.chords = new List<Chord>();

        // generate from file 
        // -1 to all numbers
        // for each chord listed: 
        // create chord 
        //    for each transition 
        //    figure out notes in chord(using current PitchUtils mode, note, (note + 2) % 7, (note + 4) % 7 )
        //    add transitions to that chord on each of those notes 

        List<int> tempChords = new List<int>();
        List<PitchUtils.ChordType> types = new List<PitchUtils.ChordType>();
        List<List<int>> tempNotes = new List<List<int>>();
        if (mode == PitchUtils.Mode.Ionian)
        {
            tempChords = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

            // This is the type of each chord in the key. 1 is major, 2 is minor, etc. 
            types = new List<PitchUtils.ChordType>() {
                PitchUtils.ChordType.Major,
                PitchUtils.ChordType.Minor,
                PitchUtils.ChordType.Minor,
                PitchUtils.ChordType.Major,
                PitchUtils.ChordType.Major,
                PitchUtils.ChordType.Minor,
                PitchUtils.ChordType.Dim
            };

            // These are the rules for the chord progression. 
            // 1 can transition to any chord, 2 can transition to 2 5 and 7, etc. 
            tempNotes = new List<List<int>>() {
                new List<int>() {1,2,3,4,5,6,7},
                new List<int>() {2,5,7},
                new List<int>() {3,4,6},
                new List<int>() {2,4,5},
                new List<int>() {1,5,6},
                new List<int>() {2,4,6},
                new List<int>() {1,7}
            };
        }
        else if (mode == PitchUtils.Mode.Aeolian)
        {
            tempChords = new List<int>() { 1, 2, 3, 4, 5, 6, 7 };

            // This is the type of each chord in the key. 1 is major, 2 is minor, etc. 
            types = new List<PitchUtils.ChordType>() {
                PitchUtils.ChordType.Minor,
                PitchUtils.ChordType.Dim,
                PitchUtils.ChordType.Major,
                PitchUtils.ChordType.Minor,
                PitchUtils.ChordType.Minor,
                PitchUtils.ChordType.Major,
                PitchUtils.ChordType.Major
            };

            // These are the rules for the chord progression. 
            // 1 can transition to any chord, 2 can transition to 2 5 and 7, etc. 
            tempNotes = new List<List<int>>() {
                new List<int>() {1,2,3,4,5,6,7},
                new List<int>() {2,5,7},
                new List<int>() {3,4,6},
                new List<int>() {2,4,5},
                new List<int>() {1,5,7},
                new List<int>() {2,4,6},
                new List<int>() {1,7}
            };
        }
        else
        {
            Debug.LogError("Cannot Create Chord pattern for current mode");
            return;
        }


        for (int i = 0; i < tempChords.Count; i++)
        {
            Chord c = new Chord(tempChords[i] - 1, types[i]); // subtracting 1 to make our system 0 based 
            // why the heck did you do this Olivia 
            // tempChords[i]-1 is literally equal to i 
            // WHY 

            // Add transition to the chord 
            for (int j = 1; j < 8; j++)
            {
                if (tempNotes[i].Contains(j))
                {
                    int curTransition = j; //one-based index of a chord we can transition to

                    // Get all of the notes in the chord. We can transition to this chord on any of these notes. 
                    int transNote1 = curTransition - 1;
                    int transNote2 = ((curTransition - 1) + 2) % 7;
                    int transNote3 = ((curTransition - 1) + 4) % 7;

                    // Add the transitions 
                    c.addTransition(transNote1, j - 1);
                    c.addTransition(transNote2, j - 1);
                    c.addTransition(transNote3, j - 1);
                }
            }
            // self loops for notes that we didn't add already 
            for (int k = 0; k < 7; k++)
            {
                if (!c.containsTransition(k))
                {
                    c.addTransition(k, c.getNote());
                }
            }
            addChord(c);
        }
        this.curChord = chords[0];
    }

    public void addChord(Chord chord)
    {
        chords.Add(chord);
    }

    public Chord nextChord(int note)
    {
        int nextChord = curChord.nextChord(note);
        foreach (Chord c in chords)
        {
            if (c.getNote() == nextChord)
            {
                curChord = c;
                return c;
            }
        }
        return curChord;
    }

}
