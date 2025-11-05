using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource oneShotSource;   // sonidos puntuales (puerta, luces, etc.)
    [SerializeField] private AudioSource loopSource;      // sonidos en loop (como pasos)

    // 🔊 Reproduce un sonido una sola vez
    public void ReproducirSonido(AudioClip audioClip)
    {
        if (oneShotSource != null && audioClip != null)
            oneShotSource.PlayOneShot(audioClip);
    }

    // 🔁 Reproduce un sonido en loop (por ejemplo pasos)
    public void ReproducirSonido(AudioClip audioClip, bool loop)
    {
        if (loopSource == null || audioClip == null)
            return;

        loopSource.clip = audioClip;
        loopSource.loop = loop;
        loopSource.Play();
    }

    // ⏹ Detiene el sonido en loop (cuando termina de caminar)
    public void DetenerSonido()
    {
        if (loopSource != null && loopSource.isPlaying)
            loopSource.Stop();
    }
}

