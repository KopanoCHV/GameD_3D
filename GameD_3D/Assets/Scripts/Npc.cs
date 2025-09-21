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

    void Update()
    {
        if (dialogueText.text == dialogue[index])
        {
            nextButton.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
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

    private void OnTriggerExit2D(Collider2D other)
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
}