using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieLivingActivate : MonoBehaviour
{
    [SerializeField] private GameObject zombieObject; 
    [SerializeField] private AudioSource zombieSound; 
    private bool _isActive = false;

    private void Awake()
    {
        if (zombieObject != null)
            zombieObject.SetActive(false);
    }

    public void PrenderZombie()
    {
        if (_isActive) return; 

        Debug.Log($"Zombie activado: {gameObject.name}");

        if (zombieObject != null)
            zombieObject.SetActive(true);

        if (zombieSound != null)
            zombieSound.Play();

        _isActive = true;
    }
}

