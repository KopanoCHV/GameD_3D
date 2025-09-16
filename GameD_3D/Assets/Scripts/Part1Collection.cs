using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Part1Collection : MonoBehaviour
{
    private int Part1 = 0;

    public TextMeshProUGUI Part1text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Part1")
        {
            Part1++;
            Part1text.text = "Part1: " + Part1.ToString();
            Debug.Log(Part1);
            Destroy(other.gameObject);
        }
    }
}
