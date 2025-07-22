using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;


public class DayNightCycle : MonoBehaviour
{
    //These are headers that will be displayed above controls in manager
    [Header("Time Settings")]
    [Range(0f, 24f)]

    public float currentTime;
    public float timeSpeed = 1f;

    [Header("Current Time")]
    public String currentTimeString;

    [Header("Sun Settings")]
    public Light sunLight;
    public float sunPosition = 1f;
    public float sunIntensity = 1f;
    public AnimationCurve sunIntensityMultiplier;
    public AnimationCurve sunLightTemperatureCurve;

    [Header("Moon Settings")]
    public Light moonLight;
    public float moonPosition = 1f;
    public float moonIntensity = 1f;
    public AnimationCurve moonIntensityMultiplier;
    public AnimationCurve moonLightTemperatureCurve;

    public bool isDay = true;

    public bool sunActive;
    public bool moonActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateTimeText();
        CheckShadowStatus();
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;

        if (currentTime >= 24)
        {
            currentTime = 0;
        }

        UpdateTimeText();
        UpdateLight();
        CheckShadowStatus();
    }

    public void OnValidate()
    {
        UpdateLight();
    }

    //Updates time displayed in manager
    void UpdateTimeText()
    {
        currentTimeString = Mathf.Floor(currentTime).ToString("00") + ":" + ((currentTime % 1) * 60).ToString("00");
    }

    //Updates Light (duh)
    void UpdateLight()
    {
        //Sun stuff
        float sunRotation = currentTime / 24f * 360f;
        sunLight.transform.rotation = Quaternion.Euler(sunRotation - 90f, sunPosition, 0f);

        float normalizedTime = currentTime / 24f;
        float sunIntensityCurve = sunIntensityMultiplier.Evaluate(normalizedTime);

        HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();

        if (sunLightData != null)
        {
            sunLight.intensity = sunIntensityCurve * sunIntensity;
        }

        float sunTemperatureMultiplier = sunLightTemperatureCurve.Evaluate(normalizedTime);
        Light sunLightComponent = sunLight.GetComponent<Light>();

        if (sunLightComponent != null)
        {
            sunLight.colorTemperature = sunTemperatureMultiplier * 10000f;
        }

        //Moon stuff
        float moonRotation = -(currentTime / 24f * 360f + 180f) % 360f;
        moonLight.transform.rotation = Quaternion.Euler(moonRotation - 90f, moonPosition, 0f);

        float moonIntensityCurve = moonIntensityMultiplier.Evaluate(normalizedTime);

        HDAdditionalLightData moonLightData = moonLight.GetComponent<HDAdditionalLightData>();

        if (moonLightData != null)
        {
            moonLight.intensity = moonIntensityCurve * moonIntensity;
        }

        //float moonTemperatureMultiplier = moonLightTemperatureCurve.Evaluate(normalizedTime);
        //Light moonLightComponent = moonLight.GetComponent<Light>();

        moonLight.colorTemperature = 14000f;

        //if (moonLightComponent != null)
        //{
        //moonLight.colorTemperature = moonTemperatureMultiplier * 10000f;
        //}
    }

    //Checks to see if it is day and shadows should be on
    void CheckShadowStatus()
    {
        HDAdditionalLightData sunLightData = sunLight.GetComponent<HDAdditionalLightData>();
        float currentSunRotation = currentTime;

        HDAdditionalLightData moonLightData = moonLight.GetComponent<HDAdditionalLightData>();
        float currentMoonRotaiton = currentTime;

        if (currentSunRotation >= 6f && currentSunRotation <= 18f)
        {
            sunLightData.EnableShadows(true);
            moonLightData.EnableShadows(false);
            isDay = true;
        }

        else
        {
            sunLightData.EnableShadows(false);
            moonLightData.EnableShadows(true);
            isDay = false;
        }

        if (currentSunRotation >= 5.7f && currentSunRotation <= 18.3f)
        {
            sunLightData.gameObject.SetActive(true);
            sunActive = true;
        }
        else
        {
            sunLightData.gameObject.SetActive(false);
            sunActive = false;
        }

        if (currentSunRotation >= 6.3f && currentSunRotation <= 17.7f)
        {
            moonLightData.gameObject.SetActive(false);
            moonActive = false;
        }
        else
        {
            moonLightData.gameObject.SetActive(true);
            moonActive = true;
        }
    }
}
