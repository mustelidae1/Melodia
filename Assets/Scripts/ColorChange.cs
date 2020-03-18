using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ColorChange : MonoBehaviour {
    public RawImage im;
    public GameObject leftHand;
    public GameObject rightHand; 
    private float hue = 0;
    private float saturation = 1;
    private float value = 1;    

    private void Start()
    {
        im.color = Color.black;
    }

    private void Update()
    {
        if (MicrophoneFeed.MicLoudness < MicrophoneFeed.MicSensitivity)
        {
            leftHand.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
            rightHand.GetComponent<SkinnedMeshRenderer>().material.color = Color.white;
            //im.color = Color.black; 
            return; 
        }

        float newHue = PitchUtils.mapNoteToHue(PitchDetection.s.m_noteIndex);
        float prevHue = hue;

        Color prevRbg = Color.HSVToRGB(prevHue, saturation, value);
        Color newRbg = Color.HSVToRGB(newHue, saturation, value);

        leftHand.GetComponent<SkinnedMeshRenderer>().material.color = newRbg;
        rightHand.GetComponent<SkinnedMeshRenderer>().material.color = newRbg;

        im.color = newRbg;

        float h;
        float s;
        float v;
        Color.RGBToHSV(newRbg, out h, out s, out v);

        hue = h;
    }

    private void OnApplicationQuit()
    {
        RenderSettings.skybox.SetColor("_SkyTint", new Color32(0xFF, 0xA0, 0xF2, 0xFF));
    }
}
