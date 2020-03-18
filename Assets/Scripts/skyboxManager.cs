using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxManager : MonoBehaviour {

    public float glowRatio;
    private float lerpSpeed = 5;
    private float maxJump = 50f;  // This prevents sudden large jumps
    private float curFreq; 
    private float[] samples;

    // FFA0F2

    private void Start()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1.3f);
    }

    void Update () {
        float newFreq = PitchDetection.s.frequencyFloat;
        newFreq = Mathf.Clamp(newFreq, curFreq - maxJump, curFreq + maxJump);

        curFreq = Mathf.Lerp(curFreq, newFreq, Time.deltaTime * lerpSpeed);  
        RenderSettings.skybox.SetFloat("_Exposure", curFreq / 300);

        RenderSettings.fogEndDistance = -8.6f * curFreq + 6720;
	}

    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1.3f);
        RenderSettings.fogEndDistance = 5; 
    }
}
