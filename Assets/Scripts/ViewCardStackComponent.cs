using System.Collections.Generic;
using UnityEngine;

public class ViewCardStackComponent : MonoBehaviour {
    private static readonly string CardPrefabPath = "Prefabs/CardPanel";
    private static readonly float CardOverlapAmount = 0.5f;
    private static readonly int PanelMargin = 50;
    public GameObject CardPrefab;
    public List<Card> Cards { get; set; }
    
    public void Awake() {
        CardPrefab = Resources.Load<GameObject>(CardPrefabPath);   
    }
    
    public void Start() {
        gameObject.transform.localScale = Vector3.one * CardComponent.DefaultCardScale;
        for (int i = 0; i < Cards.Count; i++) {
            GameObject newCardObject = Instantiate(CardPrefab);
            newCardObject.transform.SetParent(gameObject.transform);
            newCardObject.transform.localScale = Vector3.one;
            int spacing = (int)(500.0f * CardOverlapAmount);
            newCardObject.transform.localPosition = new(PanelMargin + (i*spacing),-PanelMargin);
            CardComponent cardComponent = newCardObject.GetComponent<CardComponent>();
            cardComponent.gameObject.transform.Find("CardBack").gameObject.SetActive(false);
            cardComponent.CardData = Cards[i];   
        }
            
    }
}