using Assets.Scripts.AI.Movement_AI;
using Assets.Scripts.AI.TaskData;
using System;
using UnityEngine;

namespace Assets.Scripts.AI.Tasks
{
    public class PathfindToLocationTask : ITask
    {
        private MovementAI _movementAI;
        private PathfindData _pathfindData;
        private bool _completed;
        private bool _movementNodeGenerated;
        private bool _pause;

        public PathfindToLocationTask(PathfindData pathfindData)
        {
            _pathfindData = pathfindData;
            _movementAI = new MovementAI(pathfindData.Character, new AStarPathfinding());
        }

        public void Execute()
        {
            if (_pause)
                return;

            if (_movementNodeGenerated == false)
            {
                _movementAI.ClearPath();
                _movementAI.CreatePathTo(_pathfindData.Location);
                _movementNodeGenerated = true;
            }

            Character2D character = _pathfindData.Character;
            var movementPath = _movementAI.GetCurrentPath();

            if (movementPath.Count <= 0)
            {
                character.Animator.SetBool("IDLE", true);
                SetCompleted();
                return;
            }

            Vector2 targetPosition = movementPath[movementPath.Count - 1].Position;
            float distance = targetPosition.x - character.transform.position.x;
            if (distance < 0.0f)
            {
                character.FaceRight();
                distance = -1.0f;
                character.Animator.SetBool("IDLE", false);
            }
            else if (distance > 0.0f)
            {
                character.FaceLeft();
                distance = 1.0f;
                character.Animator.SetBool("IDLE", false);
            }
            else
                character.Animator.SetBool("IDLE", true);

            _movementAI.CheckAndMoveToNextPathNode();
            character.transform.position = new Vector3(character.transform.position.x + (character.WalkSpeed * distance), character.transform.position.y, character.transform.position.z);
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public void SetCompleted()
        {
            _completed = true;
        }

        public double GetPriority()
        {
            // TODO check priority on this
            return 0;
        }

        public TaskPriorityType GetPriorityType()
        {
            return TaskPriorityType.WORK;
        }

        public void Pause()
        {
            _pathfindData.Character.Animator.SetBool("IDLE", true);
            _pause = true;
        }

        public void UnPause()
        {
            _pathfindData.Character.Animator.SetBool("IDLE", false);
            _pause = false;
        }

        public bool IsActive()
        {
            return _pause;
        }
    }
}