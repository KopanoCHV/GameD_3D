using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Rigor Mortis Tortoise (2023) How to Collect Coins in Unity 3D Tutorial 2023 (UpDated). [Online] Avalable at :  https://youtu.be/6iSJ_jh6Rdo?si=f6_5xgETHcy5Dfxa (Accessed: 16 September 2025)
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
