using UnityEngine;

public class Card : ScriptableObject, ICardInfo {
    public string Title { get; set; }
    public string Text { get; set; }
    public string Image { get; set; }
}