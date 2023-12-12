using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public TMP_Text noteText; // Reference to a UI Text component for displaying notes
     private void Start()
    {
        // Find the Player component when the item is instantiated
        //player = FindObjectOfType<PlayerControls>();
    }

    private void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Player"))
    {
        PlayerControls player = other.GetComponentInChildren<PlayerControls>();
        
        // Add the item to the player's inventory
        player.inventory.Add(item);

        // If the item is a note, display it on the screen temporarily
        if (item.isNote)
        {
            DisplayNoteText(item.description);
            // Start the destruction coroutine after displaying the note
            StartCoroutine(DestroyAfterDelay(1f));
        }
        else
        {
            // If the item is not a note, start the destruction coroutine immediately
            StartCoroutine(DestroyAfterDelay(0f));
        }
    }
}

    private void DisplayNoteText(string text)
    {
        noteText.text = text;
        // You may want to set up a coroutine to hide the note after a certain duration
        //StartCoroutine(HideNoteAfterDelay());
    }

    // Optional coroutine to hide the note after a delay
    private IEnumerator HideNoteAfterDelay()
    {
        yield return new WaitForSeconds(5f); // Adjust the duration as needed
        noteText.text = "";
    }
  private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Destroy the GameObject containing the ItemPickup script
        Destroy(gameObject);
    }
}




