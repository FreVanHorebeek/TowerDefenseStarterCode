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
            // Als de gezondheid nul of lager is, roep de AddCredits-functie van GameManager aan met het aantal punten
            GameManager.Instance.AddCredits(points);

            // Vernietig de vijand
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float step = speed * Time.deltaTime;

        Vector2 vector2 = Vector2.MoveTowards(transform.position, target.transform.position, step);
        transform.position = vector2;

        // Check how close we are to the target
        if (Vector2.Distance(transform.position, target.transform.position) < 0.1f)
        {
            // If close, request a new waypoint
            target = EnemySpawner.instance.RequestTarget(path, pathIndex);
            pathIndex++;

            // If target is null, we have reached the end of the path.
            // Destroy the enemy at this point
            if (target == null)
            {
                GameManager.Instance.AttackGate();
                Destroy(gameObject);
            }
        }
    }
    // Function added to set the path index
    public void SetPathIndex(int index)
    {
        pathIndex = index;
    }
}
