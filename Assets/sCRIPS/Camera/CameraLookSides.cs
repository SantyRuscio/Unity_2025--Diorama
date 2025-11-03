using System.Collections;
using UnityEngine;
public class CameraLookSides : MonoBehaviour
{
    [SerializeField] private float rotationAngle = 40f;
    [SerializeField] private float rotationDuration = 1f;
    [SerializeField] private float waitTime = 0.5f;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private AnimationCurve smoothCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private Transform _myTransform;

    private bool isRotating = false;
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = _myTransform.rotation;
        EventManager.Subscribe(TypeEcvents.CameraSliderHorizontal, CameraSlide);

        EventManager.Subscribe(TypeEcvents.CameraSliderFirtsInsideHouse, CameraLookBack);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.CameraSliderHorizontal, CameraSlide);

        EventManager.Unsubscribe(TypeEcvents.CameraSliderFirtsInsideHouse, CameraLookBack);
    }

    #region Look Back Event
    public void CameraLookBack(object[] parameters)
    {
        if (!isRotating)
            StartCoroutine(LookBackSequence());
    }

    private IEnumerator LookBackSequence()
    {
        isRotating = true;

        yield return new WaitForSeconds(startDelay);

        // Girar hacia atrás (180° en Y)
        yield return RotateToAngle(180f);
        yield return new WaitForSeconds(waitTime);

        // Volver al centro
        yield return RotateToAngle(0);

        isRotating = false;

        // Opcional: disparar algún evento si quieres
        // EventManager.Trigger(TypeEcvents.NextEvent);
    }
    #endregion


    #region Camera Slide Event
    private void CameraSlide(object[] parameters)
    {
        if (!isRotating)
            StartCoroutine(RotateSequence());
    }

    private IEnumerator RotateSequence()
    {
        isRotating = true;

        yield return new WaitForSeconds(startDelay);

        yield return RotateToAngle(-rotationAngle);
        yield return new WaitForSeconds(waitTime);
        yield return RotateToAngle(rotationAngle);
        yield return new WaitForSeconds(waitTime);
        yield return RotateToAngle(0);

        isRotating = false;

        // Trigger para abrir la puerta, solo al final de la secuencia
        EventManager.Trigger(TypeEcvents.OpenFirstDoor);
    }

    private IEnumerator RotateToAngle(float angle)
    {
        Quaternion startRot = _myTransform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, angle, 0) * initialRotation;
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            t = smoothCurve.Evaluate(t);
            _myTransform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        _myTransform.rotation = targetRot;
    }
    #endregion
}
