using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chord
{

    int note;
    PitchUtils.ChordType type;

    Dictionary<int, List<int>> transitions;

    public Chord(int note, PitchUtils.ChordType type)
    {
        this.note = note;
        this.type = type;
        this.transitions = new Dictionary<int, List<int>>();
    }

    public int getNote()
    {
        return note;
    }

    public PitchUtils.ChordType getChordType()
    {
        return type;
    }

    public int nextChord(int transNote)
    {
        try
        {
            List<int> choices = transitions[transNote];
            int choice = Random.Range(0, choices.Count);
            return choices[choice];
        }
        catch (KeyNotFoundException e)
        {
            Debug.LogError("Chord Transition Not Found: " + transNote);
        }
        return 0;

    }

    public bool containsTransition(int transNote)
    {
        return transitions.ContainsKey(transNote);
    }

    public void addTransition(int note, int chord)
    {
        //Debug.Log("Chord " + (this.note) + " transition: " + (note) + " --> " + (chord)); 
        if (!transitions.ContainsKey(note))
        {
            transitions[note] = new List<int>();
        }
        if (!transitions[note].Contains(chord))
        {
            transitions[note].Add(chord);
        }
    }

    public override bool Equals(object obj)
    {
        try
        {
            Chord objChord = (Chord)obj;
            if ((objChord.note == this.note) && (objChord.type == this.type))
            {
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }

    }
}