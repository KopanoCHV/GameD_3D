/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Npc : MonoBehaviour
{
    public GameObject dialogueSpace;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject nextButton;
    public float wordSpeed;
    public bool playerIsClose;
    private bool isTyping = false;

    void Update()
    {
        if (dialogueText.text == dialogue[index])
        {
            nextButton.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            if (!dialogueSpace.activeInHierarchy)
            {
                dialogueSpace.SetActive(true);
                StartCoroutine(Typing());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }

    public void Nextline()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        nextButton.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialogueSpace.SetActive(false);
    }

    IEnumerator Typing()
    {
        isTyping = true;
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }
}*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Npc : MonoBehaviour
{
    public GameObject dialogueSpace;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index;

    public GameObject nextButton;
    public float wordSpeed;
    public bool playerIsClose;
    private bool isTyping = false;

    [Header("Input Settings")]
    public KeyCode nextKey = KeyCode.Space;
   // public KeyCode skipKey = KeyCode.Space;

    void Update()
    {
        if (dialogueText.text == dialogue[index])
        {
            nextButton.SetActive(true);
        }

        // Handle keyboard input for dialogue
        if (playerIsClose && dialogueSpace.activeInHierarchy)
        {
            // Next line when typing is complete
            if (Input.GetKeyDown(nextKey) && !isTyping)
            {
                Nextline();
            }

           /* // Skip typing animation
            if (Input.GetKeyDown(skipKey) && isTyping)
            {
                SkipTyping();
            }*/
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
            if (!dialogueSpace.activeInHierarchy)
            {
                dialogueSpace.SetActive(true);
                StartCoroutine(Typing());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            zeroText();
        }
    }

    public void Nextline()
    {
        nextButton.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            zeroText();
        }
    }

    public void zeroText()
    {
        dialogueText.text = "";
        index = 0;
        dialogueSpace.SetActive(false);
    }

    IEnumerator Typing()
    {
        isTyping = true;
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
        isTyping = false;
    }

    private void SkipTyping()
    {
        StopAllCoroutines();
        dialogueText.text = dialogue[index];
        isTyping = false;
        nextButton.SetActive(true);
    }
}