using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    [Header("Ev Dolabý")]
    [SerializeField] private SpriteRenderer dolap;
    [SerializeField] private Sprite closedDolap;
    [SerializeField] private Sprite openDolap;

    [Header("EvAnims")]
    [SerializeField] private Animator kargaAnim;
    [SerializeField] private Animator blackoutAnim;
    private bool isDolapOpen = false;
    private void Awake()
    {
        blackoutAnim.gameObject.SetActive(true);
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PerformAction(string tag, GameObject obj)
    {
        Debug.Log("Etkilesim: " + tag + " | Obje: " + obj.name);

        switch (tag)
        {
            case "EvDolap":
                dolapToggle();
                break;
            case "Karga":
                DialogueManager.Instance.StartOneShot("Karga", "Gak Gak dedim!");
                break;
            default:
                Debug.Log("Bu tag için tanýmlý bir aksiyon yok: " + tag);
                break;
        }
    }
    public void dolapToggle()
    {
        isDolapOpen = !isDolapOpen;
        if (isDolapOpen)
            dolap.sprite = openDolap;
        else
            dolap.sprite = closedDolap;
    }
    public void StartFirstSpeak()
    {
        StartCoroutine(firstSpeak());
    }
    private IEnumerator firstSpeak()
    {
        yield return new WaitForSeconds(1f);
        blackoutAnim.SetBool("blackoutOn",false);

        yield return new WaitForSeconds(5f);

        List<Dialogue> chat = new List<Dialogue>();

        chat.Add(new Dialogue("Arthur", "..."));
        chat.Add(new Dialogue("Arthur", "Bedenimle bütünleþen ve ruhumu esir eden þu katlanýlmaz suratýmý alýn benden..."));
        chat.Add(new Dialogue("Arthur", "Her gündüzümü kuytulara esir eden bu yüzümü..."));
        chat.Add(new Dialogue("Arthur", "Azad edin bedenimden..."));
        chat.Add(new Dialogue("Arthur", "Zorundayým ne gelir elimden."));
        chat.Add(new Dialogue("Arthur", "Neden..."));
        chat.Add(new Dialogue("Arthur", "...", () =>
        {
            kargaFirst();
        }));
        DialogueManager.Instance.StartConversation(chat);
        GameManager.Instance.SetGameState(GameState.Gameplay);
        yield break;
    }
    public void kargaFirst()
    {
        StartCoroutine(kargaFirstAction());
    }
    private IEnumerator kargaFirstAction()
    {
        //gak sound
        kargaAnim.SetTrigger("callCrow");
        yield return new WaitForSeconds(1f);
        Debug.Log("Karga geldi dokununca interaction baþlýcak");
        yield break;
    }
}