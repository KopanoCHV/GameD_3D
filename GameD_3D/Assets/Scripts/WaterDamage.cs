//using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UI;

public class WaterDamage : MonoBehaviour
{

    public float damage;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(damage);
            
        }
    }


   
}
