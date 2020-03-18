using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class MicrophoneFeed : MonoBehaviour
{
    public bool useMicrophone = true;

    private AudioSource source;
    private string device;
    private bool prevUseMicrophone = false;
    private AudioClip prevClip = null;

    int sampleWindow = 128;
    public static float MicLoudness;
    public static float MicSensitivity = 0.0001f; 
    float volumeBuffer = 0.1f;
    float bufferDecrease = 0.1f;
    float highestVolume = 0.2f;

    public Transform hand1;
    public Transform hand2; 

    void Update()
    {
        if (useMicrophone != prevUseMicrophone)
        {
            prevUseMicrophone = useMicrophone;
            if (useMicrophone)
            {
                foreach (string m in Microphone.devices)
                {
                    device = m;
                    break;
                }

                source = GetComponent<AudioSource>();
                prevClip = source.clip;
                source.Stop();
                source.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);
                source.Play();

                int dspBufferSize, dspNumBuffers;
                AudioSettings.GetDSPBufferSize(out dspBufferSize, out dspNumBuffers);
                
                source.timeSamples = (Microphone.GetPosition(device) + AudioSettings.outputSampleRate - 3 * dspBufferSize * dspNumBuffers) % AudioSettings.outputSampleRate;
            }
            else
            {
                Microphone.End(device);
                source.clip = prevClip;
                source.Play();
            }
        }

        MicLoudness = LevelMax();

        if (MicLoudness > volumeBuffer)
        {
            volumeBuffer = MicLoudness;
            bufferDecrease = 0.0005f;
        }
        if (MicLoudness < volumeBuffer)
        {
            volumeBuffer -= bufferDecrease;
            bufferDecrease *= 1.2f;
        }

        if (volumeBuffer > highestVolume)
        {
            highestVolume = volumeBuffer;
        }

        //Debug.Log(volumeBuffer);

        float scale = PitchUtils.map(volumeBuffer, 0, highestVolume, 0.75f, 1.5f);

        hand1.localScale = new Vector3(scale, scale, scale);
        hand2.localScale = new Vector3(scale, scale, scale); 
    }

    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        source.clip.GetData(waveData, micPosition);
        //Debug.Log(waveData[0]); 
        // Getting a peak on the last 128 samples
        for (int i = 0; i < sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }
}
