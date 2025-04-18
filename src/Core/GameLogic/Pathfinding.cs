using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cr_mono.Core.GameLogic
{
    // Pathfinding file for calculating shortest route between 2 xy co-ords
    // loosely based off: https://github.com/Matthew-J-Spencer/Pathfinding

    internal class Node : IComparable<Node> {
        internal Vector2 Position { get; }
        internal bool Traversable { get; }
        internal float G { get; set; }
        internal float H { get; set; }
        internal float F => G + H;
        public Node Parent { get; set; }

        public Node(Vector2 position, bool traversable) 
        {
            Position = position;
            Traversable = traversable;
        }

        public int CompareTo(Node other) 
        {
            int compare = F.CompareTo(other.F);
            return compare == 0 ? H.CompareTo(other.H) : compare;
        }

        public override bool Equals(object obj)
        {
            return obj is Node other && Position == other.Position;
        }

        public override int GetHashCode()
        {
            return Position.GetHashCode();
        }
    }

    internal static class Pathfinding
    {
        private static Dictionary<Vector2, Node> allNodes = new Dictionary<Vector2, Node>();

        internal static List<Vector2> FindPath(
            Vector2 start, 
            Vector2 end, 
            Dictionary<Vector2, bool> navMap) 
        {
            var accessibleTiles = MapGen.FloodFillTiles(
                navMap.ToDictionary(kvp => kvp.Key, kvp => kvp.Value ? 1 : 0),
                start,
                new HashSet<Vector2>());
            var reducedNavMap = accessibleTiles.ToDictionary(tile => tile, tile => navMap[tile]);
            navMap = reducedNavMap;

            if (!navMap.ContainsKey(end))
            {
                return null;
            }

            var openList = new PriorityQueue<Node, float>();
            var closedList = new HashSet<Node>();

            var startNode = GetOrCreateNode(start, navMap[start]);
            var targetNode = GetOrCreateNode(end, navMap[end]);

            openList.Enqueue(startNode, startNode.F);

            while (openList.Count > 0) 
            {
                Node currentNode = openList.Dequeue();
                closedList.Add(currentNode);

                if (currentNode.Position == targetNode.Position) 
                {
                    return RetracePath(startNode, currentNode);
                }

                foreach (var neighbor in GetNeighbors(currentNode, navMap))
                {
                    if (!neighbor.Traversable || closedList.Contains(neighbor)) 
                    {
                        continue;
                    }

                    float newMovementCostToNeighbor = 
                        currentNode.G + GetDistance(currentNode, neighbor);
                    if (newMovementCostToNeighbor < neighbor.G ||
                        !openList.UnorderedItems.Any(item => item.Element == neighbor)) 
                    {
                        neighbor.G = newMovementCostToNeighbor;
                        neighbor.H = GetDistance(neighbor, targetNode);
                        neighbor.Parent = currentNode;

                        openList.Enqueue(neighbor, neighbor.F);
                    }
                }
            }

            return null;
        }

        private static Node GetOrCreateNode(Vector2 position, bool traversable) 
        {
            if (!allNodes.ContainsKey(position))
            {
                allNodes[position] = new Node(position, traversable);
            }
            return allNodes[position];
        }

        private static List<Node> GetNeighbors(
            Node node, 
            Dictionary<Vector2, bool> navMap) 
        {
            var neighbors = new List<Node>();
            var directions = ProcGenHelpers.GetCardinalDirections();

            foreach (var direction in directions) 
            {
                var neighborPos = node.Position + direction;
                if (navMap.ContainsKey(neighborPos)) 
                {
                    neighbors.Add(GetOrCreateNode(neighborPos, navMap[neighborPos]));
                }
            }

            return neighbors;
        }

        private static float GetDistance(Node a, Node b) 
        {
            return Math.Abs(a.Position.X - b.Position.X) + Math.Abs(a.Position.Y - b.Position.Y);
        }

        private static List<Vector2> RetracePath(Node startNode, Node endNode) 
        {
            var path = new List<Vector2>();
            var currentNode = endNode;

            while (currentNode != null && currentNode != startNode) 
            {
                path.Add(currentNode.Position);
                currentNode = currentNode.Parent;
            }

            if (currentNode == null)
            {
                return null;
            }

            path.Reverse();
            return path;
        }
    }
}