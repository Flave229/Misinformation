using Assets.Scripts.AI.TaskData;
using UnityEngine;

namespace Assets.Scripts.AI.Tasks
{
    public class IdleTask : ITask
    {
        private Character2D _character;
        private bool _complete;

        public IdleTask(IdleData dataPacket)
        {
            _character = dataPacket.General;
        }

        public void Execute()
        {
            _character.Tasks.AddToStack(new IdleTask(new IdleData
            {
                General = _character
            }));

            System.Random randomGenerator = new System.Random();
            GameObject[] interestingObjects = GameObject.FindGameObjectsWithTag("Art");
            if (interestingObjects.Length <= 0)
                return;
            GameObject chosenObject = interestingObjects[randomGenerator.Next(0, interestingObjects.Length - 1)];
            Vector2 bedPosition = chosenObject.transform.position;

            _character.Tasks.AddToStack(new LookAtArtTask());
            _character.Tasks.AddToStack(new PathfindToLocationTask(new PathfindData
            {
                GeneralMovementAI = _character.MovementAi,
                Location = bedPosition
            }));

            SetCompleted();
        }

        public bool IsComplete()
        {
            return _complete;
        }

        public void SetCompleted()
        {
            _complete = true;
        }

        public bool GetCeilingLock()
        {
            return false;
        }
    }
}
