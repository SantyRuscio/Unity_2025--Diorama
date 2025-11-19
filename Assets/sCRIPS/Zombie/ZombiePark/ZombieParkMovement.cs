using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieParkMovement : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private SoundManager soundManager;

    [Header("Movimiento")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float reachDistance = 0.2f;
    [SerializeField] private float startDelay = 0.1f;

    [Header("Audio")]
    [SerializeField] private AudioSource moveAudio;
    [SerializeField] private AudioClip suspiroAudio;

    private void Start()
    {
        // Escucha el evento
        EventManager.Subscribe(TypeEcvents.ActivateZombieParkMovemetn, OnActivateZombie);
    }

    public void OnActivateZombie(object[] parameters)
    {
        Debug.Log("se ejecuto el triigger ActivateZombiePArk");
        StartCoroutine(MoverZombie());
    }

    IEnumerator MoverZombie()
    {
        // Esperar antes de arrancar
        yield return new WaitForSeconds(startDelay);

        // Reproducir sonido de movimiento
        if (moveAudio != null)
            moveAudio.Play();

        // Recorrer todos los waypoints
        for (int i = 0; i < waypoints.Length; i++)
        {
            Transform target = waypoints[i];

            // Mover hasta llegar al waypoint
            while (Vector3.Distance(transform.position, target.position) > reachDistance)
            {
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    target.position,
                    speed * Time.deltaTime
                );

                yield return null; // Espera al siguiente frame
            }
        }

        // Parar sonido de caminar
        if (moveAudio != null)
            moveAudio.Stop();

        // Trigger final
       // EventManager.Trigger(TypeEcvents.CameraSliderHorizontal);

        // Suspiro final
        soundManager.ReproducirSonido(suspiroAudio);

        // Morir / desaparecer
        Destroy(gameObject);
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

    private void OnDestroy()
    {
        EventManager.Unsubscribe(TypeEcvents.ActivateZombieParkMovemetn, OnActivateZombie);
    }
}


