using UnityEngine;
using UnityEngine.UIElements;

public class TopMenu : MonoBehaviour
{
    private Label WaveLabel;
    private Label CreditsLabel;
    private Label GHealthLabel;
    private Button PlayButton;

    private VisualElement root;

    void Start()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        WaveLabel = root.Q<Label>("Wave");
        CreditsLabel = root.Q<Label>("Credits");
        GHealthLabel = root.Q<Label>("Gate-Health");
        PlayButton = root.Q<Button>("Play-Button");

        if (PlayButton != null)
        {
            PlayButton.clicked += OnPlayButtonClicked;
        }
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

    private void OnPlayButtonClicked()
    {
        GameManager.Instance.StartWave();
        if (PlayButton != null)
        {
            PlayButton.SetEnabled(false);
        }
    }

    public void EnableWaveButton()
    {
        // Zorgt ervoor dat de knop weer interactief is na het beëindigen van een wave
        if (PlayButton != null)
        {
            PlayButton.SetEnabled(true);
        }
    }

    private void OnDestroy()
    {
        if (PlayButton != null)
        {           
            PlayButton.clicked -= OnPlayButtonClicked;
        }
    }
}
