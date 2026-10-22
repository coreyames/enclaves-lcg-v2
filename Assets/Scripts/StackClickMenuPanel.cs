using UnityEngine;
using UnityEngine.UI;

public class StackMenuPanel : ClickMenuPanel {
    public Button ShuffleButton { get; set; }
    public Button ViewButton { get; set; }
    public CardStackComponent TargetStack { get; set; }
    
    public void Start() {
        if (TargetStack == null) {
            Debug.Log("no target stack found for card move options");
        }
        return;
    }
}