using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedRoomDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private Transform doorObject;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float rotateDuration = 2f;
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private AudioSource moveAudio;

    private bool isOpening = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    private void Awake()
    {
        EventManager.Subscribe(TypeEcvents.OpenLivingRoom, OpenLivingRoom);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.OpenLivingRoom, OpenLivingRoom);
    }

    #region DoorTrigger
    public void OpendBedRoom()
    {
        if (doorObject == null || isOpening)
            return;

        isOpening = true;
        initialRotation = doorObject.rotation;
        targetRotation = Quaternion.Euler(doorObject.eulerAngles + new Vector3(0f, openAngle, 0f));
        StartCoroutine(OpenDoorRoutine());
    }

    private IEnumerator OpenDoorRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        moveAudio?.Play();

        float elapsedTime = 0f;
        while (elapsedTime < rotateDuration)
        {
            float t = elapsedTime / rotateDuration;
            doorObject.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorObject.rotation = targetRotation;

        isOpening = false;


        EventManager.Trigger(TypeEcvents.CameraRoomPathing);
    }

    #endregion

    #region OpenLivingRoom Event
    public void OpenLivingRoom(object[] parameters)
    {
        if (doorObject == null || isOpening)
            return;

        isOpening = true;
        initialRotation = doorObject.rotation;
        targetRotation = Quaternion.Euler(doorObject.eulerAngles + new Vector3(0f, openAngle, 0f));
        StartCoroutine(OpenDoorLivingRoutine());
    }

    private IEnumerator OpenDoorLivingRoutine()
    {
        yield return new WaitForSeconds(startDelay);

        moveAudio?.Play();

        float elapsedTime = 0f;
        while (elapsedTime < rotateDuration)
        {
            float t = elapsedTime / rotateDuration;
            doorObject.rotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        doorObject.rotation = targetRotation;

        isOpening = false;

        yield return new WaitForSeconds(startDelay);

        EventManager.Trigger(TypeEcvents.CameraLivingPathing);
    }

    #endregion
}
