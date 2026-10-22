using UnityEngine;
using UnityEngine.UI;

public class CardClickMenuPanel : ClickMenuPanel {
    
    public Button RotateButton { get; set; }
    public Button FlipButton { get; set; } 
    public CardStackComponent Deck;
    public CardStackComponent Discard;      
    
    public void Start() {
        if (Deck == null || Discard == null) {
            Debug.Log("deck or discard target missing");
            
        } 
        return;
    }
}