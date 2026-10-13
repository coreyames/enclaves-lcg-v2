using UnityEngine.EventSystems;

public static class CustomMessage {
    public class CustomData<T> : BaseEventData {
        public CustomData(EventSystem eventSystem) : base(eventSystem) { }
        public T CustomDataValue;
    }
    
    public class CustomCardData {
        public CardComponent cardComponent;
    }
    
    public class CustomHandData {
        public ViewCardStackComponent stackComponent;
    }

    public interface IMessageCardToGame : IEventSystemHandler {
        void SelectedCard(CustomData<CustomCardData> eventData);
        void DroppedCard(CustomData<CustomCardData> eventData);
    } 
     
    public interface IMessageHandToGame : IEventSystemHandler {
        void SelectedHand(CustomData<CustomHandData> eventData);
        void DroppedHand(CustomData<CustomHandData> eventData);
    }
    
    public interface IMessageCardToHand : IEventSystemHandler {
        void HoveredCard(CustomData<CustomCardData> eventData);
        void ExitHoveredCard(CustomData<CustomCardData> eventData);
    }
}