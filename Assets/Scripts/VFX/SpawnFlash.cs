using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnFlash : MonoBehaviour
{
    Light2D FlashLight;

    [SerializeField]
    private float intensityMin = 0.25f;
    [SerializeField]
    private float intensityMax = 3f;

    [SerializeField]
    private float OuterRadiusMax = 2f;

    [SerializeField]
    private float increaseRate = 0.1f;

    [SerializeField]
    private float flashSpeed = 100f;

    [SerializeField]
    private float radiusSpeed = 40f;

    // Start is called before the first frame update
    void Awake()
    {
        FlashLight = GetComponent<Light2D>();
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        yield return new WaitForSeconds(0.0025f);

        if (FlashLight.intensity < intensityMax)
        {
            FlashLight.intensity += increaseRate * flashSpeed * Time.deltaTime;
            FlashLight.pointLightOuterRadius += increaseRate * radiusSpeed * Time.deltaTime;
            StartCoroutine(Flash());
        }

        else
            StartCoroutine(FadeNVanish());
    }

    IEnumerator FadeNVanish()
    {
        yield return new WaitForSeconds(0.0025f);

        if (FlashLight.intensity > intensityMin)
        {
            FlashLight.intensity -= increaseRate * flashSpeed * Time.deltaTime;
            FlashLight.pointLightOuterRadius -= increaseRate * radiusSpeed * Time.deltaTime;
            StartCoroutine(FadeNVanish());
        }

        else
            Destroy(gameObject);

    }
}
