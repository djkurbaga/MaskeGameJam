using System;

[System.Serializable]
public class Dialogue
{
    public string speakerID; 
    public string content; 
    public Action onDialogueEnd;
    public Dialogue(string id, string text, Action action = null)
    {
        this.speakerID = id;
        this.content = text;
        this.onDialogueEnd = action;
    }
}
