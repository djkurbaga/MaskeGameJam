using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
//dialogue usage:

public enum GameState
{
    Gameplay,
    Inventory,
    Dialogue,
    Paused
}
public enum PlayerAction
{
    ToggleInventory, // I tuþu
    ToggleMask,      // M tuþu
    Attack,          // Space tuþu
    Interact         // E tuþu
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Status")]
    public GameState CurrentState = GameState.Gameplay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        
    }
    void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (Keyboard.current == null) return;

        // 1. Envanter (I)
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            HandleInput(PlayerAction.ToggleInventory);
        }

        // 2. Maske (M)
        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            HandleInput(PlayerAction.ToggleMask);
        }

        // 3. attack (Space)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            HandleInput(PlayerAction.Attack);
        }

        // 4. Etkileþim (E)
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            HandleInput(PlayerAction.Interact);
            InventoryManager.Instance.AddItem("deneme");//TODO: test amaçlý eklendi, silinecek
        }
    }

    // SENÝN ÝSTEDÝÐÝN SWITCH-CASE YAPISI BURADA
    private void HandleInput(PlayerAction action)
    {
        switch (action)
        {
            case PlayerAction.ToggleInventory:
                InventoryManager.Instance.ToggleInventory();
                break;

            case PlayerAction.ToggleMask:
                // TODO: CharacterManager.Instance.ToggleMask() buraya gelecek
                Debug.Log("Input: Maske Tuþuna Basýldý");
                break;

            case PlayerAction.Attack:
                if (CurrentState == GameState.Gameplay)
                {
                    // TODO: CombatManager veya PlayerController.Attack() buraya gelecek
                    List<Dialogue> chat = new List<Dialogue>();

                    chat.Add(new Dialogue("KUZGUN", "Uyan lo"));
                    chat.Add(new Dialogue("KUZGUN", "Sana kardeþinden mektup getirdim."));

                    chat.Add(new Dialogue("KUZGUN", "al bakim.", () =>
                    {
                        Debug.Log("Mektup envantere eklendi!");
                    }));

                    chat.Add(new Dialogue("KAPSONLU", "Sagol soyle kardesime karým ona emanet..."));

                    DialogueManager.Instance.StartConversation(chat);
                    //DialogueManager.Instance.StartOneShot("KUZGUN", "Yolun açýk olsun, kardeþin adýna.");
                }
                break;

            case PlayerAction.Interact:
                if (CurrentState == GameState.Gameplay)
                {
                    // TODO: InteractionManager raycast tetiklemesi buraya gelecek
                    Debug.Log("Input: Etkileþim Tuþuna Basýldý");
                }
                break;
        }
    }

    // Oyun durumunu deðiþtirmek için dýþarýdan çaðýracaðýmýz metod
    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log($"Oyun Durumu Deðiþti: {newState}");

        // Örn: Envanter açýlýnca oyunu dondurmak istersen buraya Time.timeScale kodlarý eklenebilir.
    }
}