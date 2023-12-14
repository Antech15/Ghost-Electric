using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneJump2 : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) // Updated for 2D collision
    {
        Debug.Log("2D Collision entered!");
        if (collision.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().name == "Level 1 - Outskirts")
        {
            SceneManager.LoadScene("Slums"); // Scene you want to jump to name
        }
        //if we are in Slums, jump to the next scene
        
        if (collision.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().name == "Slums")
        {
            SceneManager.LoadScene("Boss1"); // Scene you want to jump to name
        }
        

        if (collision.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().name == "Boss1")
        {
            SceneManager.LoadScene("Sewers"); // Scene you want to jump to name
        }
    }
}
