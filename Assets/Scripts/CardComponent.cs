using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static CustomMessage;

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

    private CustomData<CustomCardData> MessageData() {
        CustomData<CustomCardData> customData = new(null);
        CustomCardData value = new() {
            cardComponent = this
        };
        customData.CustomDataValue = value;
        return customData;
    }

    public void OnPointerDown(PointerEventData eventData) {
        ExecuteEvents.Execute<IMessageCardToGame>(Game.BoardGameObject, eventData, (x,y)=>x.SelectedCard(MessageData()));
        return;
    }
}

