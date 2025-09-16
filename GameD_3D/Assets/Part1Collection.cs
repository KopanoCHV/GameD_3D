using UnityEngine;

public class Part1Collection : MonoBehaviour
{
    private int Part1 = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Part1")
        {
            Part1++;
            Debug.Log(Part1);
            Destroy(other.gameObject);
        }
    }
}
