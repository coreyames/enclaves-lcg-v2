using UnityEngine;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Game : MonoBehaviour {
    private readonly string dataPath = "data.json";

    public List<Event> Events { get; set; } 
    public List<Card> Cards { get; set; }
 
    private GameObject BoardGameObject;
    private string CardPrefabPath = "Prefabs/CardPanel";
    private GameObject CardPrefab;

    public void Start() {
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
        if (Cards.Count > 0) {
            GameObject newCardObject = Instantiate(CardPrefab);
            newCardObject.transform.SetParent(BoardGameObject.transform);
            CardComponent cc = newCardObject.GetComponent<CardComponent>();
            cc.CardData = Cards[0];
        }        
        return;
    }
}