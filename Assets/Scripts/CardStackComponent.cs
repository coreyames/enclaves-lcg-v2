using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardStackComponent : MonoBehaviour {
    private readonly Vector2 CountPanelTopPlacement = new(20, 110);
    private readonly Vector2 CountTMPTopPlacement = new(0, 20);
    private readonly Vector2 CountPanelBottomPlacement = new(20, -660);
    private readonly Vector2 CountTMPBottomPlacement = new(0, -20);
    public bool CountPanelOnBottom { get; private set; } = false;
    public void SetCountPanelOnBottom(bool toSet) {
        CountPanelOnBottom = toSet;
        UpdateStackUI();
    }
    public void InitCountPanelOnBottom(bool init) {
        CountPanelOnBottom = init;
    }
    public GameObject CountPanel;
    public GameObject CountTMPObject;
    private readonly string CardPrefabPath = "Prefabs/CardPanel";
    public GameObject Prefab;
    public GameObject CardPrefab;
    
    //index is # from top if ordered
    public List<Card> Cards { get; private set; }
    public void SetCards(List<Card> _cards) { 
        Cards = _cards;
        UpdateStackUI();
    }
    public bool IsTopCardFaceDown { get; private set; } = false;
    
    public CardComponent TopCard { get; set; }
    public bool Draggable { get; set; }
    
    public void Start() {
        CardPrefab = Resources.Load<GameObject>(CardPrefabPath);
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
    
    public void SetTopCardFaceDown(bool faceDown) {
        if (Cards.Count < 1) return;
        IsTopCardFaceDown = faceDown;
        TopCard.SetIsFaceDown(IsTopCardFaceDown);
        UpdateStackUI();
    }
    
    public void UpdateStackUI() {
        if (TopCard != null) {
            Destroy(TopCard);
        }
        GameObject newCardObject = Instantiate(CardPrefab);
        TopCard = newCardObject.GetComponent<CardComponent>();
        Image img = GameObject.Find("CardBack").GetComponent<UnityEngine.UI.Image>();
        Color c = img.color;
        if (Cards.Count > 0) {
            img.color = new Color(c.r,c.g,c.b,0);
            TopCard.CardData = Cards[0];
            TopCard.SetIsFaceDown(IsTopCardFaceDown);
        } else {
            img.color = new Color(c.r,c.g,c.b,1);
        }            
        TopCard.SetIsFaceDown(IsTopCardFaceDown);
        if (CountPanelOnBottom) {
            CountPanel.transform.localPosition = CountPanelBottomPlacement;
            CountTMPObject.transform.localPosition = CountTMPBottomPlacement;
        } else {
            CountPanel.transform.localPosition = CountPanelTopPlacement;
            CountTMPObject.transform.localPosition = CountTMPTopPlacement;
        }
        CountTMPObject.GetComponentInChildren<TextMeshProUGUI>().SetText(""+Cards.Count);
        return;
    }
}