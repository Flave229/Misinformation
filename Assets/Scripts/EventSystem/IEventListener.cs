namespace Assets.Scripts.EventSystem
{
    public interface IEventListener
    {
        void ConsumeEvent(Event subscribeEvent, object eventPacket);
        void SubscribeToEvents();
    }
}