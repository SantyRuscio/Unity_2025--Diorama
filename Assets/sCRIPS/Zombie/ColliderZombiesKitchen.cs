using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderZombiesKitchen : MonoBehaviour
{
    [Header("Zombies a activar")]
    [SerializeField] private ZombieLivingActivate[] zombieLivingActivate;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip zombieEatSound;
    [SerializeField] private bool loopWhileInside = true;

    private bool _alreadyTriggered = false;
    private bool _isInside = false;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        audioSource.playOnAwake = false;
        audioSource.loop = loopWhileInside;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("La cámara entró en el área de los zombies.");

            // Activar los zombies
            foreach (var zombie in zombieLivingActivate)
            {
                if (zombie != null)
                    zombie.PrenderZombie();
            }

            _isInside = true;

            // Reproducir sonido (una vez o loop)
            if (zombieEatSound != null && !audioSource.isPlaying)
            {
                if (loopWhileInside)
                {
                    audioSource.clip = zombieEatSound;
                    audioSource.Play();
                }
                else
                {
                    audioSource.PlayOneShot(zombieEatSound);
                }
            }

            _alreadyTriggered = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_isInside || !_alreadyTriggered) return;

        if (other.CompareTag("MainCamera") && !loopWhileInside && !audioSource.isPlaying)
        {
            // Si no es loop, reproducir repetidamente mientras esté dentro
            audioSource.PlayOneShot(zombieEatSound);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("La cámara salió del área de los zombies.");
            _isInside = false;
            audioSource.Stop();

            foreach (var zombie in zombieLivingActivate)
            {
                if (zombie != null)
                    zombie.ApagarZombie();
            }
        }
    }
}

