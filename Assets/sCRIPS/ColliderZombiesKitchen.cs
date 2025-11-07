using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderZombiesKitchen : MonoBehaviour
{
    [SerializeField] private ZombieLivingActivate[] zombieLivingActivate; 
    private static bool _alreadyTriggered = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyTriggered) return; 

        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Choque con una cámara");

            foreach (var zombie in zombieLivingActivate)
            {
                if (zombie != null)
                    zombie.PrenderZombie();
            }

            _alreadyTriggered = true; 

            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            this.enabled = false;

            // Destroy(gameObject);
        }
    }
}
