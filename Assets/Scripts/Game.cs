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
        public ResourceCountsComponent countsComponent;
    }    

    public Player Player1;
    public Player Player2;
    public Player Player3;

    public void Start() {
        GameObject p1panel = GameObject.Find("BotLeftPanel");
        GameObject p2panel = GameObject.Find("TopLeftPanel");
        GameObject p3panel = GameObject.Find("TopRightPanel");
        Player1 = new Player {
            Name = "You areBottomleft",
            ID = 1,
            PanelObject = p1panel,           
            countsComponent = p1panel.GetComponentInChildren<ResourceCountsComponent>()
        };
        Player2 = new Player {
            Name = "Opponent1 isTopLeft",
            ID = 2,
            PanelObject = p2panel,           
            countsComponent = p2panel.GetComponentInChildren<ResourceCountsComponent>()
        };
        Player3 = new Player {
            Name = "Opponent2 isTopRight",
            ID = 3,
            PanelObject = p3panel,           
            countsComponent = p3panel.GetComponentInChildren<ResourceCountsComponent>()
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