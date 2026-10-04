using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ValuePanel : MonoBehaviour {
    private Button plus;
    private Button minus;
    private TextMeshProUGUI valueTMP;
    private int value;

    public void Start() {
        Button[] buttons = GetComponentsInChildren<Button>();
        plus = buttons[0];
        plus.onClick.AddListener(OnClickPlus);
        minus = buttons[1];
        minus.onClick.AddListener(OnClickMinus);
        valueTMP = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetValue(int _value) {
        value = _value;
        valueTMP.SetText(""+value);
    }
    
    private void OnClickPlus() {
        value++;
        valueTMP.SetText(""+value);       
        return;
    }

    private void OnClickMinus() {
        value--;
        valueTMP.SetText(""+value);       
        return;
    }
}