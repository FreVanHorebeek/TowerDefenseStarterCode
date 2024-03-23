using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    public float health = 10f;
    public int points = 1;
    public Enums.Path path { get; set; }
    public GameObject target { get; set; }
    private int pathIndex = 1;

    // Methode om schade toe te passen op de vijand
    public void Damage(float damage)
    {
        health -= damage;

        // Controleer of de gezondheid van de vijand nul of kleiner is
        if (health <= 0)
        {
            // Vernietig de vijand
            Destroy(gameObject);
        }
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);

        // Controleer hoe dicht we bij het doelwit zijn
        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            // Als we dichtbij zijn, vraag een nieuw doelwit aan
            target = EnemySpawner.instance.RequestTarget(path, pathIndex);
            pathIndex++;

            // Als het doelwit null is, zijn we aan het einde van het pad gekomen. Vernietig de vijand op dit punt
            if (target == null)
            {
                Destroy(gameObject);
            }
        }
    }
}

