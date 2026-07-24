using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LampScript : MonoBehaviour
{
    private Light2D light2D;

    [SerializeField] float baseIntensity = 1f;
    [SerializeField] float intensityVariation = 0.2f;
    [SerializeField] float[] flickerTime = { 0.5f, 3f};

    [SerializeField] bool isOn = true;
    [SerializeField] bool doesFlicker = false;
    [SerializeField] float flickerProbability = 0.15f;
    private float lastFlickerDuration;

    
    private float randomisedIntensity;

    // Start is called before the first frame update
    void Awake()
    {
        light2D = GetComponent<Light2D>();
        randomisedIntensity = baseIntensity + (Random.value - 0.5f) * intensityVariation;
        doesFlicker = Random.value < flickerProbability;
        lastFlickerDuration = flickerTime[1];
        if (isOn)
        {
            light2D.intensity = randomisedIntensity;
        }
        else
        {
            light2D.intensity = 0;
        }

    }
    private void Start()
    {
        if (doesFlicker)
        {
            StartCoroutine(ExampleCoroutine());
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn)
        {
            light2D.intensity = randomisedIntensity;
        }
        else
        {
            light2D.intensity = 0;
        }
    }

    IEnumerator ExampleCoroutine()
    {
        while (true) 
        {
            if (lastFlickerDuration < 2 * Mathf.Pow(flickerTime[0],2))
            {
                lastFlickerDuration = Mathf.Pow(Random.Range(flickerTime[0], 2.2f * flickerTime[0]),2);
            }
            else
            {
                lastFlickerDuration = Mathf.Pow(Random.Range(flickerTime[0], flickerTime[1]), 2);
            }            
            yield return new WaitForSeconds(lastFlickerDuration);
            isOn = !isOn;
        }
    }


}
