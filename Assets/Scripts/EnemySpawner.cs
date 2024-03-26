using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public List<GameObject> Path1 = new List<GameObject>();
    public List<GameObject> Path2 = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();

    private int ufoCounter = 0; // Toegevoegde variabele

    private GameManager gameManager;

    // Deze methode retourneert het doelwit voor het opgegeven pad en index
    public GameObject RequestTarget(Enums.Path path, int index)
    {
        List<GameObject> selectedPath = path == Enums.Path.Path1 ? Path1 : Path2;

        if (index < selectedPath.Count)
            return selectedPath[index];
        else
            return null;
    }

    // Deze methode spawnt een vijand van het opgegeven type en plaatst deze op het begin van het opgegeven pad
    private void SpawnEnemy(int type, Enums.Path path)
    {
        List<GameObject> selectedPath = path == Enums.Path.Path1 ? Path1 : Path2;

        if (selectedPath.Count < 2)
        {
            Debug.LogError("Path doesn't have enough waypoints.");
            return;
        }

        var newEnemy = Instantiate(Enemies[type], selectedPath[0].transform.position, selectedPath[0].transform.rotation);
        var script = newEnemy.GetComponent<Enemy>();
        script.path = path;
        script.target = selectedPath[1];

        gameManager.AddInGameEnemy();
    }


    // Deze methode wordt aangeroepen bij het starten van het spel
    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        gameManager = GameManager.Instance;
    }


    public void StartWave(int number)
    {
        // Reset de teller
        ufoCounter = 0;

        // Start de wave op basis van het nummer
        switch (number)
        {
            case 1:
                InvokeRepeating("StartWave1", 1f, 1f);
                break;
            case 2:
                InvokeRepeating("StartWave2", 1f, 1f);
                break;
            case 3:
                InvokeRepeating("StartWave3", 1f, 1.5f);
                break;
            case 4:
                InvokeRepeating("StartWave4", 1f, 2f);
                break;
            case 5:
                InvokeRepeating("StartWave5", 1f, 2.5f);
                break;
        }
    }
    private void StartWave1()
    {
        SpawnEnemy(0, Enums.Path.Path1); 
        ufoCounter++;

        // Stop de wave na een bepaald aantal UFO's
        if (ufoCounter >= 5) 
        {
            CancelInvoke("StartWave1");
        }
    }
    private void StartWave2()
    {
        if (ufoCounter < 25)
        {
            SpawnEnemy(0, Enums.Path.Path2); 
            ufoCounter++;
        }
        else if (ufoCounter < 35)
        {
            SpawnEnemy(1, Enums.Path.Path2); 
            ufoCounter++;
        }
        else if (ufoCounter < 55)
        {
            SpawnEnemy(Random.Range(0, Enemies.Count), Enums.Path.Path2); // Random mix van vijanden
            ufoCounter++;
        }
        else
        {
            CancelInvoke("StartWave2");
        }
    }
    // Functie om wave 3 te starten
    private void StartWave3()
    {
        if (ufoCounter < 45)
        {
            SpawnEnemy(1, Enums.Path.Path1); 
            ufoCounter++;
        }
        else if (ufoCounter < 65)
        {
            SpawnEnemy(2, Enums.Path.Path1); // Veronderstel dat type 2 een nog moeilijkere vijand is, je kunt dit aanpassen aan je eigen logica
            ufoCounter++;
        }
        else if (ufoCounter < 85)
        {
            SpawnEnemy(Random.Range(0, Enemies.Count), Enums.Path.Path1); 
        }
        else
        {
            CancelInvoke("StartWave3");
        }
    }
    private void StartWave4()
    {
        if (ufoCounter < 65)
        {
            SpawnEnemy(2, Enums.Path.Path2); 
            ufoCounter++;
        }
        else if (ufoCounter < 85)
        {
            SpawnEnemy(3, Enums.Path.Path2); 
            ufoCounter++;
        }
        else if (ufoCounter < 115)
        {
            SpawnEnemy(Random.Range(0, Enemies.Count), Enums.Path.Path2); 
            ufoCounter++;
        }
        else
        {
            CancelInvoke("StartWave4");
        }
    }

    private void StartWave5()
    {
        if (ufoCounter < 95)
        {
            SpawnEnemy(3, Enums.Path.Path1); 
            ufoCounter++;
        }
        else if (ufoCounter < 125)
        {
            SpawnEnemy(4, Enums.Path.Path1); 
            ufoCounter++;
        }
        else if (ufoCounter < 155)
        {
            SpawnEnemy(Random.Range(0, Enemies.Count), Enums.Path.Path1); 
            ufoCounter++;
        }
        else
        {
            CancelInvoke("StartWave5");
        }
    }
}
