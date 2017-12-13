using Assets.Scripts.AI.TaskData;

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
            //Debug.Log("Pathfind to Location began execution!");
            //Debug.Log("Pathfinding to location: " + _pathfindData.Location);
            if (_movementNodeGenerated == false)
            {
                _pathfindData.GeneralMovementAI.ClearPath();
                _pathfindData.GeneralMovementAI.CreatePathTo(_pathfindData.Location);
                _movementNodeGenerated = true;
            }

            if (_pathfindData.GeneralMovementAI.GetCurrentPath().Count == 0)
                SetCompleted();
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
    }
}