using System;
using System.Collections.Generic;
using Assets.Scripts.Physics;
using UnityEngine;

namespace Assets.Scripts.AI.Movement_AI
{
    class PlayerMovementAI : IMovementAI
    {
        private AStarPathfinding _aStarPathfinding;
        private Character2D _character;

        private List<Node> _currentPath;
        private Node _previousNode;

        public PlayerMovementAI(Character2D player, AStarPathfinding movementAI)
        {
            _aStarPathfinding = movementAI;
            _character = player;
            _currentPath = new List<Node>();
        }

        public void CreatePathTo(Vector3 location)
        {
            if (_currentPath.Count > 0)
                return;

            Node sourceNode = CreateSourceNode(_character.transform.position, location);
            Node targetNode = CreateTargetNode(sourceNode, location);
            _currentPath = _aStarPathfinding.CreatePath(sourceNode, targetNode);
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
                    source.ConnectingNodes.Add(doorComponent.Node);
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

            if (chosenRoom == null)
                throw new Exception("No room could be located for the given location");

            if (chosenRoom == _character.CurrentRoom)
            {
                target.ConnectingNodes.Add(sourceNode);
                return target;
            }

            foreach (Transform doorTransform in chosenRoom.transform)
            {
                var doorComponent = doorTransform.GetComponent<Door>();
                if (doorComponent != null)
                    target.ConnectingNodes.Add(doorComponent.Node);
            }

            target.Position = new Vector2(position.x, target.ConnectingNodes[0].Position.y);
            return target;
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
                    if (currentDoor != null && nextDoor != null && nextDoor.Node.Id == currentDoor.ConnectingDoor.Node.Id)
                        currentDoor.ActivateEvent(_character);
                }

                ActivateNextPathNode();
                return true;
            }

            return false;
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
    }
}