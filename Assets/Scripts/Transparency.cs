using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparency : MonoBehaviour {
    [Range(0,1)]
    public float transparency;

    private MeshRenderer renderer; 

    private void Awake()
    {
        renderer = this.GetComponent<MeshRenderer>();
        transparency = renderer.material.color.a; 
    }

    private void Update()
    {
        float curR = renderer.material.color.r;
        float curG = renderer.material.color.g;
        float curB = renderer.material.color.b;
        renderer.material.color = new Color(curR, curG, curB, transparency);
    }
}
