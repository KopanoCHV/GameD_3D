using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class dialogue : MonoBehaviour
{
    public TextMeshProUGUI dialogueControl;
    public string[] Sentences;
    private int index;
    public float wordSpeed;
    public float sentenceDelay = 2f; // Delay between sentences

    void Start()
    {
        dialogueControl.text = ""; // Clear text at start
        StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        // Wait for one frame to ensure everything is initialized
        yield return null;

        // Process each sentence one by one
        while (index < Sentences.Length)
        {
            yield return StartCoroutine(TypeSentence(Sentences[index]));
            index++;

            // Wait before showing next sentence (if there are more)
            if (index < Sentences.Length)
            {
                yield return new WaitForSeconds(sentenceDelay);
            }
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueControl.text = "";
        foreach (char character in sentence.ToCharArray())
        {
            dialogueControl.text += character;
            yield return new WaitForSeconds(wordSpeed);
        }
    }


}