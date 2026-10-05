using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using static CustomMessage;
using System.Collections;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour, IMessageCardToGame, IDragHandler, IPointerUpHandler {
    private readonly string dataPath = "data.json";
    private readonly string CardPrefabPath = "Prefabs/CardPanel";
    public List<Event> Events { get; set; } 
    public List<Card> Cards { get; set; }
    public static GameObject BoardGameObject;
    public GameObject CardPrefab { get; set; }
    public CardComponent CurrentSelectedCard;
    public CardComponent CurrentHeldCard; 
     
    [Serializable]
    public class Player  {
        public string Name { get; set; }
        public int ID { get; set; }
        public Card[] Decklist { get; set; }
        public GameObject PanelObject; 
        public ResourceCountsComponent CountsComponent;
    
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

    public void Start() {
        GameObject p1panel = GameObject.Find("BotLeftPanel");
        GameObject p2panel = GameObject.Find("TopLeftPanel");
        GameObject p3panel = GameObject.Find("TopRightPanel");
        Player1 = new Player {
            Name = "Player1-BottomLeft",
            ID = 1,
            PanelObject = p1panel,           
            CountsComponent = p1panel.GetComponentInChildren<ResourceCountsComponent>()
        };
        Player2 = new Player {
            Name = "Player2-TopLeft",
            ID = 2,
            PanelObject = p2panel,           
            CountsComponent = p2panel.GetComponentInChildren<ResourceCountsComponent>()
        };
        Player3 = new Player {
            Name = "Player3-TopRight",
            ID = 3,
            PanelObject = p3panel,           
            CountsComponent = p3panel.GetComponentInChildren<ResourceCountsComponent>()
        };
        Debug.Log("Players: " + Player1.Name + ", " + Player2.Name + ", " + Player3.Name);
        
        // Load card and event sets
        string path = Path.Combine(Application.dataPath, dataPath);
        if (!File.Exists(path)) {
            return;
        }
        BoardGameObject = GameObject.Find("Board");
        CardPrefab = Resources.Load<GameObject>(CardPrefabPath);
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
        
        //
        // ready for start here
        //
        
        // development - testing card placement and interaction
        if (Cards.Count >= 1) {
            GameObject newCardObject = Instantiate(CardPrefab);
            newCardObject.transform.SetParent(BoardGameObject.transform);
            CardComponent cc = newCardObject.GetComponent<CardComponent>();
            cc.CardData = Cards[0];
            newCardObject.transform.localPosition = new Vector3(0,0,0);
            
            GameObject newCardObject2 = Instantiate(CardPrefab);
            newCardObject2.transform.SetParent(BoardGameObject.transform);
            CardComponent cc2 = newCardObject2.GetComponent<CardComponent>();
            cc2.CardData = Cards[1];
            newCardObject2.transform.localPosition = new Vector3(300,-300,0);
        }
        
        // development - working with resource counters         
        
        return;
    }

    public IEnumerator CheckForHold() {
        yield return new WaitForSeconds((float)0.1);
        CurrentHeldCard = CurrentSelectedCard;
    }

    // pointerdown message from card
    public void SelectedCard(CustomData<CustomCardData> data) {
        CurrentSelectedCard = data.CustomDataValue.cardComponent;
        CurrentHeldCard = CurrentSelectedCard;
        return;
    }
 
    public void OnPointerUp(PointerEventData eventData) {
        CurrentHeldCard = null;
    }
    
    public void OnDrag(PointerEventData eventData) {
        if (CurrentHeldCard != null) {
            CurrentHeldCard.gameObject.transform.localPosition += (Vector3)eventData.delta; 
        }    
    }
}