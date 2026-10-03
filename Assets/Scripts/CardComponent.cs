using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static CustomMessage;

public class CardComponent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public Card CardData { get; set; }
    public TextMeshProUGUI TitleTMP { get; set; }
    public TextMeshProUGUI TextTMP { get; set; }

    public bool Selected = false;
    public bool Held = false;

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
        Debug.Log("DOWN from CC " + CardData.Title + ": " + eventData.position);
        // delay on held = true?
        Held = true;
        CustomData<CustomCardData> customData = new(null);
        CustomCardData value = new()
        {
            CardObject = gameObject,
            CardName = CardData.Title
        };
        customData.CustomDataValue = value;
        ExecuteEvents.Execute<IMessageCardToGame>(Game.BoardGameObject, eventData, (x,y)=>x.SelectedCard(customData));
        ExecuteEvents.Execute<IMessageCardToGame>(Game.BoardGameObject, eventData, (x,y)=>x.HeldCard(customData));
        return;
    }

    public void OnPointerUp(PointerEventData eventData) {
        Debug.Log("UP from CC " + CardData.Title + ": " + eventData.position);
        Held = false;
        ExecuteEvents.Execute<IMessageCardToGame>(Game.BoardGameObject, eventData, (x,y)=>x.HeldCard(null));
        return;        
    }
}
