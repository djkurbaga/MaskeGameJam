using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public Image portraitImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI contentText;

    [Header("Settings")]
    public float typingSpeed = 0.02f;

    [Header("Speaker Database")]
    // Inspector'dan karakterleri ve ikonlarý eþleþtireceðin liste
    public List<SpeakerProfile> speakerProfiles;

    private Dictionary<string, Sprite> profileDict;
    private Coroutine typingCoroutine;
    public bool IsTyping { get; private set; }
    private string currentFullText;

    [System.Serializable]
    public struct SpeakerProfile
    {
        public string characterID; // Örn: "KUZGUN"
        public Sprite icon;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;

        // Listeyi hýzlý eriþim için Dictionary'e çeviriyoruz
        profileDict = new Dictionary<string, Sprite>();
        foreach (var profile in speakerProfiles)
        {
            if (!profileDict.ContainsKey(profile.characterID))
                profileDict.Add(profile.characterID, profile.icon);
        }

        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(Dialogue dialogue)
    {
        GameManager.Instance.SetGameState(GameState.Dialogue);
        dialoguePanel.SetActive(true);

        // Ýkonu ID'ye göre bul ve deðiþtir
        if (profileDict.ContainsKey(dialogue.speakerID))
        {
            portraitImage.sprite = profileDict[dialogue.speakerID];
            portraitImage.gameObject.SetActive(true);
            nameText.text = dialogue.speakerID; 
        }
        else
        {
            portraitImage.gameObject.SetActive(false);
            nameText.text = dialogue.speakerID;
        }

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeText(dialogue.content));
    }

    IEnumerator TypeText(string text)
    {
        IsTyping = true;
        currentFullText = text;
        contentText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            contentText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        IsTyping = false;
    }

    // Yazý yazýlýrken týklanýrsa anýnda tamamla
    public void CompleteTextImmediately()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        contentText.text = currentFullText;
        IsTyping = false;
    }

    public void ClosePanel()
    {
        dialoguePanel.SetActive(false);
        GameManager.Instance.SetGameState(GameState.Gameplay);
    }
}