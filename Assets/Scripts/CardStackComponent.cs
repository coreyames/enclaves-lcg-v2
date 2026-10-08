using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class CardStackComponent : MonoBehaviour { 
    public GameObject CountPanel;
    public GameObject CountTMPObject;
    private readonly string CardPrefabPath = "Prefabs/CardPanel";
    public GameObject Prefab;
    public GameObject CardPrefab;
    public Vector3 CardLocalLocation;
    
    //index is # from top if ordered
    public List<Card> Cards { get; private set; }
    public void SetCards(List<Card> _cards) { 
        Cards = _cards;
        UpdateStackUI();
    }
    public void InitCards(List<Card> _cards) { 
        Cards = _cards;
    }
    
    public bool IsTopCardFaceDown { get; private set; } = false;
    public void SetTopCardFaceDown(bool faceDown) {
        IsTopCardFaceDown = faceDown;
        TopCard.SetIsFaceDown(IsTopCardFaceDown);
        UpdateStackUI();
    }
    public void InitTopCardFaceDown(bool toSet) {
        IsTopCardFaceDown = toSet;
        return;
    }
    
    public CardComponent TopCard { get; set; }
    
    public void Start() {
        CardPrefab = Resources.Load<GameObject>(CardPrefabPath);
        CountPanel = gameObject.transform.Find("CountPanel").gameObject;
        CountTMPObject = CountPanel.transform.Find("CountTMP").gameObject;
        TopCard = gameObject.GetComponentInChildren<CardComponent>();
        CardLocalLocation = TopCard.gameObject.transform.localPosition;
        Cards ??= new List<Card>();
        UpdateStackUI();
    }
    
    // done in place - copy if needed before shuffling
    public void Shuffle() {
        for (int i = Cards.Count - 1; i > 0; i--) {
            Card ci = Cards[i];
            int j = Random.Range(0, i);
            Cards[i] = Cards[j];
            Cards[j] = ci;    
        }
        UpdateStackUI();
    }
        
    // various convenience access methods
    public Card Top() {
        return ViewCard(0);
    }
    
    public Card ViewCard(int index) {
        Card card = null;
        if (index < Cards.Count) {
            card = Cards[index];
        }
        return card;
    }
    
    public Card RemoveCard(int index) {
        Card card = (Card)Cards.Take(index);
        UpdateStackUI();
        return card;
    }

    public Card TakeTopCard() {
        return RemoveCard(0);
    }
    
    public void InsertCard(int index, Card card) {
        Cards.Insert(index, card);    
        UpdateStackUI(); 
    }
    
    public void PlaceOnTop(Card card) {
        Cards.Insert(0, card);    
        UpdateStackUI();
    }
    
    public void PlaceOnBottom(Card card) {
        Cards.Insert(Cards.Count-1, card);    
        UpdateStackUI();
    }
     
    public void UpdateStackUI() {
        if (TopCard != null) {
            Destroy(TopCard.gameObject);
        }
        GameObject newCardObject = Instantiate(CardPrefab);
        newCardObject.transform.localScale = Vector3.one * CardComponent.DefaultCardScale;
        newCardObject.transform.SetParent(gameObject.transform);
        newCardObject.transform.localPosition = CardLocalLocation;
        TopCard = newCardObject.GetComponent<CardComponent>();
        TopCard.InitIsFaceDown(IsTopCardFaceDown);
        if (Cards.Count > 0) {
            TopCard.CardData = Cards[0];
        }
        CountTMPObject.GetComponent<TextMeshProUGUI>().SetText(""+Cards.Count);
        return;
    }
}