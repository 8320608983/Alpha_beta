using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Clips")]
    [SerializeField] private AudioClip flipClip;
    [SerializeField] private AudioClip matchClip;
    [SerializeField] private AudioClip mismatchClip;
    [SerializeField] private AudioClip gameOverClip;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
    
    private void OnEnable()
    {
        GameEvents.OnFlip += PlayFlip;
        GameEvents.OnMatch += PlayMatch;
        GameEvents.OnMismatch += PlayMismatch;
        GameEvents.OnGameOver += PlayGameOver;
    }

    private void OnDisable()
    {
        GameEvents.OnFlip -= PlayFlip;
        GameEvents.OnMatch -= PlayMatch;
        GameEvents.OnMismatch -= PlayMismatch;
        GameEvents.OnGameOver -= PlayGameOver;
    }

    void PlayFlip() => Play(flipClip);
    void PlayMatch() => Play(matchClip);
    void PlayMismatch() => Play(mismatchClip);
    void PlayGameOver() => Play(gameOverClip);

    void Play(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
} 