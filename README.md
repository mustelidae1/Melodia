This repo includes all of the Unity script assets for Melodia. 

[Demo Video](https://www.youtube.com/watch?v=vP3Afu5f-BE&t=76s)

## Synopsis

Melodia is a VR game about singing! Each note is represented by a color, and as 
colored spheres fly at you, you have to sing the right note and hit each 
orb with your hands. 

The game also includes a free mode where the background music follows 
your voice. 

## Motivation

The main goal of Melodia is to create a positive feedback space for people who sing 
and are learning to sing. The environment changes as users get more notes right, 
allowing them to visualize their voice and their success. When singers can hear themselves 
well they often sing better, and Melodia is designed to extend this idea into a visual space.  

## Installation

To play, simply download the Melodia.exe file and run it! The Melodia_Data folder needs
to be kept in the same folder as the executable for the game to work. You will also need 
SteamVR installed and any additional software required by your VR headset. 

## How to Play 

The tutorial at the beginning will walk you through how to play the game. Your hands will 
change color based on the notes that you are singing.

In game mode, your hands must match the color of each orb to hit it. Once you hit an orb, 
flowers will grow around you! Other elements, such as the brightness of the sky and the size 
of your hands, are also affected by the notes that you are singing. There is no end or win 
condition to the game as it is designed to be more experiential. However, more visual effects 
and changing environments are planned as the user gets more and more notes correct. 

In free mode, just sing and enjoy the adaptive background music! 

## Sources / External Libraries 

**Pitch Detection: Unity Native Audio Plugin**
https://bitbucket.org/Unity-Technologies/nativeaudioplugins

I used code from the Pitch Detection sample scene to implement Melodia's pitch 
detection. 

Important to note is that this example was created in an older version of Unity 
and the way that it records audio will not work with the audio source muted in 
newer versions of Unity. To get around this, I added an audio mixer to the audio 
source and decreased the attenuation volume (under Master) to -80 dB. 

**Value Mapping** 
https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/

I used the algorithm in the first comment on this page, from mgear. 

**Virtual Reality: SteamVR** 
All virtual reality functionality is implemented using SteamVR, specifically the 
[CameraRig] prefab. Hand collision functionality is custom.

