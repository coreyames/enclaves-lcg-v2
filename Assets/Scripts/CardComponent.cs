using TMPro;
using UnityEngine;

public class CardComponent : MonoBehaviour {
    public Card CardData { get; set; }
    public TextMeshProUGUI TitleTMP { get; set; }
    public TextMeshProUGUI TextTMP { get; set; }

    public void Start() {
        if (CardData == null) {
            return;
        }
        
        TitleTMP = GameObject.Find("TitleTMP").GetComponent<TextMeshProUGUI>();
        TextTMP = GameObject.Find("TextTMP").GetComponent<TextMeshProUGUI>();
        TitleTMP.SetText(CardData.Title);
        TextTMP.SetText(CardData.Text);
        return;
    }
}