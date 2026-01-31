using UnityEngine;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    private Queue<Dialogue> dialogueQueue = new Queue<Dialogue>();
    private Dialogue currentDialogue;
    public bool isDialogueActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Update()
    {
        // Sol týk ile ilerleme
        if (isDialogueActive && Input.GetMouseButtonDown(0))
        {
            AdvanceDialogue();
        }
    }

    // --- METOD 1: Tek Satýrlýk Hýzlý Diyalog ---
    public void StartOneShot(string speakerID, string content)
    {
        List<Dialogue> singleLine = new List<Dialogue>();
        singleLine.Add(new Dialogue(speakerID, content));
        StartConversation(singleLine);
    }

    // --- METOD 2: Uzun Konuþma Baþlatma (Liste Ýle) ---
    public void StartConversation(List<Dialogue> newConversation)
    {
        if (newConversation == null || newConversation.Count == 0) return;

        isDialogueActive = true;
        dialogueQueue.Clear();

        // InteractionManager'ý durdur (Oyuncu yürüyemesin)
        if (InteractionManager.Instance != null)
            InteractionManager.Instance.SetInteractionState(false);

        // PlayerController'ý durdur (Opsiyonel, InteractionManager yetmiyorsa)
        // if (PlayerController.Instance != null) PlayerController.Instance.StopMoving();

        foreach (var line in newConversation)
        {
            dialogueQueue.Enqueue(line);
        }

        PlayNextLine();
    }

    private void AdvanceDialogue()
    {
        // Eðer yazý hala yazýlýyorsa, önce onu tamamla
        if (DialogueUI.Instance.IsTyping)
        {
            DialogueUI.Instance.CompleteTextImmediately();
            return;
        }

        // Yazý bitmiþse sýradakine geç
        PlayNextLine();
    }

    private void PlayNextLine()
    {
        // Önceki diyaloðun bir Action'ý varsa (Event) onu çalýþtýr
        if (currentDialogue != null && currentDialogue.onDialogueEnd != null)
        {
            currentDialogue.onDialogueEnd.Invoke();
            currentDialogue = null; // Tekrar tetiklenmesin diye temizle
        }

        if (dialogueQueue.Count > 0)
        {
            currentDialogue = dialogueQueue.Dequeue();
            DialogueUI.Instance.ShowDialogue(currentDialogue);
        }
        else
        {
            EndConversation();
        }
    }

    private void EndConversation()
    {
        isDialogueActive = false;
        currentDialogue = null;
        DialogueUI.Instance.ClosePanel();

        // Etkileþimi tekrar aç
        if (InteractionManager.Instance != null)
            InteractionManager.Instance.SetInteractionState(true);

        Debug.Log("Konuþma bitti.");
    }
}