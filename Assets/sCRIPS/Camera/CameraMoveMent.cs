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
    [SerializeField] private Transform[] LivingWaypointsPath;
    [SerializeField] private Transform[] ParkWaypointsPath;


    [SerializeField] private float speed = 2f;
    [SerializeField] private float Parkspeed = 50f;
    [SerializeField] private float SecondPathSpeed = 1f;
    [SerializeField] private float reachDistance = 0.2f;
    [SerializeField] private float rotationSpeed = 3f; // más bajo para rotación más suave
    [SerializeField] private float LivingPathSpeed = 0.2f;
    [SerializeField] private float LivingrotationSpeed = 1f;

    [Header("Audio")]
    [SerializeField] private AudioClip puertaAudio;
    [SerializeField] private AudioClip CortoLuces;
    [SerializeField] private AudioClip walksSound;
    [SerializeField] private AudioClip respiracion;
    [SerializeField] private SoundManager soundManager;

    private int currentWaypointIndex = 0;
    private bool finished = false;

    private void Awake()
    {
        EventManager.Subscribe(TypeEcvents.CameraFirstPathing, StartCameraPath);
        EventManager.Subscribe(TypeEcvents.CameraSecondPathing, StartSecondCameraPath);
        EventManager.Subscribe(TypeEcvents.CameraRoomPathing, StartRoomCameraPath);
        EventManager.Subscribe(TypeEcvents.CameraLivingPathing, StartLivingCameraPath);
        EventManager.Subscribe(TypeEcvents.TeleportPark, StartTelePortPark);

    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.CameraFirstPathing, StartCameraPath);
        EventManager.Unsubscribe(TypeEcvents.CameraSecondPathing, StartSecondCameraPath);
        EventManager.Unsubscribe(TypeEcvents.CameraRoomPathing, StartRoomCameraPath);
        EventManager.Unsubscribe(TypeEcvents.CameraLivingPathing, StartLivingCameraPath);
        EventManager.Unsubscribe(TypeEcvents.TeleportPark, StartTelePortPark);
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

        soundManager.ReproducirSonido(respiracion);

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

        EventManager.Trigger(TypeEcvents.OpenLivingRoom);
    }
    #endregion

    #region LIVING RECORRIDO
    public void StartLivingCameraPath(object[] parameters)
    {
        if (LivingWaypointsPath.Length > 0)
        {
            finished = false;
            currentWaypointIndex = 0;
            StartCoroutine(MoveAlongLivingWaypoints());
        }
    }

    private IEnumerator MoveAlongLivingWaypoints()
    {
        soundManager.ReproducirSonido(walksSound, true);

        while (currentWaypointIndex < LivingWaypointsPath.Length)
        {
            Transform target = LivingWaypointsPath[currentWaypointIndex];

            transform.position = Vector3.MoveTowards(transform.position, target.position, LivingPathSpeed * Time.deltaTime);

            Vector3 direction = (target.position - transform.position).normalized;
            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(
                    transform.rotation,
                    lookRotation,
                    1f - Mathf.Exp(-LivingrotationSpeed * Time.deltaTime) 
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

        soundManager.ReproducirSonido(respiracion);

        EventManager.Trigger(TypeEcvents.CameraLookLiving);
    }

    #endregion

    #region TP al PARK
    public void StartTelePortPark(object[] parameters)
    {
        if (ParkWaypointsPath.Length > 0)
        {
            finished = false;
            currentWaypointIndex = 0;
            StartCoroutine(MoveAlongParkWaypoints());
        }
    }

    private IEnumerator MoveAlongParkWaypoints()
    {
        soundManager.ReproducirSonido(walksSound, true);

        while (currentWaypointIndex < ParkWaypointsPath.Length)
        {
            Transform target = ParkWaypointsPath[currentWaypointIndex];

            // Movimiento hacia el waypoint
            transform.position = Vector3.MoveTowards(transform.position, target.position, Parkspeed * Time.deltaTime);

            // Rotación suave hacia el waypoint
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

            // Cuando llega al waypoint, pasa al siguiente
            if (Vector3.Distance(transform.position, target.position) < reachDistance)
                currentWaypointIndex++;

            yield return null;
        }

        // Cuando termina el recorrido
        soundManager.DetenerSonido();

        yield return new WaitForSeconds(1f);

        finished = true;
    }

    #endregion

}
