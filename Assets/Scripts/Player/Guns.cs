using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    public AudioSource gunshot;
    public Transform arFirePoint;
    public GameObject arBulletPrefab;

    void Start()
    {
        AudioSource[] audioSources = GetComponentsInChildren<AudioSource>();
        
        gunshot = audioSources[1];
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
            Debug.Log("Shoot");
        }
    }

    void Shoot()
    {
        // Shooting logic
        gunshot.Play();
        Instantiate(arBulletPrefab, arFirePoint.position, arFirePoint.rotation);
    }
}
