using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PerformAction(string tag, GameObject obj)
    {
        Debug.Log("Etkilesim: " + tag + " | Obje: " + obj.name);

        switch (tag)
        {
            default:
                Debug.Log("Bu tag için tanýmlý bir aksiyon yok: " + tag);
                break;
        }
    }
}