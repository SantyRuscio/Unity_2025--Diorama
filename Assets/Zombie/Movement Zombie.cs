using UnityEngine;
using System.Collections;

public class MovementZombie : MonoBehaviour
{
    [SerializeField] private SoundManager soundManager;


    [SerializeField] private Transform[] waypoints;      // Lista de puntos a seguir
    [SerializeField] private float speed = 5f;           // Velocidad del movimiento
    [SerializeField] private float reachDistance = 0.2f; // Distancia mínima para considerar que llegó
    [SerializeField] private AudioSource moveAudio;      // Sonido que se reproduce al moverse
    [SerializeField] private AudioClip suspiroAudio;     // Clip del suspiro
    [SerializeField] private float startDelay = 0.5f;    // Tiempo que espera antes de comenzar

    private int currentWaypointIndex = 0;
    private bool canMove = false;
    private bool isMoving = false;

    private void Start()
    {
        // Arranca la corutina para esperar unos segundos antes de moverse
        StartCoroutine(StartAfterDelay());
    }

    IEnumerator StartAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        canMove = true;
    }

    private void Update()
    {
        if (!canMove || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];

        // Mover hacia el waypoint actual
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        float distance = Vector3.Distance(transform.position, target.position);

        // Reproducir sonido mientras se mueve
        if (!isMoving && distance > reachDistance)
        {
            isMoving = true;
            if (moveAudio != null && !moveAudio.isPlaying)
                moveAudio.Play();
        }

        // Si llega al waypoint actual
        if (distance < reachDistance)
        {
            currentWaypointIndex++;

            // Si llegó al último waypoint
            if (currentWaypointIndex >= waypoints.Length)
            {
                if (moveAudio != null)
                    moveAudio.Stop();

                EventManager.Trigger(TypeEcvents.CameraSlider);

                soundManager.ReproducirSonido(suspiroAudio);

                Destroy(gameObject);
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
        }
    }
}

