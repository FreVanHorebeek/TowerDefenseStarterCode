using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Referentie naar het TowerMenu gameobject
    public GameObject towerMenuObject;

    // Variabele om het TowerMenu script bij te houden
    private towerMenu towerMenu; // Let op: gewijzigd van 'towerMenu' naar 'TowerMenu'

    // Variabelen om de prefab towers in op te slaan
    public List<GameObject> Archers;
    public List<GameObject> Swords;
    public List<GameObject> Wizards;

    // Variabele om de geselecteerde site bij te houden
    private ConstructionSite selectedSite;

    // Awake wordt aangeroepen voordat Start wordt aangeroepen
    private void Awake()
    {
        // Zorg ervoor dat er slechts één instantie van GameManager is
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Vernietig dit object als er al een instantie bestaat
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Haal het TowerMenu script op van het TowerMenu gameobject
        towerMenu = towerMenuObject.GetComponent<towerMenu>(); // Let op: gewijzigd van 'towerMenu' naar 'TowerMenu'
    }

    // Functie om een constructiesite te selecteren
    public void SelectSite(ConstructionSite site)
    {
        // Onthoud de geselcteerde site
        selectedSite = site;

        // Geef de geselecteerde site door aan het TowerMenu
        towerMenu.SetSite(selectedSite);
    }

    // Functie om een toren te bouwen op de geselecteerde site
    public void Build(TowerType type, SiteLevel level)
    {
        // Je kunt niets bouwen als er geen site is geselecteerd
        if (selectedSite == null)
            return;

        // Gebruik switch met de TowerType om de juiste lijst te selecteren
        List<GameObject> towerList;
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
            default:
                return; // Als er een ongeldig type is, stop dan hier
        }

        // Gebruik switch met het level om een GameObject-toren te maken
        GameObject towerPrefab = null;
        switch (level)
        {
            case SiteLevel.Unbuilt:
                return; // Kan geen toren bouwen op een onbebouwde site
            case SiteLevel.Level1:
                towerPrefab = towerList[0];
                break;
            case SiteLevel.Level2:
                towerPrefab = towerList[1];
                break;
            case SiteLevel.Level3:
                towerPrefab = towerList[2];
                break;
            default:
                return; // Als er een ongeldig level is, stop dan hier
        }

        // Instantieer de geselecteerde toren op de geselecteerde site
        GameObject newTower = Instantiate(towerPrefab, selectedSite.WorldPosition, Quaternion.identity);

        // Configureer de geselecteerde site om de toren in te stellen
        selectedSite.SetTower(newTower, level, type);

        // Geef null door aan de SetSite-functie in TowerMenu om het menu te verbergen
        towerMenu.SetSite(null);
    }
}
