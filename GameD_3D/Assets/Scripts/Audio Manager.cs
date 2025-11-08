using UnityEngine;

//Rehope Games (2023) How to Add MUSIC and SOUND EFFECTS to Game in Unity | Unity 2D Platformer Tutorial #16. [Online] Available at: https://www.youtube.com/watch?v=N8whM1GjH4w (Accessed: 7 November 2025) 
//Pixabay Wolf Howl 2. [online] Available at: https://pixabay.com/sound-effects/wolf-howl-2-359870/ (Accessed: 7 November 2025) 
//Pixabay Wolf Howl. [online] Available at: https://pixabay.com/sound-effects/wolf-howl-359873/ (Accessed: 7 November 2025) 
//Pixabay Sand Step. [Online] Available at:  https://pixabay.com/sound-effects/sand-step-95801/ (Accessed: 7 November 2025) 
//Pixabay Ouch Oof Hurt Sound Effect. [Online] Available at: https://pixabay.com/sound-effects/ouch-oof-hurt-sound-effect-262616/   (Accessed: 7 November 2025) 
//Pixabay Item Equip. [Online] Available at: https://pixabay.com/sound-effects/item-equip-6904/ (Accessed: 7 November 2025) 
//Pixabay Soft Wind. [Online] Available at: https://pixabay.com/sound-effects/soft-wind-318856/ (Accessed: 7 November 2025) 
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
