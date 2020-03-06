using System.Collections.Generic;
using System.Linq;
public enum PathFindingType : byte
{
    AStar, Dijkstra
}

namespace Pathfinding
{
    public class Graph
    {
        #region Variables
        private Dictionary<int, HashSet<int>> edges;

        public int VerticesCount { get { return edges.Count; } }
        #endregion

        #region Basic graph functionality: Add/Remove edge, Add/Remove vertice, Find if reachable vertice
        public Graph() => edges = new Dictionary<int, HashSet<int>>();

        public void AddVertice(int value) => edges.Add(value, new HashSet<int>());

        public void RemoveVertice(int value) => edges.Remove(value);

        public void AddOneDirectionEdge(int from, int to) => edges[from].Add(to);

        public void RemoveOneDirectionEdge(int from, int to) => edges[from].Remove(to);

        public void AddTwoDirectionEdge(int edgeOne, int edgeTwo)
        {
            AddOneDirectionEdge(edgeOne, edgeTwo);
            AddOneDirectionEdge(edgeTwo, edgeOne);
        }

        public void RemoveTwoDirectionEdge(int edgeOne, int edgeTwo)
        {
            RemoveOneDirectionEdge(edgeOne, edgeTwo);
            RemoveOneDirectionEdge(edgeTwo, edgeOne);
        }

        // Breath First Search algorithm
        public bool IsAccessableFrom(int fromValue, int targetValue)
        {
            if(fromValue == targetValue) return true;

            HashSet<int> encountered = new HashSet<int>();  // NOTE: Better performance for scaling than List

            // Contains values of vertices that are reached, but not checked
            Queue<int> pendingVertices = new Queue<int>();

            pendingVertices.Enqueue(fromValue);
            encountered.Add(fromValue);

            while(pendingVertices.Count > 0)
            {
                int currentVertice = pendingVertices.Dequeue();

                foreach(int connectedVertice in edges[currentVertice])
                {
                    if(connectedVertice == targetValue)
                        return true;
                    
                    if(encountered.Contains(connectedVertice))
                        continue;

                    encountered.Add(connectedVertice);
                    pendingVertices.Enqueue(connectedVertice);
                }
            }

            return false;
        }
        #endregion

        #region PathFinding A* and Dijkstra

        public List<int> GetPath(int from, int to, PathFindingType type = PathFindingType.AStar)
        {
            switch(type)
            {
                case PathFindingType.AStar:
                    return AStarPathFinding(from, to);
                case PathFindingType.Dijkstra:
                    return DijkstraPathFinding(from, to);
                default:
                    throw new System.Exception("Unknown pathfinding type");
            }
        }

        private List<int> AStarPathFinding(int from, int to)
        {
            return new List<int>();
        }

        private List<int> DijkstraPathFinding(int from, int to)
        {
            List<int> unvisited = new List<int>(edges.Keys);

            Dictionary<int, int> dist = new Dictionary<int, int>();
            Dictionary<int, int?> prev = new Dictionary<int, int?>();

            foreach(var vertice in edges)
            {
                dist.Add(vertice.Key, int.MaxValue);
                prev.Add(vertice.Key, null);
            }
            dist[from] = 0;

            while(unvisited.Count > 0)
            {
                // Finds node with smallest distance
                unvisited = unvisited.OrderBy(x => dist[x]).ToList();
                int current = unvisited[0];

                // If node is target then path is found
                if(current == to)
                    break;

                unvisited.Remove(current);

                System.Console.WriteLine("Current:{0}", current);
                if(!IsAccessableFrom(from, current))
                    continue;
                System.Console.WriteLine("-is accesable");

                int currentsDistance = dist[current];
                // Loops through out all neighbors
                foreach(int vertice in edges[current])
                {
                    // Distance from current node + edges length
                    int alt = currentsDistance + 1;
                    if(alt < dist[vertice])
                    {
                        dist[vertice] = alt;
                        prev[vertice] = current;
                    }
                }
            }

            // If target not reached
            if(prev[to] == null)
                return null;
            
            // Rebuilds path
            List<int> path = new List<int>();
            int curr = to;  // End position
            while(prev[curr] != null)
            {
                path.Add(curr);
                curr = prev[curr] ?? 0;
            };
            path.Add(from); // Start position
            path.Reverse();
            // --end of : path rebuilding --

            return path;
        }

        #endregion
    }
}