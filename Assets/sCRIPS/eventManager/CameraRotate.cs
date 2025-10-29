using System.Collections;
using UnityEngine;
public class CameraLookSides : MonoBehaviour
{
    [SerializeField] private float rotationAngle = 40f;   // Cuánto gira hacia los lados
    [SerializeField] private float rotationDuration = 1f; // Cuánto tarda en llegar al ángulo
    [SerializeField] private float waitTime = 0.5f;       // Tiempo que se queda mirando cada lado
    [SerializeField] private float startDelay = 1f;       // ⏳ Tiempo que espera antes de arrancar
    [SerializeField] private AnimationCurve smoothCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private bool isRotating = false;
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;
        EventManager.Subscribe(TypeEcvents.CameraSlider, CameraSlide);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.CameraSlider, CameraSlide);
    }

    private void CameraSlide(object[] parameters)
    {
        if (!isRotating)
            StartCoroutine(RotateSequence());
    }

    private IEnumerator RotateSequence()
    {
        isRotating = true;

        // Espera antes de iniciar la secuencia
        yield return new WaitForSeconds(startDelay);

        // Izquierda → Espera → Derecha → Espera → Centro
        yield return RotateToAngle(-rotationAngle);
        yield return new WaitForSeconds(waitTime);
        yield return RotateToAngle(rotationAngle);
        yield return new WaitForSeconds(waitTime);
        yield return RotateToAngle(0);

        isRotating = false;

        EventManager.Trigger(TypeEcvents.OpenFirstDoor);

    }

    private IEnumerator RotateToAngle(float angle)
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, angle, 0) * initialRotation;
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            t = smoothCurve.Evaluate(t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.rotation = targetRot;
    }
}