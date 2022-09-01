using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StarPulsing : MonoBehaviour
{
    Light2D PulsingStar;

    [SerializeField]
    private float startingIntensity = 1f;

    [SerializeField]
    private float intensityMin = 0.5f;    
    [SerializeField]
    private float intensityMax = 1f;

    [SerializeField]
    private float outerRadiusMax = 2.5f;
    [SerializeField]
    private float outerRadiusMin = 1f;

    [SerializeField]
    float pulseRate = 0.005f;

    [SerializeField]
    private float pulseSpeed = 150f;

    bool pulseUpwards = false;

    // Start is called before the first frame update
    void Start()
    {
        PulsingStar = GetComponent<Light2D>();
        PulsingStar.intensity = startingIntensity;
    }

    // Update is called once per frame
    void Update()
    {
        if(pulseUpwards)
        {
            PulsingStar.intensity += pulseRate * Time.deltaTime * pulseSpeed;
            PulsingStar.pointLightOuterRadius += pulseRate * Time.deltaTime * pulseSpeed;

            if (PulsingStar.intensity >= intensityMax)
                pulseUpwards = false;
        }

        else
        {
            PulsingStar.intensity -= pulseRate * Time.deltaTime * pulseSpeed;
            PulsingStar.pointLightOuterRadius -= pulseRate * Time.deltaTime * pulseSpeed;

            if (PulsingStar.intensity <= intensityMin)
                pulseUpwards = true;
        }
    }
}
