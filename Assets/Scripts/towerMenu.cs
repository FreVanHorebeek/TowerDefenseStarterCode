using UnityEngine;
using UnityEngine.UIElements;
using static Enums;

public class TowerMenu : MonoBehaviour
{
    private Button archerButton;
    private Button swordButton;
    private Button wizardButton;
    private Button updateButton;
    private Button destroyButton;
    private ConstructionSite selectedSite;
    private VisualElement root;
    private Label creditsLabel;
    private Label healthLabel;
    private Label waveLabel;

    public void SetCreditsLabel(string text)
    {
        creditsLabel.text = text;
    }

    public void SetHealthLabel(string text)
    {
        healthLabel.text = text;
    }

    public void SetWaveLabel(string text)
    {
        waveLabel.text = text;
    }
    public void EvaluateMenu()
    {
        if (selectedSite == null)
            return;

        // Haal het niveau van de geselecteerde constructieplaats op
        SiteLevel siteLevel = selectedSite.Level;

        // Haal de beschikbare credits op van de GameManager
        int availableCredits = GameManager.Instance.GetCredits();

        // Schakel alle knoppen in het torenmenu uit
        archerButton.SetEnabled(false);
        swordButton.SetEnabled(false);
        wizardButton.SetEnabled(false);
        updateButton.SetEnabled(false);
        destroyButton.SetEnabled(false);
        // Gebruik een switch om de knoppen in te schakelen op basis van het niveau van de constructieplaats
        switch (siteLevel)
        {
            case Enums.SiteLevel.Unbuilt:
                // Alleen de torenknoppen moeten worden ingeschakeld als er voldoende credits zijn
                if (availableCredits >= GameManager.Instance.GetCost(TowerType.Archer, Enums.SiteLevel.Unbuilt))
                    archerButton.SetEnabled(true);
                if (availableCredits >= GameManager.Instance.GetCost(TowerType.Sword, Enums.SiteLevel.Unbuilt))
                    swordButton.SetEnabled(true);
                if (availableCredits >= GameManager.Instance.GetCost(TowerType.Wizard, Enums.SiteLevel.Unbuilt))
                    wizardButton.SetEnabled(true);
                break;
            case Enums.SiteLevel.Level1:
            case Enums.SiteLevel.Level2:
                // Alleen de update- en destroy-knoppen moeten werken
                updateButton.SetEnabled(true);
                destroyButton.SetEnabled(true);
                break;
            case Enums.SiteLevel.Level3:
                // Alleen de destroy-knop moet werken
                destroyButton.SetEnabled(true);
                break;
            default:
                Debug.LogWarning("Unknown site level: " + siteLevel);
                break;
        }
    }

    public void SetSite(ConstructionSite site)
    {
        // Assign the site to the selectedSite variable
        selectedSite = site;

        if (selectedSite == null)
        {
            // If the selected site is null, hide the menu and return
            root.visible = false;
            return;
        }
        else
        {
            // If the selected site is not null, make sure the menu is visible
            root.visible = true;

            // Call the EvaluateMenu method to update button visibility
            EvaluateMenu();
        }
    }

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        archerButton = root.Q<Button>("archer-tower");
        swordButton = root.Q<Button>("sword-tower");
        wizardButton = root.Q<Button>("wizard-tower");
        updateButton = root.Q<Button>("upgrade");
        destroyButton = root.Q<Button>("destroy");

        if (archerButton != null)
        {
            archerButton.clicked += OnArcherButtonClicked;
        }

        if (swordButton != null)
        {
            swordButton.clicked += OnSwordButtonClicked;
        }

        if (wizardButton != null)
        {
            wizardButton.clicked += OnWizardButtonClicked;
        }

        if (updateButton != null)
        {
            updateButton.clicked += OnUpdateButtonClicked;
        }

        if (destroyButton != null)
        {
            destroyButton.clicked += OnDestroyButtonClicked;
        }

        root.visible = false;
    }

    private void OnArcherButtonClicked()
    {
        GameManager.Instance.Build(Enums.TowerType.Archer, SiteLevel.Level1);
    }

    private void OnSwordButtonClicked()
    {
        GameManager.Instance.Build(Enums.TowerType.Sword, SiteLevel.Level1);
    }

    private void OnWizardButtonClicked()
    {
        GameManager.Instance.Build(Enums.TowerType.Wizard, SiteLevel.Level1);
    }

    private void OnUpdateButtonClicked()
    {
        if (selectedSite != null && selectedSite.Level < SiteLevel.Level3)
        {
            selectedSite.Level++;
            EvaluateMenu();
        }
    }

    private void OnDestroyButtonClicked()
    {
        if (selectedSite != null)
        {
            TowerType towerType = selectedSite.GetTowerType();
            // Destroy the tower on the selected site
            selectedSite.SetTower(null, SiteLevel.Unbuilt, towerType);
            // Set the level of the site to 0
            selectedSite.Level = SiteLevel.Unbuilt;
            // Update the buttons in the tower menu
            EvaluateMenu();
        }
    }

    private void OnDestroy()
    {
        if (archerButton != null)
        {
            archerButton.clicked -= OnArcherButtonClicked;
        }

        if (swordButton != null)
        {
            swordButton.clicked -= OnSwordButtonClicked;
        }

        if (wizardButton != null)
        {
            wizardButton.clicked -= OnWizardButtonClicked;
        }

        if (updateButton != null)
        {
            updateButton.clicked -= OnUpdateButtonClicked;
        }

        if (destroyButton != null)
        {
            destroyButton.clicked -= OnArcherButtonClicked;
        }
    }
}
