using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZombieLivingActivate : MonoBehaviour
{
    [SerializeField] private GameObject zombieObject;

    private bool _isActive = false;

    private void Awake()
    {
        if (zombieObject != null)
            zombieObject.SetActive(false);
    }

    public void PrenderZombie()
    {
        if (_isActive) return; // evita que se repita

        Debug.Log($"Zombie activado: {gameObject.name}");

        // activa el zombie visualmente
        if (zombieObject != null)
            zombieObject.SetActive(true);

        _isActive = true;
    }

    public void ApagarZombie()
    {
        if (zombieObject != null)
            zombieObject.SetActive(true);

    }

}


