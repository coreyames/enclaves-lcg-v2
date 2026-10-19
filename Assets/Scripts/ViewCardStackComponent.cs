using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static CustomMessage;

public class ViewCardStackComponent : MonoBehaviour, IPointerDownHandler, IMessageCardToHand {
    private static readonly string CardPrefabPath = "Prefabs/CardPanel";
    private static readonly float CardOverlapAmount = 0.5f;
    private static readonly int PanelMargin = 50;
    private static readonly int Spacing = (int)(500.0f * CardOverlapAmount);
    public GameObject CardPrefab;
    public List<Card> Cards { get; set; } 
    public GameObject TopBarObject;
    public CardComponent CurrentHoveredCard { get; set; }
    public int HoveredCardOriginalIndex { get; set; }
    
    public void Awake() {
        CardPrefab = Resources.Load<GameObject>(CardPrefabPath);   
        TopBarObject = gameObject.transform.Find("TopBar").gameObject;
        return;
    }
    
    public CustomData<CustomHandData> MessageData() {
        CustomData<CustomHandData> customData = new(null);
        CustomHandData value = new() {
            stackComponent = this
        };
        customData.CustomDataValue = value;
        return customData;
    }
 
    public void Start() {
        gameObject.transform.localScale = Vector3.one * CardComponent.DefaultCardScale;
        UpdateHandUI();
        return;
    }
    
    public void HoveredCard(CustomData<CustomCardData> eventData) {
        Transform cardTransform = eventData.CustomDataValue.cardComponent.gameObject.transform;
        HoveredCardOriginalIndex = cardTransform.GetSiblingIndex();
        cardTransform.SetAsLastSibling();
        return;
    }
    
    public void ExitHoveredCard(CustomData<CustomCardData> eventData) {
        Transform cardTransform = eventData.CustomDataValue.cardComponent.gameObject.transform;
        cardTransform.SetSiblingIndex(HoveredCardOriginalIndex);
        return;
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        if (eventData.pointerPressRaycast.gameObject == TopBarObject) {
            ExecuteEvents.Execute<IMessageHandToGame>(Game.BoardGameObject, eventData, (x,y)=>x.SelectedHand(MessageData()));
        }
        return;        
    }
    
    public int AddCard(Card card) {
        Cards.Add(card);
        UpdateHandUI();
        return Cards.Count;
    }
    
    public void RemoveCard(Card card) {
        Cards.Remove(card);
        UpdateHandUI();
        return;
    }
   
    public void ResizeHandPanel() {
        if (Cards.Count > 5) {
            var n = Cards.Count - 1.0f;
            float width = 500.0f + (n * 250.0f) + 100.0f;
            gameObject.GetComponent<RectTransform>().sizeDelta = new(width, 800.0f);
            TopBarObject.GetComponent<RectTransform>().sizeDelta = new(width, 50.0f);
        } else {
            gameObject.GetComponent<RectTransform>().sizeDelta = new(1600.0f, 800.0f);
            TopBarObject.GetComponent<RectTransform>().sizeDelta = new(1600.0f, 50.0f);
        }
        return;
    }
    
    public void UpdateHandUI() {
        CardComponent[] oldCards = gameObject.GetComponentsInChildren<CardComponent>();
        for (int c = 0; c < oldCards.Length; c ++) {
            Destroy(oldCards[c].gameObject);
        }
        for (int i = 0; i < Cards.Count; i++) {
            GameObject newCardObject = Instantiate(CardPrefab);
            newCardObject.transform.SetParent(gameObject.transform);
            newCardObject.transform.localScale = Vector3.one;
            newCardObject.transform.localPosition = new(PanelMargin + (i*Spacing),-PanelMargin);
            CardComponent cardComponent = newCardObject.GetComponent<CardComponent>();
            cardComponent.gameObject.transform.Find("CardBack").gameObject.SetActive(false);
            cardComponent.CardData = Cards[i];   
            cardComponent.IsInHand = true;
        }
        ResizeHandPanel();
        return;
    }
}