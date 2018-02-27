using Assets.Scripts.HouseholdItems;

namespace Assets.Scripts.AI.TaskData
{
    internal class SitData
    {
        public Character2D General { get; set; }
        public Chair Chair { get; set; }
        public NeedStatus RestNeed { get; set; }
    }
}