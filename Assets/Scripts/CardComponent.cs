using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardComponent : MonoBehaviour, IPointerDownHandler {
    public Card CardData { get; set; }
    public TextMeshProUGUI TitleTMP { get; set; }
    public TextMeshProUGUI TextTMP { get; set; }

    public void Start() {
        if (CardData == null) {
            return;
        }
            
        TitleTMP = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        TextTMP = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
        TitleTMP.SetText(CardData.Title);
        TextTMP.SetText(CardData.Text);
        return;
    }

    public void OnPointerDown(PointerEventData eventData) {
        Debug.Log("from CC " + CardData.Title + ": " + eventData.position);
        return;
    }
}