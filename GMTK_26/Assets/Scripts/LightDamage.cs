using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightDamage : MonoBehaviour
{
    private Transform playerTransform;
    private CountDown countDown;
    
    [SerializeField] float baseTimeMultiplier;
    [SerializeField] float timeMultiplier;

    [SerializeField] private float rayLength;

    private Light2D light2D;
    private float lightFalloffDistance;
    private float distanceFromInner;

    // Start is called before the first frame update
    void Awake()
    {
        playerTransform = FindFirstObjectByType<PlayerMovement>(FindObjectsInactive.Include).transform;
        countDown = FindFirstObjectByType<CountDown>(FindObjectsInactive.Include);
        light2D = GetComponent<Light2D>();
        rayLength = light2D.pointLightOuterRadius;
        lightFalloffDistance = light2D.pointLightOuterRadius - light2D.pointLightInnerRadius;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerTransform.position - transform.position,rayLength);
        if (hit && hit.collider.gameObject.layer == 7)
        {
            //Debug.Log("light");
            distanceFromInner = hit.distance - light2D.pointLightInnerRadius;
            if (distanceFromInner <= 0)
            {
                timeMultiplier = baseTimeMultiplier * light2D.intensity;
            }
            else
            {
                timeMultiplier = baseTimeMultiplier * light2D.intensity * ( 1 - light2D.falloffIntensity * distanceFromInner / lightFalloffDistance);
            }
            
            countDown.timeFlowMultiplier = Mathf.Max(timeMultiplier, countDown.timeFlowMultiplier);
        }
    }
}
