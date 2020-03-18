using UnityEngine;
using System.Collections;

public class cameraFacingBillboard : MonoBehaviour
{
    public Camera m_Camera;

    [Range(-360, 360)]
    public int myrotation = 0; 

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.left);
    }
}
