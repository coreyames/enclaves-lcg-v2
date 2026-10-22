using UnityEngine;
using UnityEngine.UI;

public class ClickMenuPanel : MonoBehaviour {
    public Button ToTopButton { get; set; }
    public Button ToBottomButton{ get; set; }
    public Button ToFromTopButton{ get; set; }
    public Button ToHandButton{ get; set; }
    
    public void Awake() {
        ToTopButton     = GameObject.Find("TopStackButton")   .GetComponent<Button>();
        ToBottomButton  = GameObject.Find("BottomStackButton").GetComponent<Button>();
        ToFromTopButton = GameObject.Find("TopStackButton")   .GetComponent<Button>();
        ToHandButton    = GameObject.Find("HandButton")       .GetComponent<Button>();
        return;
    }
    
    private void ToTopOnClick() {
        return;
    }
    private void ToBottomOnClick() {
        return; 
    }
    private void ToFromTopOnClick() {
        return;
    }
    private void ToHandOnClick() {
        return;
    }
}