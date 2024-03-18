using UnityEngine;
using UnityEngine.UIElements;
using static Enums;

public class towerMenu : MonoBehaviour
{
    private Button ArcherTower;
    private Button SwordTower;
    private Button WizardTower;
    private Button Upgrade;
    private Button MenuDestroy; // Hernoem de Destroy variabele naar MenuDestroy
    private ConstructionSite selectedSite;
    private VisualElement root;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        ArcherTower = root.Q<Button>("archer-tower");
        SwordTower = root.Q<Button>("sword-tower");
        WizardTower = root.Q<Button>("wizard-tower");
        Upgrade = root.Q<Button>("button-upgrade");
        MenuDestroy = root.Q<Button>("button-destroy"); // Hernoem de variabele toegewezen aan de vernietigingsknop


        if (ArcherTower != null)
        {
            ArcherTower.clicked += OnArcherButtonClicked;
        }

        if (SwordTower != null)
        {
            SwordTower.clicked += OnSwordButtonClicked;
        }

        if (WizardTower != null)
        {
            WizardTower.clicked += OnWizardButtonClicked;
        }

        if (Upgrade != null)
        {
            Upgrade.clicked += OnUpgradeButtonClicked;
        }

        if (MenuDestroy != null) // Gebruik de hernoemde variabele
        {
            MenuDestroy.clicked += OnDestroyButtonClicked; // Gebruik de hernoemde variabele
        }

        root.visible = false;
    }

    private void OnArcherButtonClicked()
    {
        // Implementeer deze functie indien nodig
    }

    private void OnSwordButtonClicked()
    {
        // Implementeer deze functie indien nodig
    }

    private void OnWizardButtonClicked()
    {
        // Implementeer deze functie indien nodig
    }

    private void OnUpgradeButtonClicked()
    {
        // Implementeer deze functie indien nodig
    }

    private void OnDestroyButtonClicked()
    {
        // Implementeer deze functie indien nodig
    }

    private void OnDestroy()
    {
        if (ArcherTower != null)
        {
            ArcherTower.clicked -= OnArcherButtonClicked;
        }

        if (SwordTower != null)
        {
            SwordTower.clicked -= OnSwordButtonClicked;
        }

        if (WizardTower != null)
        {
            WizardTower.clicked -= OnWizardButtonClicked;
        }

        if (Upgrade != null)
        {
            Upgrade.clicked -= OnUpgradeButtonClicked;
        }

        if (MenuDestroy != null) // Gebruik de hernoemde variabele
        {
            MenuDestroy.clicked -= OnDestroyButtonClicked; // Gebruik de hernoemde variabele
        }
    }

    // Functie om het menu te evalueren en de knoppen in of uit te schakelen op basis van de huidige geselecteerde site
    public void EvaluateMenu()
    {
        if (selectedSite == null)
            return;

        switch (selectedSite.Level)
        {
            case SiteLevel.Unbuilt:
                ArcherTower.SetEnabled(true);
                SwordTower.SetEnabled(true);
                WizardTower.SetEnabled(true);
                Upgrade.SetEnabled(false);
                MenuDestroy.SetEnabled(false); // Gebruik de hernoemde variabele
                break;
            case SiteLevel.Level1:
            case SiteLevel.Level2:
                ArcherTower.SetEnabled(false);
                SwordTower.SetEnabled(false);
                WizardTower.SetEnabled(false);
                Upgrade.SetEnabled(true);
                MenuDestroy.SetEnabled(true); // Gebruik de hernoemde variabele
                break;
            case SiteLevel.Level3:
                ArcherTower.SetEnabled(false);
                SwordTower.SetEnabled(false);
                WizardTower.SetEnabled(false);
                Upgrade.SetEnabled(false);
                MenuDestroy.SetEnabled(true); // Gebruik de hernoemde variabele
                break;
        }
    }

    // Functie om een constructiesite in te stellen als de geselecteerde site
    public void SetSite(ConstructionSite site)
    {
        selectedSite = site;

        if (selectedSite == null)
        {
            root.visible = false;
            return;
        }

        root.visible = true;
        EvaluateMenu();
    }
}
