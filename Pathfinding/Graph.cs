using System.Collections.Generic;

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

        #region PathFinding
        #endregion
    }
}