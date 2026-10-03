using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using static CustomMessage;
using System.Collections;
using UnityEngine.EventSystems;

public class Game : MonoBehaviour, IMessageCardToGame, IDragHandler {
    private readonly string dataPath = "data.json";
    private readonly string CardPrefabPath = "Prefabs/CardPanel";
    public List<Event> Events { get; set; } 
    public List<Card> Cards { get; set; }
    public static GameObject BoardGameObject;
    private GameObject CardPrefab;
    private CardComponent CurrentSelectedCard;
    private CardComponent CurrentHeldCard;
    private Coroutine HoldCheckRef;
    
    [Serializable]
    public class Player  {
        public string Name { get; set; }
        public int ID { get; set; }
        public Card[] Decklist { get; set; }
    }    

    public Player Player1;
    public Player Player2;
    public Player Player3;

    public void Start() {
        Player1 = new Player {
            Name = "Player1",
            ID = 1
        };
        Player2 = new Player {
            Name = "Player2",
            ID = 2
        };
        Player3 = new Player {
            Name = "Player3",
            ID = 3
        };
        Debug.Log("Players:\n " + Player1.Name + "\n " + Player2.Name + "\n " + Player3.Name);

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

        // debug - testing adding card to board
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
        return;
    }

    public IEnumerator CheckForHold() {
        yield return new WaitForSeconds((float)0.5);
        CurrentHeldCard = CurrentSelectedCard;
    }

    public void SelectedCard(CustomData<CustomCardData> data) {
        CurrentSelectedCard = data.CustomDataValue.cardComponent;
        HoldCheckRef = StartCoroutine(CheckForHold());
        return;
    }

    public void HeldCard(CustomData<CustomCardData> data) {
        if (HoldCheckRef != null) {
            StopCoroutine(HoldCheckRef);
        } else if (data.CustomDataValue.cardComponent == CurrentHeldCard) {
            CurrentHeldCard = null;
        }
        HoldCheckRef = null;
        return;
    }

    public void OnDrag(PointerEventData eventData) {
        if (CurrentHeldCard != null) {
            CurrentHeldCard.gameObject.transform.localPosition += (Vector3)eventData.delta; 
        }    
    }

}