using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Codecodile (2023) How To Create a HealthSystem in Unity: Part 1. [Online] Available at: https://www.youtube.com/watch?v=yQers6__cLc (Accessed: 14 October 2025
//Natty GameDev (2022) #3 Player Health & Damage Effect: Let's Make a First Person Game in Unity!. [Online] Available at: https://www.youtube.com/watch?v=LugpgsMdLWw (Accessed: 16 October 2025) 
public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private float currentHealth;
    
    [Header("Health Bar")]
    public HealthBar healthBar;

    [Header("Damage Overlay")]
    public Image overlay;
    public float duration;
    public float fadeSpeed;

    private float durationTimer;

    

    private void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetSliderMax(maxHealth);
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
    }
    private void Update()
    {
        if (overlay.color.a > 0)
        {
            durationTimer += Time.deltaTime;
            if (durationTimer > duration)
            {
                float tempAlpha = overlay.color.a;
                tempAlpha -= Time.deltaTime * fadeSpeed;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetSlider(currentHealth);
        if (currentHealth <= 0)
        {
          // print("Player Died");    //For dead
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //For Restarting the scene  
        }
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
    }
}
