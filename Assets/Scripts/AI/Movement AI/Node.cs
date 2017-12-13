using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.AI.Movement_AI
{
    public class Node
    {
        public Guid Id { get; set; }
        public Node Parent { get; set; }
        public System.Object Owner { get; set; }
        public List<Node> ConnectingNodes { get; set; }
        public Vector2 Position { get; set; }
        public float Heuristic { get; set; }
        public float TotalCost { get; set; }
        public float CurrentCost { get; set; }

        public Node()
        {
            Id = Guid.NewGuid();
            ConnectingNodes = new List<Node>();
        }
    }
}