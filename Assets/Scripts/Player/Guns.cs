using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Guns : MonoBehaviour
{
    public AudioSource gunshot;
    public Transform arFirePoint;
    public GameObject arBulletPrefab;
    public int maxClipSize = 10;
    private int currentClip;

    public TextMeshProUGUI reloadingText; // Reference to the UI text element for displaying reloading text
    public float fadeDuration = 0.5f; // Duration of the fading effect

    void Start()
    {
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        
        gunshot = audioSources[1];
        currentClip = maxClipSize;
        HideReloadingText();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            Debug.Log("Shoot");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        
    }

    void Shoot()
    {
        if(currentClip>0)
        {
            currentClip--;
           
            // Shooting logic
            gunshot.Play();
            Instantiate(arBulletPrefab, arFirePoint.position, arFirePoint.rotation);
            if(currentClip <=0)
            {
                StartCoroutine(DisplayReloadPrompt());
            }
        }
    }

    void Reload()
    {
        StartCoroutine(DisplayReloadingText());
        StartCoroutine(WaitAndReload(2.0f));
    }

    IEnumerator DisplayReloadPrompt()
    {
        reloadingText.text = "Press 'R' to Reload";
        // Fade in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            reloadingText.color = new Color(reloadingText.color.r, reloadingText.color.g, reloadingText.color.b, alpha);
            yield return null;
        }
        //HideReloadingText();
      
    }

    IEnumerator DisplayReloadingText()
    {
        reloadingText.text = "Reloading...";
        // Fade in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            reloadingText.color = new Color(reloadingText.color.r, reloadingText.color.g, reloadingText.color.b, alpha);
            yield return null;
        }

        // Wait for a short duration
        yield return new WaitForSeconds(1.0f);

        // Fade out
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            reloadingText.color = new Color(reloadingText.color.r, reloadingText.color.g, reloadingText.color.b, alpha);
            yield return null;
        }
        HideReloadingText();

        
    }

    IEnumerator WaitAndReload(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        // Code to execute after waiting (in this case, reload)
        currentClip = maxClipSize;
    }

    void HideReloadingText()
    {
        reloadingText.color = new Color(reloadingText.color.r, reloadingText.color.g, reloadingText.color.b, 0f);
    }
}
