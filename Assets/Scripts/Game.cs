using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using static CustomMessage;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour, IMessageCardToGame, IMessageHandToGame, IDragHandler, 
    IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler {
    private readonly string DataPath = "data.json";
    private readonly string CardPrefabPath = "Prefabs/CardPanel";
    private readonly string ViewCardStackPrefabPath = "Prefabs/ViewCardStackPanel";
    public List<Event> Events { get; set; } 
    public List<Card> Cards { get; set; }
    public static GameObject BoardGameObject;
    public GameObject CardPrefab { get; set; }
    public GameObject ViewCardStackPrefab { get; set; }
    public CardComponent CurrentSelectedCard;
    public CardComponent CurrentHeldCard; 
    public ViewCardStackComponent HeldCardParentHand;
    public ViewCardStackComponent CurrentHeldHandComponent;
     
    [Serializable]
    public class Player  {
        public string Name { get; set; }
        public int ID { get; set; }
        public Card[] Decklist { get; set; }
        public GameObject PanelObject;
        public CardStackComponent DeckComponent; 
        public CardStackComponent DiscardComponent; 
        public ResourceCountsComponent CountsComponent;
        public ViewCardStackComponent HandComponent;
    
        public int GetCount(string resourceName) {
            return resourceName switch
            {
                "Personnel"   => CountsComponent.Personnel.GetValue(),
                "Survivalist" => CountsComponent.Survivalist.GetValue(),
                "Mechanic"    => CountsComponent.Mechanic.GetValue(),
                "Biologist"   => CountsComponent.Biologist.GetValue(),
                "Analyst"     => CountsComponent.Analyst.GetValue(),
                "Steward"     => CountsComponent.Steward.GetValue(),
                "Ancillary"   => CountsComponent.Ancillary.GetValue(),
                "EnergyReady" => CountsComponent.EnergyReady.GetValue(),
                "EnergyCap"   => CountsComponent.EnergyCap.GetValue(),
                "Food"        => CountsComponent.Food.GetValue(),
                "Water"       => CountsComponent.Water.GetValue(),
                "Component"   => CountsComponent.Component.GetValue(),
                "Stability"   => CountsComponent.Stability.GetValue(),  
                "Despair"     => CountsComponent.Stability.GetValue(),  
                _             => -100,
            };
        }
        
        public int SetCount(string resourceName, int value) {
            return resourceName switch {
                "Personnel"   => CountsComponent.Personnel.SetValue(value),
                "Survivalist" => CountsComponent.Survivalist.SetValue(value),
                "Mechanic"    => CountsComponent.Mechanic.SetValue(value),
                "Biologist"   => CountsComponent.Biologist.SetValue(value),
                "Analyst"     => CountsComponent.Analyst.SetValue(value),
                "Steward"     => CountsComponent.Steward.SetValue(value),
                "Ancillary"   => CountsComponent.Ancillary.SetValue(value),
                "EnergyReady" => CountsComponent.EnergyReady.SetValue(value),
                "EnergyCap"   => CountsComponent.EnergyCap.SetValue(value),
                "Food"        => CountsComponent.Food.SetValue(value),
                "Water"       => CountsComponent.Water.SetValue(value),
                "Component"   => CountsComponent.Component.SetValue(value),
                "Stability"   => CountsComponent.Stability.SetValue(value),  
                "Despair"     => CountsComponent.Stability.SetValue(value),  
                _             => -100,
            };
        }
    }    

    public Player Player1;
    public Player Player2;
    public Player Player3;
    
    public void Awake() {
        CardPrefab          = Resources.Load<GameObject>(CardPrefabPath);
        ViewCardStackPrefab = Resources.Load<GameObject>(ViewCardStackPrefabPath);
        return;
    }

    public void Start() {
        BoardGameObject = GameObject.Find("Board");
        Vector3 handStartOffset = new(200, -200);
        GameObject p1panel = GameObject.Find("BotLeftPanel");
        GameObject p2panel = GameObject.Find("TopLeftPanel");
        GameObject p3panel = GameObject.Find("TopRightPanel");
        GameObject p1handObject = Instantiate(ViewCardStackPrefab);
        p1handObject.transform.SetParent(BoardGameObject.transform);
        p1handObject.transform.localPosition = p1panel.transform.localPosition + handStartOffset;
        GameObject p2handObject = Instantiate(ViewCardStackPrefab);
        p2handObject.transform.SetParent(BoardGameObject.transform);
        p2handObject.transform.localPosition = p2panel.transform.localPosition + handStartOffset;
        GameObject p3handObject = Instantiate(ViewCardStackPrefab);
        p3handObject.transform.SetParent(BoardGameObject.transform);
        p3handObject.transform.localPosition = p3panel.transform.localPosition + handStartOffset;
        
        Player1 = new Player {
            Name             = "Player1-BottomLeft",
            ID               = 1,
            PanelObject      = p1panel,           
            DeckComponent    = p1panel.GetComponentsInChildren<CardStackComponent>()[0],
            DiscardComponent = p1panel.GetComponentsInChildren<CardStackComponent>()[1],
            CountsComponent  = p1panel.GetComponentInChildren<ResourceCountsComponent>(),
            HandComponent    = p1handObject.GetComponent<ViewCardStackComponent>()
        };
        Player2 = new Player {
            Name             = "Player2-TopLeft",
            ID               = 2,
            PanelObject      = p2panel,           
            DeckComponent    = p2panel.GetComponentsInChildren<CardStackComponent>()[0],
            DiscardComponent = p2panel.GetComponentsInChildren<CardStackComponent>()[1],
            CountsComponent  = p2panel.GetComponentInChildren<ResourceCountsComponent>(),
            HandComponent    = p2handObject.GetComponent<ViewCardStackComponent>()
        };
        Player3 = new Player {
            Name             = "Player3-TopRight",
            ID               = 3,
            PanelObject      = p3panel,           
            DeckComponent    = p3panel.GetComponentsInChildren<CardStackComponent>()[1],
            DiscardComponent = p3panel.GetComponentsInChildren<CardStackComponent>()[0],
            CountsComponent  = p3panel.GetComponentInChildren<ResourceCountsComponent>(),
            HandComponent    = p3handObject.GetComponent<ViewCardStackComponent>()
        };
        Player1.DeckComponent.InitTopCardFaceDown(true);
        Player2.DeckComponent.InitTopCardFaceDown(true);
        Player3.DeckComponent.InitTopCardFaceDown(true);
        Debug.Log("Players: " + Player1.Name + ", " + Player2.Name + ", " + Player3.Name);
        // Load card and event sets
        string path = Path.Combine(Application.dataPath, DataPath);
        if (!File.Exists(path)) {
            return;
        }
        string contents = File.ReadAllText(path);
        Events = new List<Event>();
        Cards = new List<Card>();
        JObject contentsJO = JObject.Parse(contents);
        JArray eventsJA = JArray.Parse(contentsJO.GetValue("Events").ToString());
        JArray cardsJA = JArray.Parse(contentsJO.GetValue("Cards").ToString());
        for (int i = 0; i < eventsJA.Count; i++) {
            Events.Add(Event.CreateInstance<Event>());
            JsonConvert.PopulateObject(eventsJA[i].ToString(), Events[i]);
        }
        for (int i = 0; i < cardsJA.Count; i++) {
            Cards.Add(Card.CreateInstance<Card>());
            JsonConvert.PopulateObject(cardsJA[i].ToString(), Cards[i]);
        }
         
        // test load deck with cardset
        Player1.DeckComponent.InitCards(Cards.ConvertAll(CloneCardsListFunc));
        Player2.DeckComponent.InitCards(Cards.ConvertAll(CloneCardsListFunc));
        Player3.DeckComponent.InitCards(Cards.ConvertAll(CloneCardsListFunc));
        Player1.HandComponent.Cards = Cards.ConvertAll(CloneCardsListFunc);
        Player2.HandComponent.Cards = Cards.ConvertAll(CloneCardsListFunc);
        Player3.HandComponent.Cards = Cards.ConvertAll(CloneCardsListFunc);
        //
        // ready for start here
        //
        return;
    }
    
    private Card CloneCardsListFunc(Card card) {
        Card _card = Card.CreateInstance<Card>();
        JsonConvert.PopulateObject(JsonConvert.SerializeObject(card), _card);
        return _card;
    }

    // pointerdown message from card
    public void SelectedCard(CustomData<CustomCardData> data) { 
        CurrentSelectedCard = data.CustomDataValue.cardComponent;
        CurrentHeldCard = CurrentSelectedCard;
        if (CurrentHeldCard.IsInHand) {
            HeldCardParentHand = CurrentHeldCard.gameObject.transform.parent.gameObject.GetComponent<ViewCardStackComponent>();
            CurrentHeldCard.gameObject.transform.SetParent(BoardGameObject.transform);    
        }
        return;
    }
 
    public void OnDrag(PointerEventData eventData) {
        if (CurrentHeldCard != null) {
            CurrentHeldCard.gameObject.transform.localPosition += (Vector3)eventData.delta;
        } else if (CurrentHeldHandComponent != null) {
            CurrentHeldHandComponent.gameObject.transform.localPosition += (Vector3)eventData.delta; 
        }
        return;    
    }

    public void SelectedHand(CustomData<CustomHandData> eventData) {   
        CurrentHeldHandComponent = eventData.CustomDataValue.stackComponent;
        return;
    }
     
    private void HandleMouseEndEvents(PointerEventData eventData) {
        CurrentSelectedCard = null;
        CurrentHeldHandComponent = null;
        if (CurrentHeldCard != null) {
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventData, results);
            for (int i = 0; i < results.Count; i++) {
                RaycastResult result = results[i];
                if (result.gameObject.name.Contains("ViewCardStackPanel")) {
                    if (!CurrentHeldCard.IsInHand) {
                        HeldCardParentHand.AddCard(CurrentHeldCard.CardData);
                    }
                    Destroy(CurrentHeldCard.gameObject);
                    break;
                    
                } else if (result.gameObject.name == "Deck" || result.gameObject.name == "Discard")  {
                    CardStackComponent cardStack = result.gameObject.GetComponent<CardStackComponent>(); 
                    cardStack.PlaceOnTop(CurrentHeldCard.CardData);
                    if (CurrentHeldCard.IsInHand && HeldCardParentHand != null ) {
                        HeldCardParentHand.RemoveCard(CurrentHeldCard.CardData);
                    }
                    Destroy(CurrentHeldCard.gameObject);
                    break;
                    
                } else if (result.gameObject.name == "Board") {
                    if (CurrentHeldCard.IsInHand && HeldCardParentHand != null) {
                        HeldCardParentHand.RemoveCard(CurrentHeldCard.CardData);
                        CurrentHeldCard.IsInHand = false;
                    }
                    break;            
                }
            }
        }
        CurrentHeldCard = null;
        HeldCardParentHand = null;
        return; 
    }
     
    public void OnPointerUp(PointerEventData eventData) {
        HandleMouseEndEvents(eventData);
        return;
    }

    public void OnEndDrag(PointerEventData eventData) {
        HandleMouseEndEvents(eventData);
        return;
    }
    
    public void OnBeginDrag(PointerEventData eventData) {
        // placehold impl required for other pointer event handlers
        return;
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        // placehold impl required for other pointer event handler
        return;
    }
    
    public void CardDrawn(CustomData<CustomStackData> eventData) {
        CardStackComponent stackComponent = eventData.CustomDataValue.stackComponent;
        Card card = stackComponent.TakeTopCard();
        
        ViewCardStackComponent handComponent = null;
        if (stackComponent == Player1.DeckComponent) {
            handComponent = Player1.HandComponent;
        } else if (stackComponent == Player2.DeckComponent) {
            handComponent = Player2.HandComponent;
        } else if (stackComponent == Player3.DeckComponent) {
            handComponent = Player3.HandComponent;
        }
        
        if (handComponent != null) {
            handComponent.AddCard(card);
        } else{
            Debug.Log("CardDrawn: could not match provided stack to player hand"); 
        }
        return;
    }
}