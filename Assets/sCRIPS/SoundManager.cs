using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource suspiroSource; 

    public void ReproducirSonido(AudioClip audioClip)
    {
        var _audioClip = audioClip;
        if (suspiroSource != null && _audioClip != null)
            suspiroSource.PlayOneShot(_audioClip);
    }
 
}
