using System.Collections;
using UnityEngine;

public class DoorEventRotator : MonoBehaviour
{
    [SerializeField] private Transform doorObject;       // La puerta que rota
    [SerializeField] private float openAngle = 90f;      // Ángulo de apertura
    [SerializeField] private float rotateDuration = 2f;  // Tiempo que tarda en abrir
    [SerializeField] private float startDelay = 0.5f;    // Espera antes de iniciar
    [SerializeField] private AudioSource moveAudio;      // Sonido opcional

    private Quaternion closedRotation;

    private void Start()
    {
        if (doorObject == null)
            doorObject = transform;

        closedRotation = doorObject.rotation;

        EventManager.Subscribe(TypeEcvents.OpenFirstDoor, OpenDoor);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.OpenFirstDoor, OpenDoor);
    }

    private void OpenDoor(object[] parameters)
    {
        StartCoroutine(RotateDoorSequence());
    }

    private IEnumerator RotateDoorSequence()
    {
        yield return new WaitForSeconds(startDelay);

        if (moveAudio != null)
            moveAudio.Play();

        Quaternion startRot = doorObject.rotation;
        Quaternion targetRot = closedRotation * Quaternion.Euler(0, openAngle, 0); // Solo rota en Y
        float elapsed = 0f;

        while (elapsed < rotateDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / rotateDuration);
            doorObject.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        doorObject.rotation = targetRot;
    }
}

