using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 100;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health -= 10;
            StartCoroutine(FlashWhite());
            if (health <= 0)
            {
                Explode();
            }
        }
    }

    IEnumerator FlashWhite()
    {
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    void Explode()
    {
        CreateExplosion(transform.position);
        Destroy(gameObject);
    }

    void CreateExplosion(Vector3 position)
    {
        // Create a new game object
        GameObject explosion = new GameObject("Explosion");

        // Set the position
        explosion.transform.position = position;

        // Add a Particle System component
        ParticleSystem particleSystem = explosion.AddComponent<ParticleSystem>();

        // Configure the Particle System
        var main = particleSystem.main;
        main.startSize = new ParticleSystem.MinMaxCurve(0.1f, 0.5f);
        main.startSpeed = 5f;
        main.startLifetime = 2f;
        main.startColor = Color.gray;

        // Make the particles fade out
        var colorOverLifetime = particleSystem.colorOverLifetime;
        colorOverLifetime.enabled = true;
        colorOverLifetime.color = new ParticleSystem.MinMaxGradient(new Color(1f, 1f, 1f, 1f), new Color(1f, 1f, 1f, 0f));

        // Make the particles become smaller over time
        var sizeOverLifetime = particleSystem.sizeOverLifetime;
        sizeOverLifetime.enabled = true;
        sizeOverLifetime.size = new ParticleSystem.MinMaxCurve(1f, 0f);

        // Assign the default particle shader
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        renderer.material = new Material(Shader.Find("Particles/Standard Unlit"));

        // Play the Particle System
        particleSystem.Play();

        // Destroy the explosion after a delay
        Destroy(explosion, main.startLifetime.constantMax);
    }
}
