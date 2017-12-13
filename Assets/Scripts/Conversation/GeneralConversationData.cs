namespace Assets.Scripts.Conversation
{
    public class GeneralConversationData
    {
        public General.General General { get; set; }

        public int Truthfulness = 0;
        public bool Lying = false;
        public string Event = "";
        public string Place = "";
        public string Time = "";
    }
}