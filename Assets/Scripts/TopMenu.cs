using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class TopMenu : MonoBehaviour
{
    public UIDocument uIDocument;

    public GameManager gameManager;

    private Label WaveLabel;
    private Label CreditsLabel;
    private Label GHealthLabel;
    private Button PlayButton;

    private VisualElement root;

    private void Start()
    {
        Debug.Log("uIDocument: " + uIDocument);
        Debug.Log("WaveLabel: " + WaveLabel);
        Debug.Log("CreditsLabel: " + CreditsLabel);
        Debug.Log("GHealthLabel: " + GHealthLabel);
        Debug.Log("PlayButton: " + PlayButton);

        // Zoek de labels en button in de UI-document hiërarchie
        WaveLabel = uIDocument.rootVisualElement.Q<Label>("Wave");
        CreditsLabel = uIDocument.rootVisualElement.Q<Label>("Credits");
        GHealthLabel = uIDocument.rootVisualElement.Q<Label>("Gate");
        PlayButton = uIDocument.rootVisualElement.Q<Button>("Play");

        // Controleer of de labels en button zijn gevonden
        if (WaveLabel == null || CreditsLabel == null || GHealthLabel == null || PlayButton == null)
        {
            Debug.LogError("One or more UI elements not found in UI document!");
        }

        if (PlayButton != null)
        {
            // Voeg een event listener toe aan de button
            PlayButton.clicked += StartWave;
        }
        else
        {
            // Log een fout als de PlayButton niet kon worden gevonden
            Debug.LogError("De PlayButton kon niet worden gevonden.");
        }
    }


    void StartWave()
    {
        GameManager.Instance.StartWave();

    }

    public void SetWaveLabel(string text)
    {
        if (WaveLabel != null)
        {
            WaveLabel.text = text;
        }
    }

    public void SetCreditsLabel(string text)
    {
        if (CreditsLabel != null)
        {
            CreditsLabel.text = text;
        }
    }

    public void SetGateHealthLabel(string text)
    {
        if (GHealthLabel != null)
        {
            GHealthLabel.text = text;
        }
    }

    void OnDestroy()
    {
        PlayButton.clicked -= StartWave;
    }
    public void startWaveButton_clicked()
    {
        SetWaveLabel("Wave " + (GameManager.Instance.currentWave + 1)); // Voeg 1 toe aan de huidige golfindex
        if (gameManager != null)
        {
            gameManager.StartWave();
            DisableWaveButton();
        }
        else
        {
            Debug.LogWarning("GameManager not found!");
        }
    }
    public void EnableWaveButton()
    {
        if (PlayButton != null)
        {
            PlayButton.SetEnabled(true);
        }
        else
        {
            Debug.LogWarning("WaveButton not assigned!");
        }
    }
    private void DisableWaveButton()
    {
        if (PlayButton != null)
        {
            PlayButton.SetEnabled(false);
        }
        else
        {
            Debug.LogWarning("WaveButton not assigned!");
        }
    }
}
