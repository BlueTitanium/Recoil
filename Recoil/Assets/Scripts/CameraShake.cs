using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraShake : MonoBehaviour
{
    public static CameraShake cs;
    public CinemachineVirtualCamera cam;
    public float shakeLeft = 0;
    public VolumeProfile profile;
    private ChromaticAberration chroma;
    // Start is called before the first frame update
    void Start()
    {
        cs = this;
        profile.TryGet<ChromaticAberration>(out chroma);
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeLeft > 0)
        {
            shakeLeft -= Time.deltaTime;
        } else
        {
            cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
            chroma.intensity.Override(0f);
        }
    }

    public void cameraShake(float time, float amplitude, bool on = true)
    {
        if(Toggles.CameraShake)cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        shakeLeft = time;
        if(on && Toggles.ChromaticAberration)
            chroma.intensity.Override(.3f);
    }
}
