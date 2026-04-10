using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip hit;
    public AudioClip shooting;

    public AudioClip fristPlayerHit;
    public AudioClip firstPlayerNotHit;

    public AudioClip secondPlayerHit;
    public AudioClip secondPlayerNotHit;


    private void Awake()
    {
            instance = this;
    }

    private void Start()
    {
        musicSource.clip = background;
        musicSource.loop = true;
        musicSource.volume = 0.3f;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}