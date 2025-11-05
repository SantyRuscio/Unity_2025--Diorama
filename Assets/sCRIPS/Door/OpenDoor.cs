using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private BedRoomDoor bedRoomDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera"))
        {
            Debug.Log("Choque con una cámara");
            bedRoomDoor.OpendBedRoom();
        }
    }
}
