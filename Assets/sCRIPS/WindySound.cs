using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class WindySound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private void OnTriggerStay(Collider other)
    {

        if (!other.CompareTag("MainCamera")) return;

        Debug.Log("eNTRO Camara");
        _audioSource.PlayOneShot(_audioClip);
    }
}

