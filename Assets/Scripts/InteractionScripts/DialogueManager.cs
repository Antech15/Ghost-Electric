using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;

    public void StartDialogue(string dialogue)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = dialogue;
    }

    public void EndDialogue()
    {
        StartCoroutine(HideDialogueAfterDelay(5f));
    }

    private IEnumerator HideDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        dialoguePanel.SetActive(false);
    }
}

