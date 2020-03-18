using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MidiPlayerTK;
using System;
using UnityEditor; 

namespace MidiPlayerTK
{
    public class MidiPlayer : MonoBehaviour
    {

        // MPTK component able to play a stream of midi events
        // Add a MidiStreamPlayer Prefab to your game object and defined midiStreamPlayer in the inspector with this prefab.
        public MidiStreamPlayer midiStreamPlayer;

        [Range(0, 127)]
        private int StartNote = 50;

        [Range(0, 127)]
        public int Velocity = 100;

        [Range(0, 16)]
        public int StreamChannel = 0;

        [Range(0, 127)]
        public int CurrentNote;

        [Range(0, 127)]       
        public int CurrentPreset;

        /// <summary>
        /// Current note playing
        /// </summary>
        private MPTKEvent NotePlaying;
        private MPTKEvent Third;
        private MPTKEvent Fifth;

        // Use this for initialization
        void Start()
        {           
            if (midiStreamPlayer != null)
            {
                if (!midiStreamPlayer.OnEventSynthStarted.HasEvent())
                    midiStreamPlayer.OnEventSynthStarted.AddListener(StartLoadingSynth);
                if (!midiStreamPlayer.OnEventSynthStarted.HasEvent())
                    midiStreamPlayer.OnEventSynthStarted.AddListener(EndLoadingSynth);
            }
            else
                Debug.LogWarning("No Stream Midi Player associed to this game object");
           
            CurrentNote = StartNote;

            midiStreamPlayer.MPTK_PlayEvent(new MPTKEvent() { Command = MPTKCommand.PatchChange, Value = CurrentPreset, Channel = StreamChannel, });


        }

        public void changeTone(int newTone)
        {
            CurrentPreset = newTone;
            midiStreamPlayer.MPTK_PlayEvent(new MPTKEvent() { Command = MPTKCommand.PatchChange, Value = CurrentPreset, Channel = StreamChannel, });
        }

        /// <summary>
        /// This call is defined from MidiPlayerGlobal event inspector. Run when SF is loaded.
        /// </summary>
        public void EndLoadingSF()
        {
            Debug.Log("End loading SF, MPTK is ready to play");
            Debug.Log("Load statistique");
            Debug.Log("   Time To Load SoundFont: " + Math.Round(MidiPlayerGlobal.MPTK_TimeToLoadSoundFont.TotalSeconds, 3).ToString() + " second");
            Debug.Log("   Time To Load Waves: " + Math.Round(MidiPlayerGlobal.MPTK_TimeToLoadWave.TotalSeconds, 3).ToString() + " second");
            Debug.Log("   Presets Loaded: " + MidiPlayerGlobal.MPTK_CountPresetLoaded);
            Debug.Log("   Waves Loaded: " + MidiPlayerGlobal.MPTK_CountWaveLoaded);
        }
        public void StartLoadingSynth(string name)
        {
            Debug.LogFormat("Synth {0} loading", name);
        }

        public void EndLoadingSynth(string name)
        {
            Debug.LogFormat("Synth {0} loaded", name);
            Debug.Log("loading synth"); 
            midiStreamPlayer.MPTK_PlayEvent(new MPTKEvent() { Command = MPTKCommand.PatchChange, Value = CurrentPreset, Channel = StreamChannel, });
        }      

        void Update()
        {
            // Check that SoundFont is loaded and add a little wait (0.5 s by default) because Unity AudioSource need some time to be started
            if (!MidiPlayerGlobal.MPTK_IsReady())
                return;
        }

        public void PlayOneNote()
        {           
            StopOneNote();

            NotePlaying = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = CurrentNote,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(NotePlaying);
        }

        public void PlayMajorChord(int inversion)
        {
            int bass = CurrentNote;
            int third = CurrentNote + 4;
            int fifth = CurrentNote + 7; 

            if (inversion == 1)
            {
                bass += 12; 
            } else if (inversion == 2)
            {
                bass += 12;
                third += 12; 
            }

            StopChord(); 
            NotePlaying = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = bass,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(NotePlaying);

            Third = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = third,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(Third);

            Fifth = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = fifth,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(Fifth);
        }

        public void PlayMinorChord(int inversion)
        {
            int bass = CurrentNote;
            int third = CurrentNote + 3;
            int fifth = CurrentNote + 7;

            if (inversion == 1)
            {
                bass += 12;
            }
            else if (inversion == 2)
            {
                bass += 12;
                third += 12;
            }

            StopChord(); 
            NotePlaying = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = bass,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(NotePlaying);

            Third = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = third,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(Third);

            Fifth = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = fifth,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(Fifth);
        }

        public void PlayDiminishedChord(int inversion)
        {
            int bass = CurrentNote;
            int third = CurrentNote + 3;
            int fifth = CurrentNote + 6;

            if (inversion == 1)
            {
                bass += 12;
            }
            else if (inversion == 2)
            {
                bass += 12;
                third += 12;
            }

            StopChord();
            NotePlaying = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = bass,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(NotePlaying);

            Third = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = third,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(Third);

            Fifth = new MPTKEvent()
            {
                Command = MPTKCommand.NoteOn,
                Value = fifth,
                Channel = StreamChannel,
                Duration = 9999999, // 9999 seconds but stop by the new note. See before.
                Velocity = Velocity // Sound can vary depending on the velocity
            };
            midiStreamPlayer.MPTK_PlayEvent(Fifth);
        }

        private void StopOneNote()
        {
            if (NotePlaying != null)
            {
                //Debug.Log("Stop note");
                // Stop the note (method to simulate a real human on a keyboard : duration is not known when note is triggered)
                midiStreamPlayer.MPTK_StopEvent(NotePlaying);
                NotePlaying = null;
            }
        }

        private void StopChord()
        {
            if (NotePlaying != null)
            {
                //Debug.Log("Stop note");
                // Stop the note (method to simulate a real human on a keyboard : duration is not known when note is triggered)
                midiStreamPlayer.MPTK_StopEvent(NotePlaying);
                midiStreamPlayer.MPTK_StopEvent(Third);
                midiStreamPlayer.MPTK_StopEvent(Fifth);
                NotePlaying = null;
            }
        }
    }
}