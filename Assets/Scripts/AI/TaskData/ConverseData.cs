namespace Assets.Scripts.AI.TaskData
{
    public class ConverseData
    {
        public ConverseData ConversationPartnerTaskData { get; set; }
        public General.General General { get; set; }
        public NeedStatus SocialNeed { get; set; }
        public bool ReadyToTalk { get; set; }
        public bool Talking { get; set; }
        public string Speech { get; set; }
        public bool Done { get; set; }
        public bool Listened { get; set; }
    }
}