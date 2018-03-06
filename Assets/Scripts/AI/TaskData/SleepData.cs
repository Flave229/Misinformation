using Assets.Scripts.HouseholdItems;

namespace Assets.Scripts.AI.TaskData
{
    internal class SleepData
    {
        public Character2D General { get; set; }
        public Bed Bed { get; set; }
        public NeedStatus RestNeed { get; set; }
    }
}