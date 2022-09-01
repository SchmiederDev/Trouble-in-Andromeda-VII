using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnginePulse : MonoBehaviour
{

    Light2D EngineLight;

    [SerializeField]
    private float intensityMin = 1f;
    [SerializeField]
    private float intensityMax = 1.5f;

    private float fallOffMin = 0.5f;

    private float fallOffMax = 1.25f;

    [SerializeField]
    private float pulseRate = 0.01f;

    [SerializeField]
    private float pulseSpeed = 350f;

    bool pulseUpwards = false;


    // Start is called before the first frame update
    void Start()
    {
        EngineLight = GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {        

        if (pulseUpwards)
        {
            EngineLight.intensity += pulseRate * Time.deltaTime * pulseSpeed;            

            if (EngineLight.intensity >= intensityMax)
                pulseUpwards = false;
        }

        else
        {
            EngineLight.intensity -= pulseRate * Time.deltaTime * pulseSpeed;

            if (EngineLight.intensity <= intensityMin)
                pulseUpwards = true;
        }

    }
}
