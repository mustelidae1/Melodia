using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class PitchUtils {
    public static int tonic = 0;

    // NOTE: note names would now need to be shifted to be accurate 
    public static string[] noteNames = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" }; // maybe we should have one flat and one sharp that we can switch between? 

    public static int[] all = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
    public static int[] ionian = { 0, 2, 4, 5, 7, 9, 11 };
    public static int[] dorian = { 0, 2, 3, 5, 7, 9, 10 };
    public static int[] lydian = { 0, 2, 4, 6, 7, 9, 11 };
    public static int[] aeolian = { 0, 2, 3, 5, 7, 8, 10 };
    public static int[] phyrigian = { 0, 1, 3, 5, 7, 8, 10 };
    public static int[] myxolydian = { 0, 2, 4, 5, 7, 9, 10 };
    public static int[] locrian = { 0, 1, 3, 5, 6, 8, 10 };
    public static int[] blues = { 0, 3, 5, 6, 7, 10 };
    public static int[] wholeTone = { 0, 2, 4, 6, 8, 10 };
    public static int[] pentatonic = { 1, 4, 6, 8, 11 };

    // Note: this changes based on user's range at the beginning of the game if tutorial happens
    public static float[] noteFreq =
    {
        261.626f,
        277.183f,
        293.665f,
        311.127f,
        329.628f,
        349.228f,
        369.994f,
        391.995f,
        415.305f,
        440.000f,
        466.164f,
        493.883f
    };

    public enum Mode
    {
        All = 0,
        Ionian = 1,
        Dorian = 2,
        Lydian = 3,
        Aeolian = 4,
        Phyrigian = 5,
        Myxolydian = 6,
        Locrian = 7,
        Blues = 8,
        WholeTone = 9,
        Pentatonic = 10
    }

    public static int[][] ModeNotes =
    {
       all,
       ionian,
       dorian,
       lydian,
       aeolian,
       phyrigian,
       myxolydian,
       locrian,
       blues,
       wholeTone,
       pentatonic
    };

    public enum ChordType
    {
        Major = 0,
        Minor = 1,
        Sus = 2,
        Maj7 = 3,
        Min7 = 4,
        Dim = 5,
        Aug = 6
    }

    public static int[][] arpeggioPatterns =
    {
       // new int[] { 0, 2, 4, 2 },  // basic arpeggio 
       // new int[] { 0, 4, 2, 4 },  // alberti bass 
       // new int[] { 0, 4, -1, 4},  
       // new int[] { -1, 0, -1, 0}, // offbeat goodness   
        new int[] {0, 2, 4}, // likely 
        new int[] {1, 3, 5, 6} // unlikely 
    };

    public static int[] chordTones =
    {
        5,  // piano
        //8,  // celeste
        //15, // dulcimer
        //24, // guitar 
        43, // double bass 
        49, // strings
        //51, // more strings 
        //61, // brass section 
        //89, // warm pad
        

    };

    public static int[] melodyTones =
    {
        //40, // violin 
        41, // viola
        42, // cello 
        45, // pizzicato strings 
        60, // french horn 
        //65, // alto sax 
        //68, // oboe
        69, // english horn 
        //71, // clarinet
        //73, // flute 
        74, // recorder
        79, // ocarina
        //110, // fiddle
    };

    public static int[] arpeggioTones =
    {
        5, // piano
        //9, // glokenspiel 
        10, // music box 
        11, // vibraphone 
        12, // marimba 
        //13, // xylophone 
        //14, // tubular bells 
        //15, // dulcimer 
        24, // nylon guitar 
        25, // steel guitar
        //31, // guitar harmonics 
        32, // acoustic bass 
        45, // pizzicato strings 
        46, // harp 

    }; 

    public static Color mapNoteToRBG(int noteIndex)
    {
        float hue = map((float)noteIndex, 0f, 11f, 0f, 0.9f);
        return Color.HSVToRGB(hue, 1, 1); 
    }

    public static float mapNoteToHue(int noteIndex)
    {
        return map((float)noteIndex, 0f, 11f, 0f, 0.9f);
    }

    public static float map(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }

    public static bool isInRange(int val, int low, int high)
    {
        if (val > high || val < low) return false;
        return true;
    }

    public static int getRandomNoteIndex(Mode mode)
    {        
        return ModeNotes[(int)mode][Random.Range(0, ModeNotes[(int)mode].Length)]; 
    }

    public static int getCloseNoteIndex(Mode mode, int note)
    {
        int rangeLow;
        int rangeHigh; 
        if (note == 0 || note == 1)
        {
            rangeLow = 0;
            rangeHigh = note + 2; 
        } else if (note == ModeNotes[(int)mode].Length - 1 || note == ModeNotes[(int)mode].Length)
        {
            rangeLow = note - 2;
            rangeHigh = ModeNotes[(int)mode].Length; 
        } else
        {
            rangeLow = note - 2;
            rangeHigh = note + 2; 
        }
        return ModeNotes[(int)mode][Random.Range(rangeLow, rangeHigh)]; 
    }

    public static int midiFromFreq(float freq)
    {
        return((int)(57.0f + 12.0f * Mathf.Log10(freq / 440.0f) / Mathf.Log10(2.0f)));
    }

    public static float freqFromMidi(int midi)
    {
        return ((Mathf.Pow(2, (midi - 57.0f) / 12.0f)) * 440f); 
    }

    public static void initalizeNoteFrequencies(int midiNote, Mode mode)
    {
        tonic = midiNote % 12; 
        int curNote = midiNote; 

        // Set up frequencies according to tonic 
        for (int i = 0; i < 12; i++)
        {            
            noteFreq[i] = freqFromMidi(curNote);
            curNote++;  
        }

        // Offset allNotes according to new tonic (to make colors match) 
        int currentIndex = tonic;
        int currentValue = 0; 
        for (int i = 0; i < 12; i++)
        {
            all[currentIndex%12] = currentValue;
            currentValue++;
            currentIndex++; 
        }
    }
}
