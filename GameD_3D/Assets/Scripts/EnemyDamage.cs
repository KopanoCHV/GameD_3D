using UnityEngine;
using UnityEngine.UI;

//Natty GameDev (2022) #3 Player Health & Damage Effect: Let's Make a First Person Game in Unity!. [Online] Available at: https://www.youtube.com/watch?v=LugpgsMdLWw (Accessed: 16 October 2025) 
//Rehope Games (2023) How to Add MUSIC and SOUND EFFECTS to Game in Unity | Unity 2D Platformer Tutorial #16. [Online] Available at: https://www.youtube.com/watch?v=N8whM1GjH4w (Accessed: 7 November 2025) 
public class EnemyDamage : MonoBehaviour
{
    public float damage;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerStats>().TakeDamage(damage);
            audioManager.PlaySFX(audioManager.Damage);
        }
    }
}