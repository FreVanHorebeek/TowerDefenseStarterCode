using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class IntroMenu : MonoBehaviour
{
    private Button playGameButton;
    private Button quitGameButton;
    private TextField textField;

    private void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        Debug.Log("Root VisualElement: " + root);

        playGameButton = root.Q<Button>("playGameButton");
        quitGameButton = root.Q<Button>("quitGameButton");
        textField = root.Q<TextField>("TextField");

        if (playGameButton != null)
            playGameButton.clicked += StartButtonClicked;
        else
            Debug.LogError("Start Button not found!");

        if (quitGameButton != null)
            quitGameButton.clicked += QuitButtonClicked;
        else
            Debug.LogError("Quit Button not found!");
    }

    private void OnDestroy()
    {
        if (playGameButton != null)
        {
            playGameButton.clicked -= StartButtonClicked;
        }

        if (quitGameButton != null)
        {
            quitGameButton.clicked -= QuitButtonClicked;
        }
    }

    private void StartButtonClicked()
    {
        SceneManager.LoadScene("GameScene");
    }

    private void QuitButtonClicked()
    {
        Application.Quit();
    }
}

