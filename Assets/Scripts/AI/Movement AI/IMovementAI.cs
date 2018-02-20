using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.Movement_AI
{
    public interface IMovementAI
    {
        void CreatePathTo(Vector3 location);
        List<Node> GetCurrentPath();
        bool CheckAndMoveToNextPathNode();
        void ActivateNextPathNode();
        void ClearPath();
        void ClearAndReturnToLastNode();
        Node CreateSourceNode(Vector2 position, Vector2 targetLocation);
        Node CreateTargetNode(Node sourceNode, Vector2 position);
    }
}