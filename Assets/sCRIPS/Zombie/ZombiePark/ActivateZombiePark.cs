using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateZombiePark : MonoBehaviour
{
    [Header("Zombie a activar")]
    public GameObject zombiePark;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Choque con la camara en el park");

            if (zombiePark != null)
                zombiePark.SetActive(true);

            StartCoroutine(TriggerNextFrame());
        }
    }

    IEnumerator TriggerNextFrame()
    {
        yield return null;
        Debug.Log("Eejecuto el triger");
        EventManager.Trigger(TypeEcvents.ActivateZombieParkMovemetn);
    }
}


