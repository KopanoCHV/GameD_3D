using UnityEngine;

//Rehope Games (2023) How to Add MUSIC and SOUND EFFECTS to Game in Unity | Unity 2D Platformer Tutorial #16. [Online] Available at: https://www.youtube.com/watch?v=N8whM1GjH4w (Accessed: 7 November 2025) 
public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clips")]
    public AudioClip background;
    public AudioClip Walk;
    public AudioClip Pickup;
    public AudioClip Win;
    public AudioClip Damage;
    public AudioClip wolf;
    public AudioClip wolf2;
    public AudioClip jump;

    private void Start()
    {
       musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip) 
    { 
        SFXSource.PlayOneShot(clip);
    }
}
