using Assets.Scripts.AI.Movement_AI;
using UnityEngine;

namespace Assets.Scripts.AI.TaskData
{
    public class PathfindData
    {
        public IMovementAI MovementAi { get; set; }
        public Vector3 Location { get; set; }
    }
}