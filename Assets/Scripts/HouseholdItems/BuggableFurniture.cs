using UnityEngine;

namespace Assets.Scripts.HouseholdItems
{
    public class BuggableFurniture : MonoBehaviour
    {
        private ListeningDevice _placedListeningDevice;

        public void PlaceListeningDevice(ListeningDevice listeningDevice)
        {
            if (_placedListeningDevice == null)
                _placedListeningDevice = listeningDevice;
        }

        public bool HasListeningDevice()
        {
            return _placedListeningDevice != null;
        }

        public ListeningDevice GetListeningDevice()
        {
            return _placedListeningDevice;
        }
    }
}