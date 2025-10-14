using Unity.VisualScripting;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
    public float threshold = 0.05f;
    public float multiplier = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        float fallSpeed = GetComponent<Rigidbody>().linearVelocity.y;
        
        if (fallSpeed <- threshold)
        {
            float damage = (-fallSpeed - threshold) * multiplier;
            Debug.Log("Fall Damage is" + damage);
            // Implement fall damage logic here
        }
    }
   
}
