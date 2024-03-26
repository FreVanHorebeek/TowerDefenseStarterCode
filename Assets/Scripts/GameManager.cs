using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private ConstructionSite selectedSite;
    public GameObject TowerMenu;
    private TowerMenu towerMenu;
    public List<GameObject> Archers = new List<GameObject>();
    public List<GameObject> Swords = new List<GameObject>();
    public List<GameObject> Wizards = new List<GameObject>();

    private int credits; // Nieuwe variabele voor credits
    private int health; // Nieuwe variabele voor gezondheid
    private int currentWave; // Nieuwe variabele voor huidige golf

    private bool waveActive = false; // Variabele om de status van de golf bij te houden

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        towerMenu = TowerMenu.GetComponent<TowerMenu>();
        StartGame(); // Start het spel wanneer GameManager wordt gestart
    }

    // Functie om het spel te starten en waarden in te stellen
    private void StartGame()
    {
        credits = 200; // Stel credits in op 200
        health = 10; // Stel gezondheid in op 10
        currentWave = 0; // Stel huidige golf in op 0

        // Stel de tekst voor elk label in het TopMenu in
        towerMenu.SetCreditsLabel("Credits: " + credits);
        towerMenu.SetHealthLabel("Health: " + health);
        towerMenu.SetWaveLabel("Wave: " + currentWave);
    }

    // Functie om een toren te bouwen of te verkopen
    public void Build(TowerType type, SiteLevel level)
    {
        // Je kunt niet bouwen als er geen site is geselecteerd
        if (selectedSite == null)
        {
            return;
        }

        // Bepaal de kosten voor het bouwen of verkopen van de toren
        int cost = GetCost(type, level, selectedSite.Level == SiteLevel.Unbuilt);

        // Controleer of de speler voldoende credits heeft
        if (cost <= credits)
        {
            // Bouw of verkoop de toren
            if (selectedSite.Level == SiteLevel.Unbuilt)
            {
                // Bouw de toren
                GameObject towerPrefab = GetTowerPrefab(type, level);
                Vector3 buildPosition = selectedSite.BuildPosition();
                GameObject towerInstance = Instantiate(towerPrefab, buildPosition, Quaternion.identity);
                selectedSite.SetTower(towerInstance, level, type);
                RemoveCredits(cost); // Verlaag de credits na het bouwen van de toren
            }
            else
            {
                // Verkoop de toren
                AddCredits(cost); // Voeg credits toe na het verkopen van de toren
                selectedSite.ClearTower();
            }

            towerMenu.SetSite(null);
        }
        else
        {
            Debug.Log("Not enough credits!");
        }
    }

    // Functie om de kosten voor het bouwen of verkopen van een toren te krijgen
    public int GetCost(TowerType type, SiteLevel level, bool selling = false)
    {
        int cost = 0;

        // Bereken de kosten op basis van het type en het niveau van de toren
        switch (type)
        {
            case TowerType.Archer:
                cost = 50; // Stel hier de kosten in voor de boogschutterstoren
                break;
            case TowerType.Sword:
                cost = 75; // Stel hier de kosten in voor de zwaardtoren
                break;
            case TowerType.Wizard:
                cost = 100; // Stel hier de kosten in voor de toren van de tovenaar
                break;
            default:
                Debug.LogError("Unknown tower type!");
                break;
        }

        // Als het gaat om een verkoop, halveer dan de kosten
        if (selling)
        {
            cost /= 2;
        }

        // Als het een upgrade is, verhoog dan de kosten op basis van het huidige niveau
        if (level != SiteLevel.Unbuilt)
        {
            cost *= ((int)level + 1);
        }

        return cost;
    }

    // Functie om credits toe te voegen
    public void AddCredits(int amount)
    {
        credits += amount;
        towerMenu.SetCreditsLabel("Credits: " + credits);
    }

    // Functie om credits te verwijderen
    private void RemoveCredits(int amount)
    {
        credits -= amount;
        towerMenu.SetCreditsLabel("Credits: " + credits);
    }
    public int GetCredits()
    {
        return credits;
    }

    // Functie om de gezondheid te verminderen
    public void AttackGate()
    {
        health--;
        towerMenu.SetHealthLabel("Health: " + health);
    }

    // Functie om het aantal golven te verhogen
    public void IncreaseWave()
    {
        currentWave++;
        towerMenu.SetWaveLabel("Wave: " + currentWave);
    }

    // Functie om het aantal golven te verlagen
    public void DecreaseWave()
    {
        currentWave--;
        if (currentWave < 0)
        {
            currentWave = 0;
        }
        towerMenu.SetWaveLabel("Wave: " + currentWave);
    }

    public void SelectSite(ConstructionSite site)
    {
        selectedSite = site;
        towerMenu.SetSite(site);
    }

    // Functie om de prefab van de toren op te halen op basis van het type en niveau
    private GameObject GetTowerPrefab(TowerType type, SiteLevel level)
    {
        List<GameObject> towerList = null;

        switch (type)
        {
            case TowerType.Archer:
                towerList = Archers;
                break;
            case TowerType.Sword:
                towerList = Swords;
                break;
            case TowerType.Wizard:
                towerList = Wizards;
                break;
        }

        return towerList[(int)level];
    }

    // Functie om een golf te starten
    public void StartWave()
    {
        currentWave++;
        towerMenu.SetWaveLabel("Wave: " + currentWave);
        waveActive = true;
        EnemySpawner.instance.StartWave(currentWave); // Start de nieuwe golf
    }

    // Functie om een golf te beëindigen
    public void EndWave()
    {
        waveActive = false;
        // Voeg hier eventuele extra logica toe voor het beëindigen van een golf
    }
}
