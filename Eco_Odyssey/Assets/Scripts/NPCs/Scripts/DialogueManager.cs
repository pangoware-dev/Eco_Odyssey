using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public Image portrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Button[] choiceButtons;

    public bool isDialogueActive = false;

    private DialogueSO currentDialogue;
    private int dialogueIndex = 0;

    private bool choosingOption = false;
    private bool canChoose = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isDialogueActive)
            return;

        // Enquanto estiver escolhendo uma opção
        if (choosingOption)
        {
            if (canChoose && Input.GetButtonDown("Slash"))
            {
                ConfirmSelectedOption();
            }

            return;
        }

        // Enquanto estiver mostrando uma fala,
        // o Slash é controlado pelo scrPlayer.
    }

    public void StartDialogue(DialogueSO dialogue)
    {
        if (dialogue == null)
            return;

        currentDialogue = dialogue;
        dialogueIndex = 0;

        isDialogueActive = true;
        choosingOption = false;
        canChoose = false;

        ShowDialogue();
    }

    public void AdvanceDialogue()
    {
        if (!isDialogueActive)
            return;

        if (choosingOption)
            return;

        if (dialogueIndex < currentDialogue.lines.Length)
        {
            ShowDialogue();
        }
        else
        {
            ShowChoices();
        }
    }

    private void ShowDialogue()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        DialogueHistoryTracker.Instance.RecordNPC(line.speaker);

        portrait.sprite = line.speaker.portrait;
        actorName.text = line.speaker.actorName;
        dialogueText.text = line.text;

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        dialogueIndex++;

        Time.timeScale = 0f;
    }

    private void ShowChoices()
    {
        ClearChoices();

        choosingOption = true;
        canChoose = false;

        if (currentDialogue.options == null ||
            currentDialogue.options.Length == 0)
        {
            EndDialogue();
            return;
        }

        for (int i = 0; i < currentDialogue.options.Length; i++)
        {
            var option = currentDialogue.options[i];

            choiceButtons[i].GetComponentInChildren<TMP_Text>().text =
                option.optionText;

            choiceButtons[i].gameObject.SetActive(true);

            DialogueSO nextDialogue = option.nextDialogue;

            choiceButtons[i].onClick.AddListener(
                () => ChooseOption(nextDialogue)
            );
        }

        // Seleciona automaticamente a primeira opção
        EventSystem.current.SetSelectedGameObject(
            choiceButtons[0].gameObject
        );

        // Impede que o mesmo Slash usado para abrir as opções
        // seja usado para confirmar a primeira opção.
        StartCoroutine(EnableChoice());
    }

    private IEnumerator EnableChoice()
    {
        // Espera até o final do frame atual.
        yield return new WaitForEndOfFrame();

        canChoose = true;
    }

    private void ConfirmSelectedOption()
    {
        GameObject selected =
            EventSystem.current.currentSelectedGameObject;

        if (selected == null)
            return;

        Button selectedButton =
            selected.GetComponent<Button>();

        if (selectedButton == null)
            return;

        selectedButton.onClick.Invoke();
    }

    private void ChooseOption(DialogueSO nextDialogue)
    {
        choosingOption = false;
        canChoose = false;

        if (nextDialogue == null)
        {
            EndDialogue();
        }
        else
        {
            ClearChoices();

            StartDialogue(nextDialogue);
        }
    }

    private void EndDialogue()
    {
        choosingOption = false;
        canChoose = false;
        isDialogueActive = false;

        dialogueIndex = 0;
        currentDialogue = null;

        ClearChoices();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        EventSystem.current.SetSelectedGameObject(null);

        Time.timeScale = 1f;
    }

    private void ClearChoices()
    {
        foreach (Button button in choiceButtons)
        {
            button.onClick.RemoveAllListeners();
            button.gameObject.SetActive(false);
        }
    }
}