
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

    [Header("Input Settings")]
    public KeyCode nextKey = KeyCode.Q;
    public KeyCode skipKey = KeyCode.Space;

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

           // Skip typing animation
            if (Input.GetKeyDown(skipKey) && isTyping)
            {
                SkipTyping();
            }
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
}*/
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Npc : MonoBehaviour
{
    [Header("Dialogue Content")]
    public string[] dialogue;

    [Header("Optional Dialogue UI - Leave empty to use player's UI")]
    public GameObject dialogueSpace;
    public TextMeshProUGUI dialogueText;
    public GameObject nextButton;

    [Header("Input Settings")]
    public InputActionReference dialogueNextAction;
   // public InputActionReference dialogueSkipAction;

    private void OnEnable()
    {
        // Enable input actions if they are assigned
        if (dialogueNextAction != null)
        {
            dialogueNextAction.action.Enable();
        }
        /*if (dialogueSkipAction != null)
        {
            dialogueSkipAction.action.Enable();
        }*/
    }

    private void OnDisable()
    {
        // Disable input actions if they are assigned
        if (dialogueNextAction != null)
        {
            dialogueNextAction.action.Disable();
        }
       /* if (dialogueSkipAction != null)
        {
            dialogueSkipAction.action.Disable();
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FPController player = other.GetComponent<FPController>();
            if (player != null && player.CanStartDialogue())
            {
                player.StartDialogue(dialogue, this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FPController player = other.GetComponent<FPController>();
            if (player != null)
            {
                // Optional: Add any behavior when player exits NPC trigger
            }
        }
    }

    public void OnDialogueEnd()
    {
        // Optional: Add any NPC-specific behavior when dialogue ends
        // For example: trigger events, animations, quest updates, etc.
        Debug.Log("Dialogue ended with " + gameObject.name);
    }
}