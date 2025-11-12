using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [Header("Configuración del parpadeo")]
    [SerializeField] private float minIntensity = 0.5f;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float flickerSpeed = 0.1f;

    private Light targetLight;
    private float baseIntensity;
    private float timer;

    private void Awake()
    {
        targetLight = GetComponent<Light>();
        baseIntensity = targetLight.intensity;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= flickerSpeed)
        {
            float randomIntensity = Random.Range(minIntensity, maxIntensity);
            targetLight.intensity = randomIntensity;
            timer = 0f;
        }
    }
}

