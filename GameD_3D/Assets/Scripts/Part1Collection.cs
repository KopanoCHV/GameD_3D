using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Rigor Mortis Tortoise (2023) How to Collect Coins in Unity 3D Tutorial 2023 (Updated). [Online] Avalable at :  https://youtu.be/6iSJ_jh6Rdo?si=f6_5xgETHcy5Dfxa (Accessed: 16 September 2025)
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
