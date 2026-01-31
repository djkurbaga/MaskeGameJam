using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance;

    public bool canInteract = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canInteract && GameManager.Instance.CurrentState == GameState.Gameplay)
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            DetectObject();
        }
    }

    void DetectObject()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null)
        {
            ActionManager.Instance.PerformAction(hit.collider.tag, hit.collider.gameObject);
        }
    }

    public void SetInteractionState(bool state)
    {
        canInteract = state;
    }
}