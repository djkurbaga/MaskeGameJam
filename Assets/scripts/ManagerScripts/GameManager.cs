using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    Attack           // Space tuþu
    // Interact (E) -> SÝLÝNDÝ, çünkü artýk sadece Mouse ile týklýyoruz.
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Status")]
    public GameState CurrentState = GameState.Gameplay;

    // "Interaction Sensors" deðiþkenleri (currentInteractableObject vs.) SÝLÝNDÝ.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.Instance.SetGameState(GameState.Dialogue);
        if (ActionManager.Instance != null)
        {
            ActionManager.Instance.StartFirstSpeak();
        }
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

        // 3. Attack (Space)
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            HandleInput(PlayerAction.Attack);
        }

        // 4. Etkileþim (E) -> SÝLÝNDÝ.
    }

    private void HandleInput(PlayerAction action)
    {
        switch (action)
        {
            case PlayerAction.ToggleInventory:
                InventoryManager.Instance.ToggleInventory();
                break;

            case PlayerAction.ToggleMask:
                // CharacterManager.Instance.ToggleMask(); 
                Debug.Log("Input: Maske Tuþuna Basýldý");
                break;

            case PlayerAction.Attack:
                if (CurrentState == GameState.Gameplay)
                {
                    // CombatManager entegrasyonu buraya
                    Debug.Log("Input: Saldýrý Yapýldý");
                }
                break;

                // Interact Case'i -> SÝLÝNDÝ.
        }
    }

    // HandleZoneAction, EnterInteractionZone, ExitInteractionZone -> HEPSÝ SÝLÝNDÝ.

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log($"Oyun Durumu Deðiþti: {newState}");
    }
}