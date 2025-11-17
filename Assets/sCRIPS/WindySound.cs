using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WindySound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    // Tiempo mínimo entre sonidos
    [SerializeField] private float _cooldown = 1f;
    private float _timer = 0f;

    private void Update()
    {
        if (_timer > 0)
            _timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("MainCamera")) return;

        if (_timer <= 0f)
        {
            _audioSource.PlayOneShot(_audioClip);
            _timer = _cooldown;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _audioSource.Stop(); 
       
    }
}