using Assets.Scripts.General;
using Assets.Scripts.HouseholdItems;

namespace Assets.Scripts.AI.Tasks
{
    public class FindListeningDeviceData
    {
        public BuggableFurniture Furniture { get; set; }
        public General.General General { get; internal set; }
    }
}