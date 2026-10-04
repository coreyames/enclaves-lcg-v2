using UnityEngine.EventSystems;

public static class CustomMessage {
    public class CustomData<T> : BaseEventData {
        public CustomData(EventSystem eventSystem) : base(eventSystem) { }
        public T CustomDataValue;
    }
    
    public class CustomCardData {
        public CardComponent cardComponent;
    }

    public interface IMessageCardToGame : IEventSystemHandler {
        void SelectedCard(CustomData<CustomCardData> eventData);
    } 
}