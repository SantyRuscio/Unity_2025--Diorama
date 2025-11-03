using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDoorCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Choque Con Una Camara");
            EventManager.Trigger(TypeEcvents.CloseFirstDoor);
        }
    }
}

