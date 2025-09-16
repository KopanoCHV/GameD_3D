using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Part2Collection : MonoBehaviour
{
    private int Part2 = 0;

    public TextMeshProUGUI Part2text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Part2")
        {
            Part2++;
            Part2text.text = "Part2: " + Part2.ToString();
            Debug.Log(Part2);
            Destroy(other.gameObject);
        }
    }
}
