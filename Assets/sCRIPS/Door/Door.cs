using System.Collections;
using UnityEngine;

public class DoorEventRotator : MonoBehaviour
{
    [SerializeField] private Transform doorObject;
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float rotateDuration = 2f;
    [SerializeField] private float startDelay = 0.5f;
    [SerializeField] private AudioSource moveAudio;

    private Quaternion closedRotation;

    private void Start()
    {
        if (doorObject == null) doorObject = transform;

        closedRotation = doorObject.rotation;

        EventManager.Subscribe(TypeEcvents.OpenFirstDoor, OpenDoor);
        EventManager.Subscribe(TypeEcvents.CloseFirstDoor, CloseDoor);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.OpenFirstDoor, OpenDoor);
        EventManager.Unsubscribe(TypeEcvents.CloseFirstDoor, CloseDoor);
    }

    private void OpenDoor(object[] parameters)
    {
        StartCoroutine(OpenDoorSequence());
    }

    private IEnumerator OpenDoorSequence()
    {
        yield return new WaitForSeconds(startDelay);

        if (moveAudio != null) moveAudio.Play();

        Quaternion startRot = doorObject.rotation;
        Quaternion targetRot = closedRotation * Quaternion.Euler(0, openAngle, 0);
        float elapsed = 0f;

        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / rotateDuration);
            doorObject.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        doorObject.rotation = targetRot;

        EventManager.Trigger(TypeEcvents.CameraFirstPathing);
    }

    private void CloseDoor(object[] parameters)
    {
        doorObject.rotation = closedRotation;
        Debug.Log("Puerta cerrada instantáneamente");
    }
}

