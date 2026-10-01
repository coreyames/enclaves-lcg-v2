using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardPile : ScriptableObject {
    //index is # from top if ordered
    public List<Card> Cards;
    public bool Ordered;

    public bool TopCardVisible { get; set; }
    
    public void ShuffleCards() {
        return;        
    }

    public List<Card> ShuffleCardsCopy() {
        return new List<Card>();
    }

    public bool FlipTopCard() {
        TopCardVisible = !TopCardVisible;
        return TopCardVisible;
    }

    public Card RemoveCard(int index) {
        return (Card)Cards.Take(index);
    }

    public Card TakeTopCard() {
        return RemoveCard(0);
    }

}