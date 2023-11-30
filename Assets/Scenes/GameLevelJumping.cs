using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneJump2 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) // Updated for 2D collision
    {
        Debug.Log("2D Collision entered!");
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("Slums"); // Scene you want to jump to name
        }
    }
}
