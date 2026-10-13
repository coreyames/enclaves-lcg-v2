using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using static CustomMessage;

public class CardComponent : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
    public static readonly float DefaultCardScale = 0.2f;
    public Card CardData { get; set; }
    public TextMeshProUGUI TitleTMP { get; set; }
    public TextMeshProUGUI TextTMP { get; set; }
    public bool IsFaceDown { get; private set; } = false;
    public bool IsInHand { get; set; } = false;
    public void InitIsFaceDown(bool toSet) {
        IsFaceDown = toSet;
        return;
    } 
    private GameObject CardBack { get; set; }

    public void Start() {
        TitleTMP = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[0];
        TextTMP = gameObject.GetComponentsInChildren<TextMeshProUGUI>()[1];
        string title = "";
        string text = "";
        
        if (CardData != null) {
            title = CardData.Title;
            text = CardData.Text;
        }
            
        TitleTMP.SetText(title);
        TextTMP.SetText(text);
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
        if (eventData.pointerPressRaycast.gameObject == gameObject) {
            ExecuteEvents.Execute<IMessageCardToGame>(Game.BoardGameObject, eventData, (x,y)=>x.SelectedCard(MessageData()));
        }
        return;
    }
    
    public void SetIsFaceDown(bool faceDown) {
        IsFaceDown = faceDown;
        if (CardBack != null) {
            CardBack.SetActive(IsFaceDown);
        }
        return;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (IsInHand) {
            GameObject parent = gameObject.transform.parent.gameObject;
            ExecuteEvents.Execute<IMessageCardToHand>(parent, eventData, (x,y)=>x.HoveredCard(MessageData()));
        }
        return;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (IsInHand) {
            GameObject parent = gameObject.transform.parent.gameObject;
            ExecuteEvents.Execute<IMessageCardToHand>(parent, eventData, (x,y)=>x.ExitHoveredCard(MessageData()));
        }
        return;
    }
}

