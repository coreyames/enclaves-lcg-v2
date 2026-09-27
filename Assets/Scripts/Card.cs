using UnityEngine;

public class Card : MonoBehaviour, ICardInfo {
    public string Title { get; set; }
    public string Text { get; set; }
    public string Image { get; set; }
}