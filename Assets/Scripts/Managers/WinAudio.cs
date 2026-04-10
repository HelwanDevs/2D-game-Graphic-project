using UnityEngine;

public class WinAudio : MonoBehaviour
{
    [SerializeField] private AudioSource winAudioSource;

    public AudioClip win;

    private void Start()
    {
        winAudioSource.clip = win;
        winAudioSource.loop = false;
        winAudioSource.volume = 0.5f;
        winAudioSource.Play();
    }
}
