using UnityEngine;
using UnityEngine.EventSystems;

public static class CustomMessage {

    public class CustomCardData {
        public GameObject CardObject;
        public string CardName;
    }

    public class CustomData<T> : BaseEventData {
        public CustomData(EventSystem eventSystem) : base(eventSystem) { }
        public T CustomDataValue;
    }
    
    public interface IMessageCardToGame : IEventSystemHandler {
        void SelectedCard(CustomData<CustomCardData> eventData);
        void HeldCard(CustomData<CustomCardData> eventData);
    }

    public interface IMessageGameToCard : IEventSystemHandler {
        
    }

}