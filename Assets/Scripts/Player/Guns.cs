using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{

    public Transform arFirePoint;
    public GameObject arBulletPrefab;

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
        Instantiate(arBulletPrefab, arFirePoint.position, arFirePoint.rotation);
    }
}
