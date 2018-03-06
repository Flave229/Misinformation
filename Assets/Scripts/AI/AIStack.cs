using Assets.Scripts.AI.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.AI
{
    public class AIStack
    {
        private ITask _executingTask;
        private ITask _concurrentTask;
        private readonly List<ITask> _tasks;
        private readonly List<ITask> _concurrentTasks;

        public AIStack()
        {
            _tasks = new List<ITask>();
            _concurrentTasks = new List<ITask>();
        }

        public void AddToStack(ITask task)
        {
            switch(task.GetPriorityType())
            {
                //case TaskPriorityType.WORK:
                //    _tasks.Add(task);
                //    break;
                //case TaskPriorityType.CONCURRENT:
                //    _concurrentTasks.Add(task);
                //    break;
                default:
                    _tasks.Add(task);
                    break;
            }
            //_tasks.Add(task);
        }

        public void Update()
        {
            HandleExecutingTask(_tasks, ref _executingTask);
            //HandleExecutingTask(_concurrentTasks, ref _concurrentTask);
        }

        private void HandleExecutingTask(List<ITask> taskList, ref ITask executingTask)
        {
            if (taskList.Count == 0 && executingTask == null)
                return;

            if (executingTask == null)
                SelectNextTask(ref taskList, ref executingTask);

            if (executingTask.IsComplete())
            {
                if (taskList.Count > 0)
                    SelectNextTask(ref taskList, ref executingTask);
                else
                {
                    executingTask = null;
                    return;
                }
            }

            executingTask.Execute();
        }

        private void SelectNextTask(ref List<ITask> taskList, ref ITask executingTask)
        {
            ITask highestPriorityTask = taskList.Aggregate((highestPriority, task) => task.GetPriority() > highestPriority.GetPriority() ? task : highestPriority);
            taskList.Remove(highestPriorityTask);
            executingTask = highestPriorityTask;
        }

        public void InterruptCurrentTask()
        {
            if (_executingTask != null)
                _executingTask.SetCompleted();
        }
        
        public void InterruptCurrentTaskIfType(Type type)
        {
            if (_executingTask != null && _executingTask.GetType() == type)
                _executingTask.SetCompleted();
        }

        public void Destroy()
        {
            if (_executingTask != null)
                _executingTask.SetCompleted();
            _tasks.Clear();
        }
    }
}
