using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Part3Collection : MonoBehaviour
{
    private int Part3 = 0;

    public TextMeshProUGUI Part3text;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Part3")
        {
            Part3++;
            Part3text.text = "Part3: " + Part3.ToString();
            Debug.Log(Part3);
            Destroy(other.gameObject);
        }
    }
}
