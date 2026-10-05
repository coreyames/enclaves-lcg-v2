using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardStackComponent : MonoBehaviour {
    private readonly string CardPrefabPath = "Prefabs/CardPanel";
    public GameObject Prefab;
    public GameObject CardPrefab;
    
    //index is # from top if ordered
    public List<Card> Cards { get; set; }
    public bool IsTopCardFaceDown { get; private set; } = false;
    public CardComponent TopCard { get; set; }
    public bool Draggable { get; set; }
    
    public void Start() {
        if (Cards.Count < 1) return;
        CardPrefab = Resources.Load<GameObject>(CardPrefabPath);
        GameObject newCardObject = Instantiate(CardPrefab);
        TopCard = newCardObject.GetComponent<CardComponent>();
        TopCard.CardData = Cards[0];
        TopCard.SetIsFaceDown(IsTopCardFaceDown);
    }
    
    // done in place - copy if needed before shuffling
    public void Shuffle() {
        for (int i = Cards.Count - 1; i > 0; i--) {
            Card ci = Cards[i];
            int j = Random.Range(0, i);
            Cards[i] = Cards[j];
            Cards[j] = ci;    
        }
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
        return (Card)Cards.Take(index);
    }

    public Card TakeTopCard() {
        return RemoveCard(0);
    }
    
    public void InsertCard(int index, Card card) {
        Cards.Insert(index, card);    
    }
    
    public void PlaceOnTop(Card card) {
        Cards.Insert(0, card);    
    }
    
    public void PlaceOnBottom(Card card) {
        Cards.Insert(Cards.Count-1, card);    
    }
    
    public void SetTopCardFaceDown(bool faceDown) {
        if (Cards.Count < 1) return;
        IsTopCardFaceDown = faceDown;
        TopCard.SetIsFaceDown(IsTopCardFaceDown);
    }
}