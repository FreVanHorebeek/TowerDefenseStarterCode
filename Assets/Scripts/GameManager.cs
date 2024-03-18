using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Referentie naar het TowerMenu gameobject
    public GameObject towerMenuObject;

    // Variabele om het TowerMenu script bij te houden
    private towerMenu towerMenu;

    // Variabelen om de prefab towers in op te slaan
    public List<GameObject> Archers;
    public List<GameObject> Swords;
    public List<GameObject> Wizards;

    // Variabele om de geselecteerde site bij te houden
    private ConstructionSite selectedSite;

    // Start is called before the first frame update
    void Start()
    {
        // Haal het TowerMenu script op van het TowerMenu gameobject
        towerMenu = towerMenuObject.GetComponent<towerMenu>();
    }

    // Functie om een constructiesite te selecteren
    public void SelectSite(ConstructionSite site)
    {
        // Onthoud de geselecteerde site
        selectedSite = site;

        // Geef de geselecteerde site door aan het TowerMenu
        towerMenu.SetSite(selectedSite);
    }
}
