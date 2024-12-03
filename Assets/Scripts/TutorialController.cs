using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    public GameObject[] tutorialPanels;
    private int currentPanelIndex = 0;

    public Button nextButton;
    public Button previousButton;

    void Start()
    {
        UpdatePanelVisibility();
    }

    public void NextPanel()
    {
        if (currentPanelIndex < tutorialPanels.Length - 1)
        {
            currentPanelIndex++;
            UpdatePanelVisibility();
        }
    }

    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PreviousPanel()
    {
        if (currentPanelIndex > 0)
        {
            currentPanelIndex--;
            UpdatePanelVisibility();
        }
    }

    private void UpdatePanelVisibility()
    {
        for (int i = 0; i < tutorialPanels.Length; i++)
        {
            tutorialPanels[i].SetActive(i == currentPanelIndex);
        }

        // Configura a visibilidade dos botões
        nextButton.gameObject.SetActive(currentPanelIndex < tutorialPanels.Length - 1);
        previousButton.gameObject.SetActive(currentPanelIndex > 0);
    }
}
