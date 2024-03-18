using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class ConstructionSite
{
    // Properties
    public Vector3Int TilePosition { get; private set; }
    public Vector3 WorldPosition { get; private set; }
    public SiteLevel Level { get; private set; }
    public TowerType TowerType { get; private set; }

    // Private fields
    private GameObject tower;

    // Constructor
    public ConstructionSite(Vector3Int tilePosition, Vector3 worldPosition)
    {
        // Wijs de tilePosition en worldPosition toe
        TilePosition = tilePosition;
        WorldPosition = new Vector3(worldPosition.x, worldPosition.y + 0.5f, worldPosition.z); // Pas de Y-waarde aan

        // Stel tower gelijk aan null
        tower = null;
    }

    // Methode om een toren in te stellen op de constructiesite
    public void SetTower(GameObject newTower, SiteLevel newLevel, TowerType newType)
    {
        // Controleer of er al een toren is op de constructiesite
        if (tower != null)
        {
            // Als er al een toren is, verwijder deze handmatig uit de scène
            GameObject.Destroy(tower);
        }

        // Wijs de nieuwe toren toe aan de constructiesite
        tower = newTower;
        Level = newLevel;
        TowerType = newType;
    }

    // Voeg extra methoden of logica toe indien nodig
}
