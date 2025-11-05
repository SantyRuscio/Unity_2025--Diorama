using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CameraMoveMent : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private Transform[] secondWaypointsPath;
    [SerializeField] private Transform[] ThirdtWaypointsPath;
    [SerializeField] private Transform[] RoomWaypointsPath;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float SecondPathSpeed = 1f;
    [SerializeField] private float reachDistance = 0.2f;
    [SerializeField] private float rotationSpeed = 3f; // más bajo para rotación más suave

    [Header("Audio")]
    [SerializeField] private AudioClip puertaAudio;
    [SerializeField] private AudioClip CortoLuces;
    [SerializeField] private AudioClip walksSound;
    [SerializeField] private SoundManager soundManager;

    private int currentWaypointIndex = 0;
    private bool finished = false;

    private void Awake()
    {
        EventManager.Subscribe(TypeEcvents.CameraFirstPathing, StartCameraPath);
        EventManager.Subscribe(TypeEcvents.CameraSecondPathing, StartSecondCameraPath);
        EventManager.Subscribe(TypeEcvents.CameraRoomPathing, StartRoomCameraPath);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.CameraFirstPathing, StartCameraPath);
        EventManager.Unsubscribe(TypeEcvents.CameraSecondPathing, StartSecondCameraPath);
        EventManager.Unsubscribe(TypeEcvents.CameraRoomPathing, StartRoomCameraPath);
    }

    private void Start()
    {
        if (soundManager != null) return;
    }

    #region PRIMER RECORRIDO
    public void StartCameraPath(object[] parameters)
    {
        if (waypoints.Length > 0)
        {
            finished = false;
            currentWaypointIndex = 0;
            StartCoroutine(MoveAlongWaypoints());
        }
    }

    private IEnumerator MoveAlongWaypoints()
    {
        soundManager.ReproducirSonido(walksSound, true);

        while (currentWaypointIndex < waypoints.Length)
        {
            Transform target = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < reachDistance)
                currentWaypointIndex++;

            yield return null;
        }

        soundManager.DetenerSonido();

        finished = true;
        soundManager.ReproducirSonido(puertaAudio);
        EventManager.Trigger(TypeEcvents.CameraSliderFirtsInsideHouse);
    }
    #endregion

    #region SEGUNDO RECORRIDO
    public void StartSecondCameraPath(object[] parameters)
    {
        if (secondWaypointsPath.Length > 0)
        {
            finished = false;
            currentWaypointIndex = 0;
            StartCoroutine(MoveAlongSecondWaypoints());
        }
    }

    private IEnumerator MoveAlongSecondWaypoints()
    { 
        soundManager.ReproducirSonido(walksSound, true);

        while (currentWaypointIndex < secondWaypointsPath.Length)
        {
            Transform target = secondWaypointsPath[currentWaypointIndex];

            transform.position = Vector3.MoveTowards(transform.position, target.position, SecondPathSpeed * Time.deltaTime);

            // Rotación suave
            Vector3 direction = (target.position - transform.position).normalized;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    lookRotation,
                    1f - Mathf.Exp(-rotationSpeed * Time.deltaTime)
                );
            }

            if (Vector3.Distance(transform.position, target.position) < reachDistance)
                currentWaypointIndex++;

            yield return null;
        }

        soundManager.DetenerSonido();

        soundManager.ReproducirSonido(CortoLuces);

        yield return new WaitForSeconds(2f);

        finished = true;
        StartThirdCameraPath();
    }
    #endregion

    #region TERCER RECORRIDO
    public void StartThirdCameraPath()
    {
        if (ThirdtWaypointsPath.Length > 0)
        {
            finished = false;
            currentWaypointIndex = 0;
            StartCoroutine(MoveAlongThirdWaypoints());
        }
    }

    private IEnumerator MoveAlongThirdWaypoints()
    {
        soundManager.ReproducirSonido(walksSound, true);

        while (currentWaypointIndex < ThirdtWaypointsPath.Length)
        {
            Transform target = ThirdtWaypointsPath[currentWaypointIndex];

            transform.position = Vector3.MoveTowards(transform.position, target.position, SecondPathSpeed * Time.deltaTime);

            Vector3 direction = (target.position - transform.position).normalized;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    lookRotation,
                    1f - Mathf.Exp(-rotationSpeed * Time.deltaTime)
                );
            }

            if (Vector3.Distance(transform.position, target.position) < reachDistance)
                currentWaypointIndex++;

            yield return null;
        }

        soundManager.DetenerSonido();

        yield return new WaitForSeconds(1f);

        finished = true;

        soundManager.ReproducirSonido(CortoLuces);
    }
    #endregion

    #region HABITACIÓN RECORRIDO
    public void StartRoomCameraPath(object[] parameters)
    {
        if (RoomWaypointsPath.Length > 0)
        {
            finished = false;
            currentWaypointIndex = 0;
            StartCoroutine(MoveAlongRoomWaypoints());
        }
    }

    private IEnumerator MoveAlongRoomWaypoints()
    {
        soundManager.ReproducirSonido(walksSound, true);

        while (currentWaypointIndex < RoomWaypointsPath.Length)
        {
            Transform target = RoomWaypointsPath[currentWaypointIndex];

            transform.position = Vector3.MoveTowards(transform.position, target.position, SecondPathSpeed * Time.deltaTime);

            Vector3 direction = (target.position - transform.position).normalized;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    lookRotation,
                    1f - Mathf.Exp(-rotationSpeed * Time.deltaTime)
                );
            }

            if (Vector3.Distance(transform.position, target.position) < reachDistance)
                currentWaypointIndex++;

            yield return null;
        }

        soundManager.DetenerSonido();

        yield return new WaitForSeconds(1f);

        finished = true;

        soundManager.ReproducirSonido(CortoLuces);
    }
    #endregion
}
