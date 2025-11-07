
using UnityEngine;
using UnityEngine.UI;

//Natty GameDev (2022) #3 Player Health & Damage Effect: Let's Make a First Person Game in Unity!. [Online] Available at: https://www.youtube.com/watch?v=LugpgsMdLWw (Accessed: 16 October 2025) 

public class WaterDamage : MonoBehaviour
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
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(damage);
            audioManager.PlaySFX(audioManager.Damage);
        }
    }


   
}
