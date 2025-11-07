using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OpenDoor : MonoBehaviour
{
    [SerializeField] private BedRoomDoor bedRoomDoor;
    private bool _alreadyTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_alreadyTriggered) return;

        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Choque con una cámara");

            bedRoomDoor.OpendBedRoom();
            _alreadyTriggered = true;

            Collider col = GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            this.enabled = false;

            // Destroy(gameObject);
        }
    }
}

