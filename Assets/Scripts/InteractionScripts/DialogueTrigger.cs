using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string dialogue;
    private DialogueManager dialogueManager;
    private bool hasTriggered = false;

    private void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered && other.CompareTag("Player") && dialogueManager != null)
        {
            dialogueManager.StartDialogue(dialogue);
            hasTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && dialogueManager != null)
        {
            dialogueManager.EndDialogue();
        }
    }
}