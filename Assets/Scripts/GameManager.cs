using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject TowerMenu;
    private TowerMenu towerMenu;

    public GameObject TopMenu;
    private TopMenu topMenu;

    private EnemySpawner enemySpawner;

    private ConstructionSite selectedSite;
    public List<GameObject> Archers = new List<GameObject>();
    public List<GameObject> Swords = new List<GameObject>();
    public List<GameObject> Wizards = new List<GameObject>();

    private int credits;
    private int health;
    public int currentWave = 0;
    private bool waveActive = false;
    public int MaxWave = 5;
    private int enemyInGameCounter = 0;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        enemySpawner = FindObjectOfType<EnemySpawner>(); // Zoek de EnemySpawner in de scene
    }

    void Start()
    {
        towerMenu = TowerMenu.GetComponent<TowerMenu>();
        topMenu = TopMenu.GetComponent<TopMenu>();
        StartGame();
    }

    private void StartGame()
    {
        if (!waveActive)
        {
            waveActive = true;
            Debug.Log("Starting Wave " + currentWave);

            // Stel de waarden in voor credits, health en currentWave
            credits = 200;
            health = 100;


            // Gebruik de functies van TopMenu om de tekst voor elk label in te stellen
            topMenu.SetCreditsLabel("Credits: " + credits.ToString());
            topMenu.SetGateHealthLabel("Health: " + health.ToString());
            topMenu.SetWaveLabel("Wave: " + currentWave.ToString());

        }
    }

        public void AddInGameEnemy()
    {
        enemyInGameCounter++;
    }

    public void RemoveInGameEnemy()
    {
        enemyInGameCounter--;
        if (!waveActive && enemyInGameCounter <= 0)
        {
            if (!waveActive && enemyInGameCounter <= 0)
            {
                // Logica voor het einde van de game
            }
            else
            {
                // Activeer de wave button in het top menu
                topMenu.EnableWaveButton();
            }
        }
    }

    public void StartWave()
    {
        if (!waveActive)
        {
            waveActive = true;
            currentWave++;
            enemyInGameCounter = 0; // Reset de teller aan het begin van de wave
            UpdateLabels();
            EnemySpawner.instance.StartWave(currentWave);
        }
    }

    public void EndWave()
    {
        waveActive = false;
    }

    private void UpdateLabels()
    {
        topMenu.SetCreditsLabel("Credits: " + credits);
        topMenu.SetGateHealthLabel("Health: " + health);
        topMenu.SetWaveLabel("Wave: " + currentWave);
    }


    public void AttackGate()
    {
        health -= 1;
        topMenu.SetGateHealthLabel("Health: " + health);
    }

    public void AddCredits(int amount)
    {
        credits += amount;
        topMenu.SetCreditsLabel("Credits: " + credits);
        // Hier toekomstige logica voor towerMenu evaluatie
    }

    public void RemoveCredits(int amount)
    {
        credits -= amount;
        topMenu.SetCreditsLabel("Credits: " + credits);
        // Hier toekomstige logica voor towerMenu evaluatie
    }

    public int GetCredits()
    {
        return credits;
    }

    public int GetCost(TowerType type, SiteLevel level, bool selling = false)
    {
        int cost = 0;

        // Bepaal de kosten op basis van het type toren en het
        switch (type)
        {
            case TowerType.Archer:
                cost = (level == SiteLevel.Level1) ? 49 : (level == SiteLevel.Level2) ? 74 : (level == SiteLevel.Level3 && !selling) ? 149 : 0;
                break;
            case TowerType.Sword:
                cost = (level == SiteLevel.Level1) ? 74 : (level == SiteLevel.Level2) ? 99 : (level == SiteLevel.Level3 && !selling) ? 199 : 0;
                break;
            case TowerType.Wizard:
                cost = (level == SiteLevel.Level1) ? 99 : (level == SiteLevel.Level2) ? 124 : (level == SiteLevel.Level3 && !selling) ? 249 : 0;
                break;
            default:
                Debug.LogError("Unknown tower type: " + type);
                break;
        }

        return cost;
    }

    public void SelectSite(ConstructionSite site)
    {
        // Onthoud de geselecteerde site
        selectedSite = site;

        // Controleer of towerMenu niet null is
        if (towerMenu != null)
        {
            // Gebruik de reeds bestaande referentie naar TowerMenu
            towerMenu.SetSite(site);
        }
        else
        {
            // Log een fout als TowerMenu om een of andere reden null is.
            Debug.LogError("TowerMenu component is null in GameManager.");
        }
    }

    public void Build(Enums.TowerType type, Enums.SiteLevel level)
    {
        // Controleer of er een site geselecteerd is. Zo niet, log een fout en keer terug.
        if (selectedSite == null)
        {
            Debug.LogError("Er is geen bouwplaats geselecteerd. Kan de toren niet bouwen.");
            return;
        }

        // Logica voor het aanpassen van credits afhankelijk van aankoop of verkoop
        if (level == Enums.SiteLevel.Unbuilt)
        {
            // Verkooplogica
            AddCredits(GetCost(type, selectedSite.Level, true));
        }
        else
        {
            // Aankooplogica
            int cost = GetCost(type, level);
            if (GetCredits() >= cost)
            {
                RemoveCredits(cost);
            }
            else
            {
                Debug.LogError("Niet genoeg credits om de toren te bouwen.");
                return;
            }
        }

        GameObject towerPrefab = null;

        // Trek 1 af van de level waarde om de correcte index te krijgen
        int prefabIndex = (int)level - 1;

        switch (type)
        {
            case Enums.TowerType.Archer:
                towerPrefab = Archers[prefabIndex];
                break;
            case Enums.TowerType.Sword:
                towerPrefab = Swords[prefabIndex];
                break;
            case Enums.TowerType.Wizard:
                towerPrefab = Wizards[prefabIndex];
                break;
        }

        if (towerPrefab == null)
        {
            Debug.LogError("Geen tower prefab gevonden voor het geselecteerde type en niveau.");
            return;
        }

        // Gebruik de WorldPosition van de selectedSite voor het positioneren van de nieuwe toren
        GameObject tower = Instantiate(towerPrefab, selectedSite.WorldPosition, Quaternion.identity);

        // Gebruik de SetTower methode van de ConstructionSite om de nieuwe toren in te stellen en te configureren
        selectedSite.SetTower(tower, level, type);

        if (towerMenu != null)
        {
            towerMenu.SetSite(null); // Verberg het towerMenu
        }
    }


    public void DestroyTower()
    {
        if (selectedSite == null)
        {
            Debug.LogError("Er is geen bouwplaats geselecteerd. Kan de toren niet verwijderen.");
            return;
        }

        // Bereken de verkoopwaarde van de toren
        int sellValue = GetCost(selectedSite.TowerType, selectedSite.Level, selling: true);

        // Voeg de verkoopwaarde toe aan de spelercredits
        AddCredits(sellValue);

        // Roep de RemoveTower methode aan van de selectedSite
        selectedSite.ClearTower();

        // Verberg het towerMenu als dat nodig is
        if (towerMenu != null)
        {
            towerMenu.SetSite(null);
        }
    }
}
