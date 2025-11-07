using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Rigor Mortis Tortoise (2023) How to Collect Coins in Unity 3D Tutorial 2023 (Updated). [Online] Avalable at :  https://youtu.be/6iSJ_jh6Rdo?si=f6_5xgETHcy5Dfxa (Accessed: 16 September 2025)
//Unity Unlocked (2025) Making a 2D Platformer in Unity 6 – Episode 9 (Win Screen). [Online] Available at: https://www.youtube.com/watch?v=0P6c38aisN0 (Accessed: 13 October 2025)
public class Part1Collection : MonoBehaviour
{
    private int Part1 = 0;
    private int Part2 = 0;
    private int Part3 = 0;

    public TextMeshProUGUI Part1text;
    public TextMeshProUGUI Part2text;
    public TextMeshProUGUI Part3text;

    public GameObject winUI;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Part1")
        {
            audioManager.PlaySFX(audioManager.Pickup);
            Part1++;
            Part1text.text = ": " + Part1.ToString();
            Debug.Log(Part1);
            Destroy(other.gameObject);

        if (Part1 == 2)
            {
                audioManager.PlaySFX(audioManager.wolf);
            }

        }

        if (other.transform.tag == "Part2")
        {
            audioManager.PlaySFX(audioManager.Pickup);
            Part2++;
            Part2text.text = ": " + Part2.ToString();
            Debug.Log(Part2);
            Destroy(other.gameObject);
            if (Part2 == 1)
            {
                audioManager.PlaySFX(audioManager.wolf2);
            }
        }

        if (other.transform.tag == "Part3")
        {
            audioManager.PlaySFX(audioManager.Pickup);
            Part3++;
            Part3text.text = ": " + Part3.ToString();
            Debug.Log(Part3);
            Destroy(other.gameObject);
        }
    }
    private void Update()
    {
        if (Part1 == 2 && Part2 == 2 && Part3 == 2)
        {
           
            winUI.SetActive(true);
            Time.timeScale = 0;
        }


    }
}
