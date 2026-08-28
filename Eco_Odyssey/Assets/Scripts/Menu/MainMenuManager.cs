using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject mainMenuPanel;
    public GameObject newGamePanel;
    public GameObject loadGamePanel;
    public GameObject optionsPanel;
    public GameObject creditsPanel;

    [Header("Botões - Menu Principal")]
    public GameObject newGameButton;
    public GameObject loadGameButton;
    public GameObject optionsButton;
    public GameObject creditsButton;
    public GameObject exitButton;

    [Header("Botões - Novo Jogo")]
    public GameObject newGameSlot1Button;
    public GameObject newGameSlot2Button;
    public GameObject newGameSlot3Button;
    public GameObject newGameBackButton;


    private void Start()
    {
        ShowMainMenu();
    }


    private void Update()
    {
        HandleKeyboard();
    }


    // =========================================================
    // CONTROLES
    // =========================================================

    private void HandleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ConfirmSelection();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Cancel();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
    }


    // =========================================================
    // MOVIMENTAÇÃO PARA CIMA
    // =========================================================

    private void MoveUp()
    {
        GameObject current =
            EventSystem.current.currentSelectedGameObject;

        if (current == null)
            return;


        // =========================
        // MENU PRINCIPAL
        // =========================

        if (current == newGameButton)
        {
            SelectButton(exitButton);
        }
        else if (current == loadGameButton)
        {
            SelectButton(newGameButton);
        }
        else if (current == optionsButton)
        {
            SelectButton(loadGameButton);
        }
        else if (current == creditsButton)
        {
            SelectButton(optionsButton);
        }
        else if (current == exitButton)
        {
            SelectButton(creditsButton);
        }


        // =========================
        // NOVO JOGO
        // =========================

        else if (current == newGameSlot1Button)
        {
            SelectButton(newGameBackButton);
        }
        else if (current == newGameSlot2Button)
        {
            SelectButton(newGameSlot1Button);
        }
        else if (current == newGameSlot3Button)
        {
            SelectButton(newGameSlot2Button);
        }
        else if (current == newGameBackButton)
        {
            SelectButton(newGameSlot3Button);
        }
    }


    // =========================================================
    // MOVIMENTAÇÃO PARA BAIXO
    // =========================================================

    private void MoveDown()
    {
        GameObject current =
            EventSystem.current.currentSelectedGameObject;

        if (current == null)
            return;


        // =========================
        // MENU PRINCIPAL
        // =========================

        if (current == newGameButton)
        {
            SelectButton(loadGameButton);
        }
        else if (current == loadGameButton)
        {
            SelectButton(optionsButton);
        }
        else if (current == optionsButton)
        {
            SelectButton(creditsButton);
        }
        else if (current == creditsButton)
        {
            SelectButton(exitButton);
        }
        else if (current == exitButton)
        {
            SelectButton(newGameButton);
        }


        // =========================
        // NOVO JOGO
        // =========================

        else if (current == newGameSlot1Button)
        {
            SelectButton(newGameSlot2Button);
        }
        else if (current == newGameSlot2Button)
        {
            SelectButton(newGameSlot3Button);
        }
        else if (current == newGameSlot3Button)
        {
            SelectButton(newGameBackButton);
        }
        else if (current == newGameBackButton)
        {
            SelectButton(newGameSlot1Button);
        }
    }


    // =========================================================
    // CONFIRMAR
    // =========================================================

    private void ConfirmSelection()
    {
        GameObject current =
            EventSystem.current.currentSelectedGameObject;

        if (current == null)
            return;

        Button button =
            current.GetComponent<Button>();

        if (button != null)
        {
            button.onClick.Invoke();
        }
    }


    // =========================================================
    // MENU PRINCIPAL
    // =========================================================

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);

        newGamePanel.SetActive(false);
        loadGamePanel.SetActive(false);
        optionsPanel.SetActive(false);
        creditsPanel.SetActive(false);

        SelectButton(newGameButton);
    }


    // =========================================================
    // NOVO JOGO
    // =========================================================

    public void OpenNewGame()
    {
        mainMenuPanel.SetActive(false);
        newGamePanel.SetActive(true);

        SelectButton(newGameSlot1Button);
    }


    // =========================================================
    // CARREGAR JOGO
    // =========================================================

    public void OpenLoadGame()
    {
        mainMenuPanel.SetActive(false);
        loadGamePanel.SetActive(true);

        // Será configurado quando fizermos o Load Game.
    }


    // =========================================================
    // OPÇÕES
    // =========================================================

    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);

        // Será configurado quando fizermos as Opções.
    }


    // =========================================================
    // CRÉDITOS
    // =========================================================

    public void OpenCredits()
    {
        mainMenuPanel.SetActive(false);
        creditsPanel.SetActive(true);

        // Será configurado quando fizermos os Créditos.
    }


    // =========================================================
    // VOLTAR
    // =========================================================

    public void Cancel()
    {
        if (mainMenuPanel.activeSelf)
        {
            return;
        }

        ShowMainMenu();
    }


    // =========================================================
    // SELECIONAR
    // =========================================================

    private void SelectButton(GameObject button)
    {
        if (button == null)
            return;

        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(button);
    }


    // =========================================================
    // SAIR
    // =========================================================

    public void QuitGame()
    {
        Debug.Log("Saindo do jogo...");

        Application.Quit();
    }
}