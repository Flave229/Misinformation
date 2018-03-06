using Assets.Scripts.AI.TaskData;
using System;

namespace Assets.Scripts.AI.Tasks
{
    public class PathfindToLocationTask : ITask
    {
        private PathfindData _pathfindData;
        private bool _completed;
        private bool _movementNodeGenerated;
        
        public PathfindToLocationTask(PathfindData pathfindData)
        {
            _pathfindData = pathfindData;
        }

        public void Execute()
        {
            try
            {
                if (_movementNodeGenerated == false)
                {
                    _pathfindData.MovementAi.ClearPath();
                    _pathfindData.MovementAi.CreatePathTo(_pathfindData.Location);
                    _movementNodeGenerated = true;
                }

                if (_pathfindData.MovementAi.GetCurrentPath().Count == 0)
                    SetCompleted();
                else
                    _pathfindData.MovementAi.CheckAndMoveToNextPathNode();
            }
            catch (Exception e)
            {
                SetCompleted();
            }

            //Debug.Log("Pathfind to Location began execution!");
            //Debug.Log("Pathfinding to location: " + _pathfindData.Location);
        }

        public bool IsComplete()
        {
            return _completed;
        }

        public void SetCompleted()
        {
            _completed = true;
        }

        public bool GetCeilingLock()
        {
            return false;
        }

        public double GetPriority()
        {
            // TODO check priority on this
            return 0;
        }
    }
}