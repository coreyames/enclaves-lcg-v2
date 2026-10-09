using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardStackComponent : MonoBehaviour, IPointerClickHandler { 
    public GameObject CountPanel;
    public GameObject CountTMPObject;
    private static readonly string CardPrefabPath = "Prefabs/CardPanel";
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
    public void InitTopCardFaceDown(bool toSet) {
        IsTopCardFaceDown = toSet;
        TopCard.SetIsFaceDown(IsTopCardFaceDown);
        return;
    }
    
    public CardComponent TopCard { get; set; }
    
    public void Awake() {
        CardPrefab = Resources.Load<GameObject>(CardPrefabPath);
        TopCard = gameObject.GetComponentInChildren<CardComponent>();
    }
    
    public void Start() {
        CountPanel = gameObject.transform.Find("CountPanel").gameObject;
        CountTMPObject = CountPanel.transform.Find("CountTMP").gameObject;
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
        if (index >= Cards.Count) {
            return null;
        }
        Card card = Cards[index];
        Cards.RemoveAt(index);
        UpdateStackUI();
        return card;
    }

    public Card TakeTopCard() {
        return RemoveCard(0);
    }
    
    public List<Card> Draw(int n) {
        if (n > Cards.Count) {
            n = Cards.Count;
        }
        if (n > 0) {
            return (List<Card>)Cards.Take(n);
        } else {
            return new List<Card>();
        }
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
        TopCard.SetIsFaceDown(IsTopCardFaceDown);
        if (Cards.Count > 0) {
            TopCard.CardData = Cards[0];
        }
        if (CountTMPObject != null) {
            CountTMPObject.GetComponent<TextMeshProUGUI>().SetText(""+Cards.Count);
        }
        return;
    }
    
    public void OnPointerClick(PointerEventData eventData) {
        if (eventData.clickCount == 2) {
            Card card = TakeTopCard();
            GameObject newCardObject = Instantiate(CardPrefab);
            newCardObject.transform.localScale = Vector3.one * CardComponent.DefaultCardScale;
            newCardObject.transform.SetParent(gameObject.transform);
            newCardObject.GetComponent<CardComponent>().CardData = card;   
        }
        return;
    }
}