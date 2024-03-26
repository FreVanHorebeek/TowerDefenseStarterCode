using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    public List<GameObject> Path1 = new List<GameObject>();
    public List<GameObject> Path2 = new List<GameObject>();
    public List<GameObject> Enemies = new List<GameObject>();

    private int ufoCounter = 0; // Toegevoegde variabele
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Deze methode retourneert het doelwit voor het opgegeven pad en index
    public GameObject RequestTarget(Enums.Path path, int index)
    {
        List<GameObject> selectedPath;

        // Selecteer het juiste pad op basis van de meegegeven 'path' parameter
        if (path == Enums.Path.Path1)
            selectedPath = Path1;
        else if (path == Enums.Path.Path2)
            selectedPath = Path2;
        else
        {
            Debug.LogError("Ongeldig pad gespecificeerd!");
            return null;
        }

        // Controleer of de index binnen het bereik van het pad ligt
        if (index >= 0 && index < selectedPath.Count)
            return selectedPath[index]; // Retourneer het doelwit op de gegeven index
        else
        {
            Debug.LogError("Ongeldige index gespecificeerd!");
            return null;
        }
    }

    // Deze methode spawnt een vijand van het opgegeven type en plaatst deze op het begin van het opgegeven pad
    private void SpawnEnemy(int type, Enums.Path path)
    {
        Vector3 spawnPosition;
        Quaternion spawnRotation;

        // Bepaal de spawnpositie en -rotatie op basis van het opgegeven pad
        if (path == Enums.Path.Path1)
        {
            spawnPosition = Path1[0].transform.position;
            spawnRotation = Path1[0].transform.rotation;
        }
        else if (path == Enums.Path.Path2)
        {
            spawnPosition = Path2[0].transform.position;
            spawnRotation = Path2[0].transform.rotation;
        }
        else
        {
            // Handel fout of standaardgeval af
            spawnPosition = Vector3.zero;
            spawnRotation = Quaternion.identity;
            Debug.LogError("Ongeldig pad gespecificeerd!");
            return;
        }

        // Instantieer de vijand op de spawnpositie met de spawnrotatie
        var newEnemy = Instantiate(Enemies[type], spawnPosition, spawnRotation);

        // Je kunt hier eventueel extra logica toevoegen om het pad en doelwit voor de vijand in te stellen
        // Haal het Enemy-component op van de nieuw gespawnde vijand
        var enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            // Stel het pad en het doelwit in voor de vijand
            enemyScript.path = path;
            enemyScript.target = RequestTarget(path, 1); // Start altijd bij index 1
        }
        else
        {
            Debug.LogError("Enemy-component niet gevonden op gespawnde vijand!");
        }
    }

    // Deze methode wordt aangeroepen bij het starten van het spel
    void Start()
    {
        // Start een golf met een bepaald nummer, bijvoorbeeld:
        StartWave(1);
    }
    public void StartWave(int number)
    {
        ufoCounter = 0;
        switch (number)
        {
            case 1:
                InvokeRepeating("StartWave1", 1f, 1.5f);
                break;
                // Voeg hier cases toe voor extra waves
        }
    }

    public void StartWave1()
    {
        ufoCounter++;
        if (ufoCounter % 6 <= 1) return;
        if (ufoCounter < 30)
        {
            SpawnEnemy(0, Enums.Path.Path1);
        }
        else
        {
            SpawnEnemy(1, Enums.Path.Path1); // Laatste vijand is niveau 2
        }
        if (ufoCounter > 30)
        {
            CancelInvoke("StartWave1"); // Beëindig deze wave
            GameManager.Instance.EndWave(); // Laat GameManager weten dat de golf voorbij is
        }
    }
    // Deze methode wordt elke seconde aangeroepen om een vijand te spawnen voor testdoeleinden
    private void SpawnTester()
    {
        // Voorbeeld: spawn een vijand van type 0 op pad 1
        SpawnEnemy(0, Enums.Path.Path1);
    }
}
