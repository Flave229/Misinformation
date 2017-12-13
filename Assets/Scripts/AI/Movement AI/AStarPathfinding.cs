using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.Movement_AI
{
    public class AStarPathfinding
    {
        public List<Node> CreatePath(Node source, Node target)
        {
            List<Node> closedList = new List<Node>();
            List<Node> openList = new List<Node>
            {
                source
            };

            while (openList.Count > 0)
            {
                Node cheapestNode = openList[0];
                int cheapestNodeIndex = 0;

                for(int i = 0; i < openList.Count; i++)
                {
                    if (openList[i].TotalCost < cheapestNode.TotalCost)
                    {
                        cheapestNode = openList[i];
                        cheapestNodeIndex = i;
                    }
                }

                openList.RemoveAt(cheapestNodeIndex);

                var connectedNodes = cheapestNode.ConnectingNodes;

                foreach (var connectedNode in connectedNodes)
                {
                    Node nodeCopy = new Node
                    {
                        Id = connectedNode.Id,
                        Parent = cheapestNode,
                        Owner = connectedNode.Owner,
                        ConnectingNodes = connectedNode.ConnectingNodes,
                        Position = connectedNode.Position,
                        CurrentCost = cheapestNode.CurrentCost + Vector2.Distance(connectedNode.Position, cheapestNode.Position),
                        Heuristic = Vector2.Distance(connectedNode.Position, target.Position)
                    };

                    nodeCopy.TotalCost = nodeCopy.CurrentCost + nodeCopy.Heuristic;

                    bool skipNode = false;

                    //Check to see if node already exists in the list
                    for (int i = 0; i < openList.Count; i++)
                    {
                        if (openList[i].Id == nodeCopy.Id && nodeCopy.TotalCost > openList[i].TotalCost)
                            skipNode = true;
                    }

                    if (skipNode)
                        continue;

                    for (int i = 0; i < closedList.Count; i++)
                    {
                        if (closedList[i].Id == nodeCopy.Id && nodeCopy.TotalCost > closedList[i].TotalCost)
                            skipNode = true;
                    }

                    if (skipNode)
                        continue;

                    openList.Add(nodeCopy);
                }

                closedList.Add(cheapestNode);
            }

            List<Node> currentPath = new List<Node>();
            Node travelNode = target;
            travelNode.TotalCost = float.MaxValue;

            for(int i = 0; i < closedList.Count; i++)
            {
                if (closedList[i].Id == travelNode.ConnectingNodes[0].Id)
                {
                    if (travelNode.Parent == null || travelNode.Parent.TotalCost > closedList[i].TotalCost)
                        travelNode.Parent = closedList[i];
                }
            }

            currentPath.Add(travelNode);

            while(travelNode.Parent != null)
            {
                travelNode = travelNode.Parent;
                currentPath.Add(travelNode);
            }

            return currentPath;
        }
    }
}