using UnityEngine;

namespace Assets.Scripts
{
    public class Exit : MonoBehaviour, ICollisionEvent
    {
        public Exit m_ExitPosition;
        public void ActivateEvent(Character2D character)
        {
            if (m_ExitPosition != null)
            {
                character.transform.position = m_ExitPosition.transform.position;
                //character.CurrentRoom = ConnectingDoor.ParentRoom;
            }
        }
    }
}