using System.Collections.Generic;
using Assets.Scripts.Physics;
using UnityEngine;

namespace Assets.Scripts.AI.Movement_AI
{
    class NPCMovementAI : IMovementAI
    {
		List<General.General> GeneralsList = new List<General.General> ();

        private Character2D _character;
        private AStarPathfinding _aStarPathfinding;

        private List<Node> _currentPath;
        private Node _previousNode;

        public NPCMovementAI(Character2D character, AStarPathfinding aStarPathfinding)
        {
            _character = character;
            _aStarPathfinding = aStarPathfinding;
            _currentPath = new List<Node>();
        }

        public bool CheckAndMoveToNextPathNode()
        {
            if (_currentPath.Count <= 0)
                return false;

            Room currentRoom = _character.CurrentRoom;
            Node nextPathNode = _currentPath[_currentPath.Count - 1];
            BoxCollider2D roomCollider = currentRoom.GetComponent<BoxCollider2D>();
            Vector3 roomPosition = currentRoom.transform.position + new Vector3(roomCollider.offset.x, roomCollider.offset.y, 0.0f);
            if (nextPathNode.Position.x - 0.2 < _character.transform.position.x
                && nextPathNode.Position.x + 0.2 > _character.transform.position.x
                && CollisionBox.PointInBoxCollision(roomPosition, roomCollider.size, _character.transform.position))
            {
                if (_currentPath.Count > 2)
                {
                    Door currentDoor = (Door)_currentPath[_currentPath.Count - 1].Owner;
                    Door nextDoor = (Door)_currentPath[_currentPath.Count - 2].Owner;
                    if (currentDoor != null && nextDoor != null && nextDoor.m_Node.Id == currentDoor.m_ConnectingDoor.m_Node.Id)
                        currentDoor.ActivateEvent(_character);
                }

                ActivateNextPathNode();
                return true;
            }

            return false;
        }

		public void CreatePathTo(Vector3? location = null)
		{
			if (_currentPath.Count > 0)
				return;
            if (location != null)
            {
                Node sourceNode = CreateSourceNode(_character.transform.position, (Vector3)location);
                Node targetNode = CreateTargetNode(sourceNode, (Vector3)location);
                _currentPath = _aStarPathfinding.CreatePath(sourceNode, targetNode);
            }
            else
            {
                Door[] doors = (Door[])UnityEngine.Object.FindObjectsOfType(typeof(Door));
                Vector3 randomDoorPosition = doors[new System.Random().Next(0, doors.Length)].transform.position;
                Node sourceNode = CreateSourceNode(_character.transform.position, randomDoorPosition);
                Node targetNode = CreateTargetNode(sourceNode, randomDoorPosition);
                _currentPath = _aStarPathfinding.CreatePath(sourceNode, targetNode);
            }
		}

		public void CreatePathToObj(Vector2 TargetPos)
		{
			if (_currentPath.Count > 0)
				return;

			Door[] doors = (Door[])UnityEngine.Object.FindObjectsOfType(typeof(Door));
			Vector3 randomDoorPosition = doors[new System.Random().Next(0, doors.Length)].transform.position;
			Node sourceNode = CreateSourceNode(_character.transform.position, TargetPos);
			Node targetNode = CreateTargetNode(sourceNode, TargetPos);
			_currentPath = _aStarPathfinding.CreatePath(sourceNode, targetNode);
		}

        public List<Node> GetCurrentPath()
        {
            return _currentPath;
        }

        public void ClearPath()
        {
            _currentPath.Clear();
        }

        public void ActivateNextPathNode()
        {
            _previousNode = _currentPath[_currentPath.Count - 1];
            _currentPath.RemoveAt(_currentPath.Count - 1);
        }

        public void ClearAndReturnToLastNode()
        {
            ClearPath();
            _currentPath.Add(_previousNode);
        }

        public Node CreateSourceNode(Vector2 position, Vector2 targetLocation)
        {
            Node source = new Node
            {
                Position = position,
                CurrentCost = 0.0f
            };
            source.Heuristic = Vector2.Distance(source.Position, targetLocation);
            source.TotalCost = source.Heuristic;

            foreach (Transform childTransform in _character.CurrentRoom.transform)
            {
                var doorComponent = childTransform.GetComponent<Door>();
                if (doorComponent != null)
                    source.ConnectingNodes.Add(doorComponent.m_Node);
            }

            return source;
        }

        public Node CreateTargetNode(Node sourceNode, Vector2 position)
        {
            Node target = new Node
            {
                Position = position,
                CurrentCost = 0.0f,
                Heuristic = 0.0f
            };
            target.TotalCost = target.Heuristic;

            Room[] rooms = (Room[])UnityEngine.Object.FindObjectsOfType(typeof(Room));
            Room chosenRoom = null;
            foreach (Room room in rooms)
            {
                var boxCollider = room.GetComponent<BoxCollider2D>();
                Vector2 boxColliderPosition = new Vector2(boxCollider.transform.position.x, boxCollider.transform.position.y);
                if (CollisionBox.PointInBoxCollision(boxColliderPosition + boxCollider.offset, new Vector2(boxCollider.size.x, boxCollider.size.y), position) == false)
                    continue;
                
                chosenRoom = room;
                break;
            }

            target.Position = new Vector2(position.x, chosenRoom.gameObject.transform.position.y);

            foreach (Transform doorTransform in chosenRoom.transform)
            {
                var doorComponent = doorTransform.GetComponent<Door>();
                if (doorComponent != null)
                    target.ConnectingNodes.Add(doorComponent.m_Node);
            }

            return target;
        }

		public void ChooseGeneral()
		{
			int r = UnityEngine.Random.Range(0, GameManager.Instance().GetGenList().Count);

			if (GameManager.Instance().GetGenList()[r]) 
			{
				var genPos = GameManager.Instance().GetGenList()[r].gameObject.transform.position;

				Node sourceNode = CreateSourceNode (_character.transform.position, genPos);
				Node targetNode = CreateTargetNode (sourceNode, genPos);

				_currentPath = _aStarPathfinding.CreatePath (sourceNode, targetNode);
			} 
		}
    }
}