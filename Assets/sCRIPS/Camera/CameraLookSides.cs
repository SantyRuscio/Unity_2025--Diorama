using System.Collections;
using UnityEngine;
using UnityEngine;
using System.Collections;

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
        EventManager.Subscribe(TypeEcvents.CameraLookLiving, CameraLookLiving);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.CameraSliderHorizontal, CameraSlide);
        EventManager.Unsubscribe(TypeEcvents.CameraSliderFirtsInsideHouse, CameraLookBack);
        EventManager.Unsubscribe(TypeEcvents.CameraLookLiving, CameraLookLiving);
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

        yield return RotateToAngle(180f);
        yield return new WaitForSeconds(waitTime);
        yield return RotateToAngle(0);

        isRotating = false;
        EventManager.Trigger(TypeEcvents.CameraSecondPathing);
    }
    #endregion

    #region Camera Slide Event AFUERA CASA
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
        EventManager.Trigger(TypeEcvents.OpenFirstDoor);
    }

    private IEnumerator RotateToAngle(float angleY)
    {
        Quaternion startRot = _myTransform.rotation;
        Quaternion targetRot = Quaternion.Euler(0, angleY, 0) * initialRotation;
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = smoothCurve.Evaluate(elapsed / rotationDuration);
            _myTransform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        _myTransform.rotation = targetRot;
    }
    #endregion

    #region CAMARA 90 GradosLiving
    public void CameraLookLiving(object[] parameters)
    {
        if (!isRotating)
            StartCoroutine(LookLivingSequence());
    }

    private IEnumerator LookLivingSequence()
    {
        isRotating = true;

        yield return new WaitForSeconds(startDelay);

        // Guarda la rotación inicial
        Quaternion startRot = _myTransform.rotation;

        // Crea una rotación que mire un poco hacia abajo (por ejemplo -20 grados en X)
        Quaternion targetRot = Quaternion.Euler(20f, _myTransform.eulerAngles.y, _myTransform.eulerAngles.z);

        float elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            t = smoothCurve.Evaluate(t);
            _myTransform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        // Vuelve a la rotación inicial
        elapsed = 0f;
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            t = smoothCurve.Evaluate(t);
            _myTransform.rotation = Quaternion.Slerp(targetRot, startRot, t);
            yield return null;
        }

        _myTransform.rotation = startRot;

        isRotating = false;

        EventManager.Trigger(TypeEcvents.TeleportPark);
    }

    #endregion
}
