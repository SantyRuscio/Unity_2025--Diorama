using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveMent : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float reachDistance = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioClip puertaAudio;
    [SerializeField] private SoundManager soundManager;

    private int currentWaypointIndex = 0;
    private bool finished = false;

    private void Awake()
    {
        Debug.Log("me suscribi al StartCameraPath ");
        EventManager.Subscribe(TypeEcvents.CameraFirstPathing, StartCameraPath);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.CameraFirstPathing, StartCameraPath);
    }

    public void StartCameraPath(object[] parameters)
    {
        Debug.Log("ENTRE al StartCameraPath ");
        if (!finished && waypoints.Length > 0)
            StartCoroutine(MoveAlongWaypoints());
    }

    private IEnumerator MoveAlongWaypoints()
    {
        Debug.Log("ENTRE al corutine StartCameraPath ");

        while (!finished)
        {
            Transform target = waypoints[currentWaypointIndex];
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target.position) < reachDistance)
            {
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    finished = true;
                    if (soundManager != null && puertaAudio != null)
                        soundManager.ReproducirSonido(puertaAudio);

                    EventManager.Trigger(TypeEcvents.CameraSliderFirtsInsideHouse);
                }
            }

            yield return null;
        }
    }
}



